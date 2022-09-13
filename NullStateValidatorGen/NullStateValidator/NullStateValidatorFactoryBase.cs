using System.Collections.Generic;

namespace NullStateValidator;

public abstract class NullStateValidatorFactoryBase : INullStateValidatorFactory
{
    protected IDictionary<string, object> NullStateValidators = new Dictionary<string, object>();

    public INullStateValidator<T>? GetFor<T>()
    {
        if (NullStateValidators.ContainsKey(typeof(T).Name))
        {
            return (INullStateValidator<T>)NullStateValidators[typeof(T).Name];
        }
        return null;
    }
}
