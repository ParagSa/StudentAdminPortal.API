using AutoMapper;
using DataModel =StudentAdminPortal.API.DataModels;
using  update=StudentAdminPortal.API.DomainModels;

namespace StudentAdminPortal.API.Profiles.AfterMaps
{
    public class UpdateStudentAfterMapRequest : IMappingAction<update.UpdateStudentRequest, DataModel.Student>
    {
        public void Process(update.UpdateStudentRequest source, DataModel.Student destination, ResolutionContext context)
        {
            destination.Address = new DataModel.Address()
            {
                PhysicalAddress = source.PhysicalAddress,
                PostalAddress= source.PostalAddress,

            };
        }
    }
}
