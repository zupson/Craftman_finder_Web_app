using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.Constants;
using BL.Dtos;
using BL.Models;
using BL.Services.Repo;
using Microsoft.EntityFrameworkCore;


namespace BL.Services
{
    public class JobPostService : ISqlRepository<ResponseJobPostDto, CreateJobPostDto, EditJobPostDto>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ContractorService _contractorService;
        private readonly LocationService _locationService;
        private readonly ContractorLocationService _contractorLocationService;
        private readonly IMapper _mapper;

        public JobPostService(DatabaseContext databaseContext, ContractorService contractorService, LocationService locationService, ContractorLocationService contractorLocationService, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _contractorService = contractorService;
            _locationService = locationService;
            _contractorLocationService = contractorLocationService;
            _mapper = mapper;
        }

        public async Task<ResponseJobPostDto> CreateAsync(CreateJobPostDto dto)
        {
            await VerifyUniqunes(dto);
            var contractor = await _contractorService.GetOrCreateAsync( new CreateContractorDto { JobTypeName = dto.Contractor.JobTypeName, PersonId = dto.Contractor.PersonId, CompanyName = dto.Contractor.CompanyName, IsFreelancer = dto.Contractor.IsFreelancer, Person = dto.Contractor.Person, Locations = dto.Contractor.Locations });
            var location = await _locationService.GetOrCreateAsync(new CreateLocationDto { PostalCode = dto.Location.PostalCode, CountryName = dto.Location.CountryName, TownName = dto.Location.TownName });

            var contractorLocationDto = new CreateContractorLocationDto { ContractorId = contractor.Id, LocationId = location.Id };
            var contractorLocation = await _contractorLocationService.GetOrCreateAsync(contractorLocationDto);

            var responseContractorLocationDto = new ResponseContractorLocationDto { ContractorId = contractor.Id, LocationId = location.Id };

            var newJobPost = new JobPost
            {
                ContractorLocation = contractorLocation,
                IsDeleted = false,
            };
            _databaseContext.JobPosts.Add(newJobPost);
            await _databaseContext.SaveChangesAsync();

            return _mapper.Map<ResponseJobPostDto>(newJobPost);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var foundJobPost = await _databaseContext.JobPosts
                .Where(jp => !jp.IsDeleted)
                .FirstOrDefaultAsync(jp => jp.Id == id);

            if (foundJobPost == null)
                throw new KeyNotFoundException(Messages.JobPostNotFound + id);//

            foundJobPost.IsDeleted = true;

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResponseJobPostDto>> GetAllAsync()
        {
            return await _databaseContext.JobPosts
                .Where(jp => !jp.IsDeleted)
                .ProjectTo<ResponseJobPostDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ResponseJobPostDto?> GetByIdAsync(int id)
        {
            var jobPostDto = await _databaseContext.JobPosts
                .Where(jp => !jp.IsDeleted && jp.Id == id)
                .ProjectTo<ResponseJobPostDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (jobPostDto == null)
                throw new KeyNotFoundException(Messages.JobPostNotFound + id);

            return jobPostDto;
        }

        public async Task<bool> EditAsync(int id, EditJobPostDto dto)
        {
            var jobPostEntity = await _databaseContext.JobPosts
                .Include(jp => jp.ContractorLocation)
                .FirstOrDefaultAsync(jp => jp.Id == id && !jp.IsDeleted);

            if (jobPostEntity == null)
                throw new KeyNotFoundException(Messages.JobPostNotFound + id);

            await VerifyUniqunes(dto, jobPostEntity.Id);

            var contractor = await _contractorService.GetOrCreateAsync(_mapper.Map<CreateContractorDto>(dto.Contractor));
            var location = await _locationService.GetOrCreateAsync(_mapper.Map<CreateLocationDto>(dto.Location));

            var contractorLocation = await _contractorLocationService
                .GetOrCreateAsync(new CreateContractorLocationDto
                {
                    ContractorId = contractor.Id,
                    LocationId = location.Id
                });

            jobPostEntity.ContractorLocation = contractorLocation;

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        private async Task VerifyUniqunes(CreateJobPostDto dto, int? currJobPostId = null)
        {
            var contractor = await _contractorService.GetOrCreateAsync(dto.Contractor);
            var location = await _locationService.GetOrCreateAsync(dto.Location);
            bool notUniqueJobPost = await _databaseContext.JobPosts
                .Include(jp => jp.ContractorLocation)
                .Where(jp => !jp.IsDeleted)
                .Where(jp => currJobPostId == null || jp.Id != currJobPostId)
                .AnyAsync(jp => jp.ContractorLocation.ContractorId == contractor.Id &&
                                jp.ContractorLocation.LocationId == location.Id);

            if (notUniqueJobPost)
            {
                throw new InvalidOperationException(Messages.DuplicateTown);
            }
        }
        private async Task VerifyUniqunes(EditJobPostDto dto, int? currJobPostId = null)
        {
            var contractor = await _contractorService.GetOrCreateAsync(_mapper.Map<CreateContractorDto>(dto.Contractor));
            var location = await _locationService.GetOrCreateAsync(_mapper.Map<CreateLocationDto>(dto.Location));
            bool notUniqueJobPost = await _databaseContext.JobPosts
                .Include(jp => jp.ContractorLocation)
                .Where(jp => !jp.IsDeleted)
                .Where(jp => currJobPostId == null || jp.Id != currJobPostId)
                .AnyAsync(jp => jp.ContractorLocation.ContractorId == contractor.Id &&
                                jp.ContractorLocation.LocationId == location.Id);

            if (notUniqueJobPost)
            {
                throw new InvalidOperationException(Messages.DuplicateTown);
            }
        }
    }
}
