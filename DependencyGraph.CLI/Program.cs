using CliFx;

public class Program
{

        public static async Task<int> Main() =>
        await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .AllowDebugMode(true)
            .Build()
            .RunAsync();
    
}