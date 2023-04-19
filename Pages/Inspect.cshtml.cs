using JustShortIt.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;

namespace JustShortIt.Pages; 

[Authorize]
public class InspectModel : PageModel {
    [BindProperty(Name = "id", SupportsGet = true)]
    public string? Id { get; set; } = string.Empty;
    [BindProperty(Name="message")]
    public string? Message { get; set; }

    public UrlRedirect? UrlRedirect { get; set; }
    
    private IDistributedCache Db { get; }

    public InspectModel(IDistributedCache db) {
        Db = db;
    }

    public async Task<IActionResult> OnPostAsync() {
        if (Id == null) return await OnGet(null, $"Delete request without ID, aborted.");

        await Db.RemoveAsync(Id);

        return await OnGet(null, $"ID '{Id}' successfully deleted.");
    }

    public async Task<IActionResult> OnGet(string? id, string? message) {
        if (id is null && message is null) return RedirectToPage("Urls");
        
        Id = id;
        Message = message;

        if (Id is not null) {
            string? url = await Db.GetStringAsync(Id);
            if (url is not null) 
                UrlRedirect = new UrlRedirect(Id, url, string.Empty);
        }

        return Page();
    }
}