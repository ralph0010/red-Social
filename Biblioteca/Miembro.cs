using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{
    public class Miembro : Usuario, IComparable
    {
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public List<Miembro> Amigos { get; set; }

        public bool Bloqueado { get; set; }

        public List<Invitacion> Pendientes { get; set; }

        //getters y setters
        
        public Miembro(string email, string contraseña, string nombre, string apellido, DateTime fechaNacimiento) : base(email, contraseña)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.FechaNacimiento = fechaNacimiento;
            this.Bloqueado = false;
            Amigos = new List<Miembro>();
            Pendientes = new List<Invitacion>();
            this.Rol = "Miembro";
        }
        //constructor
        public Miembro()
        {

        }


        private void MiembroBloqueado()
        {
            if (this.Bloqueado == true)
            {
                throw new Exception("Lo siento no puede realizar la función porque ha sido bloqueado");
            }
        }
        //verifico si el miembro esta bloqueado

        public void SolicitarVinculo(Miembro solicitante, Miembro miembroSeleccionado)
        {
            MiembroBloqueado();
        }

        public override void Validar()
        {
            ValidarEmail();
            ValidarContraseña();
            ValidarNombre();
            ValidarApellido();
            ValidarFecha();
        }
        //Metodo para validar Miembro.

        //Inicio metodos para vaidar
        private void ValidarEmail()
        {
            if (this.Email == null || this.Email.Length <= 0)
            {
                throw new Exception("Error, el email no puede estar vacío");
            }
        }
        private void ValidarContraseña()
        {
            if (this.Contraseña == null || this.Contraseña.Length <= 0)
            {
                throw new Exception("Error, la contraseña no puede estar vacía");
            }
        }
        private void ValidarNombre()
        {
            if (this.Nombre == null || this.Nombre.Length <= 0)
            {
                throw new Exception("Error, el nombre no puede estar vacío");
            }
        }
        private void ValidarApellido()
        {
            if (this.Apellido == null || this.Apellido.Length <= 0)
            {
                throw new Exception("Error, el apellido no puede estar vacío");
            }
        }

        private void ValidarFecha()
        {
            DateTime presente = DateTime.Now;
            if (this.FechaNacimiento > presente || this.FechaNacimiento == null)
            {
                throw new Exception("Fecha inválida");
            }
        }

        //Fin Metodos validar


        private Invitacion hallarInvitacionPendiente(Miembro m)
        {
            foreach(Invitacion inv in this.Pendientes)
            {
                if (inv.MiembroSolicitante.Equals(m))
                {
                    return inv;
                }
            }
            throw new Exception("No existe la solicitud pendiente");
        }

        public void AceptarSolicitud(Miembro m)
        {
            MiembroBloqueado();
            Invitacion inv = hallarInvitacionPendiente(m);
            inv.AgregarAmigos();
            this.Pendientes.Remove(inv);
        }

        public void RechazarSolicitud(Miembro m)
        {
            MiembroBloqueado();
            Invitacion inv = hallarInvitacionPendiente(m);
            inv.RechazarAmigos();
            this.Pendientes.Remove(inv);
        }

        //Acepto, Rechazo solicitud
        public void RealizarPost()
        {

        }

        public void InteractuarConPost(Post post)
        {

        }

        public void Reaccion(Publicacion publicacion)
        {

        }

        private void ExcepcionBloqueado()
        {

        }

        
        public override string ToString()
        {
            return $"{this.Nombre} {this.Apellido}";
        }

        public List<Miembro> MostrarPendientes()
        {
            List<Miembro> pendientes = new List<Miembro>();
            
            foreach (Invitacion invitacionPendiente in this.Pendientes)
            {
                pendientes.Add(invitacionPendiente.MiembroSolicitante);
            }
            return pendientes;
        }
        //retorna lista de invitaciones pendientes
        public override bool Equals(object? obj)
        {
            Miembro miembroAComparar = (Miembro)obj;
            return miembroAComparar.Email == this.Email;
        }
        //metodo Equals
        public int CompareTo(object? obj)
        {
            Miembro miembro = obj as Miembro;
            return this.Apellido.CompareTo(miembro.Apellido);
        } 
    }
}
