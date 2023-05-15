using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly StudentAdminContext dbContext;

        public StudentRepo(StudentAdminContext dbContext)
        {
            this.dbContext = dbContext;
        }

       
        public async Task<Student> GetStudentByIdAsync(Guid StudentId)
        {
            return await dbContext.Students.Include(nameof(Gender)).Include(nameof(Address)).FirstOrDefaultAsync(x => x.Id==StudentId);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {

            //List<Student> students = dbContext.Students
            //    .Where(s => s != null) // filter out null values
            //    .ToList();

            //foreach (var student in students)
            //{
            //    if (student.ProfileImageUrl?.Equals(null) ?? true)
            //    {
            //        student.ProfileImageUrl = string.Empty;
            //    }
            //}
            return await dbContext.Students.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }


    }
}
