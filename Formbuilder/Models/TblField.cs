using System;
using System.Collections.Generic;

namespace Formbuilder.Models;

public partial class TblField
{
    public int FieldId { get; set; }

    public string FieldName { get; set; } = null!;

    public string InputType { get; set; } = null!;

    public virtual ICollection<FormFiledMap> FormFiledMaps { get; set; } = new List<FormFiledMap>();

    public virtual ICollection<TblFieldOption> TblFieldOptions { get; set; } = new List<TblFieldOption>();
}
