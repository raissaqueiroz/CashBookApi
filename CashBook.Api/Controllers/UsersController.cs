using CashBook.Application.Dtos;
using CashBook.Application.Interfaces;
using CashBook.Core.Services;
using CashBook.Domain.Entities;
using CashBook.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CashBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll([FromServices] IUserService userService)
    {
        var users = await userService.Get();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserReadDto>> GetById(Guid id, [FromServices] IUserService userService)
    {
        var user = await userService.Get(id);
        
        return Ok(user);
    }

    [HttpGet("search/email")]
    public async Task<ActionResult<IEnumerable<User>>> SearchByEmail([FromQuery] string email, [FromServices] IUserService userService)
    {
        var users = await userService.SearchByEmail(email);
        return Ok(users);
    }

    [HttpGet("search/name")]
    public async Task<ActionResult<IEnumerable<User>>> SearchByName([FromQuery] string name, [FromServices] IUserService userService)
    {
        var users = await userService.SearchByName(name);
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(
        UserCreateDto user, 
        [FromServices] IUserService userService
    )
    {
        await userService.Create(user);
        
        return Created();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UserUpdateDto user, [FromServices] IUserService userService)
    {
        if (id != user.Id) return BadRequest("Usuário Inválido!");

        await userService.Update(user);
        
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromServices] IUserService userService)
    {
        await userService.Remove(id);
        return NoContent();
    }
}