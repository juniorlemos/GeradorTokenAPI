using GeradorDeTokenAPI.Models;
using GeradorDeTokenAPI.Models.ViewModels;
using GeradorDeTokenAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorTokenAPI.Controllers
{
    

        [Authorize]
        [Route("api/[controller]")]
        [ApiController]
        public class AutorizacaoController : ControllerBase
        {


            private readonly IAutorizacaoService _service;


            public AutorizacaoController(IAutorizacaoService service)
            {

                _service = service;
            }

            [AllowAnonymous]

            [HttpPost("Registrar")]
            public async Task<ActionResult> RegistrarUsuario([FromBody] EntradaUsuario usuario)
            {
                var registro = await _service.RegistrarUsuario(usuario);


                if (!registro.Succeeded)
                {
                    return BadRequest(registro.Errors);
                }

                return Ok();
            }

            [AllowAnonymous]
            [HttpPost("login")]
            public async Task<ActionResult> LoginUsuario([FromBody] EntradaUsuario usuario)
            {


                var resultado = await _service.LogarUsuario(usuario);

                if (resultado)
                {
                    var resposta = await _service.GerarJwt(usuario.nome);

                    return Ok(resposta);
                }

                return BadRequest(new TelaToken
                {
                    Usuario = usuario.nome,
                    Autenticado = false,
                    Token = "Erro na criação do Token , login ou senha invalida",
                    DataDeExpiracaoEmMinutos = 0

                });
            }




            [HttpPost("ValidarSenha")]
            public async Task<ActionResult> ValidarSenha([FromBody] EntradaSenha senha)
            {

                var resultado = await _service.ValidarSenha(senha.senha);
                if (resultado)
                {
                    return Ok(new SenhaValida { senhaValida = true });
                }

                return Ok(new SenhaValida { senhaValida = false });

            }



            [HttpGet("CriarSenha")]
            public async Task<ActionResult> CriarSenha()
            {

                var novaSenha = await _service.CriarSenha();

                return Ok(novaSenha);
            }



        }

    }