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

        public async Task<Student> AddStudent(Student request)
        {
          var student= await dbContext.Students.AddAsync(request);
           await dbContext.SaveChangesAsync();
            return student.Entity;
            
        }

        public async Task<Student> DeleteStudentAsync(Guid StudentId)
        {
            var student = await GetStudentByIdAsync(StudentId);
            if (student != null)
            {
                dbContext.Students.Remove(student);
                await dbContext.SaveChangesAsync();

                return student;


            }
            return null;
        }

        public async Task<bool> Exist(Guid studentId)
        {
           return await dbContext.Students.AnyAsync(x => x.Id == studentId);
        }

        public async Task<List<Gender>> GetAllGendersAsync()
        {
            return await dbContext.Genders.ToListAsync();
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

        public async Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl)
        {
            var student = await GetStudentByIdAsync(studentId);

            if (student !=null)
            {
                student.ProfileImageUrl = profileImageUrl;
                await  dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Student> UpdateStudent(Guid StudentId, Student request)
        {
            var existingStudent = await GetStudentByIdAsync(StudentId);

            if (existingStudent !=null) 
            {
                existingStudent.FirstName = request.FirstName;
                existingStudent.LastName = request.LastName;
                existingStudent.DateOfBirth = request.DateOfBirth;
                existingStudent.Email = request.Email;
                existingStudent.Mobile = request.Mobile;
                existingStudent.GenderId = request.GenderId;
                existingStudent.Address.PhysicalAddress = request.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress = request.Address.PostalAddress;
               
                await dbContext.SaveChangesAsync();
                return existingStudent;

            }
            return null;
            
        }
    }
}
