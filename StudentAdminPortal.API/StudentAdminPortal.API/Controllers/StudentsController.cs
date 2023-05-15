using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;

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

          
            
            return Ok(mapper.Map<List<Student>>(students));
        }
        [HttpGet]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> GetSingleStudentAsync([FromRoute] Guid studentId)
        {
            var students = await studentRepo.GetStudentByIdAsync(studentId);
            if (students ==null)
            {
                return NotFound();

            }



            return Ok(mapper.Map<Student>(students));
        }

    }
}
