using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.Constants;
using BL.Dtos;
using BL.Models;
using BL.Services;
using BL.Services.Repo;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Services
{
    public class TownService : ISqlRepository<ResponseTownDto, CreateTownDto, EditTownDto>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly CountryService _countryService;
        private readonly IMapper _mapper;

        public TownService(DatabaseContext databaseContext, CountryService countryService, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _countryService = countryService;
            _mapper = mapper;
        }

        public async Task<ResponseTownDto> CreateAsync(CreateTownDto dto)
        {

            var country = await _countryService
                .GetOrCreateAsync(new CreateCountryDto
                {
                    Name = dto.CountryName
                });

            await VerifyUniqunes(dto.Name);
            
            var newTown = _mapper.Map<Town>(dto);
            newTown.Country = country;

            _databaseContext.Towns.Add(newTown);

            await _databaseContext.SaveChangesAsync();
            return _mapper.Map<ResponseTownDto>(newTown);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var townFound = await _databaseContext.Towns
                .Include(t => t.Locations)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (townFound == null)
                throw new KeyNotFoundException(Messages.TownNotFound + id);

            foreach (var location in townFound.Locations)
            {
                location.IsDeleted = true;
            }

            _databaseContext.Towns.Remove(townFound);

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResponseTownDto>> GetAllAsync()
        {
            return await _databaseContext.Towns
                .ProjectTo<ResponseTownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ResponseTownDto?> GetByIdAsync(int id)
        {
            var townDto = await _databaseContext.Towns
                .Where(t => t.Id == id)
                .ProjectTo<ResponseTownDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (townDto == null)
                throw new KeyNotFoundException(Messages.TownNotFound + id);

            return _mapper.Map<ResponseTownDto>(townDto);
        }

        public async Task<bool> EditAsync(int id, EditTownDto dto)
        {
           await VerifyUniqunes(dto.Name, id);

           var town = await _databaseContext.Towns
                .Include(t => t.Country)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (town == null)
                throw new KeyNotFoundException(Messages.TownNotFound + id);//
            
            

            if (!string.IsNullOrEmpty(dto.CountryName))
            {
                var country = await _countryService
                    .GetOrCreateAsync(new CreateCountryDto { Name = dto.CountryName });
                town.CountryId = country.Id;
            }

            _mapper.Map(dto, town);//ovakav poziv mappera editira 

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        private async Task VerifyUniqunes(string townName, int? id = null)//je u create nemam id tek ga dobijeme nakon kreraja a to je vec kasno za provjeru
        {
            if(!string.IsNullOrEmpty(townName))
            {
                bool notUniqueTown = await _databaseContext.Towns
                .AnyAsync(t => t.Name == townName && t.Id != id);//trazimo isto ime ali razlicite Id jer nezelimo da trenutno
                                                                 //selektani entiete uspoređuje sa samim sobo  jer ce onda uvjek javlajti duplikate
                if (notUniqueTown)
                    throw new InvalidOperationException(Messages.DuplicateTown);
            }
            
        }

        internal async Task<Town> GetOrCreateAsync(CreateTownDto dto)
        {
            var town = await _databaseContext.Towns
                .FirstOrDefaultAsync(t => t.Name == dto.Name);

            if (town != null)
                return town;

            var newTown = await CreateAsync(dto);
            //ovdje provjeravaj i bacaj exc!! ako je null jer onda create puca
            //provjeri di to hendlati
            town = await _databaseContext.Towns.FindAsync(newTown.Id);
            if (town == null) 
                throw new InvalidOperationException(Messages.TownNotFound);

            return town;
        }
    }
}
