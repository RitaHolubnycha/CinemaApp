using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Helpers;

namespace CinemaApp.Controllers
{
    public class FilmsController : Controller
    {
        private readonly CinemaAppContext _context;

        public FilmsController(CinemaAppContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            var films = await _context.Films
                .Include(f => f.FilmActors)
                .ThenInclude(fa => fa.Actor)
                .ToListAsync();

            return View(films);
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var film = await _context.Films
                .Include(f => f.FilmActors)
                .ThenInclude(fa => fa.Actor)
                .FirstOrDefaultAsync(f => f.FilmId == id);

            if (film == null) return NotFound();

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,ReleaseDate,Genre,Description")] Film film)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetObject("NewFilm", film);
                return RedirectToAction("ConfirmCreate");
            }

            return View(film);
        }

        // GET: Films/ConfirmCreate
        public IActionResult ConfirmCreate()
        {
            var film = HttpContext.Session.GetObject<Film>("NewFilm");

            if (film == null)
                return RedirectToAction(nameof(Create));

            return View(film);
        }

        // POST: Films/ConfirmCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCreatePost()
        {
            var film = HttpContext.Session.GetObject<Film>("NewFilm");

            if (film != null)
            {
                _context.Films.Add(film);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("NewFilm");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var film = await _context.Films.FindAsync(id);
            if (film == null) return NotFound();

            return View(film);
        }

        // POST: Films/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("FilmId,Title,ReleaseDate,Genre,Description")] Film film)
        {
            if (id != film.FilmId) return NotFound();

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetObject("EditFilm", film);
                return RedirectToAction("ConfirmEdit");
            }

            return View(film);
        }

        // GET: Films/ConfirmEdit
        public IActionResult ConfirmEdit()
        {
            var film = HttpContext.Session.GetObject<Film>("EditFilm");

            if (film == null)
                return RedirectToAction(nameof(Index));

            return View(film);
        }

        // POST: Films/ConfirmEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ConfirmEditPost")]
        public async Task<IActionResult> ConfirmEditPost()
        {
            var film = HttpContext.Session.GetObject<Film>("EditFilm");

            if (film != null)
            {
                var existingFilm = await _context.Films.FindAsync(film.FilmId);

                if (existingFilm == null)
                    return NotFound();

                existingFilm.Title = film.Title;
                existingFilm.ReleaseDate = film.ReleaseDate;
                existingFilm.Genre = film.Genre;
                existingFilm.Description = film.Description;

                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("EditFilm");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var film = await _context.Films
                .Include(f => f.FilmActors)
                .ThenInclude(fa => fa.Actor)
                .FirstOrDefaultAsync(f => f.FilmId == id);

            if (film == null) return NotFound();

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Films
                .Include(f => f.FilmActors)
                .FirstOrDefaultAsync(f => f.FilmId == id);

            if (film != null)
            {
                _context.FilmActors.RemoveRange(film.FilmActors);
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(f => f.FilmId == id);
        }
    }
}