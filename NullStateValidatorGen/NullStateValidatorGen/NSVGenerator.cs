using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NullStateValidatorGen;

[Generator]
public class NSVGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is NullStateValidatorReceiver nullStateSyntaxReceiver)
        {
            if (nullStateSyntaxReceiver != null && nullStateSyntaxReceiver.ClassNullChecks.Any())
            {
                var factoryBuilder = new StringBuilder();

                foreach (var item in nullStateSyntaxReceiver.ClassNullChecks)
                {
                    var code = NullStateValidatorCodeGenerator.GenerateCode(item);

                    context.AddSource(code.filename, code.code);

                    factoryBuilder.AppendLine($"NullStateValidators.Add(\"{item.ClassName}\", new {item.ClassName}NullStateValidator());");
                }

                var factory =
@$"
            using NullStateValidatorGen;
    using NullStateValidator;  
            using GeneratorTests;
            namespace {nullStateSyntaxReceiver.FactoryNamespace};
            public partial class {nullStateSyntaxReceiver.FactoryName} : NullStateValidatorFactoryBase
            {{
                public NullStateValidatorFactory()
                {{
                    InitValidators();
                }}
                
                private void InitValidators()
                {{
                    {factoryBuilder}
                }}
            }}
            ";
                context.AddSource(
                       $"{nullStateSyntaxReceiver.FactoryName}.g.cs", factory);
            }
        }
        
    }

    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
#endif
        context.RegisterForSyntaxNotifications(() => new NullStateValidatorReceiver());
        Debug.WriteLine("Initalize code generator");
    }
}
