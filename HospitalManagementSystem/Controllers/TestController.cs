using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    [TokenAndGatewayKey]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("Index")]
        public string Index()
        {
            return "Hello";
        }
    }
}
