using System.ComponentModel.DataAnnotations;
using dt191g_projekt.Models;


namespace SanitationApp.Models
{
    public class CustomerModel
    {
        public int Id { get; set; } 

        [Required]
        public string? Name { get; set; } 

        [Required]
        public string? Address { get; set; }

        [Required]
        [Phone]
        public string? PhoneNumber { get; set; } 

        //En kund kan ha flera saneringsarbeten
        public List<SanitationModel>? Sanitations { get; set; }
    }
}