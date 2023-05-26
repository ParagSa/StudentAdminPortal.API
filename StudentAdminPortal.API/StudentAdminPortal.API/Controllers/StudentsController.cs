using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using DataModels= StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;
using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IMapper mapper;
        private readonly IImageRepo imageRepo;

        public StudentsController(IStudentRepo studentRepo, IMapper mapper,
            IImageRepo imageRepo)
        {
            this.studentRepo = studentRepo;
            this.mapper = mapper;
            this.imageRepo = imageRepo;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
           var students = await studentRepo.GetStudentsAsync();

          
            
            return Ok(mapper.Map<List<DomainModels.Student>>(students));
        }
        [HttpGet]
        [Route("[controller]/{studentId:guid}"),ActionName("GetSingleStudentAsync")]
        public async Task<IActionResult> GetSingleStudentAsync([FromRoute] Guid studentId)
        {
            var students = await studentRepo.GetStudentByIdAsync(studentId);
            if (students ==null)
            {
                return NotFound();

            }



            return Ok(mapper.Map<DomainModels.Student>(students));
        }
        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request) 
        {
            if(await studentRepo.Exist(studentId))
            {
               var updateStudent = await studentRepo.UpdateStudent(studentId,mapper.Map<DataModels.Student>(request));

                if (updateStudent !=null) 
                {
                    return  Ok(updateStudent);
                }

            }
            else 
            {
                return NotFound();
            }

            return null;
        }


        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
           
            if (await studentRepo.Exist(studentId))
            {
                var student = await studentRepo.DeleteStudentAsync(studentId);
                return Ok(mapper.Map<DomainModels.Student>(student));

            }

            return NotFound();

            //return Ok(mapper.Map<Student>(students));
        }

        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
            var student = await studentRepo.AddStudent(mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetSingleStudentAsync),new {studentId= student.Id},
                mapper.Map<DomainModels.Student>(student));

        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]

        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile
             profileImage) 
        {
            var validExtensions = new List<String> {
            
                ".jpeg",
                ".png",
                ".jpg"
             

            };
            if (profileImage != null && profileImage.Length>0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                if (validExtensions.Contains(extension))
                {
                    if (await studentRepo.Exist(studentId))
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName
                            );
                        var fileImagePath = await imageRepo.Upload(profileImage, fileName);

                        if (await studentRepo.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            "Error uploading image");

                    }


                }
                return BadRequest("This is not valid image format");

            }
           
            return NotFound();
        
        }




    }
}
