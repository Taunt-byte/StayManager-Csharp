using HotelApp.Forms;
using HotelApp.Services;

namespace HotelApp;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(new HotelService()));
    }
}
