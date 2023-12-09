using Dapper;
using Domain.Contracts.Repositories.AddCpf;
using Domain.Entities;
using Domain.Error;
using Infra.Repository.DbContext;

namespace Infra.Repository.Repositories.AddCpf
{
    public class CpfRepository : ICpfRepository
    {
        private readonly IDbContext _dbContext;

        public CpfRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddCpf(Cpf cpf)
        {
            try
            {
                if (validaCpfDuplicado(cpf.Cpf_Numero) != Definicao.SUCESSO)
                {
                    return Definicao.DUPLICADO;
                }

                var query = "INSERT INTO controle(cpf_numero, created_at) VALUES (@cpf_numero, @created_at)";

                var parameters = new DynamicParameters();
                parameters.Add("cpf_numero", cpf.Cpf_Numero, System.Data.DbType.String);
                parameters.Add("created_at", cpf.Created_At, System.Data.DbType.DateTime);

                using var connection = _dbContext.CreateConnection();

                connection.Execute(query, parameters);

                return Definicao.SUCESSO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Definicao.FALHA;
            }
        }

        public Cpf GetCpf(string cpf)
        {
            var query = "SELECT cpf_numero, created_at FROM controle WHERE cpf_numero = @cpf_numero";

            using var connection = _dbContext.CreateConnection();

            var parametros = new { cpf_numero = cpf };

            var result = connection.Query<Cpf>(query, parametros).FirstOrDefault();

            return result;
        }

        public List<Cpf> GetAllCpf() 
        {
            var query = "SELECT cpf_numero, created_at FROM controle";

            using var connection = _dbContext.CreateConnection();

            var result = connection.Query<Cpf>(query).ToList();

            return result;
        }

        public int DeleteCpf(string cpf)
        {
            try
            {
                var query = "DELETE FROM controle WHERE cpf_numero = @cpf_numero";

                var parameters = new DynamicParameters();
                parameters.Add("cpf_numero", cpf, System.Data.DbType.String);

                using var connection = _dbContext.CreateConnection();
                // Execute retorna o número de linhas afetadas
                int rowsAffected = connection.Execute(query, parameters);

                if (rowsAffected == 0)
                {
                    return Definicao.FALHA;
                }

                return Definicao.SUCESSO;
            }
            catch
            {
                return Definicao.FALHA;
            }
        }

        public int validaCpfDuplicado(string cpf)
        {

            var query = "SELECT COUNT(*) FROM controle WHERE cpf_numero = @cpf_numero";

            var parameters = new DynamicParameters();
            parameters.Add("cpf_numero", cpf, System.Data.DbType.String);

            using var connections = _dbContext.CreateConnection();

            var count = connections.ExecuteScalar<int>(query, parameters);

            return count > 0 ? Definicao.DUPLICADO : Definicao.SUCESSO;
        }
    }
}
