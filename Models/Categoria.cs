using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace puceAsk_dev1.Models
{
    [Table("Categoria", Schema = "ask")]
    public class Categoria
    {
        [Key, Column(Order = 0)]
        public int CategoriaId { get; set; }

       

        [Required]
        public string NombreCategoria { get; set; }

        [Required]
        public string DescCategoria { get; set; }

        public ICollection<Pregunta> Preguntas { get; set; }
    }
}