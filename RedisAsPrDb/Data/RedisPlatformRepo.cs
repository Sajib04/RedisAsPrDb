using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using RedisAsPrDb.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RedisAsPrDb.Data
{
    public class RedisPlatformRepo : IPlatormRepo
    {
        private readonly IConnectionMultiplexer _redisConnectionMultiplexer;

        public RedisPlatformRepo(IConnectionMultiplexer redisConnectionMultiplexer)
        {
            _redisConnectionMultiplexer = redisConnectionMultiplexer;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentOutOfRangeException(nameof(platform));
            }
            var db = _redisConnectionMultiplexer.GetDatabase();
            var serializedPlatform = JsonSerializer.Serialize(platform);
            //db.SetAdd("PlatformSet", serializedPlatform);
            db.StringSet(platform.Id, serializedPlatform);            

            //for hashentry
            //db.HashSet($"hashplatform", new HashEntry[]
            //    {new HashEntry(platform.Id, serializedPlatform)});
        }

        public string DeletePlatformById(string id)
        {
            var db = _redisConnectionMultiplexer.GetDatabase();
            var platform = GetPlatformById(id);
            if (platform != null)
            {
                if (platform.Id == id)
                {
                    var result = db.KeyDelete(id);
                    return result.ToString();
                }
            }
                
            return "";
        }

        public IEnumerable<Platform?>? GetAllPlatform()
        {
            List<string> listKeys = new List<string>();
            Dictionary<string, string> dictionaryKeyValue = new Dictionary<string, string>();
            var db = _redisConnectionMultiplexer.GetDatabase();
            //var completeSet = db.SetMembers("PlatformSet");
            //if (completeSet.Length > 0)
            //{
            //    var obj = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<Platform>(val));
            //    return obj;
            //}

            var endpoints = _redisConnectionMultiplexer.GetEndPoints();
            var server = _redisConnectionMultiplexer.GetServer(endpoints[0]);
            var keys = server.Keys();
            listKeys.AddRange(keys.Select(key => (string)key).ToList());
            foreach (var key in listKeys)
            {
                var value = db.StringGet(key);
                dictionaryKeyValue.Add(key, value);
            }
            var obj = JsonSerializer.Serialize<Platform>(dictionaryKeyValue);


            return obj;


            
        }



        public Platform? GetPlatformById(string id)
        {
            var db = _redisConnectionMultiplexer.GetDatabase();

            var plat = db.StringGet(id);
            
            //db.StringGetWithExpiry(serializedPlatform);
            //for hashentry
            //var plat = db.HashGet("hashplatform", id);
            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat);
            }
            return null;
        }

        public Platform? DeleteByKey( string key)
        {
            var db = _redisConnectionMultiplexer.GetDatabase();
            db.KeyDelete(key);
            return null;
        }

    }
}
