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
        [Key, Column(Order = 0)]
        public int PreguntaId { get; set; }

        [Required]
        [Display(Name = "Titulo de pregunta")]
        public string TituloPregunta { get; set; }

        [Required]
        [Display(Name = "Texto de pregunta")]
        public string DescPregunta { get; set; }

        [Required]
        [Display(Name = "Fecha de pregunta")]
        public DateTime Fechapregunta { get; set; }

        [Column(Order = 1),ForeignKey("Usuario")]
        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }

        [Column(Order = 2), ForeignKey("Categoria")]
        public  int CategoriaId { get; set; }
        public  Categoria Categoria { get; set; }

        public string MejorUsuarioRespuestaId { get; set; }

        public ICollection<Respuesta> Respuestas {get; set;}


    }
}