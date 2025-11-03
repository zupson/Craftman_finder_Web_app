using AutoMapper;
using AutoMapper.QueryableExtensions;
using BL.Constants;
using BL.Dtos;
using BL.Models;
using BL.Security;
using BL.Services.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace WebAPI.Services
{
    public class PersonService : IAuthentication
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        private readonly ISqlRepository<ResponseRoleDto, CreateRoleDto, EditRoleDto> _role;
        private readonly IMapper _mapper;

        public PersonService(DatabaseContext context, IConfiguration configuration, ISqlRepository<ResponseRoleDto, CreateRoleDto, EditRoleDto> role, IMapper mapper)
        {
            _databaseContext = context;
            _configuration = configuration;
            _role = role;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponsePersonDto>> GetAllAsync()
        {
            return await _databaseContext.People
                .Where(p => !p.IsDeleted)
                .ProjectTo<ResponsePersonDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ResponsePersonDto> GetByIdAsync(int id)
        {
            var personDto = await _databaseContext.People
                .Where(p => !p.IsDeleted && p.Id == id)
                .ProjectTo<ResponsePersonDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (personDto == null)
                throw new KeyNotFoundException(Messages.PersonNotFound + id);

            return personDto;
        }

        public async Task<(ResponsePersonDto person, string token)> RegisterAsync(RegisterPersonDto dto)
        {
            await VerifyUniqunes(dto);

            var salt = PasswordHashProvider.GetSalt();
            var hash = PasswordHashProvider.GetHash(dto.Password, salt);

            var newPerson = _mapper.Map<Person>(dto);
            newPerson.PasswordHash = hash;
            newPerson.PasswordSalt = salt;

            var userRole = await _databaseContext.Roles
                .FirstOrDefaultAsync(r => r.Name == Roles.User);//registrirani mogu biti samo useri,
                                                                //general admin se inserta u bazi
            newPerson.Roles.Add(new Role
            {
                Name = userRole.Name
            });

            _databaseContext.People.Add(newPerson);
            await _databaseContext.SaveChangesAsync();


            var rolesFromDb = await _role.GetAllAsync();
            var secureKey = _configuration[key: Jwt.SecureKey];
            if (secureKey == null)
                throw new Exception("SOmething Went wrong with token");

            string JWT = JwtTokenProvider.CreateToken(
                secureKey,
                60,
                rolesFromDb,
                dto.Username
            );
            var responsePersonDto = _mapper.Map<ResponsePersonDto>(dto);

            return (responsePersonDto, JWT);
        }

        public async Task<(ResponseLoginPersonDto person, string token)> LoginAsync(LoginPersonDto dto)
        {
            var salt = PasswordHashProvider.GetSalt();
            var hash = PasswordHashProvider.GetHash(dto.Password, salt);

            var user = await _databaseContext.People
                .Include(p=>p.Roles)
                .FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (user == null)
                throw new KeyNotFoundException(Messages.WrongPasswordMessage);

            var b64hash = PasswordHashProvider.GetHash(dto.Password, user.PasswordSalt);
            if (b64hash != user.PasswordHash)
                throw new ArgumentException(Messages.WrongPasswordMessage);

            string secureKey = _configuration[key: Jwt.SecureKey];

            var rolesFromDb = await _role.GetAllAsync();
            string JWT = JwtTokenProvider.CreateToken(secureKey, 60, rolesFromDb, dto.Username);

            var currentPerson = await _databaseContext.People.FirstOrDefaultAsync(p => p.Username == dto.Username);
            var loggedInPerson = _mapper.Map<ResponseLoginPersonDto>(user);

            return (loggedInPerson, JWT);
        }

        public async Task<bool> PasswordChangeAsync(ChangePasswordDto dto)//implementiraj Password changing
        {
            string trimmedUsername = dto.Username.Trim();
            var usernameExists = await _databaseContext.People.FirstOrDefaultAsync(p => p.Username.Equals(trimmedUsername));
            if (usernameExists == null || usernameExists.IsDeleted)
                return false;

            usernameExists.PasswordSalt = PasswordHashProvider.GetSalt();
            usernameExists.PasswordHash = PasswordHashProvider.GetHash(dto.Password, usernameExists.PasswordSalt);

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditAsync(int id, EditPersonDto dto)
        {
            await VerifyUniqunes<EditPersonDto>(dto, id);

            var person = await _databaseContext.People
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
                throw new KeyNotFoundException(Messages.ContractorNotFound + id);

            if (!string.IsNullOrEmpty(dto.FirstName) && dto.FirstName != person.FirstName)
                person.FirstName = dto.FirstName;

            if (!string.IsNullOrEmpty(dto.LastName) && dto.LastName != person.LastName)
                person.LastName = dto.LastName;

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != person.Email)
                person.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Phone) && dto.Phone != person.Phone)
                person.Phone = dto.Phone;

            if (!string.IsNullOrEmpty(dto.Password))
                await PasswordChangeAsync(new ChangePasswordDto { Username = person.Username, Password = dto.Password });

            if (!string.IsNullOrEmpty(dto.Username) && dto.Username != person.Username)
                person.Username = dto.Username;

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var providedPerson = await _databaseContext.People.FindAsync(id);
            if (providedPerson == null || providedPerson.IsDeleted)
                return false;

            providedPerson.IsDeleted = true;

            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task VerifyUniqunes<TDto>(TDto dto, int? id = null)
        {
            // Dohvaćanje Type objekta za TDto
            var dtoType = typeof(TDto);

            // Dohvaćanje PropertyInfo objekata za Username i Email
            var usernameProperty = dtoType.GetProperty("Username");
            var emailProperty = dtoType.GetProperty("Email");

            // Provjera jesu li svojstva pronađena i dohvaćanje njihovih vrijednosti
            string username = usernameProperty.GetValue(dto) as string;
            string email = emailProperty?.GetValue(dto) as string;


            bool notUniqueUsername = await _databaseContext.People
                .Where(p => !p.IsDeleted)
                .AnyAsync(p => p.Username == username && p.Id != id);

            if (notUniqueUsername)
                throw new InvalidOperationException(Messages.DuplicateUsername);

            bool notUniqueEmail = await _databaseContext.People
                .Where(p => !p.IsDeleted)
                .AnyAsync(p => p.Email == email && p.Id != id);

            if (notUniqueEmail)
                throw new InvalidOperationException(Messages.DuplicateEmail);
        }

        public async Task<Person> GetOrCreateAsync(RegisterPersonDto dto)
        {
            await VerifyUniqunes(dto);
            var existingPerson = await _databaseContext.People
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Username == dto.Username || p.Email == dto.Email);

            if (existingPerson != null)
                return existingPerson;

            var registerResult = await RegisterAsync(dto);

            existingPerson = await _databaseContext.People
                .FirstOrDefaultAsync( p => p.Username == dto.Username);

            if (existingPerson == null) throw new InvalidOperationException(Messages.PersonNotFound);
            return existingPerson;
        }
    }
}
