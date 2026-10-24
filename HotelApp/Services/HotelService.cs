using HotelApp.Data;
using HotelApp.Models;

namespace HotelApp.Services;

/// <summary>Regras de negócio do hotel. Erros são lançados como InvalidOperationException.</summary>
public class HotelService
{
    private readonly JsonRepository<Hospede> _hospedes = new("hospedes.json");
    private readonly JsonRepository<Quarto> _quartos = new("quartos.json");
    private readonly JsonRepository<Hospedagem> _hospedagens = new("hospedagens.json");

    public IEnumerable<Hospede> Hospedes => _hospedes.Todos().OrderBy(h => h.Nome);
    public IEnumerable<Quarto> Quartos => _quartos.Todos().OrderBy(q => q.Numero);
    public IEnumerable<Hospedagem> Hospedagens => _hospedagens.Todos().OrderByDescending(h => h.CheckIn);

    public Hospede? ObterHospede(int id) => _hospedes.Todos().FirstOrDefault(h => h.Id == id);
    public Quarto? ObterQuarto(int id) => _quartos.Todos().FirstOrDefault(q => q.Id == id);
    public Hospedagem? ObterHospedagem(int id) => _hospedagens.Todos().FirstOrDefault(h => h.Id == id);

    public IEnumerable<Hospede> HospedesSemHospedagemAtiva() =>
        Hospedes.Where(h => !_hospedagens.Todos().Any(x => x.HospedeId == h.Id && x.Ativa));

    public IEnumerable<Quarto> QuartosDisponiveis() =>
        Quartos.Where(q => q.Status == StatusQuarto.Disponível);

    // ---------- Hóspedes ----------
    public static string SomenteDigitos(string s) => new(s.Where(char.IsDigit).ToArray());

    public void SalvarHospede(Hospede h)
    {
        h.Nome = h.Nome.Trim();
        h.Cpf = SomenteDigitos(h.Cpf);
        h.Telefone = h.Telefone.Trim();
        h.Email = h.Email.Trim();

        if (h.Nome.Length < 3) throw Erro("Informe o nome do hóspede.");
        if (!CpfValido(h.Cpf)) throw Erro("CPF inválido.");
        if (h.Email.Length > 0 && !h.Email.Contains('@')) throw Erro("E-mail inválido.");
        if (_hospedes.Todos().Any(x => x.Cpf == h.Cpf && x.Id != h.Id))
            throw Erro("Já existe um hóspede com este CPF.");

        _hospedes.Salvar(h);
    }

    public void RemoverHospede(int id)
    {
        if (_hospedagens.Todos().Any(x => x.HospedeId == id && x.Ativa))
            throw Erro("Este hóspede está hospedado no momento.");
        _hospedes.Remover(id);
    }

    // ---------- Quartos ----------
    public void SalvarQuarto(Quarto q)
    {
        if (q.Numero <= 0) throw Erro("Número do quarto inválido.");
        if (q.Capacidade < 1) throw Erro("A capacidade deve ser de pelo menos 1 pessoa.");
        if (q.Diaria <= 0) throw Erro("Informe o valor da diária.");
        if (_quartos.Todos().Any(x => x.Numero == q.Numero && x.Id != q.Id))
            throw Erro("Já existe um quarto com este número.");

        var atual = ObterQuarto(q.Id);
        if (q.Status == StatusQuarto.Ocupado && atual?.Status != StatusQuarto.Ocupado)
            throw Erro("Para ocupar um quarto, faça o check-in na aba Hospedagens.");
        if (atual?.Status == StatusQuarto.Ocupado && q.Status != StatusQuarto.Ocupado)
            throw Erro("O quarto está ocupado. Faça o check-out antes de alterar o status.");

        _quartos.Salvar(q);
    }

    public void RemoverQuarto(int id)
    {
        if (ObterQuarto(id)?.Status == StatusQuarto.Ocupado)
            throw Erro("O quarto está ocupado.");
        _quartos.Remover(id);
    }

    // ---------- Hospedagens ----------
    public void FazerCheckIn(int hospedeId, int quartoId)
    {
        var quarto = ObterQuarto(quartoId) ?? throw Erro("Quarto não encontrado.");
        if (ObterHospede(hospedeId) == null) throw Erro("Hóspede não encontrado.");
        if (quarto.Status != StatusQuarto.Disponível) throw Erro("O quarto não está disponível.");
        if (_hospedagens.Todos().Any(x => x.HospedeId == hospedeId && x.Ativa))
            throw Erro("Este hóspede já possui uma hospedagem ativa.");

        var atualizado = quarto.Clonar();
        atualizado.Status = StatusQuarto.Ocupado;
        _quartos.Salvar(atualizado);

        _hospedagens.Salvar(new Hospedagem
        {
            HospedeId = hospedeId,
            QuartoId = quartoId,
            CheckIn = DateTime.Now,
            Diaria = quarto.Diaria
        });
    }

    /// <returns>Valor total da estadia.</returns>
    public decimal FazerCheckOut(int hospedagemId)
    {
        var h = ObterHospedagem(hospedagemId) ?? throw Erro("Hospedagem não encontrada.");
        if (!h.Ativa) throw Erro("Esta hospedagem já foi encerrada.");

        var fim = DateTime.Now;
        var encerrada = new Hospedagem
        {
            Id = h.Id, HospedeId = h.HospedeId, QuartoId = h.QuartoId,
            CheckIn = h.CheckIn, Diaria = h.Diaria, CheckOut = fim
        };
        _hospedagens.Salvar(encerrada);

        var quarto = ObterQuarto(h.QuartoId);
        if (quarto != null && quarto.Status == StatusQuarto.Ocupado)
        {
            var liberado = quarto.Clonar();
            liberado.Status = StatusQuarto.Disponível;
            _quartos.Salvar(liberado);
        }
        return encerrada.CalcularTotal(fim);
    }

    // ---------- Util ----------
    private static InvalidOperationException Erro(string msg) => new(msg);

    public static bool CpfValido(string c)
    {
        if (c.Length != 11 || c.Distinct().Count() == 1) return false;
        for (int t = 9; t < 11; t++)
        {
            int soma = 0;
            for (int i = 0; i < t; i++) soma += (c[i] - '0') * (t + 1 - i);
            if ((soma * 10) % 11 % 10 != c[t] - '0') return false;
        }
        return true;
    }
}
