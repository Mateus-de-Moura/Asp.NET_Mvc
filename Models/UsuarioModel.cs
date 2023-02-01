using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTeste.Models
{
    public class UsuarioModel
    {
        public int Id{ get; set; }
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o usuario")]
        public string usuario { get; set; }
        [Required(ErrorMessage ="Informe a senha")]
        public string senha { get; set; }

        public bool lembrar { get; set; }   
    }
}
