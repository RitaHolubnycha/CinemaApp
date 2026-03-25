using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Data;
using CinemaApp.Models;
using CinemaApp.Helpers;

namespace CinemaApp.Controllers
{
    public class ActorsController : Controller
    {
        private readonly CinemaAppContext _context;

        public ActorsController(CinemaAppContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            var actors = await _context.Actors
                .Include(a => a.FilmActors)
                .ThenInclude(fa => fa.Film)
                .ToListAsync();

            return View(actors);
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var actor = await _context.Actors
                .Include(a => a.FilmActors)
                .ThenInclude(fa => fa.Film)
                .FirstOrDefaultAsync(a => a.ActorId == id);

            if (actor == null)
                return NotFound();

            return View(actor);
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create (запис у сесію)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,BirthDate,Nationality,Biography")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetObject("NewActor", actor);
                return RedirectToAction(nameof(ConfirmCreate));
            }

            return View(actor);
        }

        // GET: Actors/ConfirmCreate
        public IActionResult ConfirmCreate()
        {
            var actor = HttpContext.Session.GetObject<Actor>("NewActor");

            if (actor == null)
                return RedirectToAction(nameof(Create));

            return View(actor);
        }

        // POST: Actors/ConfirmCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ConfirmCreate")]
        public async Task<IActionResult> ConfirmCreatePost()
        {
            var actor = HttpContext.Session.GetObject<Actor>("NewActor");

            if (actor != null)
            {
                _context.Actors.Add(actor);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("NewActor");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
                return NotFound();

            return View(actor);
        }

        // POST: Actors/Edit (запис у сесію)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ActorId,FirstName,LastName,BirthDate,Nationality,Biography")] Actor actor)
        {
            if (id != actor.ActorId)
                return NotFound();

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetObject("EditActor", actor);
                return RedirectToAction(nameof(ConfirmEdit));
            }

            return View(actor);
        }

        // GET: Actors/ConfirmEdit
        public IActionResult ConfirmEdit()
        {
            var actor = HttpContext.Session.GetObject<Actor>("EditActor");

            if (actor == null)
                return RedirectToAction(nameof(Index));

            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ConfirmEdit")]
        public async Task<IActionResult> ConfirmEditPost()
        {
            var actor = HttpContext.Session.GetObject<Actor>("EditActor");

            if (actor != null)
            {
                var existingActor = await _context.Actors.FindAsync(actor.ActorId);

                if (existingActor == null)
                    return NotFound();

                // оновлюємо вручну
                existingActor.FirstName = actor.FirstName;
                existingActor.LastName = actor.LastName;
                existingActor.BirthDate = actor.BirthDate;
                existingActor.Nationality = actor.Nationality;
                existingActor.Biography = actor.Biography;

                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("EditActor");
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var actor = await _context.Actors
                .FirstOrDefaultAsync(a => a.ActorId == id);

            if (actor == null)
                return NotFound();

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor != null)
            {
                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}