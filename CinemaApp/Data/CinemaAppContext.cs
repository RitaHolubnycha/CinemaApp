using System;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Models;

namespace CinemaApp.Data
{
    public class CinemaAppContext : DbContext
    {
        public CinemaAppContext(DbContextOptions<CinemaAppContext> options)
            : base(options)
        {
        }

        public DbSet<Actor> Actors { get; set; } = default!;
        public DbSet<Film> Films { get; set; } = default!;
        public DbSet<FilmActor> FilmActors { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmActor>()
                .HasKey(fa => new { fa.FilmId, fa.ActorId });

            modelBuilder.Entity<FilmActor>()
                .HasOne(fa => fa.Film)
                .WithMany(f => f.FilmActors)
                .HasForeignKey(fa => fa.FilmId);

            modelBuilder.Entity<FilmActor>()
                .HasOne(fa => fa.Actor)
                .WithMany(a => a.FilmActors)
                .HasForeignKey(fa => fa.ActorId);
        }
    }
}