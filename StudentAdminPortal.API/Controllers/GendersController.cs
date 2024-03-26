using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class GendersController : Controller
    {
        private readonly IStudentRepostory _studentRepostory;
        private readonly IMapper _mapper;
        public GendersController(IStudentRepostory studentRepostory, IMapper mapper)
        {
            _studentRepostory = studentRepostory;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllGenders()
        {
            var Gender =  await _studentRepostory.GetGendersAsync();
            var map = _mapper.Map<List<Gender>>(Gender);
            if (map == null || !map.Any())
                return NotFound();
            return Ok(map);
          
        }
    }
}
