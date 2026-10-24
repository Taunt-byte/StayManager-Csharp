using HotelApp.Models;
using HotelApp.Services;

namespace HotelApp.Forms;

public class CheckInDialog : DialogBase
{
    private readonly HotelService _s;
    private readonly ComboBox _hospede = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _quarto = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public CheckInDialog(HotelService s) : base("Novo check-in")
    {
        _s = s;
        _hospede.DataSource = s.HospedesSemHospedagemAtiva().ToList();
        _quarto.DataSource = s.QuartosDisponiveis().ToList();
        AddCampo("Hóspede:", _hospede);
        AddCampo("Quarto:", _quarto);
        AddBotoes();
    }

    protected override void Confirmar()
    {
        if (_hospede.SelectedItem is not Hospede h) throw new InvalidOperationException("Não há hóspedes disponíveis para check-in.");
        if (_quarto.SelectedItem is not Quarto q) throw new InvalidOperationException("Não há quartos disponíveis.");
        _s.FazerCheckIn(h.Id, q.Id);
    }
}
