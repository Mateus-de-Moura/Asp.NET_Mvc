using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTeste.Models
{
    public class Contas
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; }
        public string Situacao { get; set; }
    }
}
