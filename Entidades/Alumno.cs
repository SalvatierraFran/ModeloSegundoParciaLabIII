using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Alumno
    {
        //Atributos
        private string _apellido;
        private int _codCurso;
        private int _legajo;

        //Propiedades
        public string Apellido
        {
            get { return this._apellido; }
            set { this._apellido = value; }
        }

        public int Curso
        {
            get { return this._codCurso; }
            set { this._codCurso = value; }
        }

        public int Legajo
        {
            get { return this._legajo; }
            set { this._legajo = value; }
        }

        //Constructores
        public Alumno() { }

        public Alumno(int Legajo, string Apellido, int Curso)
        {
            this._apellido = Apellido;
            this._codCurso = Curso;
            this._legajo = Legajo;
        }
    }
}
