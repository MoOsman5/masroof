using System.ComponentModel.DataAnnotations;
using Masroof.Api.Models;
using Masroof.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Masroof.Api.Controllers;

[ApiController]
[Route("expenses")]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseRepository _repository;

    public ExpensesController(ExpenseRepository repository)
    {
        _repository = repository;
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
            Date = request.Date,
            TenantId = "demo-tenant",
        };

        var created = await _repository.CreateAsync(expense);
        return StatusCode(StatusCodes.Status201Created, created);
    }
}

public record CreateExpenseRequest(
    [Required, StringLength(200)] string Title,
    [Range(0.01, double.MaxValue)] decimal Amount,
    string? Category,
    string? Currency,
    DateTime Date
    );

