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

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Address? Address { get; private set; }
        public bool IsActive { get; private set; }
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
            if (points < 0)
            {
                throw new Exception("Points must be positive");
            }

            RewardPoints += points;
            Validate();
        }

        public void ChangeAddress(Address address)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Validate();
        }

        public void Validate()
        {
            if (Id == Guid.Empty)
            {
                throw new Exception("Id is required");
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new Exception("Name is required");
            }

            if (RewardPoints < 0)
            {
                throw new Exception("Reward points cannot be negative");
            }

            if (IsActive && Address is null)
            {
                throw new Exception("Address is required when customer is active");
            }
        }
    }
}
