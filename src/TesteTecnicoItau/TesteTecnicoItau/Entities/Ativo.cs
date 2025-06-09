using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteTecnicoItau.Entities
{
    public class Ativo
    {
        public int Id { get; set; }
        public string Cod { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;

        public ICollection<Operacao> Operacoes { get; set; } = new List<Operacao>();
        public ICollection<Cotacao> Cotacoes { get; set; } = new List<Cotacao>();
        public ICollection<Posicao> Posicoes { get; set; } = new List<Posicao>();
    }
}
