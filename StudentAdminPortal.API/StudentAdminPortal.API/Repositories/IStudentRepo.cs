using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Repositories
{
    public interface IStudentRepo
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(Guid StudentId);

        Task<List<Gender>> GetAllGendersAsync();

        Task<bool> Exist(Guid studentId);
        Task<Student> UpdateStudent(Guid StudentId, Student request);

        Task<Student> DeleteStudentAsync(Guid StudentId);

        Task<Student> AddStudent(Student request);

        Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl);

    }
}
