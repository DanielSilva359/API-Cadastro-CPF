using Cpf.Models.AddCpf;
using Domain.Contracts.UseCases.AddCpf;
using Domain.Error;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Cpf.Controllers
{
    [Route("cpf")]
    [ApiController]
    public class CpfController : ControllerBase
    {
        private readonly ICpfUseCase _cpfUseCase;
        private readonly IValidator<AddCpfInput> _cpfInputValidator;
        public CpfController(ICpfUseCase cpfUseCase, IValidator<AddCpfInput> cpfInputValidator) 
        {
            _cpfUseCase = cpfUseCase;
            _cpfInputValidator = cpfInputValidator;
        }

        [HttpPost]
        public IActionResult AddCpf(AddCpfInput input)
        {
            try
            {
                var cpfNumero = Regex.Replace(input.CpfNumero, "[^0-9]", "");
                var validatorResult = _cpfInputValidator.Validate(input);

                if (!validatorResult.IsValid)
                {
                    // Retorna um 400 Bad Request se a entrada não for válida
                    return StatusCode(400, new { type = "InvalidCpfException", message = "CPF is not Valid" });
                }

                var cpf = new Domain.Entities.Cpf(cpfNumero, DateTime.UtcNow);

                var validationResult = _cpfUseCase.AddCpf(cpf);

                switch (validationResult)
                {
                    case 1:
                        // Retorna um 201 Created se o CPF for adicionado com sucesso
                        return StatusCode(201, cpf);

                    case 2:
                        // Retorna um 409 Conflict se o CPF já existe (duplicado)
                        return StatusCode(409,  new { type = "ExistsCpfException", message = "CPF is Duplicated" });

                    default:
                        // Retorna um 500 Internal Server Error para outros casos inesperados
                        return StatusCode(500, new { type = "InternalCpfServerError", message = "CPF is ServerError" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { type = "InternalCpfServerError", message = "CPF is ServerError: " + ex.Message});
            }
        }

        [HttpGet("{cpf}")]
        public IActionResult GetCpf(string cpf)
        {
            try
            {
                // Limpa o CPF removendo caracteres não numéricos
                cpf = Regex.Replace(cpf, "[^0-9]", "");

                // Cria um objeto AddCpfInput para validar o CPF
                var cpfInput = new AddCpfInput { CpfNumero = cpf };
                var validatorResult = _cpfInputValidator.Validate(cpfInput);

                // Verifica se a entrada é válida
                if (!validatorResult.IsValid)
                {
                    // Retorna um 400 Bad Request se a entrada não for válida
                    return StatusCode(400, new { type = "InvalidCpfException", message = "CPF is not Valid" });
                }

                var validationResult = _cpfUseCase.GetCpf(cpf);

                // Verifica se o CPF foi encontrado
                if (validationResult == null)
                {
                    // Retorna um 404 Not Found se o CPF não for encontrado
                    return StatusCode(404, new { type = "CpfNotFoundException", message = "CPF is not found" });
                }

                // Retorna um 200 OK se tudo estiver correto
                return StatusCode(200, new { cpf = validationResult.Cpf_Numero, createdAt = validationResult.Created_At});
            }
            catch (Exception ex)
            {
                // Retorna um 500 Internal Server Error se ocorrer uma exceção
                return StatusCode(500, new { type = "InternalCpfServerError", message = "CPF is ServerError: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllCpf()
        {
            try
            {
                var validationResults = _cpfUseCase.GetAllCpf();

                var result = validationResults.Select(cpf => new { cpf = cpf.Cpf_Numero, createdAt = cpf.Created_At });

                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                // Retorna um 500 Internal Server Error se ocorrer uma exceção
                return StatusCode(500, new { type = "InternalCpfServerError", message = "CPF is ServerError: " + ex.Message });
            }
        }

        [HttpDelete("{cpf}")]
        public IActionResult DeleteCpf(string cpf)
        {
            try
            {
                // Limpa o CPF removendo caracteres não numéricos
                cpf = Regex.Replace(cpf, "[^0-9]", "");

                // Cria um objeto AddCpfInput para validar o CPF
                var cpfInput = new AddCpfInput { CpfNumero = cpf };
                var validatorResult = _cpfInputValidator.Validate(cpfInput);

                // Verifica se a entrada é válida
                if (!validatorResult.IsValid)
                {
                    // Retorna um 400 Bad Request se a entrada não for válida
                    return StatusCode(400, new { type = "InvalidCpfException", message = "CPF is not Valid" });
                }

                var validationResults = _cpfUseCase.DeleteCpf(cpf);

                if (validationResults != Definicao.SUCESSO)
                {
                    // Retorna um 404 Not Found se o CPF não for encontrado
                    return StatusCode(404, new { type = "CpfNotFoundException", message = "CPF is not found" });
                }

                //Retorna um 204 caso o Cpf tenha sido deletado com sucesso
                return StatusCode(204, new {type = "CpfDeleted", message = "Cpf is Deleted"});
            }
            catch (Exception ex)
            {
                // Retorna um 500 Internal Server Error se ocorrer uma exceção
                return StatusCode(500, new { type = "InternalCpfServerError", message = "CPF is ServerError: " + ex.Message });
            }
        }

    }
}
