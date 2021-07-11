using System.Net;
using System.Threading.Tasks;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class Authorization : ControllerBase
    {
        readonly IAuthService Service;
        public Authorization(IAuthService service)
        {
            Service = service;
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(SignupResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Signup(SignupRequest request) => Ok(await Service.Signup(request));

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Login(LoginRequest request) => Ok(await Service.Login(request));
    }
}