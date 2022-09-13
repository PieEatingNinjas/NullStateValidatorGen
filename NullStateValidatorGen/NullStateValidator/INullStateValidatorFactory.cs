namespace NullStateValidator;

public interface INullStateValidatorFactory
{
    public INullStateValidator<T>? GetFor<T>();
}
