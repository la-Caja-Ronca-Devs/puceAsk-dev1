using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace puceAsk_dev1.Models
{

    [NotMapped]
    public class PreguntasManager
    {
        public PreguntasManager()
        {
            this.pregunta = new Pregunta();
        }

        public IEnumerable<Pregunta> preguntas { get; set; }

        public IEnumerable<Respuesta> respuestas { get; set; }

        public IEnumerable<Categoria> categorias { get; set; }

        public Pregunta pregunta { get; set; }
        public List<Categoria> Categorias { get; set; }

        public int PaginaActual { get; set; }
        public int TotalRegistro { get; set; }
        public int RegistroPorPagina { get; set; }
    }
    
}