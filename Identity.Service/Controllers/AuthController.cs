using Identity.Service.Dtos;
using Identity.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Service.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDto dto)
        {
            //HomeWork: Conseider moving into own dedicated mapper class
            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            _logger.LogInformation("User registration attempt for {Email}", dto.Email);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LogonDto dto)
        {
            var getUser = await _userManager.FindByEmailAsync(dto.Email);
            if (getUser is null || !await _userManager.CheckPasswordAsync(getUser, dto.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            var token = GenerateToken(getUser);
            _logger.LogInformation("User login successful for {Email}", dto.Email);
            return Ok(new { token });
        }

        private string GenerateToken(ApplicationUser getUser)
        {
            var claimdata = new[]
            {
             new Claim(JwtRegisteredClaimNames.Sub,getUser.Email),
             new Claim(ClaimTypes.Name,getUser.UserName),
             new Claim("uid",getUser.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claimdata,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_configuration["JWT:DurationInMinutes"]
                )),
                signingCredentials: cred
                );
            _logger.LogInformation($"JWT Token generated for user {getUser.Email}");

            var tokenData = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenData;

        }
    }
}