using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class FilmActor
    {
        [Required]
        public int FilmId { get; set; }

        [Required]
        public int ActorId { get; set; }

        public Film? Film { get; set; }
        public Actor? Actor { get; set; }
    }
}