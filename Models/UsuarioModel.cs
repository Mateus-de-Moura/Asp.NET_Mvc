using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTeste.Models
{
    public class UsuarioModel
    {
        [Required(ErrorMessage = "Informe o usuario")]
        public string usuario { get; set; }
        [Required(ErrorMessage ="Informe a senha")]
        public string senha { get; set; }
    }
}
