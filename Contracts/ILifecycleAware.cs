namespace BusinessExpenseTracker.Contracts;

public interface ILifecycleAware
{
    void OnAppearing();
    void OnDisappearing();
}
