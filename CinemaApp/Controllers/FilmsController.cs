using CinemaApp.Data;
using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Create([Bind("Title,ReleaseDate,Genre,Description")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
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
        public async Task<IActionResult> Edit(int id, [Bind("FilmId,Title,ReleaseDate,Genre,Description")] Film film)
        {
            if (id != film.FilmId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.FilmId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(film);
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
                // видаляємо усі зв’язки перед видаленням
                _context.FilmActors.RemoveRange(film.FilmActors);
                _context.Films.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(f => f.FilmId == id);
        }
    }
}