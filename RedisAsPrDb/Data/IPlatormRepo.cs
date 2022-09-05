using RedisAsPrDb.Models;
using System.Collections.Generic;

namespace RedisAsPrDb.Data
{
    public interface IPlatormRepo
    {
        void CreatePlatform(Platform platform);
        string DeletePlatformById(string id);
        Platform? GetPlatformById(string id);
        //IEnumerable<Platform?>? GetAllPlatform();
        object GetAllPlatform();
        bool DeleteByKey(string key);
    }
}
