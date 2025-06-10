namespace TesteTecnicoItau.Entities
{
    public class Ativo
    {
        public int Id { get; set; }
        public string Cod { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoAtual { get; set; }


        public ICollection<Operacao> Operacoes { get; set; } = new List<Operacao>();
        public ICollection<Cotacao> Cotacoes { get; set; } = new List<Cotacao>();
        public ICollection<Posicao> Posicoes { get; set; } = new List<Posicao>();
        public string Codigo { get; internal set; }
    }
}