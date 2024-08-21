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
    }
}