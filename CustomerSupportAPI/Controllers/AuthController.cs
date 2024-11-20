using CustomerSupportAPI.Data;
using CustomerSupportAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CustomerSupportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)

        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;

        }


        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        //IsApproved = model.Role == "Admin" ? true : false

                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(model.Role))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(model.Role));
                        }

                        await _userManager.AddToRoleAsync(user, model.Role);
                        return Ok(model);
                    }

                    return BadRequest(result.Errors);
                }
                return BadRequest(model);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        //    [HttpPost("login")]
        //    public async Task<IActionResult> Login([FromBody] LoginModel login)
        //    {
        //        var user = await _userManager.FindByNameAsync(login.UserName);
        //        if(user == null)
        //        {
        //            return Unauthorized("User is null here");
        //        }
        //        var loginResult = await _userManager.CheckPasswordAsync(user, login.Password);

        //        if (!loginResult)
        //        {
        //            return Unauthorized("Password is not correct");
        //        }

        //        var userRoles = await _userManager.GetRolesAsync(user);

        //        //if (user != null && loginResult)
        //        //{
        //            var authClaims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Name, user.UserName),
        //                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                new Claim(ClaimTypes.NameIdentifier, user.Id)
        //            };

        //        foreach (var role in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, role));
        //        }

        //            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@123"));
        //            var token = new JwtSecurityToken(
        //                 issuer: "http://localhost:4200",
        //                audience: "http://localhost:4200",
        //                expires: DateTime.Now.AddHours(3),
        //                claims: authClaims,
        //                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //            );

        //            return Ok(new
        //            {
        //                token = new JwtSecurityTokenHandler().WriteToken(token),
        //                expiration = token.ValidTo
        //            });
        //        }

        //        //return Unauthorized();
        //   // }
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                return Unauthorized("User is null here");
            }

            var loginResult = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!loginResult)
            {
                return Unauthorized("Password is not correct");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id) // Use user.Id here, not UserName
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@12"));

            var token = new JwtSecurityToken(
                issuer: "http://localhost:4200",
                audience: "http://localhost:4200",
                expires: DateTime.Now.AddHours(4),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
