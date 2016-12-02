using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Curso
    {
        //Atributos
        private int _codigoCurso;
        private int _duracion;
        private string _nombre;

        //Propiedades
        public int CodigoCurso
        {
            get { return this._codigoCurso; }
            set { this._codigoCurso = value; }
        }

        public int Duracion
        {
            get { return this._duracion; }
            set { this._duracion = value; }
        }

        public string Nombre
        {
            get { return this._nombre; }
            set { this._nombre = value; }
        }

        //Constructores
        public Curso() { }

        public Curso(int CodigoCurso, int Duracion, string Nombre)
        {
            this._codigoCurso = CodigoCurso;
            this._duracion = Duracion;
            this._nombre = Nombre;
        }
    }
}
