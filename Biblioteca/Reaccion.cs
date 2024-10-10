using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca
{
    public class Reaccion
    {
        public bool likeDislike { get; set; }

        public Miembro miembro { get; set; }

        public Reaccion(bool LikeDislike, Miembro miembro)
        {
            this.likeDislike = LikeDislike;
            this.miembro = miembro;
        }
        public Reaccion() { }
    }





}