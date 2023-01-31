using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebTeste.entites;
using WebTeste.Models;


namespace WebTeste.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly conexao _conn;


        private readonly ILogger<HomeController> _logger;
       
        public HomeController(ILogger<HomeController> logger)
        {
            _conn = new conexao();
            _logger = logger;
        }
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {       
            var contas = _conn.GetContas();
            return View(contas);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }     

        [HttpPost]
        
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Adicionar(Contas conta)
        {
            _conn.Cadastrar(conta);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public JsonResult Add([FromBody] object conta)
        {
            if (conta != null)
            {
                var Conta = JsonConvert.DeserializeObject<Contas>(conta.ToString());

                _conn.Cadastrar(Conta);
                return Json(true);
            }
            return Json(false);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult AtualizarConta()
        {
            return Ok();

        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult DeletarConta()
        {
            return Ok();
        }
    }
}
