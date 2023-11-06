using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyMvc.Data;
using MyMvc.Helper;

namespace MyMvc.Repository;

public class StudentRepository : IStudentRepository
{
    private readonly DatabaseContext context;
    private readonly IMapper mapper;
    private readonly IClassRepository classRepository;

    public StudentRepository(DatabaseContext context, IMapper mapper, IClassRepository classRepository)
    {
        this.context = context;
        this.mapper = mapper;
        this.classRepository = classRepository;
    }

    public async Task<List<Student>> GetAllStudentAsync()
    {
        var data = await context.Students!.ToListAsync();
        return mapper.Map<List<Student>>(data);
    }

    public async Task<Student> GetStudentAsync(Guid? guid)
    {
        var data = await context.Students!.FirstOrDefaultAsync(item => item.Guid == guid);
        return mapper.Map<Student>(data);
    }

    public async Task<Student> AddStudentAsync(Student model)
    {
        var classId = model.Class.Guid;
        var cl = await classRepository.GetClassAsync(classId);
        model.StudentCode = "STD-" + Helper.StudentCodeHelper.RandomString(10);
        model.Class = cl;
        context.Add(model);
        await context.SaveChangesAsync();
        return model;
    }

    public async Task<int> UpdateStudentAsync(Guid id, Student model)
    {
        if (id != model.Guid) return 0;
        var data = await context.Students!.FindAsync(id);
        if (data == null) return 0;
        var classId = model.Class.Guid;
        var cl = await classRepository.GetClassAsync(classId);
        model.Class = cl;
        model.StudentCode = data.StudentCode;
        context.Update(model);
        await context.SaveChangesAsync();
        return 1;
    }

    public async Task DeleteStudentAsync(Guid guid)
    {
        var data = await context.Students!.FindAsync(guid);
        if (data != null)
        {
            context.Students.Remove(data);
            await context.SaveChangesAsync();
        }
    }
}