using System;
using lizzie;

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
        var lambda = LambdaCompiler.Compile(line);
        var result = lambda();
        Console.WriteLine(result ?? "null");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
