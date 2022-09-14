using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace NullStateValidatorGen;

[Generator]
public class NSVGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is NullStateValidatorReceiver nullStateSyntaxReceiver)
        {
            if (nullStateSyntaxReceiver != null && nullStateSyntaxReceiver.Data is not null)
            {
                var code = NullStateValidatorCodeGenerator.GenerateCode(nullStateSyntaxReceiver.Data);

                foreach (var item in code)
                {
                    context.AddSource(
                           $"{item.filename}", item.code);
                }
            }
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif
        context.RegisterForSyntaxNotifications(() => new NullStateValidatorReceiver());
        Debug.WriteLine("Initalize code generator");
    }
}
