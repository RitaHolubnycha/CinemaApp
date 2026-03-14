using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class Film
    {
        public int FilmId { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string? Genre { get; set; }

        public string? Description { get; set; }

        public ICollection<FilmActor>? FilmActors { get; set; }
    }
}