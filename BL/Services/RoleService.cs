using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.Constants;
using BL.Dtos;
using BL.Models;
using BL.Services.Repo;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Services
{
    public class RoleService : ISqlRepository<ResponseRoleDto, CreateRoleDto, EditRoleDto>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        public RoleService(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<ResponseRoleDto> CreateAsync(CreateRoleDto dto)
        {
            await VerifyUniqunes(dto.Name);

            var newRole = _mapper.Map<Role>(dto);
            _databaseContext.Roles.Add(newRole);

            await _databaseContext.SaveChangesAsync();
            return _mapper.Map<ResponseRoleDto>(newRole);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var roleFound = await _databaseContext.Roles.FindAsync(id);
            if (roleFound == null)
                throw new KeyNotFoundException(Messages.RoleIdNotFound + id);

            //tu implementiraj brisnaje m:N
            _databaseContext.Roles.Remove(roleFound);

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResponseRoleDto>> GetAllAsync()
        {
            return await _databaseContext.Roles
                .ProjectTo<ResponseRoleDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ResponseRoleDto?> GetByIdAsync(int id)
        {
            var roleDto = await _databaseContext.Roles
                .Where(r => r.Id == id)
                .ProjectTo<ResponseRoleDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (roleDto == null)
                throw new KeyNotFoundException(Messages.RoleIdNotFound + id);

            return roleDto;
        }

        public async Task<ResponseRoleDto?> GetByRoleNameAsync(string roleName)
        {
            var roleFound = await _databaseContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (roleFound == null)
                throw new KeyNotFoundException(Messages.RoleNameNotFound + roleName);//

            return _mapper.Map<ResponseRoleDto>(roleFound);
        }

        public async Task<bool> EditAsync(int id, EditRoleDto dto)
        {
            await VerifyUniqunes(dto.Name, id);

            var roleFound = await _databaseContext.Roles.FindAsync(id);
            if (roleFound == null) throw new KeyNotFoundException(Messages.RoleIdNotFound + id);//

            _mapper.Map(dto, roleFound);

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        private async Task VerifyUniqunes(string roleName, int? id = null)//je u create nemam id tek ga dobijeme nakon kreraja a to je vec kasno za provjeru
        {
            bool notUniqueRole = await _databaseContext.Roles
                .AnyAsync(r => r.Name == roleName && r.Id != id);//trazimo isto ime ali razlicite Id jer nezelimo da trenutno
                                                                 //selektani entiete uspoređuje sa samim sobo  jer ce onda uvjek javlajti duplikate
            if (notUniqueRole)
            {
                throw new InvalidOperationException(Messages.DuplicateRole);
            }
        }
    }
}
