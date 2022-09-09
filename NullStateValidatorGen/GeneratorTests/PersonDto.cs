using NullStateValidator;

namespace GeneratorTests;

[NullStateValidator]
public partial class PersonDto
{
    public string FirstName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public int Id { get; set; }
}

//class PersonDtoNullStateValidator : INullStateValidator<PersonDto>
//{
//    public void Validate(PersonDto instance)
//    {
//        throw new NotImplementedException();
//    }
//}