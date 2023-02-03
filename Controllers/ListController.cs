using Microsoft.AspNetCore.Mvc;

namespace WebTeste.Controllers
{
    public class ListController : Controller
    {
        public string Index([FromBody] object obj)
        {
            return "testando as rotas";
        }
    }
}
