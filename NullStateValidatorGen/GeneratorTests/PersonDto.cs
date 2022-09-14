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