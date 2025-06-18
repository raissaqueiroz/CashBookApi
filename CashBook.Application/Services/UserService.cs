using AutoMapper;
using CashBook.Application.Dtos;
using CashBook.Application.Interfaces;
using CashBook.Core.Exceptions;
using CashBook.Core.Services;
using CashBook.Domain.Entities;
using CashBook.Infra.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CashBook.Application.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper) : IUserService
{
    public async Task Create(UserCreateDto userRead)
    {
        var userExists = await userRepository.GetByEmail(userRead.Email);

        if (userExists != null)
            throw new DomainException("Erro ao criar usuário.");
        
        var user = mapper.Map<User>(userRead);

        user.Password = passwordHasher.HashPassword(user, user.Password);
        user.Validate();
        
        await userRepository.Create(user);
    }

    public async Task<UserReadDto> Update(UserUpdateDto userRead)
    {
        var userIdExists = await userRepository.GetById(userRead.Id);

        if (userIdExists == null)
            throw new DomainException("Erro ao atualizar usuário");
        
        var userEmailExists = await userRepository.GetByEmail(userRead.Email);

        if (userEmailExists != null)
            throw new DomainException("Erro ao atualizar usuário");

        var user = mapper.Map<User>(userRead);
        user.Password = passwordHasher.HashPassword(user, user.Password);
        user.Validate();
        
        var userResult = await userRepository.Update(user);
        return mapper.Map<UserReadDto>(userResult);
    }

    public async Task<IEnumerable<UserReadDto>> Get()
    {
        var users = await userRepository.GetAll();

        return mapper.Map<IEnumerable<UserReadDto>>(users);
    }

    public async Task<UserReadDto> Get(Guid id)
    {
        var userExists = await userRepository.GetById(id);

        if (userExists == null)
            throw new DomainException("Erro ao buscar usuário");
        
        return mapper.Map<UserReadDto>(userExists);
    }

    public async Task<IEnumerable<UserReadDto>> SearchByEmail(string email)
    {
        var usersExists = await userRepository.SearchByEmail(email);

        if (usersExists == null)
            throw new DomainException("Erro ao buscar usuário");
        
        return mapper.Map<IEnumerable<UserReadDto>>(usersExists);
    }

    public async Task<IEnumerable<UserReadDto>> SearchByName(string name)
    {
        var usersExists = await userRepository.SearchByName(name);

        if (usersExists == null)
            throw new DomainException("Erro ao buscar usuário");
        
        return mapper.Map<IEnumerable<UserReadDto>>(usersExists);
    }

    //TODO: Autenticação do Usuário
    public Task<IEnumerable<UserReadDto>> GetByEmailAndPassword(string email, string password)
    {
        throw new NotImplementedException();
    }

    public async Task Remove(Guid id)
    {
        await userRepository.Remove(id);
    }
}