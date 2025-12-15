using AspnetEcommercer.Domain.Customer.Entity;
using AspnetEcommercer.Domain.Customer.ValueObject;
using AspnetEcommercer.Infrastructure.Customer.Models;

namespace AspnetEcommercer.Infrastructure.Customer.Mappers;

public static class CustomerMapper
{
    public static CustomerDbModel ToDbModel(CustomerEntity entity)
    {
        return new CustomerDbModel
        {
            Id = entity.Id,
            Name = entity.Name,
            IsActive = entity.IsActive,
            RewardPoints = entity.RewardPoints,

            Street = entity.Address?.Street,
            City = entity.Address?.City,
            State = entity.Address?.State,
            ZipCode = entity.Address?.ZipCode,
            Number = entity.Address?.Number
        };
    }

    public static CustomerEntity ToDomain(CustomerDbModel model)
    {
        Address? address = null;

        // If you want to build Address only when it is complete
        if (model.Street is not null &&
            model.City is not null &&
            model.State is not null &&
            model.ZipCode is not null &&
            model.Number is not null)
        {
            address = Address.Create(
                model.Street,
                model.City,
                model.State,
                model.ZipCode,
                model.Number.Value
            );
        }

        return new CustomerEntity(
            model.Id,
            model.Name,
            address,
            model.IsActive,
            model.RewardPoints
        );
    }

    public static void ApplyToDbModel(CustomerEntity entity, CustomerDbModel model)
    {
        model.Name = entity.Name;
        model.IsActive = entity.IsActive;
        model.RewardPoints = entity.RewardPoints;

        model.Street = entity.Address?.Street;
        model.City = entity.Address?.City;
        model.State = entity.Address?.State;
        model.ZipCode = entity.Address?.ZipCode;
        model.Number = entity.Address?.Number;
    }
}
