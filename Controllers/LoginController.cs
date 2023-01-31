using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    if (retorno == true)
                    {
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
