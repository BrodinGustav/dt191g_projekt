using System.ComponentModel.DataAnnotations;
using dt191g_projekt.Models;

namespace SanitationApp.Models
{
    public class WorkerModel
    {
        public int Id { get; set; } 

         [Display(Name = "Namn")]
        [Required(ErrorMessage = "Namn är obligatorisk.")]
        public string? Name { get; set; }

         [Display(Name = "Roll")]
        [Required(ErrorMessage = "Roll är obligatorisk.")]
        public string? Role { get; set; } 

        [Required(ErrorMessage = "Telefonnummer är obligatorisk.")]
         [Display(Name = "Telefonnummer")]
        public string? PhoneNumber { get; set; } 

        //En arbetare kan ha flera saneringsarbeten
        public List<SanitationModel>? Sanitations { get; set; }
    }
}
