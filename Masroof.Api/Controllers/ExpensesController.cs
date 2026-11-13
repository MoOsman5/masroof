using System.ComponentModel.DataAnnotations;
using Masroof.Api.Models;
using Masroof.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Masroof.Api.Controllers;

[ApiController]
[Route("expenses")]
public class ExpensesController : ControllerBase
{
    // Placeholder until auth provides the real tenant.
    private const string TenantId = "demo-tenant";

    private readonly ExpenseRepository _repository;

    public ExpensesController(ExpenseRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Expense>>> GetAll()
    {
        return await _repository.GetAllAsync(TenantId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Expense>> GetById(string id)
    {
        var expense = await _repository.GetByIdAsync(TenantId, id);
        return expense is null ? NotFound() : expense;
    }

    [HttpPost]
    public async Task<ActionResult<Expense>> Create(CreateExpenseRequest request)
    {
        var expense = new Expense
        {
            Title = request.Title,
            Amount = request.Amount,
            Currency = request.Currency ?? "EGP",
            Category = request.Category ?? string.Empty,
            Date = request.Date ?? DateTime.UtcNow,
            TenantId = TenantId,
        };

        var created = await _repository.CreateAsync(expense);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Expense>> Update(string id, UpdateExpenseRequest request)
    {
        var updated = await _repository.UpdateAsync(
            TenantId,
            id,
            request.Title,
            request.Amount,
            request.Currency ?? "EGP",
            request.Category ?? string.Empty,
            request.Date!.Value);

        return updated is null ? NotFound() : updated;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _repository.DeleteAsync(TenantId, id);
        return deleted ? NoContent() : NotFound();
    }
}

public record CreateExpenseRequest(
    [Required, StringLength(200)] string Title,
    [Range(0.01, double.MaxValue)] decimal Amount,
    string? Category,
    string? Currency,
    DateTime? Date
    );

public record UpdateExpenseRequest(
    [Required, StringLength(200)] string Title,
    [Range(0.01, double.MaxValue)] decimal Amount,
    string? Category,
    string? Currency,
    [Required] DateTime? Date
    );
