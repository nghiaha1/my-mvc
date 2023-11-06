using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyMvc.Data;
using MyMvc.Helper;

namespace MyMvc.Repository;

public class ClassRepository : IClassRepository
{
    private readonly DatabaseContext context;
    private readonly IMapper mapper;

    public ClassRepository(DatabaseContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<Class>> GetAllClassAsync()
    {
        var data = await context.Classes!.ToListAsync();
        return data;
    }

    public async Task<Class?> GetClassAsync(Guid? guid)
    {
        var data = await context.Classes!.FirstOrDefaultAsync(item => item.Guid == guid);
        return data;
    }

    public async Task<Class> AddClassAsync(Class model)
    {
        var students = model.Students;
        model.ClassCode = "CLA-" + StudentCodeHelper.RandomString(10);
        context.Add(model);
        await context.SaveChangesAsync();
        return model;
    }

    public async Task<int> UpdateClassAsync(Guid id, Class model)
    {
        if (id != model.Guid) return 0;
        var data = await context.Classes!.FindAsync(id);
        if (data == null) return 0;
        model.ClassCode = data.ClassCode;
        context.Update(model);
        await context.SaveChangesAsync();
        return 1;
    }

    public async Task DeleteClassAsync(Guid guid)
    {
        var data = await context.Classes!.FindAsync(guid);
        if (data != null)
        {
            context.Classes.Remove(data);
            await context.SaveChangesAsync();
        }
    }
}