using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflix_Server.Models.Context;
using Netflix_Server.Models.MovieGroup;
using Netflix_Server.Models.MovieGroupDto;
using Netflix_Server.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _movieRep;
    IWebHostEnvironment _appEnvironment;
    public MoviesController(MovieContext context, IMapper mapper, IMovieRepository movieRepository, IWebHostEnvironment appEnvironment)
    {
        _movieRep = movieRepository;
        _appEnvironment = appEnvironment;
    }

    [HttpPost]
    public async Task<IActionResult> AddMovie([FromBody] MovieDto movieDto)
    {
        if (movieDto == null)
        {
            return BadRequest();
        }
        try
        {
            MovieDto movie = await _movieRep.AddMovie(movieDto);
            return Ok(movie);
        }
        catch
        {
            return BadRequest();
        }
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
    {
        var movieDtos = await _movieRep.GetMovies();
        return Ok(movieDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovieById(int id)
    {
        var movieDto = await _movieRep.GetMovieById(id);
        return movieDto == null ? NotFound() : Ok(movieDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDto movieDto)
    {
        if (movieDto == null || movieDto.Id != id) return BadRequest();
        var updatedMovie = await _movieRep.UpdateMovie(movieDto);

        return updatedMovie == null ? BadRequest() : Ok(updatedMovie);
    }

    [HttpPost("/film")]
    public async Task<IActionResult> UploadChunk([FromForm] IFormFile file, [FromForm] int chunkNumber, [FromForm] int totalChunks)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var uploadPath = Path.Combine(_appEnvironment.WebRootPath, "uploads");
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var tempFilePath = Path.Combine(uploadPath, file.FileName + ".tmp");

        using (var stream = new FileStream(tempFilePath, FileMode.Append))
        {
            await file.CopyToAsync(stream);
        }

        // Check if all chunks are uploaded
        if (chunkNumber == totalChunks - 1)
        {
            var finalFilePath = Path.Combine(uploadPath, file.FileName);
            System.IO.File.Move(tempFilePath, finalFilePath);
            return Ok(new { filePath = finalFilePath });
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        if (await _movieRep.RemoveMovieById(id))
        {
            return Ok();
        }

        return BadRequest();
    }
}
