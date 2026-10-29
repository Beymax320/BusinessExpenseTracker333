using BusinessExpenseTracker.Models;
using SQLite;

namespace BusinessExpenseTracker.Services;

public sealed class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;
    private readonly Task _initialization;

    public DatabaseService()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "expenses.db3");
        _database = new SQLiteAsyncConnection(path);
        _initialization = _database.CreateTableAsync<Expense>();
    }

    public async Task<List<Expense>> GetAllAsync()
    {
        await _initialization;
        return await _database.Table<Expense>().OrderByDescending(x => x.Id).ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(int id)
    {
        await _initialization;
        return await _database.Table<Expense>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> InsertAsync(Expense item)
    {
        await _initialization;
        return await _database.InsertAsync(item);
    }

    public async Task<int> UpdateAsync(Expense item)
    {
        await _initialization;
        return await _database.UpdateAsync(item);
    }

    public async Task<int> DeleteAsync(Expense item)
    {
        await _initialization;
        return await _database.DeleteAsync(item);
    }
}
