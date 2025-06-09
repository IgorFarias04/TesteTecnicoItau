using TesteTecnicoItau.Entities;

public class Cotacao
{
    public int Id { get; set; }
    public int AtivoId { get; set; }
    public decimal PrecoUnit { get; set; }
    public DateTime DataHora { get; set; }


    //public Ativo? Ativo { get; set; }
}
