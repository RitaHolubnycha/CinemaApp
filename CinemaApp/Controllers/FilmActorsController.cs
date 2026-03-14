using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;

namespace CinemaApp.Controllers
{
    public class FilmActorsController : Controller
    {
        private readonly CinemaAppContext _context;

        public FilmActorsController(CinemaAppContext context)
        {
            _context = context;
        }

        // GET: FilmActors
        public async Task<IActionResult> Index()
        {
            var cinemaAppContext = _context.FilmActors
                .Include(f => f.Actor)
                .Include(f => f.Film);

            return View(await cinemaAppContext.ToListAsync());
        }

        // GET: FilmActors/Details
        public async Task<IActionResult> Details(int? filmId, int? actorId)
        {
            if (filmId == null || actorId == null)
                return NotFound();

            var filmActor = await _context.FilmActors
                .Include(f => f.Actor)
                .Include(f => f.Film)
                .FirstOrDefaultAsync(m => m.FilmId == filmId && m.ActorId == actorId);

            if (filmActor == null)
                return NotFound();

            return View(filmActor);
        }

        // GET: FilmActors/Create
        public IActionResult Create(int? filmId, int? actorId, string returnUrl)
        {
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "Title", filmId);
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "LastName", actorId);
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        // POST: FilmActors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmActor filmActor, string returnUrl)
        {
            if (!_context.FilmActors.Any(fa =>
                fa.FilmId == filmActor.FilmId &&
                fa.ActorId == filmActor.ActorId))
            {
                _context.FilmActors.Add(filmActor);
                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
            ViewData["ActorId"] = new SelectList(_context.Actors, "ActorId", "LastName", filmActor.ActorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "Title", filmActor.FilmId);
            return View(filmActor);
        }

        // GET: FilmActors/Delete
        public async Task<IActionResult> Delete(int? filmId, int? actorId)
        {
            if (filmId == null || actorId == null)
                return NotFound();

            var filmActor = await _context.FilmActors
                .Include(f => f.Actor)
                .Include(f => f.Film)
                .FirstOrDefaultAsync(m => m.FilmId == filmId && m.ActorId == actorId);

            if (filmActor == null)
                return NotFound();

            return View(filmActor);
        }

        // POST: FilmActors/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int filmId, int actorId)
        {
            var filmActor = await _context.FilmActors
                .FirstOrDefaultAsync(m => m.FilmId == filmId && m.ActorId == actorId);

            if (filmActor != null)
            {
                _context.FilmActors.Remove(filmActor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
