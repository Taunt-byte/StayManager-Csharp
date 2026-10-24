namespace HotelApp.Forms;

/// <summary>Base para os diálogos de cadastro: monta rótulo + campo e os botões Salvar/Cancelar.</summary>
public abstract class DialogBase : Form
{
    private int _y = 12;

    protected DialogBase(string titulo)
    {
        Text = titulo;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
    }

    protected void AddCampo(string rotulo, Control campo)
    {
        Controls.Add(new Label { Text = rotulo, Left = 12, Top = _y + 4, Width = 110 });
        campo.Left = 130; campo.Top = _y; campo.Width = 230;
        Controls.Add(campo);
        _y += 32;
    }

    protected void AddBotoes()
    {
        var salvar = new Button { Text = "Salvar", Left = 190, Top = _y + 8, Width = 80 };
        var cancelar = new Button { Text = "Cancelar", Left = 280, Top = _y + 8, Width = 80, DialogResult = DialogResult.Cancel };
        salvar.Click += (_, _) =>
        {
            try { Confirmar(); DialogResult = DialogResult.OK; }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        };
        AcceptButton = salvar;
        CancelButton = cancelar;
        Controls.AddRange(new Control[] { salvar, cancelar });
        ClientSize = new Size(380, _y + 48);
    }

    /// <summary>Valida e salva. Lance uma exceção para manter o diálogo aberto.</summary>
    protected abstract void Confirmar();
}
