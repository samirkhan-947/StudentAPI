using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentRepostory _studentRepostory;
        private readonly IMapper _mapper;
        public StudentController(IStudentRepostory studentRepostory, IMapper mapper)
        {
            this._studentRepostory = studentRepostory;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudentAsync()
        {
            var student = await _studentRepostory.GetStudentsAsync();
            var map = _mapper.Map<List<Student>>(student);            
           return Ok(map);
        }
        [HttpGet]
        [Route("[controller]/{studentid:guid}"),ActionName("GetStudentAsync")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentid)
        {
            var student = await _studentRepostory.GetStudentAsync(studentid);
            var map = _mapper.Map<Student>(student);
            if(map == null)
                return NotFound();
            return Ok(map);
        }
        [HttpPut]
        [Route("[controller]/{studentid:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentid, [FromBody] UpdateStudentRequest request)
        {
            if(await _studentRepostory.Exists(studentid)){

                

                var map =  _mapper.Map<DataModel.Student>(request);
                var  updatestudents = await _studentRepostory.UpdateStudent(studentid, map);
                if(updatestudents != null)
                {
                    return Ok(_mapper.Map<Student>((updatestudents)));
                }
            }
            return NotFound();

        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await _studentRepostory.Exists(studentId))
            {
                var student = await _studentRepostory.DeleteStudent(studentId);
                return Ok(_mapper.Map<Student>(student));
            }

            return NotFound();
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request) 
        {
            var student = await _studentRepostory.AddStudent(_mapper.Map<DataModel.Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = student.Id },_mapper.Map<Student>(student));
        }
    }
}
