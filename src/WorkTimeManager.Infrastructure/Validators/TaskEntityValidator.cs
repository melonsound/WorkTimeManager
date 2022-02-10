using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Core.Models;

namespace WorkTimeManager.Infrastructure.Validators
{
    public class TaskEntityValidator : AbstractValidator<TaskEntity>
    {
        public TaskEntityValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(TaskErrorMessage.NullOrEmpty)
                .NotNull().WithMessage(TaskErrorMessage.NullOrEmpty);


        }
    }
}
