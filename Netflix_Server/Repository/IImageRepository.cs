using AutoMapper;
using Netflix_Server.Models.Context;
using Netflix_Server.Models.MovieGroup;
using Netflix_Server.Models.MovieGroupDto;

namespace Netflix_Server.Repository
{
    public interface IImageRepository
    {
        ICollection<Image> GetImagesFromDto(ICollection<ImageDto> images);
        Task<ICollection<Image>> SaveNewImagesFromDto(MovieDto dto);
        Task<ICollection<Image>> SaveNewImagesFromDto(ActorDto dto);

    }

    public class ImageRepository : IImageRepository
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public ImageRepository(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<Image>> SaveNewImagesFromDto(MovieDto dto)
        {
            if (dto.Images == null) return new List<Image>();

            var images = _mapper.Map<List<Image>>(dto.Images.Where(x => x.Id == null));
            await _context.Images.AddRangeAsync(images);
            await _context.SaveChangesAsync();

            return images;
        }

        public async Task<ICollection<Image>> SaveNewImagesFromDto(ActorDto dto)
        {
            if (dto.Images == null) return new List<Image>();

            var images = _mapper.Map<List<Image>>(dto.Images.Where(x => x.Id == null));
            await _context.Images.AddRangeAsync(images);
            await _context.SaveChangesAsync();

            return images;
        }

        public ICollection<Image> GetImagesFromDto(ICollection<ImageDto> images)
        {
            return _mapper.Map<List<Image>>(images);
        }
    }
}