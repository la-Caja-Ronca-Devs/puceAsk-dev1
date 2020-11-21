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
        [Key]
        [Column("id_mensaje")]
        public int MensajeId { get; set; }

        public int ReceptorId { get; set; }
        public Cuenta Receptor { get; set; }

        public int EmisorId { get; set; }
        public Cuenta Emisor { get; set; }

        [Required]
        [Column("mensaje")]
        public string MensajeDesc { get; set; }

    }
}