using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteTecnicoItau.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal PercCorretagem { get; set; }

        public ICollection<Operacao> Operacoes { get; set; } = new List<Operacao>();
        public ICollection<Posicao> Posicoes { get; set; } = new List<Posicao>();
    }
}
