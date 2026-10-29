
using BusinessExpenseTracker.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessExpenseTracker;

public partial class AppShell : Shell
{
    public AppShell(IServiceProvider services)
    {
        InitializeComponent();

        ((ShellContent)Items[0].Items[0].Items[0]).ContentTemplate =
            new DataTemplate(() =>
                services.GetRequiredService<MainPage>());

        Routing.RegisterRoute(
            nameof(ExpenseDetailPage),
            typeof(ExpenseDetailPage));
    }
}
