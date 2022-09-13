using System.Text;

namespace NullStateValidatorGen
{
    internal static class NullStateValidatorCodeGenerator
    {
        internal static (string filename, string code) GenerateCode(NullStateClassCheckData item)
        {
            var validations = new StringBuilder();

            foreach (var member in item.Members)
            {
                validations.AppendLine($"if(instance.{member} is null) throw new NullStateViolationException(\"{member}\");");
            }

            var validator =
            @$"
using NullStateValidator;
using GeneratorTests;
namespace NullStateValidatorGen;
public class {item.ClassName}NullStateValidator : INullStateValidator<{item.ClassName}>
{{
    public void Validate({item.ClassName} instance)
    {{
        {validations}
    }}
}}";
            return ($"{item.ClassName}NullStateValidator.g.cs", validator);
        }
    }
}
