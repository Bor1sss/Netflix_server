using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflix_Server.Models.Context;
using Netflix_Server.Models.MovieGroup;
using Netflix_Server.Models.MovieGroupDto;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly MovieContext _context;
    private readonly IMapper _mapper;

    public GenresController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
    {
        var genres = await _context.Genres.ToListAsync();
        var genreDtos = _mapper.Map<IEnumerable<GenreDto>>(genres);
        return Ok(genreDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetGenreById(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
        {
            return NotFound();
        }

        var genreDto = _mapper.Map<GenreDto>(genre);
        return Ok(genreDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddGenre([FromBody] GenreDto genreDto)
    {
        if (genreDto == null)
        {
            return BadRequest();
        }

        var genre = _mapper.Map<Genre>(genreDto);
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<GenreDto>(genre));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGenre(int id, [FromBody] GenreDto genreDto)
    {
        if (id != genreDto.Id || genreDto == null)
        {
            return BadRequest();
        }

        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
        {
            return NotFound();
        }

        _mapper.Map(genreDto, genre);
        _context.Genres.Update(genre);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
        {
            return NotFound();
        }

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
