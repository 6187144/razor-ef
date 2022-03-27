#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor_ef.Model;

namespace razor_ef.Pages.Games
{
    public class IndexModel : PageModel
    {
        private readonly GameStoreContext _context;
        private readonly ILogger<IndexModel> _logger;
        [BindProperty(SupportsGet = true, Name = "Query")]
        public string Query { get; set; }
        [BindProperty(SupportsGet = true, Name = "BefAft")]
        public string BefAft { get; set; }
        [BindProperty(SupportsGet = true, Name = "DQuery")]
        public string DQuery { get; set; }
        public IndexModel(GameStoreContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IList<Game> Game { get; set; }

        public async Task OnGetAsync()
        {
            var games = from g in _context.Game select g;
            if (!string.IsNullOrEmpty(Query))
            {
                games = games.Where(g => g.Title.ToLower().Contains(Query.ToLower()));
            }
            if (!string.IsNullOrEmpty(DQuery) && !string.IsNullOrEmpty(BefAft))
            {
                DateTime time1 = Convert.ToDateTime(DQuery);
                if (BefAft == "After")
                {
                    games = games.Where(g => (DateTime.Compare(g.DatePublished, time1) >= 0));
                }
                if (BefAft == "Before")
                {
                    games = games.Where(g => (DateTime.Compare(g.DatePublished, time1) <= 0));
                }
            }
            //_logger.Log(LogLevel.Information, Query);
            // Game = await _context.Game.ToListAsync();
            Game = await games.ToListAsync();
        }
    }
}
