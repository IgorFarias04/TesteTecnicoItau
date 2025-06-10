namespace TesteTecnicoItau.Entities;

public class Posicao
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int AtivoId { get; set; }
    public int Qtd { get; set; }
    public decimal PrecoMedio { get; set; }
    public decimal Pl { get; set; }
    public int Quantidade { get; set; }
}