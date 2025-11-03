using AutoMapper;
using BL.Constants;
using BL.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly PersonService _personService;
        private readonly IMapper _mapper;
        public UserController(PersonService personService, IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }
        
        public IActionResult Login(string returnUrl)
        {
            var loginVm = new LoginVm
            {
                ReturnUrl = returnUrl
            };
            return View(loginVm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid)
                return View(loginVm);
            try
            {
                var (personDto, token) = await _personService
                    .LoginAsync(_mapper.Map<LoginPersonDto>(loginVm));

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, personDto.Username)
                };
                
                foreach (var role in personDto.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties()
                );
                return AccessBasedOnRole();
            }
            catch (InvalidOperationException ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                    message += " Detalji: " + ex.InnerException.Message;
                ModelState.AddModelError("", message);
                return View(loginVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(loginVm);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
             );
            return RedirectToAction("Login", "User");
        }

        //GET
        public IActionResult Register()
        {
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (!ModelState.IsValid)
                return View(registerVm);
            try
            {
                var (personDto, token) = await _personService.RegisterAsync(_mapper.Map<RegisterPersonDto>(registerVm));

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, personDto.Username)
                };

                if (personDto.Roles == null || !personDto.Roles.Any())
                {
                    personDto.Roles = new List<ResponseRoleDto>
                    {
                       new ResponseRoleDto { Name = Roles.User }
                    };
                }

                foreach (var role in personDto.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties()
                );

                return AccessBasedOnRole();
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("Error:", ex.Message);
                return View(registerVm);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Error:", ex.Message);
                return View(registerVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error:", ex.Message);
                return View(registerVm);
            }
        }

        private IActionResult AccessBasedOnRole()
        {
            //ovdje ako je user ulogirani contractor prilazi josb applicatione!
            return RedirectToAction("Search", "Contractor");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAdminProfile()
        {
            var admin = await _personService.GetByIdAsync(1);
            var vm = _mapper.Map<AdminProfileVm>(admin);
            return View("AdminProfile", vm); 
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAdminProfile(AdminProfileVm vm)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Neispravni podaci" });

            var adminEntity = await _personService.GetByIdAsync(1);

            adminEntity.FirstName = vm.FirstName;
            adminEntity.LastName = vm.LastName;
            adminEntity.Email = vm.Email;
            adminEntity.Phone = vm.Phone;
            adminEntity.Username = vm.Username;

            await _personService.EditAsync(1, _mapper.Map<EditPersonDto>(adminEntity));

            return Json(new { success = true, message = "Profil je uspješno ažuriran" });
        }
    }
}
