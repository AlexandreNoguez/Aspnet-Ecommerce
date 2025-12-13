using AspnetEcommercer.Domain.Customer.ValueObject;

namespace AspnetEcommercer.Domain.Customer.Entity
{
    public class CustomerEntity
    {
        public CustomerEntity(Guid id, string name, Address? address, bool isActive, int rewardPoints)
        {
            Id = id;
            Name = name;
            Address = address;
            IsActive = isActive;
            RewardPoints = rewardPoints;
            this.Validate();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Address? Address { get; set; }
        public bool IsActive { get; set; }
        public int RewardPoints { get; private set; }

        public Guid GetId()
        {
            return Id;
        }

        public static CustomerEntity Create(Guid id, string name, Address address)
        {
            return new CustomerEntity(id, name, address, false, 0);
        }

        public void Activate()
        {
            if (Address == null)
            {
                throw new Exception("Address is mandatory to activate a customer");
            }
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void AddRewardPoints(int points)
        {
            RewardPoints += points;
        }

        public void ChangeAddress(Address address)
        {
            Address = address;
        }

        public void Validate()
        {
            if (Id == Guid.Empty)
            {
                throw new Exception("Id is required");
            }
            if (Name.Length == 0)
            {
                throw new Exception("Name is required");
            }
        }
    }
}
