using System;
using System.Collections.Generic;

namespace Formbuilder.Models;

public partial class FormFiledMap
{
    public int FormFiledMapId { get; set; }

    public int? FormsMasterId { get; set; }

    public int? FieldOptionId { get; set; }

    public int? FieldId { get; set; }

    public virtual TblField? Field { get; set; }

    public virtual TblFieldOption? FieldOption { get; set; }

    public virtual FormsMaster? FormsMaster { get; set; }
}
