using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Threading.Tasks;

namespace LearnItAll.Areas.Blueprints.Pages
{
    [AllowAnonymous]

    public class IndexModel : PageModel
    {

        public List<CareerPath> CareerPaths { get; set; } = new();

        public async Task OnGet()
        {
            CareerPaths = DB.CareerPaths;
            await Task.CompletedTask;
        }
        public async Task<IActionResult> OnPostDelete(string name)
        {
            DB.CareerPaths.RemoveAll(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

           return await Task.FromResult(new ContentResult());
        }

        public static class DB
        {
            public static List<CareerPath> CareerPaths { get; set; } = new();

            static DB()
            {
                CareerPaths = new List<CareerPath>() {
                new CareerPath { Industry ="Technology", Name = "C# Software Developer"},
                new CareerPath { Industry ="Technology", Name = "MSSQL DBA"},
                new CareerPath { Industry ="Technology", Name = "Scrum Master"}
            };
            }
        }

        public class CareerPath
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Industry { get; set; } = string.Empty;
            public List<string> Skills { get; set; } = new();

        }

       
    }
}