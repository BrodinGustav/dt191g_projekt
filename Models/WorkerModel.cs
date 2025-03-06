using System.ComponentModel.DataAnnotations;
using dt191g_projekt.Models;

namespace SanitationApp.Models
{
    public class WorkerModel
    {
        public int Id { get; set; } 

        [Required]
        public string? Name { get; set; } 

        [Required]
        public string? Role { get; set; } 
        [Required]
        public int? PhoneNumber { get; set; } 

        //En arbetare kan ha flera saneringsarbeten
        public List<SanitationModel>? Sanitations { get; set; }
    }
}
