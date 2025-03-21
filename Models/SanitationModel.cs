using System.ComponentModel.DataAnnotations;                //För användning av Require
using System.ComponentModel.DataAnnotations.Schema;         //För användning av FK
using SanitationApp.Models;                                 //Hämtar in modeller

namespace dt191g_projekt.Models {

    public class SanitationModel {
        //Properties
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Saneringstyp är obligatorisk.")]
         [Display(Name = "Saneringstyp")]
        public string? SanitationType { get; set; }
        
        [Required(ErrorMessage = "Adress är obligatorisk.")]
         [Display(Name = "Adress")]
        public string? Location { get; set; }
       
        [Required(ErrorMessage = "Beskrivning är obligatorisk.")]
         [Display(Name = "Beskrivning")]
        public string? Description { get; set; }
      
         [Display(Name = "Avfallsmängd")]  
        public int? WasteAmount { get; set; }
         [Display(Name = "Skapad av")] 
        
        public string? CreatedBy { get; set; }


        //Relationer

          //FK till Customer
           [Display(Name = "Kund")]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public CustomerModel? Customer { get; set; }

        //FK till Worker
         [Display(Name = "Sanerare")]
        public int WorkerId { get; set; }
        [ForeignKey("WorkerId")]
        public WorkerModel? Worker { get; set; }


    }
}

