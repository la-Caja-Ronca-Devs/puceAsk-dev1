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
        

        [Required]
        public DateTime FechaPublicacion { get; set; }
        [Key,Column(Order = 0), ForeignKey("Usuario")]
        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }


        [Required]
        public String DescRespuesta { get; set; }
      
        [Key,Column(Order = 1), ForeignKey("Pregunta")]
        public int PreguntaId { get; set; }
        public Pregunta Pregunta { get; set; }

    }
}