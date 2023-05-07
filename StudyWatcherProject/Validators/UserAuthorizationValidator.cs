using System.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using StudyWatcherProject.Models;

namespace StudyWatcherProject.Validators;

public class UserAuthorizationValidator : AbstractValidator<UserStudent>
{
    public UserAuthorizationValidator()
    {
        //RuleFor(x => x.UserLogin).
    }
}