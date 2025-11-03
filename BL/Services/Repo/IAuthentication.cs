using BL.Dtos;

namespace BL.Services.Repo
{
    public interface IAuthentication
    {
        Task<IEnumerable<ResponsePersonDto>> GetAllAsync();
        Task<ResponsePersonDto?> GetByIdAsync(int id);
        Task<(ResponsePersonDto person, string token)> RegisterAsync(RegisterPersonDto dto);
        Task<(ResponseLoginPersonDto person, string token)> LoginAsync(LoginPersonDto dto);
        Task<bool> PasswordChangeAsync(ChangePasswordDto dto);
        Task<bool> EditAsync(int id, EditPersonDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
