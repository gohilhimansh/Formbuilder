namespace Formbuilder.Models
{
    public class FormViewModel
    {
        public int FormId { get; set; }
        public string FormName { get; set; } = null!;
        public List<FormFiledMapViewModel> Fields { get; set; } = new List<FormFiledMapViewModel>();
    }   
}
