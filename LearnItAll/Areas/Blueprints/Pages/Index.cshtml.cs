using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace LearnItAll.Areas.Blueprints.Pages
{
    [AllowAnonymous]

    public class IndexModel : PageModel
    {

        public List<CareerPath> CareerPaths { get; set; } = new();

        public void OnGet()
        {
            CareerPaths = new List<CareerPath>() {
                new CareerPath { Industry ="Technology", Name = "C# Software Developer"},
                new CareerPath { Industry ="Technology", Name = "MSSQL DBA"},
                new CareerPath { Industry ="Technology", Name = "Scrum Master"}
            };
        }




        public class CareerPath
        {
            public Guid Id {  get; set; }
            public string Name { get; set; } = string.Empty;
            public string Industry { get; set; } = string.Empty;
            public List<string> Skills { get; set; } = new();
        }

        public class Skill
        {
            public Guid Id {  get; set; }
            public string Name { get; set; } = string.Empty;
            public List<Part> Parts { get; set; } = new();
            public List<Node> Nodes { get; set; } = new();

        }

        public class Part
        {
            public Guid Id {  get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class Node
        {
            public Guid Id {  get; set; }
            public string Name { get; set; } = string.Empty;
            public List<Part> Parts { get; set; } = new();
        }
    }
}