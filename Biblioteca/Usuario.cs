using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{
    public class Usuario : IValidable
    {
        private static int UltimaId { get; set; } = 0;
        public int Id { get; set; } 
        public string Email { get; set; }

        public string Contraseña { get; set; }

        public string Rol { get; set; }

        public Usuario(string email, string contraseña)
        {
            this.Email = email;
            this.Contraseña = contraseña;
            this.Id = UltimaId++;
            
        }

        public Usuario()
        {
            Id = UltimaId++;
        }
       
        //Validamos usuario
        public virtual void Validar()
        {
            ValidarEmail();
            ValidarContraseña();
        }
        //Metodos internos de validarUsuario
        private void ValidarEmail()
        {
            if(this.Email == null || this.Email.Length <= 0)
            {
                throw new Exception("Error, el email no puede estar vacío");
            }
        }   
        private void ValidarContraseña()
        {
            if(this.Contraseña == null || this.Contraseña.Length <= 0)
            {
                throw new Exception("La contraseña no puede estar vacía");
            }
        }

        public override bool Equals(object? obj)
        {
            Usuario user = (Usuario)obj;
            return user.Email == this.Email;
        }
        //metodo Equals

        public override string ToString()
        {
            return $"{this.Email}";
        }
    }
}
