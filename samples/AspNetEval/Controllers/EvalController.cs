using System;
using Microsoft.AspNetCore.Mvc;
using lizzie;
using lizzie.Runtime;
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
            var ctx = RuntimeProfiles.ServerDefaults();
            var lambda = LambdaCompiler.Compile(ctx, request.Script);
            var result = lambda();
            return Ok(new { result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
