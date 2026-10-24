using HotelApp.Models;
using HotelApp.Services;

namespace HotelApp.Forms;

public class MainForm : Form
{
    private readonly HotelService _s;
    private readonly DataGridView _gH = NovaGrade(), _gQ = NovaGrade(), _gS = NovaGrade();
    private readonly Label _resumo = new()
    {
        Dock = DockStyle.Top, Height = 36, Padding = new Padding(8, 0, 0, 0),
        TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10, FontStyle.Bold)
    };

    public MainForm(HotelService s)
    {
        _s = s;
        Text = "Sistema de Hotel";
        Size = new Size(920, 560);
        MinimumSize = new Size(700, 400);
        StartPosition = FormStartPosition.CenterScreen;

        var abas = new TabControl { Dock = DockStyle.Fill };
        abas.TabPages.Add(CriarAba("Hóspedes", _gH,
            Botao("Novo", NovoHospede), Botao("Editar", EditarHospede), Botao("Excluir", ExcluirHospede)));
        abas.TabPages.Add(CriarAba("Quartos", _gQ,
            Botao("Novo", NovoQuarto), Botao("Editar", EditarQuarto), Botao("Excluir", ExcluirQuarto)));
        abas.TabPages.Add(CriarAba("Hospedagens", _gS,
            Botao("Novo check-in", NovoCheckIn), Botao("Check-out", FazerCheckOut)));

        Controls.Add(abas);
        Controls.Add(_resumo);
        Atualizar();
    }

    // ---------- Hóspedes ----------
    private void NovoHospede() => Abrir(new HospedeDialog(_s, new Hospede()));

    private void EditarHospede()
    {
        var h = Selecionado(_gH, _s.ObterHospede);
        if (h != null) Abrir(new HospedeDialog(_s, h.Clonar()));
    }

    private void ExcluirHospede()
    {
        var h = Selecionado(_gH, _s.ObterHospede);
        if (h != null && Confirmar($"Excluir o hóspede {h.Nome}?")) Executar(() => _s.RemoverHospede(h.Id));
    }

    // ---------- Quartos ----------
    private void NovoQuarto() => Abrir(new QuartoDialog(_s, new Quarto()));

    private void EditarQuarto()
    {
        var q = Selecionado(_gQ, _s.ObterQuarto);
        if (q != null) Abrir(new QuartoDialog(_s, q.Clonar()));
    }

    private void ExcluirQuarto()
    {
        var q = Selecionado(_gQ, _s.ObterQuarto);
        if (q != null && Confirmar($"Excluir o quarto {q.Numero}?")) Executar(() => _s.RemoverQuarto(q.Id));
    }

    // ---------- Hospedagens ----------
    private void NovoCheckIn() => Abrir(new CheckInDialog(_s));

    private void FazerCheckOut()
    {
        var h = Selecionado(_gS, _s.ObterHospedagem);
        if (h == null) return;
        if (!h.Ativa) { Aviso("Esta hospedagem já foi encerrada."); return; }
        if (!Confirmar("Confirmar o check-out?")) return;

        Executar(() =>
        {
            decimal total = _s.FazerCheckOut(h.Id);
            MessageBox.Show($"Check-out realizado.\nTotal da estadia: {total:C2}", "Check-out",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    // ---------- Atualização da tela ----------
    private void Atualizar()
    {
        Preencher(_gH, _s.Hospedes.Select(h => new
        {
            h.Id, h.Nome, CPF = FormatarCpf(h.Cpf), h.Telefone, h.Email
        }).ToList());

        Preencher(_gQ, _s.Quartos.Select(q => new
        {
            q.Id, Número = q.Numero, q.Tipo, q.Capacidade, Diária = q.Diaria.ToString("C2"), q.Status
        }).ToList());

        Preencher(_gS, _s.Hospedagens.Select(x => new
        {
            x.Id,
            Hóspede = _s.ObterHospede(x.HospedeId)?.Nome ?? "(removido)",
            Quarto = _s.ObterQuarto(x.QuartoId)?.Numero.ToString() ?? "(removido)",
            Entrada = x.CheckIn.ToString("dd/MM/yyyy HH:mm"),
            Saída = x.CheckOut?.ToString("dd/MM/yyyy HH:mm") ?? "Em andamento",
            Total = x.CalcularTotal(x.CheckOut ?? DateTime.Now).ToString("C2")
        }).ToList());

        var quartos = _s.Quartos.ToList();
        _resumo.Text = $"Quartos: {quartos.Count}   |   Disponíveis: {quartos.Count(q => q.Status == StatusQuarto.Disponível)}" +
                       $"   |   Ocupados: {quartos.Count(q => q.Status == StatusQuarto.Ocupado)}" +
                       $"   |   Hóspedes cadastrados: {_s.Hospedes.Count()}";
    }

    // ---------- Helpers de UI ----------
    private static DataGridView NovaGrade() => new()
    {
        Dock = DockStyle.Fill, ReadOnly = true, MultiSelect = false,
        AllowUserToAddRows = false, AllowUserToDeleteRows = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        RowHeadersVisible = false, BackgroundColor = SystemColors.Window
    };

    private static void Preencher<T>(DataGridView g, List<T> dados)
    {
        g.DataSource = dados;
        if (g.Columns.Contains("Id")) g.Columns["Id"]!.Visible = false;
    }

    private static Button Botao(string texto, Action acao)
    {
        var b = new Button { Text = texto, AutoSize = true, Padding = new Padding(8, 2, 8, 2) };
        b.Click += (_, _) => acao();
        return b;
    }

    private static TabPage CriarAba(string titulo, DataGridView grade, params Button[] botoes)
    {
        var barra = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 42, Padding = new Padding(6) };
        barra.Controls.AddRange(botoes);
        var aba = new TabPage(titulo);
        aba.Controls.Add(grade);
        aba.Controls.Add(barra);
        return aba;
    }

    private T? Selecionado<T>(DataGridView g, Func<int, T?> obter) where T : class
    {
        if (g.CurrentRow?.Cells["Id"].Value is int id) return obter(id);
        Aviso("Selecione um item na lista.");
        return null;
    }

    private void Abrir(Form dialogo)
    {
        using (dialogo)
            if (dialogo.ShowDialog(this) == DialogResult.OK) Atualizar();
    }

    private void Executar(Action acao)
    {
        try { acao(); Atualizar(); }
        catch (InvalidOperationException ex) { Aviso(ex.Message); }
    }

    private static bool Confirmar(string msg) =>
        MessageBox.Show(msg, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

    private static void Aviso(string msg) =>
        MessageBox.Show(msg, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    private static string FormatarCpf(string cpf) =>
        cpf.Length == 11 ? $"{cpf[..3]}.{cpf[3..6]}.{cpf[6..9]}-{cpf[9..]}" : cpf;
}
