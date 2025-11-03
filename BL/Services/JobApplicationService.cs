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
    public class JobApplicationService : ISqlRepository<ResponseJobApplicationDto, CreateJobApplicationDto, EditJobApplicationDto>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly PersonService _personService;
        private readonly IMapper _mapper;

        public JobApplicationService(DatabaseContext databaseContext, PersonService personService, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _personService = personService;
            _mapper = mapper;
        }

        public async Task<ResponseJobApplicationDto> CreateAsync(CreateJobApplicationDto dto)
        {
            var person = await _personService.GetOrCreateAsync(dto.Person);

            await VerifyUniqunes(dto);

            var newJobApplication = new JobApplication
            {
                JobPostId = dto.JobPostId,
                PersonId = person.Id,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _databaseContext.JobApplications.Add(newJobApplication);
            await _databaseContext.SaveChangesAsync();

            var jobApplicationDto = await _databaseContext.JobApplications
                .Where(ja => ja.Id == newJobApplication.Id)
                .ProjectTo<ResponseJobApplicationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (jobApplicationDto == null)
                throw new Exception(Messages.JobApplicationNotFound);

            return jobApplicationDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var jobApplicationFound = await _databaseContext.JobApplications
                .Where(ja => !ja.IsDeleted && ja.Id == id)
                .FirstOrDefaultAsync();

            if (jobApplicationFound == null)
                throw new KeyNotFoundException(Messages.JobApplicationNotFound + id);//

            jobApplicationFound.IsDeleted = true;

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResponseJobApplicationDto>> GetAllAsync()
        {
            return await _databaseContext.JobApplications
                .Where(ja => !ja.IsDeleted)
                .ProjectTo<ResponseJobApplicationDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ResponseJobApplicationDto?> GetByIdAsync(int id)
        {
            var jobApplicationDto = await _databaseContext.JobApplications
            .Where(ja => !ja.IsDeleted && ja.Id == id)
            .ProjectTo<ResponseJobApplicationDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

            if (jobApplicationDto == null)
                throw new KeyNotFoundException(Messages.JobApplicationNotFound + id);

            return jobApplicationDto;
        }

        public async Task<bool> EditAsync(int id, EditJobApplicationDto dto)
        {
            var jobApplication = await _databaseContext.JobApplications
                .Where(ja => !ja.IsDeleted && ja.Id == id)
                .FirstOrDefaultAsync();

            if (jobApplication == null)
                throw new KeyNotFoundException(Messages.JobApplicationNotFound + id);

            await VerifyUniqunes(dto, jobApplication.Id);

            _mapper.Map(dto, jobApplication);

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        private async Task VerifyUniqunes(CreateJobApplicationDto dto, int? currJobApplicationId = null)
        {
            var person = await _personService.GetOrCreateAsync(dto.Person);
            var notUniqueJobApplication = await _databaseContext.JobApplications
                .Where(ja => !ja.IsDeleted
                     && ja.PersonId == person.Id
                     && ja.JobPostId == dto.JobPostId
                     && (!currJobApplicationId.HasValue || ja.Id != currJobApplicationId.Value))
                .AnyAsync();

            if (notUniqueJobApplication)
                throw new InvalidOperationException(Messages.DuplicateJobApplication);
        }

        private async Task VerifyUniqunes(EditJobApplicationDto dto, int? currJobApplicationId = null)
        {
            var registerPersonDto = _mapper.Map<RegisterPersonDto>(dto.Person);

            var person = await _personService.GetOrCreateAsync(registerPersonDto);

            var notUniqueJobApplication = await _databaseContext.JobApplications
                .Where(ja => !ja.IsDeleted
                     && ja.PersonId == person.Id
                     && ja.JobPostId == dto.JobPostId
                     && (!currJobApplicationId.HasValue || ja.Id != currJobApplicationId.Value))
                .AnyAsync();

            if (notUniqueJobApplication)
                throw new InvalidOperationException(Messages.DuplicateJobApplication);
        }
    }
}
