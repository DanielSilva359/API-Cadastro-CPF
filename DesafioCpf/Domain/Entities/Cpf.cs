namespace Domain.Entities
{
    public class Cpf
    {

        public Cpf(string cpf, DateTime createdAt)
        {
            Cpf_Numero = cpf;
            Created_At = createdAt;
        }
        public Cpf() { }

        public string Cpf_Numero { get; set; }
        public DateTime Created_At { get; set; }
    }
}
