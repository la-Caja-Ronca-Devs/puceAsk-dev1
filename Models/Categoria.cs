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
        [Key]
        [Column("id_categoria")]
        public int CategoriaId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [Column("nombre_categoria")]
        public string NombreCategoria { get; set; }

        [Required]
        [Column("desc_categoria")]
        public string DescCategoria { get; set; }

        public ICollection<Pregunta> Preguntas { get; set; }
    }
}