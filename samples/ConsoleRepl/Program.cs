using System;
using lizzie;
using lizzie.Runtime;

var ctx = RuntimeProfiles.ServerDefaults();
Console.WriteLine("Lizzie REPL - type 'exit' to quit");
string? line;
while ((line = Console.ReadLine()) != null)
{
    if (string.Equals(line, "exit", StringComparison.OrdinalIgnoreCase))
        break;

    if (string.IsNullOrWhiteSpace(line))
        continue;

    try
    {
        var lambda = LambdaCompiler.Compile(ctx, line);
        var result = lambda();
        Console.WriteLine(result ?? "null");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
