using FluentValidation;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;

namespace StudentAdminPortal.API.Validation
{
    public class AddStudentRequestValidator:AbstractValidator<AddStudentRequest>
    {
        public AddStudentRequestValidator(IStudentRepostory studentRepostory)
        {
            RuleFor(x=>x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Mobile).NotEmpty();
            RuleFor(x => x.GenderId).NotEmpty().Must(id =>
            {
                var gender = studentRepostory.GetGendersAsync().Result.ToList().FirstOrDefault(x=>x.Id==id);

                if (gender != null)
                {
                    return true;
                }else { return false; }
            }).WithMessage("Please select a valid Gender");
            RuleFor(x=>x.PhysicalAddress).NotEmpty();
            RuleFor(x => x.PostalAddress).NotEmpty();


        }
    }
}
