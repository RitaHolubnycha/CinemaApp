using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class Film
    {
        public int FilmId { get; set; }

        [Required(ErrorMessage = "Назва фільму обов'язкова")]
        [StringLength(100, ErrorMessage = "Максимум 100 символів")]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Required(ErrorMessage = "Жанр обов'язковий")]
        [StringLength(50)]
        public string Genre { get; set; }

        [StringLength(1000, ErrorMessage = "Максимум 1000 символів")]
        public string? Description { get; set; }

        public ICollection<FilmActor>? FilmActors { get; set; }
    }
}