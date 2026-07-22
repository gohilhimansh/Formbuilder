namespace Formbuilder.Models
{
    public class FormFiledMapViewModel
    {
        public int FormFiledMapId { get; set; }
        public int FormId { get; set; }
        public int FieldId { get; set; }
        public int? FieldOptionId { get; set; }
        public string? OptionTitle { get; set; }
        public bool Isrequired { get; set; }
    }
    public class DataTableResponseViewModel<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T> Data { get; set; } = new List<T>();
    }

    public class FormsListRequestViewModel
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string? SearchValue { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
    }
    public class FormsListDto
    {
        public int FormId { get; set; }
        public string FormName { get; set; } = null!;
    }
}
