using AutoMapper;
using StudentAdminPortal.API.DataModel;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Profiles.AfterMap;
using DomainModels = StudentAdminPortal.API.DomainModels;

namespace StudentAdminPortal.API.Profiles
{
    public class AutoMapperProiles:Profile
    {
        public AutoMapperProiles()
        {
            CreateMap<DomainModels.Student, DataModel.Student>().ReverseMap();
            CreateMap<DomainModels.Gender, DataModel.Gender>().ReverseMap();
            CreateMap<DomainModels.Address, DataModel.Address>().ReverseMap();

            CreateMap<DomainModels.AddStudentRequest, DataModel.Student>().ReverseMap();


            CreateMap<UpdateStudentRequest, DataModel.Student>()
               .AfterMap<UpdateStudentRequestAfterMap>();

            CreateMap<AddStudentRequest, DataModel.Student>()
              .AfterMap<AddStudentRequestAfterMap>();

        }
    }
}
