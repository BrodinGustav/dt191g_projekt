using System.ComponentModel.DataAnnotations;

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
        public string? WasteAmount { get; set; } 
        public string? CreatedBy { get; set; }



    }
}

