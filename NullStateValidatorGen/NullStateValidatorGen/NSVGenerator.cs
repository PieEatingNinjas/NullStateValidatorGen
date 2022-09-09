using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NullStateValidator;
using System.Collections.Generic;
using System.ComponentModel;
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
        var syntaxReceiver = (SyntaxReceiver)context.SyntaxContextReceiver;

        if (syntaxReceiver != null && syntaxReceiver.Checks.Any())
        {
            foreach (var item in syntaxReceiver.Checks)
            {
                context.AddSource(
                    $"{item.Key}.g.cs", item.Value);
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
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        Debug.WriteLine("Initalize code generator");
    }
}

internal class SyntaxReceiver : ISyntaxContextReceiver
{
    public Dictionary<string, string> Checks { get; set; } = new Dictionary<string, string>();
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is ClassDeclarationSyntax classDeclarationSyntax)
        {
            var attributes = classDeclarationSyntax.AttributeLists;


            var att1 = attributes.FirstOrDefault();
            if(att1 is not null)
            {
                // if(attributes.SingleOrDefault(a => a.d))
                if (att1.Attributes.First().Name.ToFullString().Contains("NullStateValidator"))
                {
                    var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                    List<string> nonNullMembers = new List<string>();
                    foreach (var item in classDeclarationSyntax.Members)
                    {
                        //  var atts = classSymbol.GetAttributes();
                        var y = context.SemanticModel.GetDeclaredSymbol(item);

                        if (y is IPropertySymbol ps)
                        {
                            if (ps.Type.IsValueType)
                                continue;

                            if (ps.NullableAnnotation == NullableAnnotation.Annotated)
                            {
                                //Nullable
                                System.Diagnostics.Debug.WriteLine($"{ps.ToDisplayString()} is nullable");
                            }
                            else if (ps.NullableAnnotation == NullableAnnotation.NotAnnotated)
                            {
                                //Not nullable
                                System.Diagnostics.Debug.WriteLine($"{ps.ToDisplayString()} is NOT nullable");
                                nonNullMembers.Add(ps.Name);
                            }
                        }
                    }

                    if (nonNullMembers.Any())
                    {
                        GenerateChecks(context, classSymbol, nonNullMembers);
                    }
                }
            }
        }
    }

    private void GenerateChecks(GeneratorSyntaxContext context, ISymbol classSymbol, List<string> nonNullMembers)
    {
        var validations = new StringBuilder();

        foreach (var item in nonNullMembers)
        {
            validations.AppendLine($"if({item} is null) throw new NullStateViolationException(\"{item}\");");
        }


        var @class =
$@"// Auto-generated code for {classSymbol.Name}
using NullStateValidator;
namespace {classSymbol.ContainingNamespace.ToDisplayString()};
public partial class {classSymbol.Name}
{{
    partial void ValidateInternal ()
    {{
        {validations}
    }}
}}";
        Checks.Add(classSymbol.Name, @class);
    }
}