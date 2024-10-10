using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{
    public class Post : Publicacion
    {
        public string NombreImagen { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public bool Baneado { get; set; }

        //getters y setters

        public Post(string titulo, string texto, Miembro autor, bool privado, DateTime fecha, string nombreImagen, bool censurado) : base(titulo, texto, autor, privado, fecha)
        {
            this.NombreImagen = nombreImagen;
            Comentarios = new List<Comentario>();
            this.Baneado = false;
            
            this.reacciones = new List<Reaccion>();
        }
        public Post() { }
        //constructor

        //metodos de validar
        private void ValidarImagenNulla()
        {
            if (this.NombreImagen == null || this.NombreImagen.Length == 0)
            {
                throw new Exception("La imagen no puede ser vacía");
            }
        }
        public void ValidarImagen()
        {
            string nombreImagen = this.NombreImagen;
            string extension = nombreImagen.Length >=3 ? nombreImagen.Substring(nombreImagen.Length - 3) : nombreImagen;
                if (extension != "jpg" && extension != "png")
                {
                    throw new Exception("Error, la imagen no válida. Solo puede ser en formato jpg, png o jpeg");
                }
            
            
        }

        private void ValidarUsuarioNoBloqueado()
        {
            if (this.Autor.Bloqueado)
            {
                throw new Exception("Error, usuario bloqueado");
            }
        }
        public override void Validar()
        {
            base.Validar();
            ValidarUsuarioNoBloqueado();
            ValidarImagenNulla();
        }
        public override string ToString()
        {
            return $"Post: \n Título: {this.Titulo} \nTexto: {this.Texto} \n Autor: \n{this.Autor} \n Fecha: {this.Fecha} \n Imagen \n Id: {this.Id} \n";
        }
        //retorna un string post
        public override int ValorAceptacion()
        {
            
            int FVALike = cantLike()* 5;
            int FVADislike = cantDisLike() * (-2);
            int esPublico = 0;
            if (!this.Privado)
            {
                esPublico = 10;
            }
            int valorFinal = FVALike + FVADislike + esPublico;
            return valorFinal;
        }

        

       


    }
}
