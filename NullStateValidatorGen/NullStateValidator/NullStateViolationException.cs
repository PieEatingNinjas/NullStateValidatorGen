using System;

namespace NullStateValidator
{
    public class NullStateViolationException : Exception
    {
        public NullStateViolationException(string property) : base($"Member '{property}' should not be null!")
        {

        }
    }
}
