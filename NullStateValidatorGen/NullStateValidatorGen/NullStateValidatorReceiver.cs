using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace NullStateValidatorGen;

internal class NullStateValidatorReceiver : ISyntaxContextReceiver
{
    internal NullStateCheckData? Data { get; private set; }

    [MemberNotNull(nameof(Data))]
    private void AssureDataNotNull()
        => Data ??= new NullStateCheckData();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is ClassDeclarationSyntax classDeclarationSyntax)
        {
            var attributes = classDeclarationSyntax.AttributeLists;

            //var x = context.SemanticModel.GetDeclaredSymbol(context.Node) as INamedTypeSymbol;

            //var a = x.GetAttributes();

            var att1 = attributes.FirstOrDefault(); //Meh...
            if (att1 is not null)
            {
                if (att1.Attributes.First().Name.ToFullString().Contains("NullStateValidatorFactory"))
                {
                    var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                    if (classSymbol is not null)
                    {
                        AssureDataNotNull();
                        Data.SetFactory(classSymbol.Name, classSymbol.ContainingNamespace.ToString());
                    }
                }
                else if (att1.Attributes.First().Name.ToFullString().Contains("NullStateValidator"))
                {
                    var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                    if (classSymbol is not null)
                    {
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
                            AssureDataNotNull();
                            Data.AddClass(new NullStateClassCheckData(classSymbol.Name,
                                classSymbol.ContainingNamespace.ToString(), nonNullMembers));
                        }
                    }
                }
            }
        }
    }
}