using BusinessExpenseTracker.Services;
using BusinessExpenseTracker.ViewModels;
using BusinessExpenseTracker.Views;

namespace BusinessExpenseTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<ExpenseDetailViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ExpenseDetailPage>();
        return builder.Build();
    }
}
