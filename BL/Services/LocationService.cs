using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.Constants;
using BL.Dtos;
using BL.Models;
using BL.Services.Repo;
using Microsoft.EntityFrameworkCore;
using WebAPI.Services;

namespace BL.Services
{
    public class LocationService : ISqlRepository<ResponseLocationDto, CreateLocationDto, EditLocationDto>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly TownService _townService;
        private readonly IMapper _mapper;
        public LocationService(DatabaseContext databaseContext, TownService townService, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _townService = townService;
            _mapper = mapper;
        }

        public async Task<ResponseLocationDto> CreateAsync(CreateLocationDto dto)
        {
            var town = await _townService.GetOrCreateAsync(
                new CreateTownDto
                {
                    Name = dto.TownName,
                    CountryName = dto.CountryName
                });

            await VerifyUniqunes(dto.PostalCode);

            var location = _mapper.Map<Location>(dto);
            location.TownId = town.Id;
            location.IsDeleted = false;

            _databaseContext.Locations.Add(location);

            await _databaseContext.SaveChangesAsync();
            return _mapper.Map<ResponseLocationDto>(location);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var foundLocation = await _databaseContext.Locations
                .Where(l => !l.IsDeleted)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (foundLocation == null)
                throw new KeyNotFoundException(Messages.LocationNotFound + id);
            foundLocation.IsDeleted = true;

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResponseLocationDto>> GetAllAsync()
        {
            return await _databaseContext.Locations
                .Where(l => !l.IsDeleted)
                .ProjectTo<ResponseLocationDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ResponseLocationDto?> GetByIdAsync(int id)
        {
            var locationDto = await _databaseContext.Locations
                .Where(l => !l.IsDeleted && l.Id == id)
                .ProjectTo<ResponseLocationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (locationDto == null)
                throw new KeyNotFoundException(Messages.LocationNotFound + id);

            return locationDto;
        }

        public async Task<bool> EditAsync(int id, EditLocationDto dto)
        {
            var town = await _townService.GetOrCreateAsync(
                new CreateTownDto
                {
                    Name = dto.TownName,
                    CountryName =
                    dto.CountryName
                });

            await VerifyUniqunes(dto.PostalCode, id);

            var location = await _databaseContext.Locations
                .Where(l => !l.IsDeleted)
                .Include(l => l.Town)
                    .ThenInclude(l => l.Country)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
                throw new KeyNotFoundException(Messages.LocationNotFound + id);

            location.TownId = town.Id;
            _mapper.Map(dto, location);

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        private async Task VerifyUniqunes(string postalCode, int? id = null)//je u create nemam id tek ga dobijeme nakon kreraja a to je vec kasno za provjeru
        {
            if (!string.IsNullOrEmpty(postalCode))
            {
                bool notUniqueLocation = await _databaseContext.Locations
                    .Where(l => !l.IsDeleted)
                    .AnyAsync(l => l.PostalCode == postalCode && l.Id != id);//trazimo isto ime ali razlicite Id jer nezelimo da trenutno
                                                                             //selektani entiete uspoređuje sa samim sobo  jer ce onda uvjek javlajti duplikate
                if (notUniqueLocation)
                    throw new InvalidOperationException(Messages.DuplicateLocation);
            }
        }

        internal async Task<Location> GetOrCreateAsync(CreateLocationDto dto)
        {
            var location = await _databaseContext.Locations
                .Include(l => l.Town)
                    .ThenInclude(l => l.Country)
                .FirstOrDefaultAsync(l => l.PostalCode == dto.PostalCode);

            if (location != null)
                return location;

            var newLocation = await CreateAsync(dto);

            location = await _databaseContext.Locations
                .FindAsync(newLocation.Id);

            if (location == null)
                throw new KeyNotFoundException(Messages.LocationNotFound);

            return location;
        }
    }
}
