
using NullStateValidator;

namespace GeneratorTests.Dtos;

[GenerateNullStateValidator]
public class PersonDto
{
    public string FirstName { get; set; } = string.Empty;

    public string? Address { get; set; }
    public int Id { get; set; }

    //ToDo
    //public PersonDto Spouce { get; set; } = default(PersonDto)!;

    //ToDo
    //public List<PetDto> Pets { get; set; } = null!;
}


#nullable disable
[GenerateNullStateValidator]
public class PersonDto_NoNullableAnnotationContext
{
    public string FirstName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public int Id { get; set; }
}
#nullable restore

[GenerateNullStateValidator]
public class PersonDto_NonNullableFirstName_NullableDisableLastName
{
    public string FirstName { get; set; } = string.Empty;
#nullable disable
    public string LastName { get; set; } = string.Empty;
#nullable restore
    public string? Address { get; set; }
    public int Id { get; set; }
}