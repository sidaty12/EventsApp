using EventsApp.Configuration;
using EventsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("createOrganisateur")]
        public async Task<IActionResult> CreateOrganisateur([FromBody] RegisterModel registerModel) 
        {
            if (await _userManager.FindByEmailAsync(registerModel.Email) != null)
            {
                return BadRequest("Email already exists");
            }

            var user = new ApplicationUser { Email = registerModel.Email,
                UserName  = registerModel.Name,
                PasswordHash = registerModel.Password
                
            };
           

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assuming you have an "OrganisationAdmin" role
            if (!await _roleManager.RoleExistsAsync("Organisateur"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Organisateur"));
            }

            await _userManager.AddToRoleAsync(user, "Organisateur");

            return Ok(new { Message = "Organisateur created successfully" });
        }

        [HttpPost("createParticipant")]
        public async Task<IActionResult> CreateParticipant([FromBody] RegisterModel registerModel)
        {
            if (await _userManager.FindByEmailAsync(registerModel.Email) != null)
            {
                return BadRequest("Email already exists");
            }

            var user = new ApplicationUser
            {
                Email = registerModel.Email,
                UserName = registerModel.Name,
                PasswordHash = registerModel.Password

            };


            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assuming you have an "Participant" role
            if (!await _roleManager.RoleExistsAsync("Participant"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Participant"));
            }

            await _userManager.AddToRoleAsync(user, "Participant");

            return Ok(new { Message = "Participant created successfully" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {

            // var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            // Étape 1: Trouver l'utilisateur par email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Utilisateur non trouvé.");
            }

            // Étape 2: Vérifier le mot de passe
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordCorrect)
            {
                return BadRequest("Mot de passe incorrect.");
            }

            // Étape 3: Connecter l'utilisateur manuellement
            await _signInManager.SignInAsync(user, isPersistent: false); // Mettez isPersistent à true si vous voulez que la session persiste entre les fermetures du navigateur

           

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        //Une fois l'utilisateur logé, on génnère un token d'authentification
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (_jwtSettings.Value.SecretKey == null)
            {
                throw new ArgumentNullException("SecretKey", "SecretKey is null");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Set token expiration to 10 days
            var expires = DateTime.Now.AddMinutes(10);

            var token = new JwtSecurityToken(
                _jwtSettings.Value.Issuer,
                _jwtSettings.Value.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

}
