using System.Collections.Generic;

namespace NullStateValidatorGen;

internal class NullStateClassCheckData
{
    internal string ClassName { get; }
    internal List<string> Members { get; set; }

    internal NullStateClassCheckData(string className, List<string> members)
    {
        ClassName = className;
        Members = members;
    }
}