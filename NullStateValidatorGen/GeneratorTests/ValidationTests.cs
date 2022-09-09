using NullStateValidator;

namespace GeneratorTests
{
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


            validPerson.Validate();
          //  NullStateValidator.GetFor<PersonDto>()!.Validate(invalidPerson);

            var ex = Assert.Throws<NullStateViolationException>(() => invalidPerson.Validate());
            Assert.Equal($"Member '{nameof(PersonDto.FirstName)}' should not be null!", ex.Message);
        }
    }
}