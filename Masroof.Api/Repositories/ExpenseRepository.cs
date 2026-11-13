using Masroof.Api.Models;
using MongoDB.Driver;

namespace Masroof.Api.Repositories;
public class ExpenseRepository
{
    private readonly IMongoCollection<Expense> _collection;

    public ExpenseRepository(IMongoDatabase db)
    {
        _collection = db.GetCollection<Expense>("expenses");
    }

    public async Task<Expense> CreateAsync(Expense expense)
    {
        await _collection.InsertOneAsync(expense);
        return expense;
    }
}