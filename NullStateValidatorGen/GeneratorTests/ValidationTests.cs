using NullStateValidator;
using System.Diagnostics;

namespace GeneratorTests
{
    public class ValidationTests
    {
        #region DEMO
        [Fact]
        public void Demo()
        {
            var validPerson = new PersonDto()
            {
                Address = null,
                FirstName = "Pieter",
                Id = 5
            };

            PrintPerson(validPerson);

            var invalidPerson = new PersonDto()
            {
                Address = null,
                FirstName = null!,
                Id = 5
            };

            PrintPerson(invalidPerson);
        }

        private void PrintPerson(PersonDto personDto)
        {
            Debug.WriteLine($"{personDto.FirstName.ToLower()} - {personDto.Address?.ToUpper()}");
        } 

        #endregion





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

            var validator = new NullStateValidatorFactory().GetFor<PersonDto>();

            Assert.NotNull(validator);


            validator!.Validate(validPerson);

            

            var ex = Assert.Throws<NullStateViolationException>(() => validator!.Validate(invalidPerson));
            Assert.Equal($"Member '{nameof(PersonDto.FirstName)}' should not be null!", ex.Message);
            Assert.Equal(nameof(PersonDto.FirstName), ex.Member);
        }
    }
}