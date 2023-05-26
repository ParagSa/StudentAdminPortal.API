namespace StudentAdminPortal.API.Repositories
{
    public interface IImageRepo
    {
        Task<string> Upload(IFormFile file, string fileName);
    }
}
