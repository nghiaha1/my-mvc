using System.ComponentModel.DataAnnotations;

namespace MyMvc.Data;

public class Student
{
    [Key] public Guid Guid { get; set; }
    [Required] public string FullName { get; set; }
    public string StudentCode { get; set; }
    public Class? Class { get; set; }
    public Guid ClassGuid { get; set; }
}