using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using WEB_253501_LAVRIV.Data;
using Web_253501_Lavriv.Domain.Entities;

namespace WEB_253501_LAVRIV.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        //private readonly WEB_253501_LAVRIV.Data.TemporaryDbContext _context;

        //public DeleteModel(WEB_253501_LAVRIV.Data.TemporaryDbContext context)
        //{
        //    _context = context;
        //}

        //[BindProperty]
        //public Detail Detail { get; set; } = default!;

        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var detail = await _context.Details.FirstOrDefaultAsync(m => m.Id == id);

        //    if (detail == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        Detail = detail;
        //    }
        //    return Page();
        //}

        //public async Task<IActionResult> OnPostAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var detail = await _context.Details.FindAsync(id);
        //    if (detail != null)
        //    {
        //        Detail = detail;
        //        _context.Details.Remove(Detail);
        //        await _context.SaveChangesAsync();
        //    }

        //    return RedirectToPage("./Index");
        //}
    }
}
