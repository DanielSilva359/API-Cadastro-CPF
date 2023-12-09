using Domain.Entities;

namespace Domain.Contracts.UseCases.AddCpf
{
    public interface ICpfUseCase
    {
        int AddCpf(Cpf cpf);

        Cpf GetCpf(string cpf);

        List<Cpf> GetAllCpf();

        int DeleteCpf(string cpf);
    }
}
