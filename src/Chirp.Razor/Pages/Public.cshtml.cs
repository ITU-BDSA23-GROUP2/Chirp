﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _service;

    public IEnumerable<CheepDto> Cheeps { get; set; }

    public PublicModel(ICheepRepository service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        var t = Convert.ToInt32(Request.Query["page"]);
        if (t == 0) t = 1;
        Cheeps = _service.GetCheeps(t);
        return Page();
    }
}