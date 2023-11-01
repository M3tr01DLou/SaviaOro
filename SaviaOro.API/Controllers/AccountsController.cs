﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SaviaOro.API.Helpers;
using SaviaOro.Shared.DTOs;
using SaviaOro.Shared.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SaviaOro.API.Controllers
{
	[ApiController]
	[Route("/api/accounts")]

	public class AccountsController : ControllerBase
	{
		private readonly IUserHelper _userHelper;
		private readonly IConfiguration _configuration;
        private readonly IImageHelper _imageHelper;
        private readonly string _container;

        public AccountsController(IUserHelper userHelper, IConfiguration configuration, IImageHelper imageHelper)
		{
			_userHelper = userHelper;
			_configuration = configuration;
            _imageHelper = imageHelper;
            _container = "users";
        }

		[HttpPost("createUser")]
		public async Task<ActionResult> CreateUser([FromBody] UserDTO model)
		{
			User user = model;
            if (!string.IsNullOrEmpty(model.Photo))
            {
                var photoUser = Convert.FromBase64String(model.Photo);
                model.Photo = _imageHelper.UploadImageAsync(photoUser, _container);
            }

            var result = await _userHelper.AddUserAsync(user, model.Password);
			if (result.Succeeded)
			{
				await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
				return Ok(BuildToken(user));
			}

			if(result.Errors.FirstOrDefault().Code.ToLower().Contains("duplicate"))
			{
                return BadRequest("El email que intenta registrar ya existe. Por favor, pruebe con otro email.");
            }

			return BadRequest(result.Errors.FirstOrDefault());
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login([FromBody] LoginDTO model)
		{
			var result = await _userHelper.LoginAsync(model);
			if (result.Succeeded)
			{
				var user = await _userHelper.GetUserAsync(model.Email);
				return Ok(BuildToken(user));
			}

			return BadRequest("Email o contraseña incorrectos.");
		}

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.Photo))
                {
                    var photoUser = Convert.FromBase64String(user.Photo);
                    user.Photo = _imageHelper.UploadImageAsync(photoUser, _container);
                }

                var currentUser = await _userHelper.GetUserAsync(user.Email!);
                if (currentUser == null)
                {
                    return NotFound();
                }

                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? user.Photo : currentUser.Photo;

                var result = await _userHelper.UpdateUserAsync(currentUser);
                if (result.Succeeded)
                {
                    //return NoContent();
                    return Ok(BuildToken(currentUser));
                }

                return BadRequest(result.Errors.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Get()
        {
            return Ok(await _userHelper.GetUserAsync(User.Identity!.Name!));
        }

        private TokenDTO BuildToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Email!),
				new Claim(ClaimTypes.Role, user.UserType.ToString()),
				new Claim("FirstName", user.FirstName),
				new Claim("LastName", user.LastName),
				new Claim("Photo", user.PhotoFullPath ?? string.Empty),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiration = DateTime.UtcNow.AddDays(30);
			var token = new JwtSecurityToken(
				issuer: null,
				audience: null,
				claims: claims,
				expires: expiration,
				signingCredentials: credentials);

			return new TokenDTO
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = expiration
			};
		}

	}
}
