using NullStateValidator;
using GeneratorTests.Dtos;

namespace GeneratorTests;

public class ValidationTests
{
    [Fact]
    public void ValidatePerson()
    {
        var validPerson = new PersonDto()
        {
            Address = null,
            FirstName = "Pieter",
            Id = 5
        };

        var invalidPerson = new PersonDto()
        {
            Address = null,
            FirstName = null!,
            Id = 6
        };

        INullStateValidatorFactory factory = new MyNullStateValidatorFactory(); //can be injected
        
        var validator = factory.GetFor<PersonDto>();

        Assert.NotNull(validator);

        validator!.Validate(validPerson);
        //validPerson is valid!

        var ex = Assert.Throws<NullStateViolationException>(() => validator!.Validate(invalidPerson));
        Assert.Equal($"Member '{nameof(PersonDto.FirstName)}' should not be null!", ex.Message);
        Assert.Equal(nameof(PersonDto.FirstName), ex.Member);
    }
}