using Mapster;
using Restaurant.API.Dto.Responses;
using Restaurant.API.Entities;

namespace Restaurant.API.Mapping;

public sealed class MappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Customer, CustomerResponse>()
            .Map(d => d.UserId, s => s.User.Id)
            .Map(d => d.Name, s => s.User.Name)
            .Map(d => d.Email, s => s.User.Email)
            .Map(d => d.IsVerified, s => s.User.IsVerified);

        config
            .NewConfig<Tuple<Customer, string>, LoginCustomerResponse>()
            .Map(d => d.CustomerId, s => s.Item1.Id)
            .Map(d => d.UserId, s => s.Item1.User.Id)
            .Map(d => d.UserName, s => s.Item1.User.Name)
            .Map(d => d.UserEmail, s => s.Item1.User.Email)
            .Map(d => d.IsVerified, s => s.Item1.User.IsVerified)
            .Map(d => d.AccessToken, s => s.Item2);
    }
}