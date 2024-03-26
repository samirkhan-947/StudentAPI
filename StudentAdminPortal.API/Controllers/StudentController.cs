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
        private readonly IImageRepository _imageRepository;
        public StudentController(IStudentRepostory studentRepostory, IMapper mapper, IImageRepository imageRepository)
        {
            this._studentRepostory = studentRepostory;
            _mapper = mapper;
            _imageRepository = imageRepository;
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

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            var validExtensions = new List<string>
            {
               ".jpeg",
               ".png",
               ".gif",
               ".jpg"
            };

            if (profileImage != null && profileImage.Length > 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                if (validExtensions.Contains(extension))
                {
                    if (await _studentRepostory.Exists(studentId))
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                        var fileImagePath = await _imageRepository.Upload(profileImage, fileName);

                        if (await _studentRepostory.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
                    }
                }

                return BadRequest("This is not a valid Image format");
            }

            return NotFound();
        }
    }
}
