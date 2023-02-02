using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
using WebTeste.ViewModel;

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

        [AllowAnonymous]
        public IActionResult Index(string mes)
        {
            
            @TempData["usuario"] = Request.Cookies["MyCookie"].Split('.')[0];           
                
            var user = Request.Cookies["MyCookie"];
            var conta = new ContasViewModel();

            if (string.IsNullOrEmpty(mes))
            {
                
                conta.Contas = _conn.GetContas("1");
            }
            else
            {               
                 conta.Contas = _conn.GetContas(mes);
            }

           
            return View(conta);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Privacy()
        {
            @TempData["usuario"] = Request.Cookies["MyCookie"].Split('.')[0];
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
            _conn.Cadastrar(conta, Request.Cookies["MyCookie"].Split('.')[1].ToString());
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public JsonResult Add([FromBody] object conta)
        {
            if (conta != null)
            {
                var Conta = JsonConvert.DeserializeObject<Contas>(conta.ToString());

                _conn.Cadastrar(Conta, Request.Cookies["MyCookie"].Split('.')[1].ToString());
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public JsonResult getconta([FromBody] object id)
        {
            var Conta = JsonConvert.DeserializeObject<dynamic>(id.ToString());
            string teste = Conta.ToString();
            var conta = _conn.GetContasPorID(teste.Split(":")[1]);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1),
                HttpOnly = false,
            };
            Response.Cookies.Append("Conta", conta.ToString(), cookieOptions);

            return Json(conta);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult AtualizarConta()
        {
            return Ok();

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public JsonResult DeletarConta([FromBody] object id)
        {
            var Conta = JsonConvert.DeserializeObject<dynamic>(id.ToString());
            string teste = Conta.ToString();

            _conn.deleteConta(teste.Split(":")[1]);

            return Json(true);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult ListarPormes([FromBody] object id)
        {
            var Conta = JsonConvert.DeserializeObject<dynamic>(id.ToString());
            string teste = Conta.ToString();

            TempData["Mes"] = teste.Split(":")[1];
            @TempData["usuario"] = Request.Cookies["MyCookie"].Split('.')[0];
            var contas = _conn.GetContas(TempData["Mes"].ToString());
            RedirectToAction("Index", contas);
            //return View("Index",contas);
            return null;
            //return View("Index", contas);
           
            
            
        }
    }
}
