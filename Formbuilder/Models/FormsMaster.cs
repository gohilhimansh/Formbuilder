using System;
using System.Collections.Generic;

namespace Formbuilder.Models;

public partial class FormsMaster
{
    public int FormsMasterId { get; set; }

    public string FormName { get; set; } = null!;

    public virtual ICollection<FormFiledMap> FormFiledMaps { get; set; } = new List<FormFiledMap>();
}
