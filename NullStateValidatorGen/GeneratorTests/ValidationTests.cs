using NullStateValidator;
using GeneratorTests.Dtos;

namespace GeneratorTests;

public class ValidationTests
{
    [Fact]
    public void ValidatePersonDto()
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

    [Fact]
    public void ValidatePersonDto_NoNullableAnnotationContext()
    {
        INullStateValidatorFactory factory = new MyNullStateValidatorFactory(); //can be injected

        var validator = factory.GetFor<PersonDto_NoNullableAnnotationContext>();

        Assert.Null(validator);
    }

    [Fact]
    public void ValidatePersonDto_NonNullableFirstName_NullableDisableLastName()
    {
        var validPerson = new PersonDto_NonNullableFirstName_NullableDisableLastName()
        {
            Address = null,
            FirstName = "Pieter",
            LastName = "Nijs",
            Id = 5
        };

        var alsoValidPerson = new PersonDto_NonNullableFirstName_NullableDisableLastName()
        {
            Address = null,
            FirstName = "Pieter",
            LastName = null,
            Id = 5
        };

        var invalidPerson = new PersonDto_NonNullableFirstName_NullableDisableLastName()
        {
            Address = null,
            FirstName = null!,
            LastName = null,
            Id = 6
        };

        INullStateValidatorFactory factory = new MyNullStateValidatorFactory(); //can be injected

        var validator = factory.GetFor<PersonDto_NonNullableFirstName_NullableDisableLastName>();

        Assert.NotNull(validator);

        validator!.Validate(validPerson);
        //validPerson is valid!

        validator!.Validate(alsoValidPerson);
        //alsoValidPerson is valid!

        var ex = Assert.Throws<NullStateViolationException>(() => validator!.Validate(invalidPerson));
        Assert.Equal($"Member '{nameof(PersonDto.FirstName)}' should not be null!", ex.Message);
        Assert.Equal(nameof(PersonDto.FirstName), ex.Member);
    }
}