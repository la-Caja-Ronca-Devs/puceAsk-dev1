using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace puceAsk_dev1.Models
{
    public class PreguntasManager
    {
        public IEnumerable<Pregunta> preguntas { get; set; }
        public IEnumerable<Respuesta> respuestas { get; set; }
    }
}