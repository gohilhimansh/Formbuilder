using Formbuilder.Models;
using Microsoft.AspNetCore.Mvc;

namespace Formbuilder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : Controller
    {
        private readonly FormbuilderContext _context;

        public FieldController(FormbuilderContext context)
        {
            _context = context;
        }
        [HttpGet("GetFields")]
        public async Task<IActionResult> GetFields()
        {
            var fields = _context.TblFields
                .OrderBy(f => f.FieldName)
                .Select(f => new
                {
                    f.FieldId,
                    f.FieldName,
                    f.InputType
                }).ToList();
            return Ok(fields);
        }

        [HttpGet("GetItemsByFieldOptions/{fieldId}")]
        public async Task<IActionResult> GetItemsByFieldOptions(int fieldId)
        {
            var fields = _context.TblFieldOptions
                .Where(f => f.FieldId == fieldId)
                .OrderBy(f => f.OptionTitle)
                .Select(f => new
                {
                    f.FieldOptionId,
                    f.OptionTitle,
                    f.Isrequired
                }).ToList();
            return Ok(fields);
        }
    }
}
