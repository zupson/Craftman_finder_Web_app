using AutoMapper;
using BL.Dtos;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class JobPostController : Controller
    {
        private readonly JobPostService _jobPostService;
        private readonly IMapper _mapper;
        private readonly ContractorService _contractorService;
        private readonly LocationService _locationService;

        public JobPostController(JobPostService jobPostService, IMapper mapper, ContractorService contractorService, LocationService locationService)
        {
            _jobPostService = jobPostService;
            _mapper = mapper;
            _contractorService = contractorService;
            _locationService = locationService;
        }

        // GET: JobPostController
        public async Task<ActionResult> Index()
        {
            var responseJobPostsDtoList = await _jobPostService.GetAllAsync();
            var responseJobPostsVmList = _mapper.Map<IEnumerable<ResponseJobPostVm>>(responseJobPostsDtoList);
            return View(responseJobPostsVmList);
        }

        // GET: JobPostController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var resposejobPostDto = await _jobPostService.GetByIdAsync(id);
            var responseJobPostVm = _mapper.Map<ResponseJobPostVm>(resposejobPostDto);
            return View(responseJobPostVm);
        }

        // GET: JobPostController/Create
        public ActionResult Create()
        {
            var createJobPostVm = new CreateJobPostVm();
            return View();
        }

        // POST: JobPostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateJobPostVm jobPostVm)
        {
            if (!ModelState.IsValid)
                return View(jobPostVm);
            try
            {
                var newResponseJobPostDto = await _jobPostService.CreateAsync(_mapper.Map<CreateJobPostDto>(jobPostVm));
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(jobPostVm);
            }
            catch (InvalidOperationException ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                    message += " Detalji: " + ex.InnerException.Message;
                ModelState.AddModelError("", message);
                return View(jobPostVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(jobPostVm);
            }
        }

        // GET: JobPostController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var responseJobPostDto = await _jobPostService.GetByIdAsync(id);
            if (responseJobPostDto == null)
                return NotFound();

            var responseContractorDto = await _contractorService.GetByIdAsync(responseJobPostDto.ContractorLocation.ContractorId);
            var contractorDto = _mapper.Map<EditContractorDto>(responseContractorDto);

            var responseLocationDto = await _locationService.GetByIdAsync(responseJobPostDto.ContractorLocation.LocationId);
            var locationDto = _mapper.Map<EditLocationDto>(responseLocationDto);

            if (contractorDto == null || locationDto == null)
                return NotFound();

            var editVm = new EditJobPostVm
            {
                Id = responseJobPostDto.Id,
                Contractor = contractorDto,
                Location = locationDto
            };

            return View(editVm);
        }

        // POST: JobPostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditJobPostVm editJobPostVm)
        {
            try
            {
                await _jobPostService.EditAsync(id, new EditJobPostDto
                {
                    Contractor = editJobPostVm.Contractor,
                    Location = editJobPostVm.Location,
                });
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(editJobPostVm);
            }
            catch (InvalidOperationException ex)
            {
                var message = ex.Message;

                if (ex.InnerException != null)
                    message += " Detalji: " + ex.InnerException.Message;

                ModelState.AddModelError("", message);
                return View(editJobPostVm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Došlo je do pogreške: " + ex.Message);
                return View(editJobPostVm);
            }
        }

        // GET: JobPostController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var responseJobPostDto = await _jobPostService.GetByIdAsync(id);
            if (responseJobPostDto == null)
                return NotFound();

            var responseLocationDto = await _locationService.GetByIdAsync(responseJobPostDto.ContractorLocation.LocationId);
            var responseContractorDto = await _contractorService.GetByIdAsync(responseJobPostDto.ContractorLocation.ContractorId);

            var responseJobPostVm = new ResponseJobPostVm
            {
                Id = responseJobPostDto.Id,
                ContractorLocation = responseJobPostDto.ContractorLocation,
            };
            return View(responseJobPostVm);
        }

        // POST: JobPostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ResponseJobPostVm jobPostVm)
        {
            try
            {
                await _jobPostService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                var jobPostDto = await _jobPostService.GetByIdAsync(id);
                var responseJobPostVm = _mapper.Map<ResponseJobPostVm>(jobPostDto);
                ModelState.AddModelError("", "Nije pronađen job post prilikom birsanja: " + ex.Message);
                return View(responseJobPostVm);
            }
            catch (Exception ex)
            {
                var jobPostDto = await _jobPostService.GetByIdAsync(id);
                var responseJobPostVm = _mapper.Map<ResponseJobPostVm>(jobPostDto);
                ModelState.AddModelError("", "Greška pri brisanju: " + ex.Message);
                return View(responseJobPostVm);
            }
        }
    }
}
