using FluentValidation;
using ResourceHub.Api.DTOs;

/// <summary>
/// Validator for CreateResourcDto
/// Ensure that all required fields are provided and valid when creating a new resource
/// </summary>

namespace ResourceHub.Api.Validation
{
    public class CreateResourceDtoValidator : AbstractValidator<CreateResourceDto>
    {
        public CreateResourceDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Resource name is required");

            RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage("Location is required");

            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("Capacity is required")
                .GreaterThan(0)
                .WithMessage("Capacity must be greater than 0");
        }
    }
}