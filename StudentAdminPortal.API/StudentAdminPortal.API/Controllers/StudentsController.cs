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

        public StudentsController(IStudentRepo studentRepo, IMapper mapper)
        {
            this.studentRepo = studentRepo;
            this.mapper = mapper;
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




    }
}
