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
    public class ContractorService : ISqlRepository<ResponseContractorDto, CreateContractorDto, EditContractorDto>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly JobTypeService _jobTypeService;
        private readonly LocationService _locationService;
        private readonly PersonService _perosnService;
        private readonly RoleService _roleService;
        private readonly IMapper _mapper;

        public ContractorService(DatabaseContext databaseContext, JobTypeService jobTypeService, LocationService locationService, PersonService perosnService, IMapper mapper, RoleService roleService)
        {
            _databaseContext = databaseContext;
            _jobTypeService = jobTypeService;
            _locationService = locationService;
            _perosnService = perosnService;
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<ResponseContractorDto> CreateAsync(CreateContractorDto dto)
        {
            await using var transaction = await _databaseContext.Database.BeginTransactionAsync();//sprijecava da se samo određeni dio izvrsi(ILI SE IZVRSI SVE ILI SE ROLLBACK-A)
            //ako u kontroleru nuđe u catche automatski se rollbacka a ako zavrsi u tryu onda se commita 

            var jobType = await _jobTypeService.GetOrCreateAsync(new CreateJobTypeDto
            {
                Name = dto.JobTypeName
            });

            await VerifyUniqunes(dto);

            var person = await _perosnService.GetOrCreateAsync(
                dto.Person == null ? 
                throw new KeyNotFoundException(Messages.PersonNotFound + dto.Person) : 
                dto.Person);

            var roleName = dto.IsFreelancer == true ? 
                    Roles.Freelancer : 
                    Roles.CompanyAdmin;

            var roleDto = await _roleService.GetByRoleNameAsync(roleName);

            var role = await _databaseContext.Roles.FirstOrDefaultAsync(r => r.Id == roleDto.Id);
                
                //_mapper.Map<Role>(
            //    roleDto == null ? 
            //    throw new KeyNotFoundException(Messages.RoleNameNotFound + roleName) : 
            //    roleDto);

            if (!person.Roles.Any(r => r.Id == role.Id))
            {
                person.Roles.Add(role);
                await _databaseContext.SaveChangesAsync();
            }

            if (dto.IsFreelancer != true && string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                throw new InvalidOperationException(Messages.ContractorMustHaveCompanyOrBeFreelancer);
            }
            var contractor = _mapper.Map<Contractor>(dto);
            contractor.JobTypeId = jobType.Id;
            contractor.PersonId = person.Id;

            _databaseContext.Contractors.Add(contractor);
            await _databaseContext.SaveChangesAsync();

            foreach (var loc in dto.Locations)
            {
                var location = await _locationService.GetOrCreateAsync(loc);

                _databaseContext.ContractorLocations.Add(new ContractorLocation
                {
                    ContractorId = contractor.Id,
                    LocationId = location.Id,
                });
            }
            await _databaseContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var contractorDto = await GetByIdAsync(contractor.Id);

            return contractorDto == null ? 
                throw new KeyNotFoundException(Messages.ContractorNotFound + contractor.Id) : 
                contractorDto; 
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var transaction = await _databaseContext.Database.BeginTransactionAsync();//sprijecava da se samo određeni dio izvrsi(ILI SE IZVRSI SVE ILI SE ROLLBACK-A)
            //ako u kontroleru nuđe u catche automatski se rollbacka a ako zavrsi u tryu onda se commita 

            var contractor = await _databaseContext.Contractors
                .Include(c => c.ContractorLocations)
                .Include(c => c.JobType)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contractor == null)
                throw new KeyNotFoundException(Messages.ContractorNotFound + id);

            contractor.IsDeleted = true;
            foreach (var cl in contractor.ContractorLocations)
            {
                _databaseContext.ContractorLocations.Remove(cl);
            }

            await _databaseContext.SaveChangesAsync();
            await transaction.CommitAsync();

            bool jobTypeInUse = await _databaseContext.Contractors.AnyAsync(c => !c.IsDeleted && c.JobTypeId == contractor.JobType.Id);

            if (!jobTypeInUse)
                await _jobTypeService.DeleteAsync(contractor.JobType.Id);

            return true;
        }

        public async Task<IEnumerable<ResponseContractorDto>> GetAllAsync()
        {
            return await _databaseContext.Contractors
                .Where(c => !c.IsDeleted)
                .ProjectTo<ResponseContractorDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PagingDto<ResponseContractorDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _databaseContext.Contractors
                .Where(c => !c.IsDeleted)
                .ProjectTo<ResponseContractorDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var pagedItems = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return new PagingDto<ResponseContractorDto>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ResponseContractorDto> GetByIdAsync(int id)
        {
            var contractorDto = await _databaseContext.Contractors
                .Where(c => !c.IsDeleted && c.Id == id)
                .ProjectTo<ResponseContractorDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (contractorDto == null)
                throw new KeyNotFoundException(Messages.ContractorNotFound + id);

            return contractorDto;
        }

        public async Task<bool> EditAsync(int id, EditContractorDto dto)
        {
            await using var transaction = await _databaseContext.Database.BeginTransactionAsync();//sprijecava da se samo određeni dio izvrsi(ILI SE IZVRSI SVE ILI SE ROLLBACK-A)
            //ako u kontroleru nuđe u catche automatski se rollbacka a ako zavrsi u tryu onda se commita 

            await VerifyUniqunes(dto, id);

            var contractor = await _databaseContext.Contractors
                .Include(c => c.ContractorLocations)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (contractor == null)
                throw new KeyNotFoundException(Messages.ContractorNotFound + id);

            if (!string.IsNullOrEmpty(dto.JobTypeName))
            {
                var jobType = await _jobTypeService.GetOrCreateAsync(new CreateJobTypeDto { Name = dto.JobTypeName });
                contractor.JobTypeId = jobType.Id;
            }

            if (dto.Person != null)
            {
                var editPersonDto = _mapper.Map<EditPersonDto>(dto.Person);
                await _perosnService.EditAsync(dto.PersonId,editPersonDto);
            }
            _mapper.Map(dto, contractor);

            _databaseContext.ContractorLocations.RemoveRange(contractor.ContractorLocations);

            foreach (var locDto in dto.Locations)
            {
                var location = await _locationService.GetOrCreateAsync(new CreateLocationDto { Id = locDto.Id, PostalCode = locDto.PostalCode, TownName = locDto.TownName, CountryName = locDto.CountryName });
                _databaseContext.ContractorLocations.Add(new ContractorLocation
                {
                    ContractorId = contractor.Id,
                    LocationId = location.Id
                });
            }

            await _databaseContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }

        public async Task<Contractor> GetOrCreateAsync(CreateContractorDto dto)
        {
            Contractor? contractor = null;
            if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                contractor = await _databaseContext.Contractors
                                                        .Where(c => !c.IsDeleted)
                                                        .FirstOrDefaultAsync(c => c.CompanyName == dto.CompanyName);

                if (contractor != null) return contractor;

                var newCompany = await CreateAsync(dto);
                contractor = await _databaseContext.Contractors.FindAsync(newCompany.Id);

                if (contractor != null) return contractor;
            }

            if (dto.Person != null)
            {
                var person = await _perosnService.GetOrCreateAsync(dto.Person);

                contractor = await _databaseContext.Contractors
                                                        .Where(c => !c.IsDeleted)
                                                        .FirstOrDefaultAsync(c => c.PersonId == person.Id);

                if (contractor != null) return contractor;

                var newFreelancer = await CreateAsync(dto);
                contractor = await _databaseContext.Contractors.FindAsync(newFreelancer.Id);

                if (contractor != null) return contractor;
            }

            throw new InvalidOperationException(Messages.ContractorNotFound);
        }

        private async Task VerifyUniqunes(CreateContractorDto dto, int? id = null)
        {
            if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                bool notUniquecompany = await _databaseContext.Contractors
                    .AnyAsync(c => !c.IsDeleted && c.CompanyName == dto.CompanyName && c.Id != id);

                if (notUniquecompany)
                    throw new InvalidOperationException(Messages.DuplicateCompanyInContractor);
            }

            if (dto.Person != null)
            {
                int? personId = dto.PersonId;
                if (personId == null && dto.PersonId != 0)
                    personId = dto.PersonId;

                if (personId.HasValue)
                {
                    bool personExists = await _databaseContext.Contractors
                        .AnyAsync(c => !c.IsDeleted && c.PersonId == personId.Value && c.Id != id);

                    if (personExists)
                        throw new InvalidOperationException(Messages.DuplicateFreelancerInContractor);
                }
            }
        }
        private async Task VerifyUniqunes(EditContractorDto dto, int? id = null)
        {
            if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                bool notUniquecompany = await _databaseContext.Contractors
                    .AnyAsync(c => !c.IsDeleted && c.CompanyName == dto.CompanyName && c.Id != id);

                if (notUniquecompany)
                    throw new InvalidOperationException(Messages.DuplicateCompanyInContractor);
            }

            if (dto.Person != null)
            {
                int? personId = dto.PersonId;
                if (personId == null && dto.PersonId != 0)
                    personId = dto.PersonId;

                if (personId.HasValue)
                {
                    bool personExists = await _databaseContext.Contractors
                        .AnyAsync(c => !c.IsDeleted && c.PersonId == personId.Value && c.Id != id);

                    if (personExists)
                        throw new InvalidOperationException(Messages.DuplicateFreelancerInContractor);
                }
            }
        }
    }
}
