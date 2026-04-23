﻿﻿﻿using Microsoft.AspNetCore.Mvc;
using Voxel.Application.DTOs;
using Voxel.Application.Interfaces;
using System.Threading.Tasks;

namespace Voxel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);

            if (response == null)
                return Unauthorized(new { mensagem = "Usuário ou senha inválidos" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var sucesso = await _authService.RegisterAsync(request);
            if (!sucesso)
                return BadRequest(new { mensagem = "Erro ao cadastrar. Verifique os dados ou o aceite dos termos." });

            return Ok(new { mensagem = "Cadastro realizado com sucesso!" });
        }
    }
}
