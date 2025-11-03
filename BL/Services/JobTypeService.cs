using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.Constants;
using BL.Dtos;
using BL.Models;
using BL.Services.Repo;
using Microsoft.EntityFrameworkCore;


namespace WebAPI.Services
{
    public class JobTypeService : ISqlRepository<ResponseJobTypeDto, CreateJobTypeDto, EditJobTypeDto>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public JobTypeService(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<ResponseJobTypeDto> CreateAsync(CreateJobTypeDto dto)
        {
            await VerifyUniqunes(dto.Name);

            var newJobType = _mapper.Map<JobType>(dto);

            _databaseContext.JobTypes.Add(newJobType);
            await _databaseContext.SaveChangesAsync();

            return _mapper.Map<ResponseJobTypeDto>(newJobType);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var foundJobType = await _databaseContext.JobTypes
                .Where(jt => !jt.IsDeleted)
                .FirstOrDefaultAsync(jt => jt.Id == id);

            if (foundJobType == null)
                throw new KeyNotFoundException(Messages.JobTypeNotFound + id);

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResponseJobTypeDto>> GetAllAsync()
        {
            return await _databaseContext.JobTypes
                .Where(jt => !jt.IsDeleted)
                .ProjectTo<ResponseJobTypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ResponseJobTypeDto?> GetByIdAsync(int id)
        {
            var JobTypeDto = await _databaseContext.JobTypes
                .Where(jt => !jt.IsDeleted && jt.Id == id)
                .ProjectTo<ResponseJobTypeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (JobTypeDto == null)
                throw new KeyNotFoundException(Messages.CountryNotFound + id);

            return JobTypeDto;
        }

        public async Task<bool> EditAsync(int id, EditJobTypeDto dto)
        {
            var jobType = await _databaseContext.JobTypes
                .Where(jt => !jt.IsDeleted && jt.Id == id)
                .FirstOrDefaultAsync();

            if (jobType == null)
                throw new KeyNotFoundException(Messages.CountryNotFound + id);

            await VerifyUniqunes(dto.Name, id);

            _mapper.Map(dto, jobType);

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task VerifyUniqunes(string jobTypeName, int? id = null)//je u create nemam id tek ga dobijeme nakon kreraja a to je vec kasno za provjeru
        {
            bool notUniqueJobType = await _databaseContext.JobTypes
                .AnyAsync(jt => jt.Name == jobTypeName && jt.Id != id);//trazimo isto ime ali razlicite Id jer nezelimo da trenutno
                                                                       //selektani entiete uspoređuje sa samim sobo  jer ce onda uvjek javlajti duplikate
            if (notUniqueJobType)
                throw new InvalidOperationException(Messages.DuplicateJobType);
        }

        public async Task<JobType> GetOrCreateAsync(CreateJobTypeDto dto)
        {
            var jobType = await _databaseContext.JobTypes
                .Where(jt => !jt.IsDeleted && jt.Name == dto.Name)
                .FirstOrDefaultAsync();

            if (jobType != null) 
                return jobType;

            var newJobType = await CreateAsync(dto);

            jobType = await _databaseContext.JobTypes.FindAsync(newJobType.Id);

            if (jobType == null) 
                throw new InvalidOperationException(Messages.CountryNotFound);

            return jobType;
        }
    }
}
