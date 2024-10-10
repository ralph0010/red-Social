    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{
    public abstract class Publicacion :IComparable, IValidable
    {
        public int Id { get; set; }
        public static int Incremental { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public Miembro Autor { get; set; }
        public bool Privado { get; set; }
        public DateTime Fecha { get; set; }
        public int like { get; set; }
        public int dislike { get; set; }
        
        
        //getters y setters
        public List<Reaccion> reacciones { get; set; }

        public Publicacion(string titulo, string texto, Miembro autor, bool privado, DateTime fecha)
        {
            this.Id = Incremental++;
            this.Titulo = titulo;
            this.Texto = texto;
            this.Autor = autor;
            this.Privado = privado;
            this.Fecha = fecha;
            this.like = 0;
            this.dislike = 0;
            this.reacciones = new List<Reaccion>();
            
        }
        public Publicacion() { }

        //constructor 
        private void ValidarTitulo()
        {
            if (this.Titulo == null || this.Titulo.Length <= 3)
            {
                throw new Exception("El título no puede ser menor a 3 caracteres");
            }
        }

        public virtual void Validar()
        {
            ValidarTitulo();
            ValidarContenido();
        }

        //private void VerificarMiembroEnLista(Miembro miembro)
        //{
        //    bool reaccionado = false;
        //    //if (this.reacciones.Contains(miembro))
        //    //{
        //    //    throw new Exception("Error, su like o dislike quedó asignado anteriormente");
        //    //}
        //}
        
       
        //dado un miembro agrego la reaccion y al miembro a la lista de reacciones
        public abstract int ValorAceptacion();
        
        private void ValidarContenido()
        {
            if (this.Texto == null || this.Texto.Length == 0)
            {
                throw new Exception("El contenido no puede ser vacio");
            }
        }
        //valido que el contenido no sea vacio
        public int CompareTo(object? obj)
        {
            Post comparar = (Post)obj;

            return comparar.Titulo.CompareTo(this.Titulo);
        }
        //metodo comparar
        public virtual int cantLike()
        {
            int cantLike = 0;

            foreach (Reaccion r in this.reacciones)
            {
                if (r.likeDislike == true)
                {
                    cantLike++;
                }
            }

            return cantLike;

        }

        public virtual int cantDisLike()
        {
            int cantDislike = 0;

            foreach (Reaccion r in this.reacciones)
            {
                if (r.likeDislike == false)
                {
                    cantDislike++;
                }
            }

            return cantDislike;

        }
        
        public List<Publicacion> VerificoExistePublicacion(string Texto, int valorAceptacion)
        {
            List<Publicacion> retorno = new List<Publicacion>();

            foreach(Publicacion p in Sistema.ObtenerInstancia.publicaciones)
            {
                if (this.Titulo.Contains(Texto) || this.Texto.Contains(Texto))
                {
                    if (ValorAceptacion() >= valorAceptacion)
                    {
                        retorno.Add(p);
                    }
                }
            }          
            
            return retorno;

        }
    }
}
