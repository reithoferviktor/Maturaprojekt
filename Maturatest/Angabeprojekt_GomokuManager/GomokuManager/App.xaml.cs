using System.Windows;
using GomokuManager.Models;

namespace GomokuManager;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        GomokuDb.Init();
    }
}
