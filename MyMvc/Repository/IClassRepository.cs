using MyMvc.Data;

namespace MyMvc.Repository;

public interface IClassRepository
{
    public Task<List<Class>> GetAllClassAsync();
    public Task<Class?> GetClassAsync(Guid? guid);
    public Task<Class> AddClassAsync(Class modelReq);
    public Task<int> UpdateClassAsync(Guid id, Class modelReq);
    public Task DeleteClassAsync(Guid guid);
}