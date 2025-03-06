using System.ComponentModel.DataAnnotations;                //För användning av Require
using System.ComponentModel.DataAnnotations.Schema;         //För användning av FK
using SanitationApp.Models;                                 //Hämtar in modeller

namespace dt191g_projekt.Models {

    public class SanitationModel {
        //Properties
        public int Id { get; set; }
        [Required]
        public string? SanitationType { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public string? Description { get; set; }
        public int? WasteAmount { get; set; } 
        public string? CreatedBy { get; set; }


        //Relationer

          //FK till Customer
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public CustomerModel? Customer { get; set; }

        //FK till Worker
        public int WorkerId { get; set; }
        [ForeignKey("WorkerId")]
        public WorkerModel? Worker { get; set; }


    }
}

