using System.Text.Encodings.Web;
using JustShortIt.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Distributed;

namespace JustShortIt.Pages; 

[Authorize]
public class UrlsModel : PageModel {
    [BindProperty]
    public UrlRedirect? Model { get; set; }
    [BindProperty(Name="message")]
    public string? Message { get; set; }

    private string BaseUrl { get; }
    private IDistributedCache Db { get; }

    public UrlsModel(IConfiguration configuration, IDistributedCache db) {
#if DEBUG
	    BaseUrl = "https://localhost/";
#else 
        BaseUrl = new Uri(BaseUrl, UriKind.Absolute).ToString();
#endif
        Db = db;
    }
    
    public async Task<IActionResult> OnPostInspectAsync() {
        string? id = Request.Form["Inspect_Id"];
        if (id is null || string.IsNullOrEmpty(id)) {
            ModelState.AddModelError("Inspect_Id", "ID is a required field");
            return Page();
        }

        if (await Db.GetAsync(id) is null) {
            ModelState.AddModelError("Inspect_Id", "ID does not exist");
            return Page();
        }

        return RedirectToPage("Inspect", new { Id = id });
    }

    public async Task<IActionResult> OnPostAsync() {
        if (!ModelState.IsValid) return Page();
        string id = HttpUtility.UrlEncode(Model.Id);

        if (await Db.GetAsync(id) is not null) {
            Message = "This ID is already taken, sorry!";
            return Page();
        }

        if (Uri.TryCreate($"{BaseUrl}{id}", UriKind.Absolute, out Uri? link) is false) {
            Message = "This ID cannot be used in a URL, sorry!";
            return Page();
        }

        await Db.SetStringAsync(id, Model.Target, new DistributedCacheEntryOptions {
            AbsoluteExpiration = DateTime.FromBinary(long.Parse(Model.ExpirationDate))
        });
        
        ModelState.Clear(); 
        ModelState.SetModelValue(nameof(UrlRedirect.Id), GenerateNewId(), GenerateNewId());

        Message = $"URL Generated! <a href='{link}'>{link}</a>. " +
                  $"<button class='button is-link is-small' onclick='navigator.clipboard.writeText(\"{link}\")'>Copy</button>";
        return OnGet(Message);
    }
    
    public IActionResult OnGet(string message) {
        Message = message;
        Model = new UrlRedirect(GenerateNewId(), string.Empty, string.Empty);
        return Page();
    }
    
    private static string GenerateNewId() {
        string base64Guid = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");  ;
        return base64Guid[..6];
    }
}