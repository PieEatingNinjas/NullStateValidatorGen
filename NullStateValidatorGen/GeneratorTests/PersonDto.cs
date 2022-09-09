namespace GeneratorTests;

public partial class PersonDto
{
    public string FirstName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public int Id { get; set; }

    public void Validate() => ValidateInternal();

    partial void ValidateInternal();
}
