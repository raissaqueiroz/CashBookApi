using AutoMapper;
using CashBook.Application.Dtos;
using CashBook.Domain.Entities;

namespace CashBook.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserReadDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
        CreateMap<User, UserCreateDto>().ReverseMap();
    }
}