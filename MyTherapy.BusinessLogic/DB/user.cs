namespace MyTherapy.BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MyTherapy.user")]
    public partial class user
    {
        public int UserID { get; set; }

        [Required]
        [StringLength(30)]
        public string Nome { get; set; }

        [Required]
        [StringLength(30)]
        public string Cognome { get; set; }

        [StringLength(20)]
        public string PartitaIVA { get; set; }
    }
}
