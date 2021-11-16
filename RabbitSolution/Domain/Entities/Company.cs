using System;

namespace Domain.Entities
{
    public class Company : IEntity
    {
        public Company(string name, string cnpj, DateTime foundationDate)
        {
            Name = name;
            Cnpj = cnpj;
            FoundationDate = foundationDate;
        }

        public Company(int id, string name, string cnpj, string email, DateTime foundationDate)
        {
            Id = id;
            Name = name;
            Cnpj = cnpj;
            FoundationDate = foundationDate;

            UpdateEmail(email);
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime FoundationDate { get; private set; }

        public void UpdateName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }
        }

        public void UpdateEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                Email = email;
            }
        }

        public void UpdateFoundationDate(DateTime? foundationDate)
        {
            if (foundationDate.HasValue)
            {
                FoundationDate = foundationDate.Value;
            }
        }
    }
}
