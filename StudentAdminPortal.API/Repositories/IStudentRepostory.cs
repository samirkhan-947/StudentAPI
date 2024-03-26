using StudentAdminPortal.API.DataModel;

namespace StudentAdminPortal.API.Repositories
{
    public interface IStudentRepostory
    {
       Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(Guid guid);
        Task<List<Gender>> GetGendersAsync();
        Task <bool> Exists(Guid studentId);
        Task<Student> UpdateStudent(Guid studentId, Student request);
        Task<Student> DeleteStudent(Guid studentId);
        Task<Student> AddStudent(Student student);
        Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl);
    }
}
