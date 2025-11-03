using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.Constants;
using BL.Dtos;
using BL.Models;
using BL.Services.Repo;
using Microsoft.EntityFrameworkCore;


namespace BL.Services
{
    public class ContractorLocationService : ISqlRepository<ResponseContractorLocationDto, CreateContractorLocationDto, EditContractorLocationDto>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        public ContractorLocationService(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<ResponseContractorLocationDto> CreateAsync(CreateContractorLocationDto dto)
        {
            await VerifyUniqunes(dto);

            var newContractorLoction = _mapper.Map<ContractorLocation>(dto);

            _databaseContext.ContractorLocations.Add(newContractorLoction);
            await _databaseContext.SaveChangesAsync();

            return _mapper.Map<ResponseContractorLocationDto>(newContractorLoction);
        }


        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResponseContractorLocationDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseContractorLocationDto> GetByIdAsync(int id)
        {
            var contractorLocationDto = await _databaseContext.ContractorLocations
                .Where(cl => cl.Id == id)
                .ProjectTo<ResponseContractorLocationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (contractorLocationDto == null)
                throw new KeyNotFoundException(Messages.ContractorLocationNotFound + id);

            return contractorLocationDto;
        }

        public Task<bool> EditAsync(int id, EditContractorLocationDto dto)
        {
            throw new NotImplementedException();
        }

        private async Task VerifyUniqunes(CreateContractorLocationDto dto)
        {
            bool notUniqueContractorLocation = await _databaseContext.ContractorLocations
                    .AnyAsync(cl => cl.ContractorId == dto.ContractorId && cl.LocationId != dto.LocationId);
            if (notUniqueContractorLocation)
            {
                throw new InvalidOperationException(Messages.DuplicateContractorLocation);
            }
        }

        public async Task<ContractorLocation> GetOrCreateAsync(CreateContractorLocationDto dto)
        {
            var contracotrLocation = await _databaseContext.ContractorLocations.FirstOrDefaultAsync(cl => cl.ContractorId == dto.ContractorId && cl.LocationId == dto.LocationId);

            if (contracotrLocation != null)
                return contracotrLocation;

            var newContractorLocation = await CreateAsync(dto);

            contracotrLocation = await _databaseContext.ContractorLocations.FindAsync(newContractorLocation.Id);

            if (contracotrLocation == null) throw new InvalidOperationException(Messages.LocationNotFound);

            return contracotrLocation;
        }
    }
}
