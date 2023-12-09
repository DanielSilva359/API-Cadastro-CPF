using Domain.Contracts.Repositories.AddCpf;
using Domain.Contracts.UseCases.AddCpf;
using Domain.Entities;

namespace Application.UseCases.AddCpf
{
    public class AddCpfUseCase : ICpfUseCase
    {
        private readonly ICpfRepository _cpfRepository;

        public AddCpfUseCase(ICpfRepository cpfRepository)
        {
            _cpfRepository = cpfRepository;
        }

        public int AddCpf(Cpf cpf)
        {
            return _cpfRepository.AddCpf(cpf);
        }

        public List<Cpf> GetAllCpf()
        {
            return _cpfRepository.GetAllCpf();
        }

        public Cpf GetCpf(string cpf)
        {
            return _cpfRepository.GetCpf(cpf);
        }

        public int DeleteCpf(string cpf)
        {
            return _cpfRepository.DeleteCpf(cpf);
        }
    }
}
