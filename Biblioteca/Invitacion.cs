using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{
    public class Invitacion 
    {
        public Miembro MiembroSolicitante { get; set; }

        public Miembro MiembroSolicitado { get; set; }

        public EstadoSolicitud Estado { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public string id { get; set; }

        //getters y setters


        public Invitacion(Miembro MiembroSolicitante, Miembro MiembroSolicitado, DateTime FechaSolicitud)
        {

            this.MiembroSolicitante = MiembroSolicitante;
            this.MiembroSolicitado = MiembroSolicitado;
            this.Estado = EstadoSolicitud.pendiente_aprobacion;
            this.FechaSolicitud = FechaSolicitud;
            this.id = MiembroSolicitante.Apellido + "_" + MiembroSolicitado.Apellido;
            this.MiembroSolicitado.Pendientes.Add(this);
        }
        //constructor

        public void AgregarAmigos()
        {
            this.Estado = EstadoSolicitud.aprobada;
            MiembroSolicitante.Amigos.Add(MiembroSolicitado);
            MiembroSolicitado.Amigos.Add(MiembroSolicitante);

        }
        //agrega amigos
        public void RechazarAmigos()
        {
            this.Estado = EstadoSolicitud.rechazada;
        }
        //rechaza una solicitud
        
    }
}
