using BusinessExpenseTracker.Contracts;
using BusinessExpenseTracker.ViewModels;

namespace BusinessExpenseTracker.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ILifecycleAware)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        (BindingContext as ILifecycleAware)?.OnDisappearing();
        base.OnDisappearing();
    }
}
