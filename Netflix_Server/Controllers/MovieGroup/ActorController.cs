using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflix_Server.Models.Context;
using Netflix_Server.Models.MovieGroup;
using Netflix_Server.Models.MovieGroupDto;
using Netflix_Server.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly IActorRepository _actorRep;

    public ActorsController(MovieContext context, IMapper mapper, IActorRepository actorRepository)
    {
        _actorRep = actorRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
    {
        var actorDtos = await _actorRep.GetActors();
        return Ok(actorDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDto>> GetActorById(int id)
    {
        var actorDto = await _actorRep.GetActorById(id);
        return actorDto == null ? NotFound() : Ok(actorDto);
    }
    [HttpPost]
    public async Task<IActionResult> AddActor([FromBody] ActorDto actorDto)
    {
        if (actorDto == null)
        {
            return BadRequest();
        }
        try
        {
            ActorDto actor = await _actorRep.AddActor(actorDto);
            return Ok(actor);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActor(int id, [FromBody] ActorDto actorDto)
    {
        if (actorDto == null || actorDto.Id != id) return BadRequest();
        var updatedActor = await _actorRep.UpdateActor(actorDto);

        return updatedActor == null ? BadRequest() : Ok(updatedActor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActor(int id)
    {
        if (await _actorRep.RemoveActorById(id))
        {
            return Ok();
        }

        return BadRequest();
    }
}
