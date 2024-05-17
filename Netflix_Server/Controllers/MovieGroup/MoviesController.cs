using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflix_Server.Controllers.FTP;
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
    private readonly FTPClient _ftpClient;
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

    [HttpGet("/film")]
    public async Task<IActionResult> GetF(string fileName)
    {
        string ftpServerUrl = "ftp://127.0.0.1";
        string ftpUsername = "Anonymous";
        string ftpPassword = "password";

        string filePath = $"/data/{fileName}";
        FTPClient _ftpClient = new FTPClient();
        Stream fileStream = await _ftpClient.DownloadFile(ftpServerUrl, ftpUsername, ftpPassword, filePath);

        if (fileStream != null)
        {
            // Загрузка завершена, возвращаем файл
            var b = File(fileStream, "application/octet-stream");
            return b;
        }
        else
        {
            // Если загрузка не удалась, возвращаем ошибку 500
            return StatusCode(500, "Failed to download file.");
        }
    }
    [HttpPost("/film")]
    public async Task<IActionResult> UploadChunk([FromForm] IFormFile file, [FromForm] int chunkNumber, [FromForm] int totalChunks, [FromForm] string ftpUsername, [FromForm] string ftpPassword)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл не загружен.");
            }

            if (string.IsNullOrWhiteSpace(ftpUsername) || string.IsNullOrWhiteSpace(ftpPassword))
            {
                return BadRequest("Отсутствуют учетные данные FTP.");
            }

            var uploadPath = Path.Combine(_appEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var tempFilePath = Path.Combine(uploadPath, file.FileName + ".tmp");

            // Добавляем текущий чанк к временному файлу
            using (var stream = new FileStream(tempFilePath, FileMode.Append))
            {
                await file.CopyToAsync(stream);
            }

            // Проверяем, загружены ли все чанки
            if (chunkNumber == totalChunks - 1)
            {
                try
                {
                    var finalFilePath = Path.Combine(uploadPath, file.FileName);
                    System.IO.File.Move(tempFilePath, finalFilePath);

                    // Загружаем окончательный файл на FTP сервер
                    using (var fileStream = System.IO.File.OpenRead(finalFilePath))
                    {
                        FTPClient _ftpClient = new FTPClient();
                        await _ftpClient.UploadFile("ftp://127.0.0.1/data", ftpUsername, ftpPassword, file.FileName, fileStream);
                    }

                    // Опционально удаляем файл с сервера после загрузки
                    System.IO.File.Delete(finalFilePath);

                    return Ok(new { filePath = finalFilePath });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Failed to upload file: {ex.Message}");
                }
            }

            return Ok("Чанк загружен успешно.");
        }
        catch
        {
            return BadRequest();
        }
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
