using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace puceAsk_dev1.Models
{

    [Table("Cuenta", Schema = "ask")]
    public class Cuenta
    {
        [Key]
        [Column("id_cuenta")]
        public int CuentaId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Column("saldo_cuenta")]
        public int SaldoCuenta { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
        public ICollection<Pregunta> Preguntas { get; set; }
        public ICollection<Respuesta> Respuestas { get; set; }
        public ICollection<Mensaje> Enviados { get; set; }
        public ICollection<Mensaje> Recibidos { get; set; }
    }
}