using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace puceAsk_dev1.Models
{
    [Table("Mensaje", Schema = "ask")]
    public class Mensaje
    {
        [Key, Column(Order = 0)]
        public int MensajeId { get; set; }
        [Column(Order = 1), ForeignKey("Receptor")]
        public string ReceptorId { get; set; }
        public ApplicationUser Receptor { get; set; }
        [Column(Order = 2), ForeignKey("Emisor")]
        public string EmisorId { get; set; }
        public ApplicationUser Emisor { get; set; }

        [Required]
        public string MensajeDesc { get; set; }
        [Required]
        [Column("fechaMensaje")]
        public DateTime FechaMensaje { get; set; }

        [Required]
        public DateTime FechaMensaje { get; set; }

    }
}