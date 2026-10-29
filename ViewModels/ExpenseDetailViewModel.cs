using System.Diagnostics;
using BusinessExpenseTracker.Contracts;
using BusinessExpenseTracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BusinessExpenseTracker.ViewModels;

public partial class ExpenseDetailViewModel : ObservableObject, IQueryAttributable, ILifecycleAware
{
    private readonly DatabaseService _database;
    [ObservableProperty] private int expenseId;
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string amount = string.Empty;
    [ObservableProperty] private string category = string.Empty;
    [ObservableProperty] private string dateText = string.Empty;

    public ExpenseDetailViewModel(DatabaseService database) => _database = database;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("id", out var raw) || !int.TryParse(raw?.ToString(), out var id)) return;
        ExpenseId = id;
        Preferences.Set("last_expense_id", id);
        _ = LoadExpenseAsync();
    }

    private async Task LoadExpenseAsync()
    {
        try
        {
            var expense = await _database.GetByIdAsync(ExpenseId);
            if (expense is null) return;
            Name = expense.Name;
            Amount = expense.Amount.ToString("F2");
            Category = expense.Category;
            DateText = expense.Date.ToString("dd.MM.yyyy HH:mm");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SQLite detail read error: {ex}");
            await Shell.Current.DisplayAlert("Помилка", "Не вдалося відкрити витрату.", "OK");
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Category) ||
            !decimal.TryParse(Amount, out var value) || value <= 0)
        {
            await Shell.Current.DisplayAlert("Перевірка", "Вкажіть назву, категорію та додатну суму.", "OK");
            return;
        }
        try
        {
            var expense = await _database.GetByIdAsync(ExpenseId);
            if (expense is null) return;
            expense.Name = Name.Trim();
            expense.Amount = value;
            expense.Category = Category.Trim();
            await _database.UpdateAsync(expense);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SQLite update error: {ex}");
            await Shell.Current.DisplayAlert("Помилка", "Не вдалося зберегти зміни.", "OK");
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        var confirm = await Shell.Current.DisplayAlert("Видалення", "Видалити цю витрату?", "Так", "Ні");
        if (!confirm) return;
        try
        {
            var expense = await _database.GetByIdAsync(ExpenseId);
            if (expense is null) return;
            await _database.DeleteAsync(expense);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"SQLite delete error: {ex}");
            await Shell.Current.DisplayAlert("Помилка", "Не вдалося видалити витрату.", "OK");
        }
    }

    public void OnAppearing() => Debug.WriteLine($"[VM] Detail.OnAppearing id={ExpenseId}");
    public void OnDisappearing() => Debug.WriteLine($"[VM] Detail.OnDisappearing id={ExpenseId}");
}
