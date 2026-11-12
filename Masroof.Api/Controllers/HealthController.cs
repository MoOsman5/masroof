using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet("db")]
    public async Task<IActionResult> Db(IMongoDatabase db, ILogger<HealthController> logger)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        try
        {
            await db.RunCommandAsync<BsonDocument>(
                new BsonDocument("ping", 1),
                cancellationToken: cts.Token);

            return Ok(new { status = "ok" });
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "MongoDB health check failed");
            return StatusCode(503, new { status = "down" });
        }
    }
}