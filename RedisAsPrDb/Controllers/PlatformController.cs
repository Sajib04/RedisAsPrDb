using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisAsPrDb.Data;
using RedisAsPrDb.Models;
using System.Collections.Generic;

namespace RedisAsPrDb.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private IPlatormRepo _platormRepo;

        public PlatformController(IPlatormRepo platormRepo)
        {
            _platormRepo = platormRepo;
        }

        [HttpPost]
        [Route("Platform/CreatePlatform")]
        public ActionResult<Platform> CreatePlatform(Platform platform)
        {
            _platormRepo.CreatePlatform(platform);
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id }, platform);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        [Route("Platform/GetPlatformById/{Id}")]
        public ActionResult<IEnumerable<Platform>> GetPlatformById(string id)
        {

            var platform = _platormRepo.GetPlatformById(id);

            if (platform != null)
            {
                return Ok(platform);
            }

            return NotFound();
        }

        

        [HttpGet]
        [Route("Platform/GetAll")]
        public ActionResult<IEnumerable<Platform>> GetAllPlatforms()
        {
            return Ok(_platormRepo.GetAllPlatform());
        }

        [HttpPost]
        [Route("Platform/DeleteKey/{Id}")]
        public ActionResult DeleteKey(string id)
        {
            return Ok();
        }

        //[HttpPost]
        //[Route("Platform/DeleteAllKey")]


    }
}
