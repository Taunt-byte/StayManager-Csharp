using HotelApp.Models;
using HotelApp.Services;

namespace HotelApp.Forms;

public class HospedeDialog : DialogBase
{
    private readonly HotelService _s;
    private readonly Hospede _h;
    private readonly TextBox _nome = new(), _cpf = new() { MaxLength = 14 }, _tel = new(), _email = new();

    public HospedeDialog(HotelService s, Hospede h) : base(h.Id == 0 ? "Novo hóspede" : "Editar hóspede")
    {
        _s = s; _h = h;
        _nome.Text = h.Nome; _cpf.Text = h.Cpf; _tel.Text = h.Telefone; _email.Text = h.Email;
        AddCampo("Nome:", _nome);
        AddCampo("CPF:", _cpf);
        AddCampo("Telefone:", _tel);
        AddCampo("E-mail:", _email);
        AddBotoes();
    }

    protected override void Confirmar()
    {
        _h.Nome = _nome.Text; _h.Cpf = _cpf.Text; _h.Telefone = _tel.Text; _h.Email = _email.Text;
        _s.SalvarHospede(_h);
    }
}
