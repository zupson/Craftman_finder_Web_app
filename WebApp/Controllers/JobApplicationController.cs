using AutoMapper;
using BL.Dtos;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly JobApplicationService _jobApplicationService;
        private readonly PersonService _personService;
        private readonly IMapper _mapper;

        public JobApplicationController(JobApplicationService jobApplicationService, PersonService personService, IMapper mapper)
        {
            _jobApplicationService = jobApplicationService;
            _personService = personService;
            _mapper = mapper;
        }

        // GET: JobApplicationController
        public async Task<ActionResult> Index()
        {
            var jobApplicationsDtoList = await _jobApplicationService.GetAllAsync();
            var responseJobApplicationVmList = _mapper.Map<IEnumerable<ResponseJobApplicationVm>>(jobApplicationsDtoList);
            return View(responseJobApplicationVmList);
        }

        // GET: JobApplicationController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var jobApplicationDto = await _jobApplicationService.GetByIdAsync(id);
            var responseJobApplicationVm = _mapper.Map<ResponseJobApplicationVm>(jobApplicationDto);

            return View(responseJobApplicationVm);
        }

        // GET: JobApplicationController/Create
        public ActionResult Create()
        {
            var createJobApplicationVm = new CreateJobApplicationVm();
            return View();
        }

        // POST: JobApplicationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateJobApplicationVm jobApplicationVm)
        {
            if (!ModelState.IsValid)
                return View(jobApplicationVm);
            try
            {
                await _jobApplicationService.CreateAsync(_mapper.Map<CreateJobApplicationDto>(jobApplicationVm));
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(jobApplicationVm);
            }
            catch (InvalidOperationException ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                    message += " Detalji: " + ex.InnerException.Message;
                ModelState.AddModelError("", message);
                return View(jobApplicationVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(jobApplicationVm);
            }
        }

        // GET: JobApplicationController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var responseJobApplicationDto = await _jobApplicationService.GetByIdAsync(id);
            if (responseJobApplicationDto == null)
                return NotFound();
            var responsePersonDto = await _personService.GetByIdAsync(responseJobApplicationDto.Person.Id);

            var editPersonDto = _mapper.Map<EditPersonDto>(responsePersonDto);

            var editJobApplicationVm = _mapper.Map<EditJobApplicationVm>(responseJobApplicationDto);

            return View(editJobApplicationVm);
        }

        // POST: JobApplicationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditJobApplicationVm editJobApplicationVm)
        {
            try
            {
                await _jobApplicationService.EditAsync(id, _mapper.Map<EditJobApplicationDto>(editJobApplicationVm));

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(editJobApplicationVm);
            }
            catch (InvalidOperationException ex)
            {
                var message = ex.Message;

                if (ex.InnerException != null)
                    message += " Detalji: " + ex.InnerException.Message;

                ModelState.AddModelError("", message);
                return View(editJobApplicationVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(editJobApplicationVm);
            }
        }

        // GET: JobApplicationController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var responseJobApplicationDto = await _jobApplicationService.GetByIdAsync(id);
            if (responseJobApplicationDto == null)
                return NotFound();
            var responseJobApplicationVm = _mapper.Map<ResponseJobApplicationVm>(responseJobApplicationDto);

            return View(responseJobApplicationVm);
        }

        // POST: JobApplicationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _jobApplicationService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                var jobApplicationDto = await _jobApplicationService.GetByIdAsync(id);
                var responseJobApplicationVm = _mapper.Map<ResponseJobApplicationVm>(jobApplicationDto);
                ModelState.AddModelError("", "Nije pronađen job application prilikom birsanja: " + ex.Message);
                return View(responseJobApplicationVm);
            }
            catch (Exception ex)
            {
                var jobApplicationDto = await _jobApplicationService.GetByIdAsync(id);
                var responseJobApplicationVm = _mapper.Map<ResponseJobApplicationVm>(jobApplicationDto);
                ModelState.AddModelError("", "Greška pri brisanju: " + ex.Message);
                return View(responseJobApplicationVm);
            }
        }
    }
}
