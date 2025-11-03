using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HelloLib;

namespace Helloweb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string Greeting { get; private set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        Greeting = Greeter.Message();
    }
}
