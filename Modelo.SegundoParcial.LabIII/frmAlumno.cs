using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;

namespace Modelo.SegundoParcial.LabIII
{
    public partial class frmAlumno : Form
    {
        private Entidades.Alumno _unAlumno;

        public Entidades.Alumno UnAlumno
        {
            get { return this._unAlumno; }
        }

        public frmAlumno()
        {
            InitializeComponent();
            this.cmbCurso.Items.Add("1");
            this.cmbCurso.Items.Add("2");
            this.cmbCurso.Items.Add("3");
            this.cmbCurso.Items.Add("4");
        }

        public frmAlumno(Alumno unAlumno):this()
        {
            this._unAlumno = unAlumno;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this._unAlumno = new Entidades.Alumno();
            this._unAlumno.Apellido = this.txtApellido.Text;
            this._unAlumno.Legajo = int.Parse(this.txtLeg.Text);
            this._unAlumno.Curso = int.Parse(this.cmbCurso.SelectedItem.ToString());
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
