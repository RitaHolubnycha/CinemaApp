using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class Actor
    {
        public int ActorId { get; set; }

        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [StringLength(50, ErrorMessage = "Максимум 50 символів")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Прізвище обов'язкове")]
        [StringLength(50, ErrorMessage = "Максимум 50 символів")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Національність обов'язкова")]
        [StringLength(50)]
        public string Nationality { get; set; }

        [StringLength(500, ErrorMessage = "Максимум 500 символів")]
        public string? Biography { get; set; }

        public ICollection<FilmActor> FilmActors { get; set; } = new List<FilmActor>();
    }
}