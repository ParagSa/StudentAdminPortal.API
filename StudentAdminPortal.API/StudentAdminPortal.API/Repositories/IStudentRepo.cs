using StudentAdminPortal.API.DataModels;

namespace StudentAdminPortal.API.Repositories
{
    public interface IStudentRepo
    {
        Task<List<Student>> GetStudentsAsync();
    }
}
