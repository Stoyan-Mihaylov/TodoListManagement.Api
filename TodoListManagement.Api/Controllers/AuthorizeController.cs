using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoListManagement.Api.Common.Configuration;
using TodoListManagement.Api.Models.Auth;

namespace TodoListManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizeController : ControllerBase
{
    private const string LoginError = "Invalid email or password";

    private readonly AuthOptions _options;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthorizeController(
        AuthOptions options,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [HttpPost]
    public async Task<IActionResult> AuthorizeAsync([FromBody] AuthorizeRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return BadRequest(new AuthorizeErrorResponse(LoginError));
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (!signInResult.Succeeded)
        {
            return BadRequest(new AuthorizeErrorResponse(LoginError));
        }

        var response = new AuthorizeResponse
        {
            AccessToken = await GetAccessTokenAsync(user),
            TokenType = "Bearer",
            ExpiresIn = _options.TokenLifetimeSeconds
        };

        return Ok(response);
    }

    private async Task<string> GetAccessTokenAsync(IdentityUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var signingKey = new SymmetricSecurityKey(_options.SecurityKeyAsBytes);
        var roles = await _userManager.GetRolesAsync(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(GetClaims(user, roles)),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Expires = DateTime.UtcNow.AddSeconds(_options.TokenLifetimeSeconds)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    private static IEnumerable<Claim> GetClaims(IdentityUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        if (roles != null && roles.Any())
        {
            claims.Add(new Claim("role", roles.FirstOrDefault()));
        }

        return claims;
    }
}
