using Microsoft.EntityFrameworkCore;
using StudentAdminPortal.API.DataModel;

namespace StudentAdminPortal.API.Repositories
{
    public class SqlStudentRepository : IStudentRepostory
    {
        private readonly StudentAdminContext _context;
        public SqlStudentRepository(StudentAdminContext context)
        {
            _context = context;
        }

      

        public async Task<bool> Exists(Guid studentId)
        {
           return await _context.Students.AnyAsync(x=>x.Id == studentId);
        }

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await _context.Genders.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid guid)
        {
                 return await _context.Students.Include(nameof(Gender))
                .Include(nameof(Address))
                .FirstOrDefaultAsync(x=>x.Id==guid);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
           return await _context.Students.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student request)
        {
            var existstudent = await GetStudentAsync(studentId);
            if(existstudent != null)
            {
                existstudent.FirstName = request.FirstName;
                existstudent.LastName = request.LastName;
                existstudent.Email = request.Email;
                existstudent.DateOfBirth = request.DateOfBirth;
                existstudent.Gender.Id = request.GenderId;
                existstudent.Address.PhysicalAddress = request.Address.PhysicalAddress;
                existstudent.Address.PostalAddress = request.Address.PostalAddress;
                existstudent.GenderId = request.GenderId;
                existstudent.Mobile= request.Mobile;
                _context.SaveChangesAsync();
                return existstudent;
            }
            return null;
        }
        public async Task<Student> DeleteStudent(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);

            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return student;
            }

            return null;
        }

        public async Task<Student> AddStudent(Student student)
        {
            var studen = await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return studen.Entity;
        }
    }
}
