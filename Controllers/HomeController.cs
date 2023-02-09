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
using System.Security.Permissions;
using System.Threading.Tasks;
using WebTeste.entites;
using WebTeste.Models;
using WebTeste.ViewModel;

namespace WebTeste.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {

        private readonly conexao _conn;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _conn = new conexao();
            _logger = logger;
        }
        public IActionResult Index(string mes)
        {

            if (!string.IsNullOrEmpty(Request.Cookies["MyCookie"]))
            {
                @TempData["usuario"] = Request.Cookies["MyCookie"].Split('.')[0];
            }
            else
            {
              @TempData["usuario"] = Request.Cookies["UserNaoManterLog"].Split('.')[0];
            }

            var user = Request.Cookies["MyCookie"];
            var conta = new ContasViewModel();

            if (string.IsNullOrEmpty(mes))
            {
                if (!string.IsNullOrEmpty(Request.Cookies["MyCookie"]))
                {
                    string mesatual = DateTime.Now.Month.ToString();
                    conta.Contas = _conn.GetContas(mesatual, int.Parse(Request.Cookies["MyCookie"].Split('.')[1]));
                }
                else
                {
                    string mesatual = DateTime.Now.Month.ToString();
                    conta.Contas = _conn.GetContas(mesatual, int.Parse(Request.Cookies["UserNaoManterLog"].Split('.')[1]));
                }               
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.Cookies["MyCookie"]))
                {
                    conta.Contas = _conn.GetContas(mes, int.Parse(Request.Cookies["MyCookie"].Split('.')[1]));
                }
                else
                {
                    conta.Contas = _conn.GetContas(mes, int.Parse(Request.Cookies["UserNaoManterLog"].Split('.')[1]));
                }
               
            }
            return View(conta);
        }

        public IActionResult Details(int idConta)
        {
            @TempData["usuario"] = Request.Cookies["MyCookie"].Split('.')[0];

            var conta = _conn.GetContasPorID(idConta);
            return View(conta);
        }

        public IActionResult Delete(string id)
        {
            _conn.deleteConta(int.Parse(id));

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            string mesatual = DateTime.Now.Month.ToString();
            var conta = new ContasViewModel();
            conta.Contas = _conn.GetContas(mesatual, int.Parse(Request.Cookies["UserNaoManterLog"].Split('.')[1]));
            @TempData["usuario"] = Request.Cookies["MyCookie"].Split('.')[0];
            return View(conta);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] object conta)
        {
            try
            {
                var Conta = JsonConvert.DeserializeObject<Contas>(conta.ToString());
                _conn.Cadastrar(Conta, Request.Cookies["MyCookie"].Split('.')[1].ToString());
                return Json(true);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        
            //return RedirectToAction("Index");
        }
       
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

        public IActionResult AtualizarConta(Contas Conta)
        {
            _conn.AtualizarConta(Conta, int.Parse( Request.Cookies["MyCookie"].Split('.')[1]));           
            return RedirectToAction("Index","Home");

        }     
    }
}
