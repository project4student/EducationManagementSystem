namespace EducationManagementSystem.ViewModels;
public class EmailViewModel
{
#nullable disable
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Attachment { get; set; }
    public Boolean isHTML { get; set; }
}
