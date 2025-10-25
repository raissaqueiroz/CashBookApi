using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CashBook.Application.Dtos;
using CashBook.Application.Interfaces;
using CashBook.Application.Services;
using CashBook.Core.Exceptions;
using CashBook.Domain.Entities;
using CashBook.Infra.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashBook.UnitTests.Application.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordHasher> _hasherMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepoMock = new(MockBehavior.Strict);
            _mapperMock = new(MockBehavior.Strict);
            _hasherMock = new(MockBehavior.Strict);
            _service = new UserService(_userRepoMock.Object, _hasherMock.Object, _mapperMock.Object);
        }

        #region Create

        [Fact(DisplayName = "Given valid create DTO When creating user Then should hash password and save")]
        public async Task Create_ValidDto_ShouldHashPasswordAndSave()
        {
            var dto = new UserCreateDto("Raissa", "raissa@example.com", "123456");
            var user = new User(dto.Name, dto.Email, dto.Password);

            _userRepoMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync((User)null);
            _mapperMock.Setup(m => m.Map<User>(dto)).Returns(user);
            _hasherMock.Setup(h => h.HashPassword(user, user.Password)).Returns("hashed");
            _userRepoMock.Setup(r => r.Create(user)).ReturnsAsync(user);

            Func<Task> act = async () => await _service.Create(dto);

            await act.Should().NotThrowAsync();
            user.Password.Should().Be("hashed");

            _userRepoMock.VerifyAll();
            _mapperMock.VerifyAll();
            _hasherMock.VerifyAll();
        }

        [Fact(DisplayName = "Given duplicate email When creating user Then should throw DomainException")]
        public async Task Create_DuplicateEmail_ShouldThrow()
        {
            var dto = new UserCreateDto("Raissa", "raissa@example.com", "123456");
            var existing = new User(dto.Name, dto.Email, dto.Password);

            _userRepoMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync(existing);

            Func<Task> act = async () => await _service.Create(dto);

            await act.Should().ThrowAsync<DomainException>().WithMessage("Erro ao criar usuário*");

            _userRepoMock.VerifyAll();
        }

        #endregion

        #region Update

        [Fact(DisplayName = "Given valid update DTO When updating Then should return mapped result")]
        public async Task Update_ValidDto_ShouldReturnMappedResult()
        {
            var dto = new UserUpdateDto(Guid.NewGuid(), "Raissa", "raissa@example.com", "123456");
            var existing = new User(dto.Name, dto.Email, dto.Password);
            var mapped = new User(dto.Name, dto.Email, dto.Password);
            var updated = new User(dto.Name, dto.Email, dto.Password);
            var readDto = new UserReadDto(updated.Id, updated.Name, updated.Email, updated.Status);

            _userRepoMock.Setup(r => r.GetById(dto.Id)).ReturnsAsync(existing);
            _userRepoMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync((User)null);
            _mapperMock.Setup(m => m.Map<User>(dto)).Returns(mapped);
            _hasherMock.Setup(h => h.HashPassword(mapped, mapped.Password)).Returns("hashed");
            _userRepoMock.Setup(r => r.Update(mapped)).ReturnsAsync(updated);
            _mapperMock.Setup(m => m.Map<UserReadDto>(updated)).Returns(readDto);

            var result = await _service.Update(dto);

            result.Should().BeEquivalentTo(readDto);
            _userRepoMock.VerifyAll();
            _mapperMock.VerifyAll();
            _hasherMock.VerifyAll();
        }

        [Fact(DisplayName = "Given non-existing user When updating Then should throw DomainException")]
        public async Task Update_NonExistingUser_ShouldThrow()
        {
            var dto = new UserUpdateDto(Guid.NewGuid(), "Raissa", "raissa@example.com", "123456");

            _userRepoMock.Setup(r => r.GetById(dto.Id)).ReturnsAsync((User)null);

            Func<Task> act = async () => await _service.Update(dto);

            await act.Should().ThrowAsync<DomainException>().WithMessage("Erro ao atualizar usuário");

            _userRepoMock.VerifyAll();
        }

        [Fact(DisplayName = "Given email already exists When updating Then should throw DomainException")]
        public async Task Update_DuplicateEmail_ShouldThrow()
        {
            var dto = new UserUpdateDto(Guid.NewGuid(), "Raissa", "raissa@example.com", "123456");
            var existingUser = new User("Outro", "raissa@example.com", "654321");
            var currentUser = new User("Raissa", "raissa@old.com", "123456");

            _userRepoMock.Setup(r => r.GetById(dto.Id)).ReturnsAsync(currentUser);
            _userRepoMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync(existingUser);

            Func<Task> act = async () => await _service.Update(dto);

            await act.Should().ThrowAsync<DomainException>().WithMessage("Erro ao atualizar usuário");

            _userRepoMock.VerifyAll();
        }

        #endregion

        #region Remove

        [Fact(DisplayName = "Given existing user When removing Then should call repository")]
        public async Task Remove_ExistingUser_ShouldCallRepository()
        {
            var id = Guid.NewGuid();

            _userRepoMock.Setup(r => r.Remove(id)).Returns(Task.CompletedTask);

            Func<Task> act = async () => await _service.Remove(id);

            await act.Should().NotThrowAsync();

            _userRepoMock.VerifyAll();
        }

        #endregion

        #region GetById

        [Fact(DisplayName = "Given existing user When getting by ID Then should return UserReadDto")]
        public async Task GetById_Existing_ShouldReturnUserReadDto()
        {
            var id = Guid.NewGuid();
            var user = new User("Raissa", "raissa@example.com", "123456");
            var readDto = new UserReadDto(user.Id, user.Name, user.Email, user.Status);

            _userRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserReadDto>(user)).Returns(readDto);

            var result = await _service.Get(id);

            result.Should().BeEquivalentTo(readDto);

            _userRepoMock.VerifyAll();
            _mapperMock.VerifyAll();
        }

        [Fact(DisplayName = "Given non-existing user When getting by ID Then should throw DomainException")]
        public async Task GetById_NonExisting_ShouldThrow()
        {
            var id = Guid.NewGuid();

            _userRepoMock.Setup(r => r.GetById(id)).ReturnsAsync((User)null);

            Func<Task> act = async () => await _service.Get(id);

            await act.Should().ThrowAsync<DomainException>().WithMessage("Erro ao buscar usuário");

            _userRepoMock.VerifyAll();
        }

        #endregion

        #region GetAll

        [Fact(DisplayName = "Given users exist When getting all Then should return list of UserReadDto")]
        public async Task GetAll_ShouldReturnListOfUserReadDto()
        {
            var users = new List<User> { new User("Raissa", "raissa@example.com", "123456") };
            var dtos = new List<UserReadDto> { new UserReadDto(users[0].Id, users[0].Name, users[0].Email, users[0].Status) };

            _userRepoMock.Setup(r => r.GetAll()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserReadDto>>(users)).Returns(dtos);

            var result = await _service.Get();

            result.Should().BeEquivalentTo(dtos);

            _userRepoMock.VerifyAll();
            _mapperMock.VerifyAll();
        }

        #endregion

        #region SearchByEmail

        [Fact(DisplayName = "Given email exists When searching Then should return list of UserReadDto")]
        public async Task SearchByEmail_Existing_ShouldReturnList()
        {
            var email = "raissa";
            var users = new List<User> { new User("Raissa", email + "@example.com", "123456") };
            var dtos = new List<UserReadDto> { new UserReadDto(users[0].Id, users[0].Name, users[0].Email, users[0].Status) };

            _userRepoMock.Setup(r => r.SearchByEmail(email)).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserReadDto>>(users)).Returns(dtos);

            var result = await _service.SearchByEmail(email);

            result.Should().BeEquivalentTo(dtos);

            _userRepoMock.VerifyAll();
            _mapperMock.VerifyAll();
        }

        [Fact(DisplayName = "Given email does not exist When searching Then should throw DomainException")]
        public async Task SearchByEmail_NonExisting_ShouldThrow()
        {
            var email = "naoexiste";

            _userRepoMock.Setup(r => r.SearchByEmail(email)).ReturnsAsync((IEnumerable<User>)null);

            Func<Task> act = async () => await _service.SearchByEmail(email);

            await act.Should().ThrowAsync<DomainException>().WithMessage("Erro ao buscar usuário");

            _userRepoMock.VerifyAll();
        }

        #endregion

        #region SearchByName

        [Fact(DisplayName = "Given name exists When searching Then should return list of UserReadDto")]
        public async Task SearchByName_Existing_ShouldReturnList()
        {
            var name = "Raissa";
            var users = new List<User> { new User(name, "raissa@example.com", "123456") };
            var dtos = new List<UserReadDto> { new UserReadDto(users[0].Id, users[0].Name, users[0].Email, users[0].Status) };

            _userRepoMock.Setup(r => r.SearchByName(name)).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserReadDto>>(users)).Returns(dtos);

            var result = await _service.SearchByName(name);

            result.Should().BeEquivalentTo(dtos);

            _userRepoMock.VerifyAll();
            _mapperMock.VerifyAll();
        }

        [Fact(DisplayName = "Given name does not exist When searching Then should throw DomainException")]
        public async Task SearchByName_NonExisting_ShouldThrow()
        {
            var name = "NaoExiste";

            _userRepoMock.Setup(r => r.SearchByName(name)).ReturnsAsync((IEnumerable<User>)null);

            Func<Task> act = async () => await _service.SearchByName(name);

            await act.Should().ThrowAsync<DomainException>().WithMessage("Erro ao buscar usuário");

            _userRepoMock.VerifyAll();
        }

        #endregion
    }
}
