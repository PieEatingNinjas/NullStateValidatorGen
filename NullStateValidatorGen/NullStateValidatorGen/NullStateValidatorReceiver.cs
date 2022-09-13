using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NullStateValidatorGen;

internal class NullStateValidatorReceiver : ISyntaxContextReceiver
{
    internal List<NullStateClassCheckData> ClassNullChecks { get; } = new List<NullStateClassCheckData>();
    internal string FactoryName { get; private set; }
    internal string FactoryNamespace { get; private set; }

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is ClassDeclarationSyntax classDeclarationSyntax)
        {
            var attributes = classDeclarationSyntax.AttributeLists;

            var att1 = attributes.FirstOrDefault(); //Meh...
            if (att1 is not null)
            {
                if (att1.Attributes.First().Name.ToFullString().Contains("NullStateValidatorFactory"))
                {
                    var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                    FactoryName = classSymbol.Name;
                    FactoryNamespace = classSymbol.ContainingNamespace.Name;
                }
                else if (att1.Attributes.First().Name.ToFullString().Contains("NullStateValidator"))
                {
                    var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                    List<string> nonNullMembers = new List<string>();
                    foreach (var item in classDeclarationSyntax.Members)
                    {
                        var y = context.SemanticModel.GetDeclaredSymbol(item);

                        if (y is IPropertySymbol ps)
                        {
                            if (ps.Type.IsValueType)
                                continue;

                            if (ps.NullableAnnotation == NullableAnnotation.Annotated)
                            {
                                //Nullable
                                Debug.WriteLine($"{ps.ToDisplayString()} is nullable");
                            }
                            else if (ps.NullableAnnotation == NullableAnnotation.NotAnnotated)
                            {
                                //Not nullable
                                Debug.WriteLine($"{ps.ToDisplayString()} is NOT nullable");
                                nonNullMembers.Add(ps.Name);
                            }
                        }
                    }

                    if (nonNullMembers.Any())
                    {
                        ClassNullChecks.Add(new NullStateClassCheckData(classSymbol.Name, nonNullMembers));
                    }
                }
            }
        }
    }
}