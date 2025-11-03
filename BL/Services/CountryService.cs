using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.Constants;
using BL.Dtos;
using BL.Models;
using BL.Services.Repo;
using Microsoft.EntityFrameworkCore;

namespace BL.Services
{
    public class CountryService : ISqlRepository<ResponseCountryDto, CreateCountryDto, EditCountryDto>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public CountryService(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<ResponseCountryDto> CreateAsync(CreateCountryDto dto)
        {
            await VerifyUniqunes(dto.Name);

            var newCountry = _mapper.Map<CreateCountryDto, Country>(dto);

            _databaseContext.Countries.Add(newCountry);
            await _databaseContext.SaveChangesAsync();

            return _mapper.Map<ResponseCountryDto>(newCountry);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var countryFound = await _databaseContext.Countries.FindAsync(id);

            if (countryFound == null)
                throw new KeyNotFoundException(Messages.CountryNotFound + id);

            _databaseContext.Countries.Remove(countryFound);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResponseCountryDto>> GetAllAsync()
        {
            return await _databaseContext.Countries
                .ProjectTo<ResponseCountryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ResponseCountryDto?> GetByIdAsync(int id)
        {
            var countryDto = await _databaseContext.Countries
                .Where(c => c.Id == id)
                .ProjectTo<ResponseCountryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (countryDto == null)
                throw new KeyNotFoundException(Messages.CountryNotFound + id);

            return countryDto;
        }

        public async Task<bool> EditAsync(int id, EditCountryDto dto)
        {
            await VerifyUniqunes(dto.Name, id);

            var countryFound = await _databaseContext.Countries.FindAsync(id);

            if (countryFound == null)
                throw new KeyNotFoundException(Messages.CountryNotFound + id);

            _mapper.Map(dto, countryFound);

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task VerifyUniqunes(string countryName, int? id = null)//je u create nemam id tek ga dobijeme nakon kreraja a to je vec kasno za provjeru
        {
            if (!string.IsNullOrEmpty(countryName))
            {
                bool notUniqueCountry = await _databaseContext.Countries
                    .AnyAsync(c => c.Name == countryName && c.Id != id);//trazimo isto ime ali razlicite Id jer nezelimo da trenutno
                                                                        //selektani entiete uspoređuje sa samim sobo  jer ce onda uvjek javlajti duplikate
                if (notUniqueCountry)
                    throw new InvalidOperationException(Messages.DuplicateCountry);
            }
        }

        internal async Task<Country> GetOrCreateAsync(CreateCountryDto dto)
        {
            var country = await _databaseContext.Countries
                .FirstOrDefaultAsync(c => c.Name == dto.Name);

            if (country != null)
                return country;

            var newCountry = await CreateAsync(dto);

            country = await _databaseContext.Countries.FindAsync(newCountry.Id);

            if (country == null)
                throw new InvalidOperationException(Messages.CountryNotFound);

            return country;
        }
    }
}
