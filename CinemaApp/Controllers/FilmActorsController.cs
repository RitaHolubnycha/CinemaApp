using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;
using CinemaApp.Helpers;

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

        // 🔥 POST: FilmActors/Create → ТЕПЕР ЧЕРЕЗ SESSION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FilmActor filmActor, string returnUrl)
        {
            HttpContext.Session.SetObject("NewFilmActor", filmActor);
            HttpContext.Session.SetString("ReturnUrl", returnUrl ?? "");

            return RedirectToAction(nameof(ConfirmCreate));
        }

        // ✅ GET: ConfirmCreate
        public IActionResult ConfirmCreate()
        {
            var filmActor = HttpContext.Session.GetObject<FilmActor>("NewFilmActor");

            if (filmActor == null)
                return RedirectToAction(nameof(Index));

            ViewBag.Film = _context.Films.Find(filmActor.FilmId);
            ViewBag.Actor = _context.Actors.Find(filmActor.ActorId);

            return View(filmActor);
        }

        // ✅ POST: ConfirmCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ConfirmCreate")]
        public async Task<IActionResult> ConfirmCreatePost()
        {
            var filmActor = HttpContext.Session.GetObject<FilmActor>("NewFilmActor");

            if (filmActor != null)
            {
                if (!_context.FilmActors.Any(fa =>
                    fa.FilmId == filmActor.FilmId &&
                    fa.ActorId == filmActor.ActorId))
                {
                    _context.FilmActors.Add(filmActor);
                    await _context.SaveChangesAsync();
                }

                HttpContext.Session.Remove("NewFilmActor");
            }

            var returnUrl = HttpContext.Session.GetString("ReturnUrl");

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult Delete(int filmId, int actorId, string returnUrl)
        {
            var filmActor = new FilmActor
            {
                FilmId = filmId,
                ActorId = actorId
            };

            HttpContext.Session.SetObject("DeleteFilmActor", filmActor);
            HttpContext.Session.SetString("ReturnUrl", returnUrl ?? "");

            return RedirectToAction(nameof(ConfirmDelete));
        }
        public IActionResult ConfirmDelete()
        {
            var filmActor = HttpContext.Session.GetObject<FilmActor>("DeleteFilmActor");

            if (filmActor == null)
                return RedirectToAction(nameof(Index));

            ViewBag.Film = _context.Films.Find(filmActor.FilmId);
            ViewBag.Actor = _context.Actors.Find(filmActor.ActorId);

            return View(filmActor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ConfirmDelete")]
        public async Task<IActionResult> ConfirmDeletePost()
        {
            var filmActor = HttpContext.Session.GetObject<FilmActor>("DeleteFilmActor");

            if (filmActor != null)
            {
                var entity = await _context.FilmActors
                    .FirstOrDefaultAsync(f =>
                        f.FilmId == filmActor.FilmId &&
                        f.ActorId == filmActor.ActorId);

                if (entity != null)
                {
                    _context.FilmActors.Remove(entity);
                    await _context.SaveChangesAsync();
                }

                HttpContext.Session.Remove("DeleteFilmActor");
            }

            var returnUrl = HttpContext.Session.GetString("ReturnUrl");

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }
    }
}
