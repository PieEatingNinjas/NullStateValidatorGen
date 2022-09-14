using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NullStateValidatorGen;

internal class NullStateClassCheckData
{
    internal string ClassName { get; }
    internal string ClassNamespace { get; }
    internal IReadOnlyCollection<string> Members { get; set; }

    internal NullStateClassCheckData(string className, string classNamespace, IEnumerable<string> members)
    {
        ClassName = className;
        ClassNamespace = classNamespace;
        Members = members.ToList().AsReadOnly();
    }
}

internal class NullStateCheckData
{
    internal string? FactoryName { get; private set; }
    internal string? FactoryNamespace { get; private set; }

    List<NullStateClassCheckData>? _classes = null;

    internal IReadOnlyCollection<NullStateClassCheckData>? Classes { get => _classes?.AsReadOnly(); }

    [MemberNotNullWhen(true, 
        nameof(FactoryName), 
        nameof(FactoryNamespace),
        nameof(Classes))]
    internal bool HasData()
        => FactoryName is not null && FactoryNamespace is not null && Classes is not null;

    internal void SetFactory(string factoryName, string factoryNamespace)
    {
        FactoryName = factoryName;
        FactoryNamespace = factoryNamespace;
    }

    internal void AddClass(NullStateClassCheckData data)
        => (_classes ??= new List<NullStateClassCheckData>()).Add(data);
}