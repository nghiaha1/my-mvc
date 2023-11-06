using System.ComponentModel.DataAnnotations;

namespace MyMvc.Data;

public class Class
{
    [Key] public Guid Guid { get; set; }
    [Required] public string Name { get; set; }
    public string ClassCode { get; set; }
    public List<Student> Students { get; set; } = new List<Student>();
}