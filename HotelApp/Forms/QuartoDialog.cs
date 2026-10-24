using HotelApp.Models;
using HotelApp.Services;

namespace HotelApp.Forms;

public class QuartoDialog : DialogBase
{
    private readonly HotelService _s;
    private readonly Quarto _q;
    private readonly NumericUpDown _num = new() { Minimum = 1, Maximum = 9999 };
    private readonly NumericUpDown _cap = new() { Minimum = 1, Maximum = 10 };
    private readonly NumericUpDown _diaria = new() { DecimalPlaces = 2, Maximum = 100000, Increment = 10 };
    private readonly ComboBox _tipo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _status = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public QuartoDialog(HotelService s, Quarto q) : base(q.Id == 0 ? "Novo quarto" : "Editar quarto")
    {
        _s = s; _q = q;
        _tipo.DataSource = Enum.GetValues<TipoQuarto>();
        _status.DataSource = Enum.GetValues<StatusQuarto>();

        _num.Value = Math.Max(1, q.Numero);
        _cap.Value = Math.Clamp(q.Capacidade, 1, 10);
        _diaria.Value = Math.Clamp(q.Diaria, 0, 100000);
        _tipo.SelectedItem = q.Tipo;
        _status.SelectedItem = q.Status;
        if (q.Status == StatusQuarto.Ocupado) _status.Enabled = false;

        AddCampo("Número:", _num);
        AddCampo("Tipo:", _tipo);
        AddCampo("Capacidade:", _cap);
        AddCampo("Diária (R$):", _diaria);
        AddCampo("Status:", _status);
        AddBotoes();
    }

    protected override void Confirmar()
    {
        _q.Numero = (int)_num.Value;
        _q.Tipo = (TipoQuarto)_tipo.SelectedItem!;
        _q.Capacidade = (int)_cap.Value;
        _q.Diaria = _diaria.Value;
        _q.Status = (StatusQuarto)_status.SelectedItem!;
        _s.SalvarQuarto(_q);
    }
}
