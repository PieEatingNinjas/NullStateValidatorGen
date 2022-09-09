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

            Assert.Throws<Exception>(() => invalidPerson.Validate());
        }
    }
}