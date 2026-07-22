using Azure.Core;
using Formbuilder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Formbuilder.Controllers
{
    public class FormsController : Controller
    {
        private readonly FormbuilderContext _context;

        public FormsController(FormbuilderContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Create(FormViewModel formViewModel)
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> GetFormsList([FromBody] FormsListRequestViewModel request)
        {
            try
            {
                var query = _context.FormsMasters
                    .Include(o => o.FormFiledMaps)
                    .ThenInclude(f => f.Field)
                    .Include(o => o.FormFiledMaps)
                    .ThenInclude(f => f.FieldOption)
                    .AsNoTracking();

                int totalRecords = await query.CountAsync();

                if (!string.IsNullOrWhiteSpace(request.SearchValue))
                {
                    string search = request.SearchValue.Trim().ToLower();
                    query = query.Where(o => o.FormName.ToLower().Contains(search));
                }

                int filteredRecords = await query.CountAsync();

                if (!string.IsNullOrEmpty(request.SortColumn))
                {
                    bool isAsc = string.Equals(request.SortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                    query = request.SortColumn.ToLower() switch
                    {
                        "formname" => isAsc ? query.OrderBy(o => o.FormName) : query.OrderByDescending(o => o.FormName),
                        _ => query.OrderByDescending(o => o.FormsMasterId)
                    };
                }
                else
                {
                    query = query.OrderByDescending(o => o.FormsMasterId);
                }

                // Pagination (Skip & Take)
                var pagedData = await query
                    .Skip(request.Start)
                    .Take(request.Length > 0 ? request.Length : 10)
                    .Select(o => new FormsListDto
                    {
                        FormId = o.FormsMasterId,
                        FormName = o.FormName,
                    })
                    .ToListAsync();

                var response = new DataTableResponseViewModel<FormsListDto>
                {
                    Draw = request.Draw,
                    RecordsTotal = totalRecords,
                    RecordsFiltered = filteredRecords,
                    Data = pagedData
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteForm(int id)
        {
            try
            {
                var formsMaster = await _context.FormsMasters.Include(x=> x.FormFiledMaps)
                    .ThenInclude(y=> y.FieldOption)
                    .FirstOrDefaultAsync(x=> x.FormsMasterId == id);
                if (formsMaster == null)
                {
                    return NotFound(new { success = false, message = "formsMaster not found." });
                }
                var optionIds = formsMaster.FormFiledMaps.Select(x => x.FieldOptionId);
                _context.FormFiledMaps.RemoveRange(formsMaster.FormFiledMaps.ToList());
                var options = _context.TblFieldOptions.Where(x => optionIds.Contains(x.FieldOptionId)).ToList();
                _context.TblFieldOptions.RemoveRange(options);
                _context.Remove(formsMaster);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Order deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
