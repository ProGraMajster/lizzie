using System;
using Microsoft.AspNetCore.Mvc;
using lizzie;
using AspNetEval.Models;

namespace AspNetEval.Controllers;

[ApiController]
[Route("eval")]
public class EvalController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] EvalRequest request)
    {
        try
        {
            var lambda = LambdaCompiler.Compile(request.Script);
            var result = lambda();
            return Ok(new { result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
