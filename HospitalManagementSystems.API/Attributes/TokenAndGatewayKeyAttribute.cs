using HospitalManagementSystem.Services.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace HospitalManagementSystems.API.Attributes
{
    public class TokenAndGatewayKeyAttribute : Attribute, IAsyncActionFilter
    {
        private readonly LoginService _loginService;
        public TokenAndGatewayKeyAttribute(LoginService loginService)
        {
            _loginService = loginService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var token = request.Headers["Authorization"].ToString();
            var gatewayKey = request.Headers["GatewayKey"].ToString();

            // Basic validation (replace with your actual logic)
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (string.IsNullOrEmpty(gatewayKey) || gatewayKey != "b29995f8-c911-4f55-9193-4a48ebcfbcae")
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var response = _loginService.ValidateToken(token);
            if (!response.IsValid)
            {
                throw new Exception("UnAuthorized User");
            }
            await next();
        }
    }
}
