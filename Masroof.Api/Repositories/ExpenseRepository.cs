using Masroof.Api.Models;
using MongoDB.Bson;
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

    public async Task<List<Expense>> GetAllAsync(string tenantId)
    {
        return await _collection
            .Find(e => e.TenantId == tenantId)
            .SortByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(string tenantId, string id)
    {
        if (!ObjectId.TryParse(id, out _)) return null;

        return await _collection
            .Find(e => e.TenantId == tenantId && e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Expense?> UpdateAsync(
        string tenantId,
        string id,
        string title,
        decimal amount,
        string currency,
        string category,
        DateTime date)
    {
        if (!ObjectId.TryParse(id, out _)) return null;

        var update = Builders<Expense>.Update
            .Set(e => e.Title, title)
            .Set(e => e.Amount, amount)
            .Set(e => e.Currency, currency)
            .Set(e => e.Category, category)
            .Set(e => e.Date, date);

        return await _collection.FindOneAndUpdateAsync(
            e => e.TenantId == tenantId && e.Id == id,
            update,
            new FindOneAndUpdateOptions<Expense> { ReturnDocument = ReturnDocument.After });
    }

    public async Task<bool> DeleteAsync(string tenantId, string id)
    {
        if (!ObjectId.TryParse(id, out _)) return false;

        var result = await _collection.DeleteOneAsync(e => e.TenantId == tenantId && e.Id == id);
        return result.DeletedCount > 0;
    }
}
