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
        public IActionResult Index()
        {
            var user  = Request.Cookies["MyCookie"];
            if (user == null)
            {
                return RedirectToAction("Index","Login");
            }
            else
            {
                @TempData["usuario"] = Request.Cookies["MyCookie"].Split('.')[0];
                var contas = _conn.GetContas();
                return View(contas);
            }            
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
            Response.Cookies.Append("Conta", conta.ToString() , cookieOptions);

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
        public JsonResult DeletarConta([FromBody]object id)
        {
            var Conta = JsonConvert.DeserializeObject<dynamic>(id.ToString());
            string teste = Conta.ToString();
            
            _conn.deleteConta(teste.Split(":")[1]);
            
            return Json(true);
        }
    }
}
