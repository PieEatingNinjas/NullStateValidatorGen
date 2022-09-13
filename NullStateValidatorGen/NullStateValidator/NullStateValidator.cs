namespace NullStateValidator
{
    public interface INullStateValidator<T>
    {
        void Validate(T instance);
    }
}
