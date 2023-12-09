using FluentValidation;

namespace Cpf.Models.AddCpf
{
    public class AddCpfInputValidator : AbstractValidator<AddCpfInput>
    {
        public AddCpfInputValidator() 
        {
            RuleFor(c => c.CpfNumero).IsValidCPF().WithMessage("CPF is not valid.");
        }
    }
}
