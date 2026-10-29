using System.Collections.ObjectModel;
using System.Diagnostics;
using BusinessExpenseTracker.Contracts;
using BusinessExpenseTracker.Models;
using BusinessExpenseTracker.Services;
using BusinessExpenseTracker.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;

namespace BusinessExpenseTracker.ViewModels;

public partial class MainViewModel : ObservableObject, ILifecycleAware
{
    private readonly DatabaseService _database;
    private const string DraftNameKey = "draft_expense_name";
    private const string DraftAmountKey = "draft_expense_amount";
    private const string DraftCategoryKey = "draft_expense_category";

    [ObservableProperty] private string expenseName = string.Empty;
    [ObservableProperty] private string amount = string.Empty;
    [ObservableProperty] private string category = string.Empty;
    [ObservableProperty] private Expense? selectedExpense;

    public ObservableCollection<Expense> Expenses { get; } = new();

    public MainViewModel(DatabaseService database)
    {
        _database = database;
        RestoreDraft();
    }

    [RelayCommand]
    private async Task LoadExpensesAsync()
    {
        try
        {
            var items = await _database.GetAllAsync();
            Expenses.Clear();
            foreach (var item in items) Expenses.Add(item);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SQLite read error: {ex}");
            await Shell.Current.DisplayAlert("Помилка", "Не вдалося завантажити витрати.", "OK");
        }
    }

    [RelayCommand]
    private async Task AddExpense()
    {
        if (string.IsNullOrWhiteSpace(ExpenseName) || string.IsNullOrWhiteSpace(Category) ||
            !decimal.TryParse(Amount, out var value) || value <= 0)
        {
            await Shell.Current.DisplayAlert("Перевірка", "Вкажіть назву, категорію та додатну суму.", "OK");
            return;
        }
        try
        {
            await _database.InsertAsync(new Expense
            {
                Name = ExpenseName.Trim(), Amount = value,
                Category = Category.Trim(), Date = DateTime.Now
            });
            ExpenseName = Amount = Category = string.Empty;
            SaveDraft();
            await LoadExpensesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SQLite insert error: {ex}");
            await Shell.Current.DisplayAlert("Помилка", "Не вдалося додати витрату.", "OK");
        }
    }

    [RelayCommand]
    private async Task OpenExpense(Expense? expense)
    {
        if (expense is null) return;
        SelectedExpense = null;
        await Shell.Current.GoToAsync($"{nameof(ExpenseDetailPage)}?id={expense.Id}");
    }

    public void OnAppearing()
    {
        RestoreDraft();
        _ = LoadExpensesAsync();
    }

    public void OnDisappearing() => SaveDraft();

    private void SaveDraft()
    {
        Preferences.Set(DraftNameKey, ExpenseName ?? string.Empty);
        Preferences.Set(DraftAmountKey, Amount ?? string.Empty);
        Preferences.Set(DraftCategoryKey, Category ?? string.Empty);
    }

    private void RestoreDraft()
    {
        ExpenseName = Preferences.Get(DraftNameKey, string.Empty);
        Amount = Preferences.Get(DraftAmountKey, string.Empty);
        Category = Preferences.Get(DraftCategoryKey, string.Empty);
    }
}
