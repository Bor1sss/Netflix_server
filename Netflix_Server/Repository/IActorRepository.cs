﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Netflix_Server.Models.Context;
using Netflix_Server.Models.MovieGroup;
using Netflix_Server.Models.MovieGroupDto;
using System.Linq;

namespace Netflix_Server.Repository
{
    public interface IActorRepository
    {
        Task<ActorDto> AddActor(ActorDto actor);
        Task<bool> RemoveActorById(int id);
        Task<ICollection<ActorDto>> GetActors();
        Task<ICollection<ActorDto>> GetActors(FilterMovieDto filter);
        Task<ActorDto> GetActorById(int id);
        Task<ActorDto> UpdateActor(ActorDto actor);
    }

    public class ActorRepository : IActorRepository
    {

        private readonly MovieContext _context;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRep;

        public ActorRepository(MovieContext context, IMapper mapper, IImageRepository imageRep)
        {
            _context = context;
            _mapper = mapper;
            _imageRep = imageRep;
        }

        public async Task<Actor> ConvertFromDto(ActorDto dto)
        {
            var actor = new Actor
            {
                Name = dto.Name,
                ActorImages = new List<ActorImage>()
            };

            if (dto.Images != null && dto.Images.Length > 0)
            {
                // Assuming Image class has Id property
                var imageIds = dto.Images.Select(img => img.Id).ToList();
                var images = await _context.Images.Where(img => imageIds.Contains(img.Id)).ToListAsync();

                // Map images to ActorImages
                var actorImages = images.Select(image => new ActorImage
                {
                    ImageId = image.Id,
                    ActorId = actor.Id // This will be set after actor is saved and Id is generated
                }).ToList();

                actor.ActorImages = actorImages;
            }

            return actor;
        }


        public async Task<ActorDto> AddActor(ActorDto dto)
        {
            // Convert from DTO to Actor model
            var actor = await ConvertFromDto(dto);
            actor.ActorImages?.Clear();  // Clear existing images if any

            // Add actor to the context
            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();

            // If there are images in the DTO, save and associate them with the actor
            if (dto.Images != null)
            {
                var images = await _imageRep.SaveNewImagesFromDto(dto);
                var actorImages = images.Select(i => new ActorImage { ImageId = i.Id, ActorId = actor.Id }).ToList();

                _context.ActorImages.AddRange(actorImages);
                await _context.SaveChangesAsync();
                actor.ActorImages = actorImages;
            }

            // Map the saved actor back to a DTO
            return _mapper.Map<ActorDto>(actor);
        }


        public async Task<ActorDto> GetActorById(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.ActorImages).ThenInclude(ai => ai.Image)
                .Include(a => a.Movies).ThenInclude(m => m.MovieImages).ThenInclude(mi => mi.Image)
                .Include(a => a.Movies).ThenInclude(m => m.Remark)
                .Include(a => a.Movies).ThenInclude(m => m.Rating)
                .Include(a => a.Movies).ThenInclude(m => m.Genres)
                .Include(a => a.Movies).ThenInclude(m => m.Director).ThenInclude(d => d.DirectorImages).ThenInclude(di => di.Image)
                .Include(a => a.Movies).ThenInclude(m => m.Company).ThenInclude(c => c.CompanyImages).ThenInclude(ci => ci.Image)
                .FirstOrDefaultAsync(a => a.Id == id);

            return _mapper.Map<ActorDto>(actor);
        }


        public async Task<ICollection<ActorDto>> GetActors()
        {
            var actors = await _context.Actors
                .Include(a => a.ActorImages).ThenInclude(ai => ai.Image)
                .Include(a => a.Movies).ThenInclude(m => m.MovieImages).ThenInclude(mi => mi.Image)
                .Include(a => a.Movies).ThenInclude(m => m.Remark)
                .Include(a => a.Movies).ThenInclude(m => m.Rating)
                .Include(a => a.Movies).ThenInclude(m => m.Genres)
                .Include(a => a.Movies).ThenInclude(m => m.Director).ThenInclude(d => d.DirectorImages).ThenInclude(di => di.Image)
                .Include(a => a.Movies).ThenInclude(m => m.Company).ThenInclude(c => c.CompanyImages).ThenInclude(ci => ci.Image)
                .ToListAsync();

            return _mapper.Map<List<ActorDto>>(actors);
        }


        public async Task<ICollection<ActorDto>> GetActors(FilterMovieDto filter)
        {
            var query = _context.Actors
                .Include(a => a.ActorImages).ThenInclude(ai => ai.Image)
                .Include(a => a.Movies)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchSubstring))
            {
                query = query.Where(a => a.Name.ToLower().Contains(filter.SearchSubstring.ToLower()));
            }

            if (filter.GenreIds?.Any() == true)
            {
                query = query.Where(a => a.Movies.Any(m => m.Genres.Any(g => filter.GenreIds.Contains(g.Id))));
            }

            if (query.Count() == 0) return new List<ActorDto>();

            query = query
                .OrderBy(a => a.Id)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize);

            var actors = await query.ToListAsync();
            return _mapper.Map<List<ActorDto>>(actors);
        }


        public async Task<bool> RemoveActorById(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.ActorImages)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null)
            {
                return false;
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<ActorDto> UpdateActor(ActorDto dto)
        {
            var existingActor = await _context.Actors
                .Include(a => a.ActorImages)
                .FirstOrDefaultAsync(a => a.Id == dto.Id);

            if (existingActor == null)
            {
                return null;
            }

            existingActor.Name = dto.Name;

            // Update images
            if (dto.Images != null)
            {
                // Remove existing images
                var existingImageIds = existingActor.ActorImages.Select(ai => ai.ImageId).ToList();
                var imagesToRemove = existingActor.ActorImages.Where(ai => !dto.Images.Select(i => i.Id).Contains(ai.ImageId)).ToList();
                foreach (var imageToRemove in imagesToRemove)
                {
                    existingActor.ActorImages.Remove(imageToRemove);
                }

                // Add new images
                var imagesToAdd = dto.Images.Where(i => i.Id.HasValue && !existingImageIds.Contains(i.Id.Value)).ToList();
                foreach (var imageDto in imagesToAdd)
                {
                    var image = await _context.Images.FindAsync(imageDto.Id.Value);
                    if (image != null)
                    {
                        existingActor.ActorImages.Add(new ActorImage { ActorId = existingActor.Id, ImageId = image.Id });
                    }
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return _mapper.Map<ActorDto>(existingActor);
        }

    }
}
