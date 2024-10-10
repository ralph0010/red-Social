using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{ 
    public class Comentario : Publicacion
    {
        public Comentario(string titulo, string texto, Miembro autor, bool privado, DateTime fecha) : base(titulo, texto, autor, privado, fecha)
        {
            this.like = 0;
            this.dislike = 0;            
        }
        //constructor
        public Comentario() { }
        public override string ToString()
        {
            return $"COMENTARIO: \n Título: ${this.Titulo} \n Texto: {this.Texto} \n Autor:\n{this.Autor}\n Fecha:{this.Fecha}";
        }
        //retorna un string comentario
        public override int ValorAceptacion()
        {
            
            int FVALike = cantLike() * 5;
            int FVADislike = cantDisLike() * (-2);
            int final = FVALike + FVADislike;
            return final;
        }

    }

}
