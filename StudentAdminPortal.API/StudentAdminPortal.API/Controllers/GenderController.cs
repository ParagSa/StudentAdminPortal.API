using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class GenderController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IMapper mapper;

        public GenderController( IStudentRepo studentRepo, IMapper mapper) 
        {
            this.studentRepo = studentRepo;
            this.mapper = mapper;
        }


        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllGenders()
        {
            var genderList = await studentRepo.GetAllGendersAsync();
            if (genderList==null || !genderList.Any()) 
            { 
                return NotFound();
            }

            return Ok(mapper.Map<List<Gender>>(genderList));
        }
    }
}
