using MyMvc.Data;

namespace MyMvc.Repository;

public interface IStudentRepository
{
    public Task<List<Student>> GetAllStudentAsync();
    public Task<Student> GetStudentAsync(Guid? guid);
    public Task<Student> AddStudentAsync(Student modelReq);
    public Task<int> UpdateStudentAsync(Guid id, Student modelReq);
    public Task DeleteStudentAsync(Guid guid);
}