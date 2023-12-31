﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities
{
    //[Index(nameof(Name), IsUnique = true)]
    public class Genre: AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } 
        public string Example { get; set; }
        //public string Example2 { get; set; }
        public HashSet<Movie> Movies { get; set; }
    }
}
