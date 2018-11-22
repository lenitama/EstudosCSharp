using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeiraTarefa.Models
{
    public class Cliente
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; } 
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Pais { get; set; }
        public string Cep { get; set; }
        public string CpfOuCnpj { get; set; }
        public string Email { get; set; }
    }
}
