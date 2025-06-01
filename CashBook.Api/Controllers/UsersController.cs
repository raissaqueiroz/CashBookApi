using CashBook.Domain.Entities;
using CashBook.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CashBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
     [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll([FromServices] IUserRepository userRepository)
    {
        var users = await userRepository.GetAll();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetById(Guid id, [FromServices] IUserRepository userRepository)
    {
        var user = await userRepository.GetById(id);
        if (user is null) return NotFound();
        return Ok(user);
    }

    [HttpGet("search/email")]
    public async Task<ActionResult<IEnumerable<User>>> SearchByEmail([FromQuery] string email, [FromServices] IUserRepository userRepository)
    {
        var users = await userRepository.SearchByEmail(email);
        return Ok(users);
    }

    [HttpGet("search/name")]
    public async Task<ActionResult<IEnumerable<User>>> SearchByName([FromQuery] string name, [FromServices] IUserRepository userRepository)
    {
        var users = await userRepository.SearchByName(name);
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user, [FromServices] IUserRepository userRepository)
    {
        await userRepository.Create(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, User user, [FromServices] IUserRepository userRepository)
    {
        if (id != user.Id) return BadRequest("ID da URL difere do corpo da requisição.");

        var existing = await userRepository.GetById(id);
        if (existing is null) return NotFound();

        await userRepository.Update(user);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromServices] IUserRepository userRepository)
    {
        var existing = await userRepository.GetById(id);
        if (existing is null) return NotFound();

        await userRepository.Remove(existing.Id);
        return NoContent();
    }
}