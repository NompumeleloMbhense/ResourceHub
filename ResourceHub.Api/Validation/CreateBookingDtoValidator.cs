using FluentValidation;
using ResourceHub.Shared.DTOs;

/// <summary>
/// Validator for CreateBookingDto
/// Ensures that all required fields are provided and valid when creating a new booking
/// </summary>
namespace ResourceHub.Api.Validation
{
    public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingDtoValidator()
        {
            RuleFor(x => x.ResourceId)
                .NotEmpty().WithMessage("Resource ID is required")
                .GreaterThan(0)
                .WithMessage("Resource ID must be greater than 0");

            RuleFor(x => x.StartTime)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Start time is required")
                .LessThan(x => x.EndTime)
                .WithMessage("Start time must be before end time");

            RuleFor(x => x.EndTime)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("End time is required")
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.BookedBy)
                .NotEmpty().WithMessage("BookedBy is required")
                .MaximumLength(100).WithMessage("BookedBy cannot exceed 100 characters");

            RuleFor(x => x.Purpose)
                .NotEmpty().WithMessage("Purpose is required")
                .MaximumLength(200).WithMessage("Purpose cannot exceed 200 characters");
        }
    }
}