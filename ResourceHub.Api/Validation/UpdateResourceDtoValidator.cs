using FluentValidation;
using ResourceHub.Shared.DTOs;

namespace ResourceHub.Api.Validation
{
    public class UpdateResourceDtoValidator : AbstractValidator<UpdateResourceDto>
    {
        public UpdateResourceDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Resource name is required");

            RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage("Location is required");

            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .WithMessage("Capacity must be greater than 0");
        }
    }
}