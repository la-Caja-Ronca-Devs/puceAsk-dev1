using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace puceAsk_dev1.Models
{
    [Table("pregunta", Schema = "ask")]
    public class Pregunta
    {
        [Key]
        [Column("id_pregunta")]
        public int PreguntaId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("titulo_pregunta")]
        public string TituloPregunta { get; set; }

        [Required]
        [Column("desc_pregunta")]
        public string DescPregunta { get; set; }

        [Required]
        [Column("fecha_pregunta")]
        public DateTime Fechapregunta { get; set; }

        [Required]
        [Column("id_cuenta")]
        public Cuenta Cuenta { get; set; }

        [Required]
        [Column("id_categoria")]
        public Categoria Categoria { get; set; }

        [Column("mejor_respuesta")]
        public virtual Respuesta MejorRespuesta { get; set; }

        public ICollection<Respuesta> Respuestas { get; set; }

    }
}