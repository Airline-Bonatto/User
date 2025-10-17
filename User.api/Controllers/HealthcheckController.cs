using System.Data;

using Microsoft.AspNetCore.Mvc;

namespace User.api.Controllers;

[ApiController]
public class HealthcheckController : ControllerBase
{
    [HttpGet("/healthcheck")]
    public IActionResult Healthcheck()
    {
        return Ok(new { Success = "true" });
    }

    [HttpGet("/healthcheck/database")]
    public IActionResult DatabaseHealthcheck([FromServices] IDbConnection dbConnection)
    {
        try
        {
            dbConnection.Open();
            dbConnection.Close();
            return Ok(new { Success = "true" });
        }
        catch(Exception ex)
        {
            return StatusCode(500, new { Success = "false", Error = ex.Message });
        }
    }
}