using System.Globalization;

namespace Biblioteca
{
    public class Sistema
    {
        private static Sistema instancia;

        public static Sistema ObtenerInstancia
        {
            get
            {
                if (instancia == null) instancia = new Sistema();
                return instancia;
            }
        }

        private Sistema()
        {
            usuarios = new List<Usuario>();
            publicaciones = new List<Publicacion>();
            invitaciones = new List<Invitacion>();
            IngresarMiembrosPrederteminados();
            IngresarAdministradores();
        }
        //constructor

        //---------------------------------Lista--------------------------------------------------
        public List<Usuario> usuarios { get; set; }

        public List<Publicacion> publicaciones { get; set; }

        public List<Invitacion> invitaciones { get; set; }
        //----------------------------- Fin Lista-----------------------------------------------------        
        public bool ExisteInvitacionPendienteMiembro(Miembro mSolicitante, Miembro mSolicitado)
        {
            bool ret = false;
            foreach (Invitacion inv in invitaciones)
            {
                Miembro miSollicitante = inv.MiembroSolicitante;
                Miembro miSolicitado = inv.MiembroSolicitado;
                if (miSollicitante.Equals(mSolicitante) && miSolicitado.Equals(mSolicitado) && inv.Estado == EstadoSolicitud.pendiente_aprobacion)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        public List<Miembro> MostrarMiembrosParaAdmin()
        {
            List<Miembro> miembros = new List<Miembro>();
            foreach (Usuario us in usuarios)
            {
                if (us is Miembro)
                {
                    Miembro m = (Miembro)us;
                    miembros.Add(m);
                }
            }
            miembros.Sort();
            return miembros;
        }
        public List<Post> MostrarPostParaAdmin()
        {
            List<Post> posts= new List<Post>();
            foreach(Publicacion p in publicaciones)
            {
                if(p is Post)
                {
                    Post po = (Post)p;
                    posts.Add(po);
                }
            }
            return posts;
        }


        public List<Miembro> MostrarMiembros(Miembro m)
        {
            List<Miembro> filtrar = new List<Miembro>();
            foreach (Usuario us in usuarios)
            {
                if (us is Miembro)
                {
                    Miembro mfiltro = (Miembro)us;
                    if (!m.Amigos.Contains(mfiltro) && m != mfiltro && !ExisteInvitacionPendienteMiembro(mfiltro, m) && !ExisteInvitacionPendienteMiembro(m, mfiltro))
                    {
                        filtrar.Add(mfiltro);
                    }
                }
            }
            return filtrar;
        }
        //retorno un string de todos los usuarios
        public void CrearVinculo(Miembro MiembroSolicitante, Miembro MiembroSolicitado)
        {
            DateTime ahora = DateTime.Now;
            Invitacion invitacion = new Invitacion(MiembroSolicitante, MiembroSolicitado, ahora);
            invitaciones.Add(invitacion);
            MiembroSolicitado.Pendientes.Add(invitacion);
        }
        //creo un nuevo vinculo invitacion



        public Usuario IniciarSesion(string email, string contraseña)
        {
            foreach (Usuario u in usuarios)
            {
                if (u.Email == email && u.Contraseña == contraseña)
                {
                    return u;

                }
            }
            throw new Exception("Usuario o contraseña inválida");

        }

        public List<Publicacion> ListarPost(string texto, int valorAceptacion)
        {
            if (texto != null && valorAceptacion != null)
            {
                List<Publicacion> publicaciones = Sistema.ObtenerInstancia.publicaciones;

                List<Publicacion> retorno = new List<Publicacion>();

                foreach (Publicacion p in publicaciones)
                {
                    if (p.Titulo.Contains(texto) || p.Texto.Contains(texto))
                    {
                        if (p.ValorAceptacion() >= valorAceptacion)
                        {
                            retorno.Add(p);
                        }
                    }
                }

                return retorno;
            }
            else
            {
                throw new Exception("Debe completar ambos campos");
            }
                      
                     
        }


        private void VerificarExistenciaUsuario(string email)
        {
            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Email == (email))
                {
                    throw new Exception("Error, el correo ya existe");
                    break;
                }
            }

        }
        //Verificamos si el correo ingresado para Alta usuario ya existe en el Sistema


        public void AltaMiembro(string email, string contraseña, string nombre, string apellido, DateTime fechaNacimiento)
        {
            VerificarExistenciaUsuario(email);
            Usuario usuario = new Miembro(email, contraseña, nombre, apellido, fechaNacimiento);
            usuario.Validar();
            usuarios.Add(usuario);
        }

        public Usuario ObtenerMiembroPorEmail(string email)
        {
            foreach (Usuario u in usuarios)
            {
                if (u.Email == email)
                {
                    return u;
                }
            }
            throw new Exception("El usuario no existe");
        }
        //Ingresar un nuevo Miembro al sistema.

        //----------------------Metodos para la consola en la opción 2 y 3-------------------------------
        // Contamos las publicaciones de un miembro

        public List<Publicacion> ListarPublicacionesDeUnMiembro(string email)
        {
            List<Publicacion> publicacionesFiltradas = new List<Publicacion>();


            foreach (Publicacion publicacion in publicaciones)
            {
                if (email == publicacion.Autor.Email)
                {
                    publicacionesFiltradas.Add(publicacion);

                }

            }
            return publicacionesFiltradas;
        }


        public List<Post> ListarPostsMiembro(Usuario m)
        {
            List<Post> filtrar = new List<Post>();
            foreach (Publicacion p in publicaciones)
            {
                if (p is Post)
                {
                    Post post = (Post)p;
                    if (post.Autor.Email.Equals(m.Email) || post.Autor.Amigos.Contains(m) || !post.Privado)
                    {
                        if (!post.Baneado)
                        {
                            filtrar.Add(post);
                        }
                        

                    }
                }

            }
            return filtrar;
        }
        //verificamos que el miembro tenga al menos una publicacion
        private void NoHayPublicacionesMiembro(string email)
        {

            if (ListarPublicacionesDeUnMiembro(email).Count == 0)
            {
                throw new Exception("Error, no hay publicaciones del Miembro ingresado");
            }
        }
        //Verificamos si el miembro ingresado realizó alguna publicación
        public void VerificarSiExistePublicaciones(string email)
        {
            VerificoExisteUsuario(email);
            NoHayPublicacionesMiembro(email);
        }
        //Mostrarmos las publicaciones de un miembro
        public string MostrarPublicacionesMiembro(List<Publicacion> publicacionesYaFiltradas)
        {
            List<Publicacion> publicaciones = new List<Publicacion>();
            publicaciones = publicacionesYaFiltradas;

            string publicacionFinal = "";
            foreach (Publicacion publicacion in publicaciones)
            {
                publicacionFinal += $"{publicacion} \n";
            }
            return publicacionFinal;
        }



        //Listamos post comentados por un usuario
        public List<Publicacion> ListarPostComentados(string email)
        {
            VerificoExisteUsuario(email);

            List<Publicacion> PostComentados = new List<Publicacion>();

            foreach (Publicacion publicacion in publicaciones)
            {
                if (publicacion is Post)
                {
                    Post post = (Post)publicacion;
                    foreach (Comentario comentario in post.Comentarios)
                    {
                        if (comentario.Autor.Email == email)
                        {
                            PostComentados.Add(post);
                            break;
                        }
                    }
                }
            }



            return PostComentados;
        }

        //Para verificar si el usuario ingresado existe en el program (2 y 3)
        private void VerificoExisteUsuario(String email)
        {

            bool existe = false;

            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Email == email)
                {
                    existe = true;
                    break;
                }
            }

            if (!existe)
            {
                throw new Exception("No existe el email ingresado");
            }

        }


        //------------------------------- Fin funciones Listar Post y Comentarios----------------------------------------

        //Listamos los post
        private List<Publicacion> PostFecha(DateTime FechaInicio, DateTime FechaFin)
        {

            List<Publicacion> PostEntreFechas = new List<Publicacion>();


            VerificoExistePostEntreFechas(FechaInicio, FechaFin);
            foreach (Publicacion publicacion in publicaciones)
            {
                if (publicacion is Post)
                {
                    Post post = (Post)publicacion;
                    if (post.Fecha >= FechaInicio && post.Fecha <= FechaFin)
                    {
                        PostEntreFechas.Add(post);
                    }
                }
            }


            //mostrar solo id, fecha titulo y texto
            //falta ordenar la lista
            PostEntreFechas.Sort();
            return PostEntreFechas;

        }
        //Para la opción 4 del program. Mostramos los datos pedidos por el obligatorio
        public string MostrarAtributosSolicitados(DateTime FechaInicio, DateTime FechaFin)
        {
            List<Publicacion> lista = PostFecha(FechaInicio, FechaFin);

            string inicial = "";
            string TextoNuevo = "";
            foreach (Publicacion post in lista)
            {
                if (post.Texto.Length > 50)
                {
                    TextoNuevo = post.Texto.Substring(0, 49);
                }
                else
                {
                    TextoNuevo = post.Texto;
                }

                inicial += $"Id: {post.Id} \n Fecha: {post.Fecha} \n Titulo: {post.Titulo} \nTexto: {TextoNuevo}";
            }

            return inicial;
        }



        //Verificamos si existe algun post entre las fechas asignadas
        private void VerificoExistePostEntreFechas(DateTime FechaInicio, DateTime FechaFin)
        {
            ValidarFecha(FechaInicio, FechaFin);

            bool existe = false;

            foreach (Post post in publicaciones)
            {

                if (post.Fecha >= FechaInicio && post.Fecha <= FechaFin)
                {
                    existe = true;
                    break;
                }
            }
            if (!existe)
            {
                throw new Exception("No existen post en las fechas indicadas");
            }
        }

        //Validamos que los datos ingresados de las fechas sean correctos
        private void ValidarFecha(DateTime FechaInicio, DateTime FechaFin)
        {
            if (FechaInicio >= FechaFin)
            {
                throw new Exception("Las fechas ingresadas no son correctas");
            }
        }

        //Contamos el usuario que hizo más publicaciones
        private int ContadorMayorPublicacionesMiembro()
        {
            int contadorMayor = 0;
            foreach (Usuario usuario in usuarios)
            {
                int contador = 0;
                if (usuario is Miembro)
                {
                    Miembro miembro = (Miembro)usuario;
                    foreach (Publicacion publicacion in publicaciones)
                    {
                        if (publicacion.Autor.Equals(miembro))
                        {
                            contador++;
                        }
                    }
                }
                if (contador > contadorMayor)
                {
                    contadorMayor = contador;
                }
            }
            return contadorMayor;
        }
        //Agregamos en una lista los usuarios con mas publicaciones
        private List<Usuario> ListaUsuariosConMasPublicaciones()
        {

            List<Usuario> usuariosFiltrados = new List<Usuario>();
            int mayorPublicacion = ContadorMayorPublicacionesMiembro();

            foreach (Usuario usuario in usuarios)
            {
                int contador = 0;
                if (usuario is Miembro)
                {
                    Miembro miembro = (Miembro)usuario;
                    foreach (Publicacion publicacion in publicaciones)
                    {
                        if (publicacion.Autor.Equals(miembro))
                        {
                            contador++;
                        }
                    }
                    if (contador == mayorPublicacion)
                    {
                        usuariosFiltrados.Add(miembro);

                    }
                }

            }

            return usuariosFiltrados;
        }
        //Mostramos finalmente los miembros con mas publicaciones ya enlistada previamente
        public string MostrarMiembrosConMasPublicaciones()
        {

            string mostrar = "";
            List<Usuario> UsuariosFiltrados = ListaUsuariosConMasPublicaciones();
            foreach (Usuario usuario in UsuariosFiltrados)
            {
                mostrar += $"{usuario}";
            }
            return $"{mostrar}";
        }



        public void AltaPost(string titulo, string texto, Miembro autor, bool privado, string imagen)
        {
            bool censurado = false;
            DateTime fecha = DateTime.Now;
            if (!autor.Bloqueado)
            {
                Post post = new Post(titulo, texto, autor, privado, fecha, imagen, censurado);

                post.Validar();
                post.ValidarImagen();

                publicaciones.Add(post);
            }
            else
            {
                throw new Exception("El usuario esta bloqueado");
            }
        }

        public void AltaComentario(string titulo, Miembro autor, string texto, int Id)
        {

            Publicacion pu = ObtenerPostPorId(Id);

            DateTime fecha = DateTime.Now;
            bool esPrivado = pu.Privado;

            if (!autor.Bloqueado)
            {
                Comentario comentario = new Comentario(titulo, texto, autor, esPrivado, fecha);

               if(pu is Post)
                {
                    Post p = (Post)pu;
                    p.Comentarios.Add(comentario);
                }              

            }
            else
            {
                throw new Exception("Error, no se puede realizar el comentario");
            }

        }

        public Miembro ObtenerMiembroPorID(int? Id)
        {
            foreach (Miembro m in usuarios)
            {
                if (m.Id == Id)
                {
                    return m;
                }
            }
            return null;
        }

        public Post ObtenerPostPorId(int id)
        {
            foreach (Post p in publicaciones)
            {
                if (p.Id == id)
                {
                    return p;
                }
            }
            return null;
        }
        public Publicacion ObtenerPublicacionPorId(int id)
        {
            foreach(Publicacion p in publicaciones)
            {
                if (p.Id.Equals(id))
                {
                    return p;
                    break;
                }
            }
            return null;
            
        }







        public void ReaccionLike(Miembro miembro, Publicacion p)
        {
            bool existe = false;
                    if(p.reacciones.Count() > 0)
            {
                foreach (Reaccion unaReaccion in p.reacciones)
                {
                    if (unaReaccion.miembro.Equals(miembro))
                    {
                        existe = true;

                        if (unaReaccion.likeDislike == true)
                        {

                            p.reacciones.Remove(unaReaccion);
                            break;
                        }
                        else
                        {
                            unaReaccion.likeDislike = true;
                        }
                        break;
                    }
                }
            }
                    
                    if (existe == false)
                    {
                        Reaccion nuevaReaccionLike = new Reaccion(true, miembro);

                        p.reacciones.Add(nuevaReaccionLike);
                    }                 
          
        }
        //bool existee = false;
        //Publicacion pu = ObtenerPublicacionPorId(id);//El metodo lo hice sin querer. Era para algo que no iba al final pero sirve en este caso
        //foreach(Reaccion r in pu.reacciones)
        //{
        //    if (r.miembro.Equals(miembro))
        //    {
        //        existee = true;
        //        if (likeD.Equals(r.likeDislike))
        //        {
        //            pu.reacciones.Remove(r);
        //        } else if(likeD!= r.likeDislike)
        //        {
        //            r.likeDislike = likeD;

        //        }
        //        pu.cantDisLike();
        //        pu.cantDisLike();
        //        break;
        //    }
        //}
        //if(!existee)
        //{
        //    Reaccion agregarReaccion = new Reaccion(likeD, miembro);
        //    pu.reacciones.Add(agregarReaccion);
        //}

        public void ReaccionDislike(Miembro miembro, Publicacion p)
        {
            bool existe = false;

            foreach (Reaccion unaReaccion in p.reacciones)
            {
                if (unaReaccion.miembro.Equals(miembro))
                {
                    existe = true;

                    if (unaReaccion.likeDislike == false)
                    {

                        p.reacciones.Remove(unaReaccion);
                        break;
                    }
                    else
                    {
                        unaReaccion.likeDislike = false;
                    }
                    break;
                }
            }
            if (existe == false)
            {
                Reaccion nuevaReaccionLike = new Reaccion(false, miembro);

                p.reacciones.Add(nuevaReaccionLike);
            }
        }







        

        //}
        //public void ReaccionDislike(Miembro miembro)
        //{
        //    bool yaReacciono = false;

        //    if (!yaReacciono)
        //    {
        //        VerificarMiembroEnLista(miembro);
        //        this.dislike++;
        //        this.reacciones.Add(miembro);

        //        yaReacciono = true;
        //    }


        //}

        //obtener post por id


        //Creamos usuarios, comentarios y post.
        private void IngresarMiembrosPrederteminados()
        {
            Miembro usuario = new Miembro("nicor", "123", "Juan", "Pérez", new DateTime(1990, 03, 15));
            usuarios.Add(usuario);
            Miembro us2 = new Miembro("maria.garcia@email.com", "Contraseña456", "María", "García", new DateTime(1985, 07, 22));
            usuarios.Add(us2);
            Miembro us3 = new Miembro("carlos.rodriguez@email.com", "Segura789", "Carlos", "Rodríguez", new DateTime(1995, 12, 10));
            usuarios.Add(us3);
            Miembro us4 = new Miembro("laura.lopez@email.com", "Acceso2021", "Laura", "López", new DateTime(1998, 09, 05));
            usuarios.Add(us4);
            Miembro us5 = new Miembro("ana.martinez@email.com", "Privacidad23", "Ana", "Martínez", new DateTime(1992, 04, 28));
            usuarios.Add(us5);
            Miembro us6 = new Miembro("pedro.sanchez@email.com", "Segura2022", "Pedro", "Sánchez", new DateTime(1983, 11, 17));
            usuarios.Add(us6);
            Miembro us7 = new Miembro("marta.fernandez@email.com", "Prueba123", "Marta", "Fernández", new DateTime(1993, 06, 03));
            usuarios.Add(us7);
            Miembro us8 = new Miembro("david.gonzalez@email.com", "ClaveTest", "David", "González", new DateTime(1987, 01, 20));
            usuarios.Add(us8);
            Miembro us9 = new Miembro("isabel.ramirez@email.com", "isabel.ramirez@email.com", "Isabel", "Ramírez", new DateTime(1997, 08, 12));
            usuarios.Add(us9);
            Miembro us10 = new Miembro("jorge.herrera@email.com", "Prueba2023", "Jorge", "Herrera", new DateTime(1984, 02, 09));
            usuarios.Add(us10);
            Miembro us11 = new Miembro("rafa.m@email.com", "123", "Rafa", "Mer", new DateTime(2001, 11, 28));
            usuarios.Add(us11);
            //Precargar de Invitaciones entre usuarios (2 vinculos de amistad y precargar invitaciones en todos los estados posibles
            Invitacion uno = new Invitacion(usuario, us2, new DateTime(2023, 02, 28));
            //us2.AceptarSolicitud(usuario);
            Invitacion dos = new Invitacion(usuario, us4, new DateTime(2023, 01, 30));
            //us4.AceptarSolicitud(usuario);
            Invitacion tres = new Invitacion(usuario, us8, new DateTime(2023, 04, 20));
            //us8.RechazarSolicitud(usuario);
            Invitacion cuatro = new Invitacion(usuario, us10, new DateTime(2023, 05, 30));
            //us10.AceptarSolicitud(usuario);
            Invitacion cinco = new Invitacion(usuario, us3, new DateTime(2023, 06, 11));
            Invitacion seis = new Invitacion(usuario, us5, new DateTime(2023, 07, 22));
            Invitacion siete = new Invitacion(us2, us3, new DateTime(2023, 06, 21));
            //us3.AceptarSolicitud(us2);

            Invitacion ocho = new Invitacion(us2, us10, new DateTime(2023, 08, 30));
            Invitacion nueve = new Invitacion(us2, us5, new DateTime(2023, 07, 22));
            //Invitacion diez = new Invitacion(us2, us7, new DateTime(2023, 08, 22));
            Invitacion once = new Invitacion(us2, us8, new DateTime(2023, 07, 23));
            //us7.AceptarSolicitud(us2);
            Invitacion doce = new Invitacion(us10, us8, new DateTime(2023, 10, 22));
            Invitacion trece = new Invitacion(us10, us7, new DateTime(2023, 11, 28));
            Invitacion catorce = new Invitacion(us10, us5, new DateTime(2023, 12, 22));
            //us8.AceptarSolicitud(us10);
            //us5.AceptarSolicitud(us10);
            Invitacion quince = new Invitacion(us9, us8, new DateTime(2023, 03, 11));
            //Invitacion dieciseis = new Invitacion(us9, us6, new DateTime(2023, 01, 23));
            Invitacion diecisiete = new Invitacion(us9, us3, new DateTime(2023, 08, 28));
            Invitacion dieciocho = new Invitacion(us9, us4, new DateTime(2023, 07, 22));
            Invitacion diecinueve = new Invitacion(us9, us10, new DateTime(2023, 04, 24));
            //us6.AceptarSolicitud(us9);
            //us4.AceptarSolicitud(us9);
            Invitacion veinte = new Invitacion(us8, us5, new DateTime(2023, 08, 09));
            Invitacion veintiuno = new Invitacion(us8, us7, new DateTime(2023, 07, 31));
            //Invitacion veintidos = new Invitacion(us8, us4, new DateTime(2023, 05, 30));
            Invitacion veintitres = new Invitacion(us8, us6, new DateTime(2023, 05, 30));
            Invitacion veinticuatro = new Invitacion(us8, us3, new DateTime(2023, 04, 30));
            //us3.AceptarSolicitud(us8);
            //us5.AceptarSolicitud(us8);
            Invitacion veinticinco = new Invitacion(us3, us7, new DateTime(2023, 01, 23));
            Invitacion veintiseis = new Invitacion(us3, us5, new DateTime(2023, 03, 22));
            //Invitacion veintisiete = new Invitacion(us3, us10, new DateTime(2023, 05, 26));
            Invitacion veintiocho = new Invitacion(us3, us4, new DateTime(2023, 06, 07));
            Invitacion veintinueve = new Invitacion(us3, us6, new DateTime(2023, 07, 28));
            //us6.AceptarSolicitud(us3);
            //us4.AceptarSolicitud(us3);
            Invitacion treinta = new Invitacion(us4, us10, new DateTime(2023, 07, 30));
            Invitacion treintaYUno = new Invitacion(us4, us7, new DateTime(2023, 08, 22));
            Invitacion treintaYDos = new Invitacion(us4, us2, new DateTime(2023, 05, 21));
            //Invitacion treintaYTres = new Invitacion(us4, us6, new DateTime(2023, 01, 22));
            Invitacion treintaYCuatro = new Invitacion(us4, us5, new DateTime(2023, 01, 01));
            //us2.AceptarSolicitud(us4);
            //us6.AceptarSolicitud(us4);
            Invitacion treintaYCinco = new Invitacion(us5, us9, new DateTime(2023, 02, 22));
            Invitacion treintaYSeis = new Invitacion(us5, us7, new DateTime(2023, 02, 20));
            //Invitacion treintaYSiete = new Invitacion(us5, us6, new DateTime(2023, 03, 21));
            //us9.AceptarSolicitud(us5);
            Invitacion treintaYOcho = new Invitacion(us6, usuario, new DateTime(2023, 04, 11));
            Invitacion treintaYNueve = new Invitacion(us7, usuario, new DateTime(2023, 07, 11));
            //Agregar invitaciones
            invitaciones.Add(uno);
            invitaciones.Add(dos);
            invitaciones.Add(tres);
            invitaciones.Add(cuatro);
            invitaciones.Add(cinco);
            invitaciones.Add(seis);
            invitaciones.Add(siete);
            invitaciones.Add(ocho);
            invitaciones.Add(nueve);
            //invitaciones.Add(diez);
            invitaciones.Add(once);
            invitaciones.Add(doce);
            invitaciones.Add(trece);
            invitaciones.Add(catorce);
            invitaciones.Add(quince);
            //invitaciones.Add(dieciseis);
            invitaciones.Add(diecisiete);
            invitaciones.Add(dieciocho);
            invitaciones.Add(diecinueve);
            invitaciones.Add(veinte);
            invitaciones.Add(veintiuno);
            //invitaciones.Add(veintidos);
            invitaciones.Add(veintitres);
            invitaciones.Add(veinticuatro);
            invitaciones.Add(veinticinco);
            invitaciones.Add(veintiseis);
            //invitaciones.Add(veintisiete);
            invitaciones.Add(veintiocho);
            invitaciones.Add(veintinueve);
            invitaciones.Add(treinta);
            invitaciones.Add(treintaYUno);
            invitaciones.Add(treintaYDos);
            //invitaciones.Add(treintaYTres);
            invitaciones.Add(treintaYCuatro);
            invitaciones.Add(treintaYCinco);
            invitaciones.Add(treintaYSeis);
            //invitaciones.Add(treintaYSiete);
            invitaciones.Add(treintaYOcho);
            invitaciones.Add(treintaYNueve);
            //Precargar Post
            Post post1 = new Post("Cómo Cultivar un Jardín de Hierbas en Casa", "En este post, compartiré consejos sobre cómo cultivar un jardín de hierbas en casa, desde la elección de las hierbas hasta los cuidados necesarios. Hablaré sobre la importancia de la luz, el riego adecuado y cómo utilizar estas hierbas frescas en tu cocina diaria.", us3, false, new DateTime(2023, 02, 22), "1.png", false);
            Post post2 = new Post("Cómo Mantener la Productividad Trabajando desde Casa", "En este post, abordaré estrategias efectivas para mantener altos niveles de productividad al trabajar desde casa. Hablaré sobre la importancia de tener un espacio de trabajo dedicado, establecer horarios y lidiar con posibles distracciones.", us7, false, new DateTime(2023, 05, 12), "2.png", false);
            Post post3 = new Post("Recetas Saludables de Batidos Energéticos", "En este post, compartiré recetas saludables de batidos energéticos cargados de nutrientes. Incluiré ingredientes clave, beneficios para la salud y sugerencias para adaptar las recetas según los gustos personales.", us5, false, new DateTime(2022, 09, 25), "3.png", false);
            Post post4 = new Post("Cómo Empezar a Practicar Yoga en Casa", "En este post, proporcionaré una guía para principiantes sobre cómo comenzar a practicar yoga en casa. Cubriré posturas básicas, la importancia de la respiración y sugerencias para crear un ambiente tranquilo.", us2, true, new DateTime(2023, 07, 02), "4.png", false);
            Post post5 = new Post("Consejos para una Rutina de Noche Relajante", "En este post, compartiré consejos para crear una rutina de noche relajante que favorezca el descanso y la calidad del sueño. Incluiré prácticas como apagar dispositivos electrónicos, actividades relajantes y hábitos para preparar el cuerpo y la mente para el descanso.", us2, true, new DateTime(2023, 10, 03), "5.png", false);
            publicaciones.Add(post1);
            publicaciones.Add(post2);
            publicaciones.Add(post3);
            publicaciones.Add(post4);
            publicaciones.Add(post5);


            //Precargar 2 comentarios en cada Post
            //string titulo, string texto, Miembro autor, bool privado, DateTime fecha

            Comentario comentario1Post2 = new Comentario("Dominando la Productividad en Casa", "¡Entiendo completamente! En este artículo, comparto estrategias clave para mantener alta la productividad mientras trabajas desde casa. ¿Tienes alguna situación específica que te desconcierte?", us3, false, new DateTime(2023, 04, 7));
            Comentario comentario2Post2 = new Comentario("Desconectando para Recargar Energías", "Aquí te dejo algunos consejos sobre cómo desconectar al final del día para evitar el agotamiento. ¡Es esencial cuidar de tu bienestar! ¿Cuál es tu actividad favorita para desconectar?", us10, true, new DateTime(2023, 06, 13));
            Comentario comentario3Post2 = new Comentario("Navegando por Reuniones Virtuales con Éxito", "Navegando por Reuniones Virtuales con Éxito\"\r\nDescripción: \"Las reuniones virtuales pueden ser desafiantes, pero con las estrategias adecuadas, puedes mantener la productividad. Compartiré algunos consejos para hacerlas más efectivas. ¿Alguna situación específica que te gustaría abordar?", us3, false, new DateTime(2023, 04, 7));
            post2.Comentarios.Add(comentario1Post2);
            post2.Comentarios.Add(comentario2Post2);
            post2.Comentarios.Add(comentario3Post2);
            publicaciones.Add(comentario1Post2);
            publicaciones.Add(comentario2Post2);
            publicaciones.Add(comentario3Post2);

            Comentario comentario1Post1 = new Comentario("Mi Experiencia Cultivando Hierbas Frescas", "Estoy emocionado de compartir mi experiencia cultivando hierbas frescas en casa. Descubrí que tener hierbas a mano transforma por completo mis comidas. ¿Cuál es tu platillo favorito con hierbas frescas?", us2, false, new DateTime(2023, 05, 8));
            Comentario comentario2Post1 = new Comentario("Hierbas Perfectas para Principiantes", "Aquí te presento algunas hierbas que son ideales para aquellos que se inician en la jardinería. Exploraremos opciones resistentes y versátiles. ¿Tienes alguna hierba en mente que te gustaría cultivar?", us4, false, new DateTime(2023, 05, 8));
            Comentario comentario3Post1 = new Comentario("Jardinería en Espacios Reducidos", "Cultivar hierbas en espacios pequeños puede ser desafiante pero emocionante. Compartiré algunos trucos para aprovechar al máximo tu espacio. ¿Tienes un rincón específico que te gustaría convertir en tu propio jardín de hierbas?", usuario, true, new DateTime(2023, 07, 01));
            post1.Comentarios.Add(comentario1Post1);
            post1.Comentarios.Add(comentario2Post1);
            post1.Comentarios.Add(comentario3Post1);
            publicaciones.Add(comentario1Post1);
            publicaciones.Add(comentario2Post1);
            publicaciones.Add(comentario3Post1);


            Comentario comentario1Post3 = new Comentario("Deliciosos Batidos sin Lácteos", "¡Me alegra que te gusten! En este artículo, encontrarás deliciosas recetas de batidos sin lácteos. Exploraremos opciones con leches vegetales y más. ¿Hay alguna fruta específica que te gustaría incluir?", us8, true, new DateTime(2023, 04, 4));
            Comentario comentario2Post3 = new Comentario("Mi Batido Energético Preferido", "Compartiré mi receta personal de batido energético favorito. Es refrescante y lleno de nutrientes. ¿Te gustaría probarlo tal cual o prefieres ajustar algunos ingredientes?", us4, false, new DateTime(2023, 2, 3));
            Comentario comentario3Post3 = new Comentario("Preparando Batidos para Toda la Semana", "¡Claro! Te mostraré cómo preparar lotes de batidos para toda la semana. ¿Hay alguna receta en particular que te gustaría tener lista para los desayunos ocupados?", us3, true, new DateTime(2023, 01, 09));
            post3.Comentarios.Add(comentario1Post3);
            post3.Comentarios.Add(comentario2Post3);
            post3.Comentarios.Add(comentario3Post3);
            publicaciones.Add(comentario1Post3);
            publicaciones.Add(comentario2Post3);
            publicaciones.Add(comentario3Post3);

            Comentario comentario1Post4 = new Comentario("Iniciando tu Viaje en el Yoga", "Comenzar con yoga puede ser abrumador, pero estoy aquí para guiarte. Compartiré algunos consejos para principiantes y posturas simples para empezar. ¿Hay alguna postura que te gustaría aprender primero?", us7, false, new DateTime(2023, 06, 12));
            Comentario comentario2Post4 = new Comentario("Equipamiento Básico para Practicar Yoga en Casa", "No es necesario mucho equipo, pero hay algunas cosas útiles. Discutiré lo esencial para empezar con éxito en casa. ", us10, false, new DateTime(2023, 08, 19));
            Comentario comentario3Post4 = new Comentario("Bienestar Mental a Través del Yoga", "Definitivamente experimentarás beneficios mentales. Exploraré cómo el yoga puede mejorar la claridad mental y reducir el estrés.", usuario, true, new DateTime(2023, 03, 30));
            post4.Comentarios.Add(comentario1Post4);
            post4.Comentarios.Add(comentario2Post4);
            post4.Comentarios.Add(comentario3Post4);
            publicaciones.Add(comentario1Post4);
            publicaciones.Add(comentario2Post4);
            publicaciones.Add(comentario3Post4);

            Comentario comentario1Post5 = new Comentario("Descanso Garantizado: Mi Experiencia con la Rutina Nocturna", "Entiendo completamente tu situación. Compartiré cómo estos consejos han mejorado significativamente mi calidad de sueño.", us2, false, new DateTime(2023, 03, 17));
            Comentario comentario2Post5 = new Comentario("Actividades Relajantes para una Noche Tranquila", "¡Claro! En este artículo, exploraré diversas actividades que puedes incorporar en tu rutina nocturna para relajarte antes de dormir. ¿Tienes alguna actividad en mente que prefieras?", us5, false, new DateTime(2023, 06, 27));
            Comentario comentario3Post5 = new Comentario("Timing Perfecto: Cuándo Comenzar tu Rutina Nocturna", "¡Eso suena emocionante! Discutiré el momento óptimo para comenzar tu rutina nocturna y cómo ajustarla según tus necesidades.", usuario, false, new DateTime(2023, 09, 10));
            post5.Comentarios.Add(comentario1Post5);
            post5.Comentarios.Add(comentario2Post5);
            post5.Comentarios.Add(comentario3Post5);
            publicaciones.Add(comentario1Post5);
            publicaciones.Add(comentario2Post5);
            publicaciones.Add(comentario3Post5);

            //precarga reacciones
            //post5.ReaccionLike(us2);
            //post4.ReaccionDislike(us3);

            //comentario1Post3.ReaccionLike(us10);
            //comentario2Post3.ReaccionDislike(us7);

        }
        //Creamos el administrador
        private void IngresarAdministradores()
        {
            Administrador admin = new Administrador("Admin", "Admin");
            usuarios.Add(admin);
        }

    }

}

    

