using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities
{
    public class Log
    {
        public Guid Id { get; set; }
        public string Message { get; set; }

        public string Example { get; set; }
    }
}
