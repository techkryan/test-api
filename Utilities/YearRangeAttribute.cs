using System.ComponentModel.DataAnnotations;

namespace MusicCatalog.Api.Utilities;

public class YearRangeAttribute : ValidationAttribute
{
    public YearRangeAttribute(int minimumYear) : this(minimumYear, DateTime.Now.Year)
    {
    }

    public YearRangeAttribute(int minimumYear, int maximumYear)
    {
        MinimumYear = minimumYear;
        MaximumYear = maximumYear;

        string defaultErrorMessage = "The property '{0}' must fall into the year range between {1} and {2}";
        ErrorMessage ??= defaultErrorMessage;
    }

    public int MinimumYear { get; }
    public int MaximumYear { get; }

    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        var releaseYear = ((DateOnly)value!).Year;

        if (releaseYear < MinimumYear || releaseYear > MaximumYear)
        {
            return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
    
    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessage!, name, MinimumYear, MaximumYear);
    }
}
