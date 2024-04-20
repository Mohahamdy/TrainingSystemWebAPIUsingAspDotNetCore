using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Day02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            if(userName == "mohamed" && password == "123")
            {
                //claims
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("Role","Admin"),
                    new Claim(ClaimTypes.Email,"Test@tst.com")
                };

                //credintials
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hello From The Other Side ya Brother"));
                var credintials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credintials
                    );

                var finalToken = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(finalToken);
            }

            return Unauthorized();
        }


    }
}
