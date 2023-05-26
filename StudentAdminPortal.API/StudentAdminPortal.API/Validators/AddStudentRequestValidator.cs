using FluentValidation;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;

namespace StudentAdminPortal.API.Validators
{
    public class AddStudentRequestValidator : AbstractValidator<AddStudentRequest>
    {
        public AddStudentRequestValidator(IStudentRepo studentRepo)
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x=>x.DateOfBirth).NotEmpty();
            RuleFor(x=>x.Mobile).GreaterThan(99999).LessThan(10000000);
            RuleFor(x => x.GenderId).NotEmpty().Must(id =>
            {
                var gender = studentRepo.GetAllGendersAsync().Result.ToList().
                FirstOrDefault(x => x.Id == id);
                if (gender != null)
                {
                    return true;

                }
                return false;
            }).WithMessage("please select a valid gender");

            RuleFor(x => x.PhysicalAddress).NotEmpty();
            RuleFor(x=>x.PostalAddress).NotEmpty();

        }

    }
}
