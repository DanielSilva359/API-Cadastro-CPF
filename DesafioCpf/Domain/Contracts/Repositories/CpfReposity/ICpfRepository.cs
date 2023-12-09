using Domain.Entities;

namespace Domain.Contracts.Repositories.AddCpf
{
    public interface ICpfRepository
    {
        int AddCpf(Cpf cpf);

        Cpf GetCpf(string cpf);

        List<Cpf> GetAllCpf();

        int DeleteCpf(string cpf);
    }
}
