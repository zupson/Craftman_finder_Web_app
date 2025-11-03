using AutoMapper;
using BL.Dtos;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly LocationService _locationService;
        private readonly IMapper _mapper;

        public LocationController(LocationService locationService, IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }

        // GET: LocationController
        public async Task<ActionResult> Index()
        {
            var responseLocationsDtoList = await _locationService.GetAllAsync();
            var responseLocationVms = _mapper.Map<IEnumerable<ResponseLocationVm>>(responseLocationsDtoList);
            return View(responseLocationVms);
        }

        // GET: LocationController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var responseLocationDto = await _locationService.GetByIdAsync(id);
            var responseLocationVm = _mapper.Map<ResponseLocationVm>(responseLocationDto);
            return View(responseLocationVm);
        }

        // GET: LocationController/Create
        public async Task<ActionResult> Create()
        {
            var vm = new CreateContractorVm();
            return View(vm);
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLocationVm locationVm)
        {
            if (!ModelState.IsValid)
                return View(locationVm);
            try
            {
                var location = await _locationService.CreateAsync(_mapper.Map<CreateLocationDto>(locationVm));
                TempData["NewLocationId"] = location.Id;
                TempData["NewLocationName"] = $"{location.TownName} {location.PostalCode}";

                return RedirectToAction("Create", "Contractor");
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(locationVm);
            }
            catch (InvalidOperationException ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                    message += " Detalji: " + ex.InnerException.Message;
                ModelState.AddModelError("", message);
                return View(locationVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(locationVm);
            }
        }


        // GET: LocationController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var responseLocationDto = await _locationService.GetByIdAsync(id);
            if (responseLocationDto == null)
                return NotFound();

            var responseLocationVm = _mapper.Map<ResponseLocationVm>(responseLocationDto);
            return View(responseLocationVm);
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ResponseLocationVm locationVm)
        {
            try
            {
                await _locationService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                var responseContractorDto = await _locationService.GetByIdAsync(id);
                var responseLocationVm = _mapper.Map<ResponseLocationVm>(responseContractorDto);

                ModelState.AddModelError("", "Nije pronađen location prilikom birsanja: " + ex.Message);
                return View(responseLocationVm);
            }
            catch(Exception ex)
            {
                var responseContractorDto = await _locationService.GetByIdAsync(id);
                var responseLocationVm = _mapper.Map<ResponseLocationVm>(responseContractorDto);

                ModelState.AddModelError("", "Greška pri brisanju");
                return View(responseLocationVm);
            }
        }
    }
}
