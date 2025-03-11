using System.ComponentModel.DataAnnotations;
using dt191g_projekt.Models;


namespace SanitationApp.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        [Display(Name = "Namn")]
        [Required]
        public string? Name { get; set; }
         [Display(Name = "Adress")]
        [Required]
        public string? Address { get; set; }
         [Display(Name = "Telefonnummer")]
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }

        //En kund kan ha flera saneringsarbeten
        public List<SanitationModel>? Sanitations { get; set; }
    }
}