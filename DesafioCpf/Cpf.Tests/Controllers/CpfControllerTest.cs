using Cpf.Controllers;
using Cpf.Models.AddCpf;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Domain.Contracts.UseCases.AddCpf;

namespace Cpf.Tests.Controllers
{
    public class CpfControllerTest
    {
        private CpfController cpfController;

        public CpfControllerTest()
        {
            // Configuração básica com Moq
            var mockCpfUseCase = new Mock<ICpfUseCase>();
            var mockCpfInputValidator = new Mock<IValidator<AddCpfInput>>();

            cpfController = new CpfController(mockCpfUseCase.Object, mockCpfInputValidator.Object);
        }

        [Fact]
        public void Post_CadastrandoCpf_ComSucesso()
        {
            // Configuração do Mock para o validator
            var mockValidator = new Mock<AddCpfInputValidator>();
            mockValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<AddCpfInput>>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            // Configuração do Mock para o caso de uso
            var mockCpfUseCase = new Mock<ICpfUseCase>();
            mockCpfUseCase.Setup(x => x.AddCpf(It.IsAny<Domain.Entities.Cpf>())).Returns(1);

            // Atribui os mocks ao controller
            cpfController = new CpfController(mockCpfUseCase.Object, mockValidator.Object);

            // Chama o método
            var result = cpfController.AddCpf(new AddCpfInput() { CpfNumero = "09711774909" });

            // Verifica o resultado
            var createdAtActionResult = Assert.IsType<CreatedResult>(result);
            var dateTime = DateTime.UtcNow;

            Assert.Equal(createdAtActionResult.Value, createdAtActionResult.Value);
        }
    }
}
