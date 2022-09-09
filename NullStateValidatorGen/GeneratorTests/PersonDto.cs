using NullStateValidator;

namespace GeneratorTests;

[NullStateValidator]
public partial class PersonDto
{
    public string FirstName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public int Id { get; set; }
}


public static class NullStateValidator
{
    public static INullStateValidator<T>? GetFor<T>()
    {
        if(typeof(T) == typeof(PersonDto))
        {
            return (INullStateValidator<T>)new PersonDtoNullStateValidator();
        }
        return null;
    }
}

class PersonDtoNullStateValidator : INullStateValidator<PersonDto>
{
    public void Validate(PersonDto instance)
    {
        throw new NotImplementedException();
    }
}