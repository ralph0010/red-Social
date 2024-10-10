using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{    
    public class Administrador : Usuario
    {
        public Administrador(string email, string contraseña) : base(email, contraseña)
        {
            this.Rol = "Admin";
        }
        //constructor
        public Administrador() { }
        public void Bloquear(Miembro miembro)
        {
            miembro.Bloqueado = true;

        }
        public void Desbloquear(Miembro miembro)
        {
            miembro.Bloqueado = false;
        }
        //bloquea un miembro
        public void Censurar(Post p)
        {
            p.Baneado = true;
        }
        public void DesCensurar(Post p)
        {
            p.Baneado = false;
        }
        //censura un post
    }
}
