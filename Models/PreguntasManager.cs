using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace puceAsk_dev1.Models
{
    [NotMapped]
    public class PreguntasManager
    {
        public IEnumerable<Pregunta> preguntas { get; set; }

        public IEnumerable<Respuesta> respuestas { get; set; }

        public IEnumerable<Categoria> categorias { get; set; }
        public Pregunta pregunta { get; set; }
    }
}