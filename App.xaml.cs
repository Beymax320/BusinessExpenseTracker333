using System.Diagnostics;

namespace BusinessExpenseTracker;

public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Debug.WriteLine("[APP] CreateWindow");
        var window = new Window(new AppShell(_services));
        window.Created += (_, _) => Debug.WriteLine("[APP] OnStart / Created");
        window.Stopped += (_, _) => Debug.WriteLine("[APP] OnSleep / Stopped");
        window.Resumed += (_, _) => Debug.WriteLine("[APP] OnResume / Resumed");
        return window;
    }
}
