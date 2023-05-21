using AutoMapper;
using DataModel= StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Profiles.AfterMaps;

namespace StudentAdminPortal.API.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<DataModel.Student, Student>().ReverseMap();
            CreateMap<DataModel.Gender, Gender>().ReverseMap();
            CreateMap<DataModel.Address, Address>().ReverseMap();
            CreateMap<UpdateStudentRequest, DataModel.Student>().AfterMap<UpdateStudentAfterMapRequest>();
            CreateMap<AddStudentRequest, DataModel.Student>().AfterMap<AddStudentAfterMapRequest>();

        }
    }
}
