using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteTecnicoItau.Entities
{
    public class Operacao
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int AtivoId { get; set; }
        public int Qtd { get; set; }
        public decimal PrecoUnit { get; set; }
        public string TipoOp { get; set; } = string.Empty; // COMPRA ou VENDA
        public decimal Corretagem { get; set; }
        public DateTime DataHora { get; set; }

        public Usuario Usuario { get; set; } = null!;
        public Ativo Ativo { get; set; } = null!;
    }
}
