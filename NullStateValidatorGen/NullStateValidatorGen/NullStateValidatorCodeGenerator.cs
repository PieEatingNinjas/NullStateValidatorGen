using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NullStateValidatorGen
{
    internal static class NullStateValidatorCodeGenerator
    {
        internal static IEnumerable<(string filename, string code)> GenerateCode(NullStateCheckData data)
        {
            if (data.HasData())
            {
                var validatorsCode =
                    data.Classes.Select(
                        c => new
                        {
                            Validator = GenerateValidatorCode(c),
                            FactoryRegistration = GenerateFactoryRegisrationCode(c),
                        }).ToList();

                var factory = GenerateFactoryCode(data, validatorsCode.Select(c => c.FactoryRegistration));

                return validatorsCode.Select(c => c.Validator).Concat(new[] { factory });
            }
            return Enumerable.Empty<(string, string)>();
        }

        private static (string filename, string code) GenerateFactoryCode(NullStateCheckData data,
            IEnumerable<string> validatorRegistrations)
        {
            using var writer = new StringWriter();
            using var factory = new IndentedTextWriter(writer, "\t");

            //usings
            factory.WriteLine("using NullStateValidatorGen;");
            factory.WriteLine("using NullStateValidator;");

            //ns
            factory.WriteLine("");
            factory.WriteLine($"namespace {data.FactoryNamespace};");
            factory.WriteLine("");

            //class definition
            factory.WriteLine($"public partial class {data.FactoryName} : NullStateValidatorFactoryBase");
            factory.WriteLine("{");

            //ctor
            factory.Indent++;
            factory.WriteLine($"public {data.FactoryName}()");
            factory.WriteLine("{");
            factory.Indent++;
            factory.WriteLine("InitValidators();");
            factory.Indent--;
            factory.WriteLine("}");
            factory.WriteLine("");

            //InitValidators
            factory.WriteLine("private void InitValidators()");
            factory.WriteLine("{");
            factory.Indent++;
            factory.WriteLine($"{string.Join(";", validatorRegistrations)}");
            factory.Indent--;
            factory.WriteLine("}");

            factory.Indent--;
            factory.WriteLine("}");

            return ($"{data.FactoryName!}.g.cs", factory.InnerWriter.ToString());
        }

        private static string GenerateFactoryRegisrationCode(NullStateClassCheckData item)
        => $"NullStateValidators.Add(\"{item.ClassName}\", new {item.ClassName}NullStateValidator());";

        private static (string filename, string code) GenerateValidatorCode(NullStateClassCheckData item)
        {
            var validations = new StringBuilder();

            foreach (var member in item.Members)
            {
                validations.AppendLine($"if(instance.{member} is null) throw new NullStateViolationException(\"{member}\");");
            }

            using var writer = new StringWriter();
            using var validator = new IndentedTextWriter(writer, "\t");

            //usings
            validator.WriteLine("using NullStateValidator;");
            validator.WriteLine($"using {item.ClassNamespace};");

            //ns
            validator.WriteLine("");
            validator.WriteLine("namespace NullStateValidatorGen;");
            validator.WriteLine("");

            //class definition
            validator.WriteLine($"public class {item.ClassName}NullStateValidator : INullStateValidator<{item.ClassName}>");
            validator.WriteLine("{");

            //validate
            validator.Indent++;
            validator.WriteLine($"public void Validate({item.ClassName} instance)");
            validator.WriteLine("{");
            validator.Indent++;
            validator.WriteLine($"{validations}");
            validator.Indent--;
            validator.WriteLine("}");

            validator.Indent--;
            validator.WriteLine("}");

            return ($"{item.ClassName}NullStateValidator.g.cs", validator.InnerWriter.ToString());
        }
    }
}
