using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTeste.entites;
using WebTeste.Models;
using WebTeste.Repositories;

namespace WebTeste.Controllers
{
    public class LoginController : Controller
    {
        private readonly conexao _conexao;

        public LoginController()
        {
            _conexao = new conexao();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logar(UsuarioModel UsuarioModel)
        {

            Criptografia cripto = new Criptografia();
            UsuarioModel.senha = cripto.Encript(UsuarioModel.senha);
            var retorno = _conexao.ConsultarUsuario(UsuarioModel.usuario, UsuarioModel.senha);
            try
            {
                if (ModelState.IsValid)
                {
                    if (retorno.Item1 == true)
                    {

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, retorno.Item2.usuario),
                        };

                        var UserIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        ClaimsPrincipal principal = new ClaimsPrincipal(UserIdentity);

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddHours(2),
                        });

                        TempData["MensagemErro"] = null;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Usuario nao  permitido";
                    }
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["MenssagemErro"] = $"Erro ao logar {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public void Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        public IActionResult CadastrarUsuario(UsuarioModel UsuarioModel)
        {
            Criptografia cripto = new Criptografia();
            UsuarioModel.senha = cripto.Encript(UsuarioModel.senha);
            _conexao.CadastrarUsuario(UsuarioModel);
            return View("Index");
        }
    }
}
