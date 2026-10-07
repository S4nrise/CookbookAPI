using CookbookAPI.Contracts;
using FluentValidation;

namespace CookbookAPI.Services.Validators
{
    public class SignUpValidator : AbstractValidator<SignUpDto>
    {
        public SignUpValidator()
        {
            RuleFor(createDto => createDto.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(createDto => createDto.Password)
                .NotNull()
                .NotEmpty()
                .MaximumLength(256);
        }
    }
}
