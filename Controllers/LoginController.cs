using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
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
            var nome = Request.Cookies["MyCookie"];
            if (nome == null)
            {
                return View();
            }
            else
            {               
                create_cookie_auth(Request.Cookies["MyCookie"].Split('.')[0]);
                return RedirectToAction("Index", "Home");
            }
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
                        create_cookie_auth(retorno.Item2.Nome);
                        Create_Cookie("UserNaoManterLog", retorno.Item2.Nome, retorno.Item2.Id.ToString());

                        if (UsuarioModel.lembrar)
                        {
                            Create_Cookie("MyCookie", retorno.Item2.Nome, retorno.Item2.Id.ToString());
                        };
                      
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

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(-10),
                HttpOnly = true,
            };
            Response.Cookies.Append("MyCookie", "", cookieOptions);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CadastrarUsuario(UsuarioModel UsuarioModel)
        {
            Criptografia cripto = new Criptografia();
            UsuarioModel.senha = cripto.Encript(UsuarioModel.senha);
            _conexao.CadastrarUsuario(UsuarioModel);
            return View("Index");
        }

        public void create_cookie_auth(string nome)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nome),                
            };

            var UserIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principal = new ClaimsPrincipal(UserIdentity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddHours(2),
            });
        }

        public void Create_Cookie(string NomeCookie,string nome, string id)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(10),
                HttpOnly = true,
            };
            Response.Cookies.Append(NomeCookie, nome + "." + id, cookieOptions);
        }
    }
}
