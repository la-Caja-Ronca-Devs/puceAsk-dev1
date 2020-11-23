using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace puceAsk_dev1.Models
{
    [Table("Respuestas", Schema = "ask")]
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

        public int CuentaId { get; set; }
        public Cuenta Cuenta { get; set; }

        [Required]
        [Column("desc_respuesta")]
        public String DescRespuesta { get; set; }
        
   

        public int PreguntaId { get; set; }
        public Pregunta Pregunta { get; set; }

    }
}