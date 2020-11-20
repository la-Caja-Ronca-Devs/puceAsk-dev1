using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace puceAsk_dev1.Models
{
    [Table("Respuesta", Schema = "ask")]
    public class Respuesta
    {
        [Key]
        [Column("id_respuesta")]
        public int RespuestaId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [Column("fecha_publicacion")]
        public DateTime FechaPublicacion { get; set; }

        [Required]
        [Column("id_cuenta")]
        public Cuenta Cuenta { get; set; }

        [Required]
        [Column("desc_respuesta")]
        public String DescRespuesta { get; set; }

        [Required]
        [Column("id_pregunta")]
        public Pregunta Pregunta { get; set; }
        public virtual Pregunta MejorRespuesta { get; set; }

    }
}