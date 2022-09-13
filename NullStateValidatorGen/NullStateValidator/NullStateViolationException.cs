using System;

namespace NullStateValidator;

public class NullStateViolationException : Exception
{
    public string Member { get; }
    public NullStateViolationException(string member) : base($"Member '{member}' should not be null!")
    => Member = member;
}
