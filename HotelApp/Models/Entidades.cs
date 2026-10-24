using System.Text.Json.Serialization;

namespace HotelApp.Models;

public interface IEntidade { int Id { get; set; } }

public enum TipoQuarto { Solteiro, Casal, Suíte, Família }
public enum StatusQuarto { Disponível, Ocupado, Manutenção }

public class Hospede : IEntidade
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Cpf { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string Email { get; set; } = "";

    public Hospede Clonar() => (Hospede)MemberwiseClone();
    public override string ToString() => Nome;
}

public class Quarto : IEntidade
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public TipoQuarto Tipo { get; set; } = TipoQuarto.Casal;
    public int Capacidade { get; set; } = 2;
    public decimal Diaria { get; set; }
    public StatusQuarto Status { get; set; } = StatusQuarto.Disponível;

    public Quarto Clonar() => (Quarto)MemberwiseClone();
    public override string ToString() => $"{Numero} - {Tipo} ({Diaria:C2})";
}

public class Hospedagem : IEntidade
{
    public int Id { get; set; }
    public int HospedeId { get; set; }
    public int QuartoId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public decimal Diaria { get; set; } // valor da diária no momento do check-in

    [JsonIgnore] public bool Ativa => CheckOut == null;

    public decimal CalcularTotal(DateTime fim)
    {
        int dias = Math.Max(1, (int)Math.Ceiling((fim - CheckIn).TotalDays));
        return dias * Diaria;
    }
}
