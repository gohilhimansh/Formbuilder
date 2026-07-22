using System;
using System.Collections.Generic;

namespace Formbuilder.Models;

public partial class TblFieldOption
{
    public int FieldOptionId { get; set; }

    public int FieldId { get; set; }

    public string? OptionTitle { get; set; }

    public bool Isrequired { get; set; }

    public virtual TblField Field { get; set; } = null!;

    public virtual ICollection<FormFiledMap> FormFiledMaps { get; set; } = new List<FormFiledMap>();
}
