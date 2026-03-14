using CinemaApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class Actor
    {
        public int ActorId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Nationality { get; set; }

        public string? Biography { get; set; }

        public ICollection<FilmActor>? FilmActors { get; set; }
    }
}