using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflix_Server.Models.Context;
using Netflix_Server.Models.MovieGroupDto;
using Netflix_Server.Models.SupportGroup;

namespace Netflix_Server.Controllers.MovieGroup;



[ApiController]
[Route("api/[controller]")]
public class FaqController : Controller
{
    private readonly MovieContext _context;
    private readonly IMapper _mapper;

    public FaqController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Faq>>> GetRatings()
    {
        var faqs = await _context.Faqs.ToListAsync();
        return Ok(faqs);
    }
}
