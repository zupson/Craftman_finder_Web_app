using AutoMapper;
using BL.Constants;
using BL.Dtos;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAPI.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]

    public class ContractorController : Controller
    {
        private readonly ContractorService _contractorService;
        private readonly LocationService _locationService;
        private readonly PersonService _personService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public ContractorController(ContractorService contractorService, IMapper mapper, LocationService locationService, PersonService personService, IConfiguration configuration)
        {
            _contractorService = contractorService;
            _mapper = mapper;
            _locationService = locationService;
            _personService = personService;
            _configuration = configuration;
        }

        public async Task<ActionResult> Search(SearchVm searchVm)
        {
            try
            {
                if (string.IsNullOrEmpty(searchVm.Q) && string.IsNullOrEmpty(searchVm.Submit))
                    searchVm.Q = Request.Cookies["query"];

                var contractors = await _contractorService.GetAllAsync();

                if (!string.IsNullOrEmpty(searchVm.Q))
                {
                    contractors = contractors.Where(c =>
                        (c.CompanyName.Contains(searchVm.Q)) ||
                        (c.Person.FirstName != null && c.Person.FirstName.Contains(searchVm.Q)) ||
                        (c.Person.LastName != null && c.Person.LastName.Contains(searchVm.Q)) ||
                        (c.JobTypeName != null && c.JobTypeName.Contains(searchVm.Q)) ||
                        c.Locations.Any(l =>
                            (l.TownName != null && l.TownName.Contains(searchVm.Q)) ||
                            (l.CountryName != null && l.CountryName.Contains(searchVm.Q))
                        )
                    );
                }

                var filteredCount = contractors.Count();

                switch (searchVm.OrderBy.ToLower())
                {
                    case "id":
                        contractors = contractors.OrderBy(c => c.Id);
                        break;
                    case "firstname":
                        contractors = contractors.OrderBy(c => c.Person.FirstName);
                        break;
                    case "lastname":
                        contractors = contractors.OrderBy(c => c.Person.LastName);
                        break;
                    case "jobtypename":
                        contractors = contractors.OrderBy(c => c.JobTypeName);
                        break;
                    case "companyname":
                        contractors = contractors.OrderBy(c => c.CompanyName);
                        break;
                    case "postalcode":
                        contractors = contractors.OrderBy(c => c.Locations.Select(l => l.PostalCode).FirstOrDefault());
                        break;
                    case "townname":
                        contractors = contractors.OrderBy(c => c.Locations.Select(l => l.TownName).FirstOrDefault());
                        break;
                    case "countryname":
                        contractors = contractors.OrderBy(c => c.Locations.Select(l => l.CountryName).FirstOrDefault());
                        break;
                }

                contractors = contractors
                    .Skip((searchVm.Page - 1) * searchVm.Size)
                    .Take(searchVm.Size);

                searchVm.Contractors = _mapper.Map<List<ResponseContractorVm>>(contractors.ToList());

                // BEGIN PAGER
                var expandPages = _configuration.GetValue<int>("Paging:ExpandPages");
                searchVm.LastPage = (int)Math.Ceiling(1.0 * filteredCount / searchVm.Size);
                searchVm.FromPager = searchVm.Page > expandPages ?
                    searchVm.Page - expandPages :
                    1;
                searchVm.ToPager = (searchVm.Page + expandPages) < searchVm.LastPage ?
                    searchVm.Page + expandPages :
                    searchVm.LastPage;
                // END PAGER

                var option = new CookieOptions { Expires = DateTime.Now.AddMinutes(15) };
                Response.Cookies.Append("query", searchVm.Q ?? "", option);

                return View(searchVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(searchVm);
            }
        }

        // GET: ContractorController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var contractorDto = await _contractorService.GetByIdAsync(id);
            var contractorVm = _mapper.Map<ResponseContractorVm>(contractorDto);
            return View(contractorVm);
        }

        // GET: ContractorController/Create
        public async Task<ActionResult> CreateAsync()
        {
            var allLocations = await _locationService.GetAllAsync();

            var createVm = new CreateContractorVm
            {
                AllLocations = allLocations
                    .Select(l => new SelectListItem
                    {
                        Value = l.Id.ToString(),
                        Text = $"({l.PostalCode}) {l.TownName}, {l.CountryName}"
                    }).ToList()
            };

            return View(createVm);
        }

        // POST: ContractorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateContractorVm contractorVm)
        {

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    Console.WriteLine($"Key: {entry.Key}, State: {entry.Value.ValidationState}, Errors: {entry.Value.Errors.Count}");
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"  ErrorMessage: {error.ErrorMessage}");
                        if (error.Exception != null)
                            Console.WriteLine($"  Exception: {error.Exception.Message}");
                    }
                }
                return View(contractorVm);
            }

            try
            {
                if (string.IsNullOrEmpty(contractorVm.CompanyName))
                    contractorVm.CompanyName = string.Empty;

                var createContractorDto = _mapper.Map<CreateContractorDto>(contractorVm);
                var locations = new List<CreateLocationDto>();

                foreach (var l in contractorVm.SelectedLocationIds)
                {
                    var location = await _locationService.GetByIdAsync(l);
                    locations.Add(_mapper.Map<CreateLocationDto>(location));
                }

                createContractorDto.Locations = locations;

                await _contractorService.CreateAsync(createContractorDto);
                return RedirectToAction(nameof(Search));
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(contractorVm);
            }
            catch (InvalidOperationException ex)
            {
                var message = ex.Message;

                if (ex.InnerException != null)
                    message += " Detalji: " + ex.InnerException.Message;

                ModelState.AddModelError("", message);
                return View(contractorVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(contractorVm);
            }
        }

        // GET: ContractorController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var responseContractorDto = await _contractorService.GetByIdAsync(id);

            if (responseContractorDto == null)
                return NotFound();

            var editVm = _mapper.Map<EditContractorVm>(responseContractorDto);
            editVm.Id = responseContractorDto.Id;
            await PrepareEditVm(editVm);

            editVm.SelectedLocationIds = responseContractorDto.Locations
                .Select(loc => loc.Id)
                .ToList();

            return View(editVm);
        }

        private async Task PrepareEditVm(EditContractorVm editVm)
        {
            var allLocations = await _locationService.GetAllAsync();

            editVm.AllLocations = allLocations
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = $"({l.PostalCode}) {l.TownName}, {l.CountryName}"
            })
            .ToList();
        }

        // POST: ContractorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditContractorVm editContractorVm)
        {
            try
            {
                if (editContractorVm.Person.Id == 0)
                    throw new Exception($"Person Id nije postavljen: {editContractorVm.Person.Id}");

                //var person = editContractorVm.Person;
                //await _personService.EditAsync(editContractorVm.Person.Id, _mapper.Map<EditPersonDto>(person));

                var locations = new List<EditLocationDto>();//inicijalizacija liste

                if (editContractorVm.SelectedLocationIds != null &&
                    editContractorVm.SelectedLocationIds.Any())
                {
                    var selectedLocationIds = editContractorVm.SelectedLocationIds;
                    foreach (var locationId in selectedLocationIds)
                    {
                        var responseLocationDto = await _locationService.GetByIdAsync(locationId);
                        locations.Add(_mapper.Map<EditLocationDto>(responseLocationDto));
                    }
                }

                locations = locations.GroupBy(l => l.Id).Select(g => g.FirstOrDefault()).ToList();

                var contractorDto = _mapper.Map<EditContractorDto>(editContractorVm);
                contractorDto.Locations = locations;

                await _contractorService.EditAsync(editContractorVm.Id, contractorDto);
                return RedirectToAction(nameof(Search));
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                await PrepareEditVm(editContractorVm);
                return View(editContractorVm);
            }
            catch (InvalidOperationException ex)
            {
                var message = ex.Message;

                if (ex.InnerException != null)
                    message += " Detalji: " + ex.InnerException.Message;

                ModelState.AddModelError("", message);
                await PrepareEditVm(editContractorVm);
                return View(editContractorVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                await PrepareEditVm(editContractorVm);
                return View(editContractorVm);
            }
        }


        // GET: ContractorController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var contractorDto = await _contractorService.GetByIdAsync(id);
            if (contractorDto == null)
                return NotFound();

            var responseContractorVm = _mapper.Map<ResponseContractorVm>(contractorDto);
            return View(responseContractorVm);
        }


        // POST: ContractorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ResponseContractorVm contractorVm)
        {
            try
            {
                await _contractorService.DeleteAsync(id);
                return RedirectToAction(nameof(Search));
            }
            catch (KeyNotFoundException ex)
            {
                var contractorDto = await _contractorService.GetByIdAsync(id);
                var responseContractorVm = _mapper.Map<ResponseContractorVm>(contractorDto);
                ModelState.AddModelError("", "Nije pronađen contractor prilikom birsanja: " + ex.Message);
                return View(responseContractorVm);
            }
            catch (Exception ex)
            {
                var contractorDto = await _contractorService.GetByIdAsync(id);
                var responseContractorVm = _mapper.Map<ResponseContractorVm>(contractorDto);
                ModelState.AddModelError("", "Greška pri brisanju: " + ex.Message);
                return View(responseContractorVm);
            }
        }
    }
}
