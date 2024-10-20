using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using WEB_253501_LAVRIV.Data;
using Web_253501_Lavriv.Domain.Entities;

namespace WEB_253501_LAVRIV.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        //private readonly WEB_253501_LAVRIV.Data.TemporaryDbContext _context;

        //public EditModel(WEB_253501_LAVRIV.Data.TemporaryDbContext context)
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

        //    var detail =  await _context.Details.FirstOrDefaultAsync(m => m.Id == id);
        //    if (detail == null)
        //    {
        //        return NotFound();
        //    }
        //    Detail = detail;
        //   ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        //    return Page();
        //}

        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more information, see https://aka.ms/RazorPagesCRUD.
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.Attach(Detail).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DetailExists(Detail.Id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return RedirectToPage("./Index");
        //}

        //private bool DetailExists(int id)
        //{
        //    return _context.Details.Any(e => e.Id == id);
        //}
    }
}
