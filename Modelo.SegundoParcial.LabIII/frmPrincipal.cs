using System;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;
using System.IO;
using System.Data.SqlClient;

namespace Modelo.SegundoParcial.LabIII
{
    public partial class frmPrincipal : Form
    {
        private DataSet _dataSetAlumnos_Cursos;
        private SqlDataAdapter _dataAdapterAlumnos;
        private string _esquemaPath = @"E:\XMLGuardados\CursosEsquema.xml";
        private string _datosPath = @"E:\XMLGuardados\CursosDatos.xml";

        public frmPrincipal()
        {
            InitializeComponent();
        }

        public DataTable CrearDataTableCursos()
        {
            if (!File.Exists(_esquemaPath) && !File.Exists(_datosPath))
            {
                DataTable dtCursos = new DataTable("Cursos");
                dtCursos.TableName = "Cursos";
                dtCursos.Columns.Add("Codigo", typeof(int));
                dtCursos.Columns["Codigo"].AutoIncrement = true;
                dtCursos.Columns["Codigo"].AutoIncrementSeed = 1000;
                dtCursos.Columns["Codigo"].AutoIncrementStep = 5;
                dtCursos.Columns.Add("Duracion", typeof(int));
                dtCursos.Columns.Add("Nombre", typeof(string));

                dtCursos.Rows.Add(new Object[] { null, 2, "Matematica" });
                dtCursos.Rows.Add(new Object[] { null, 4, "Base de Datos" });
                dtCursos.Rows.Add(new Object[] { null, 1, "Estadisticas" });

                try
                {
                    dtCursos.WriteXmlSchema(this._esquemaPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                try
                {
                    dtCursos.WriteXml(this._datosPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return dtCursos;
            }
            else
            {
                DataTable dt = new DataTable();
                dt.ReadXml(this._datosPath);
                dt.ReadXmlSchema(this._esquemaPath);

                return dt;
            }
        }

        public void ConfigurarDataAdapter()
        {
            this._dataAdapterAlumnos = new SqlDataAdapter();
            SqlConnection cnn = new SqlConnection(Modelo.SegundoParcial.LabIII.Properties.Settings.Default.miConexion);

            this._dataAdapterAlumnos.SelectCommand = new SqlCommand("SELECT Legajo_Alumno as Legajo, Apellido_Alumno as Apellido, Curso_Alumno as Curso FROM Alumnos", cnn);
            this._dataAdapterAlumnos.UpdateCommand = new SqlCommand("UPDATE Alumnos SET Apellido_Alumnos=@apellido, Curso_Alumno=@curso WHERE Legajo_Alumno = @legajo", cnn);
            this._dataAdapterAlumnos.DeleteCommand = new SqlCommand("DELETE FROM Alumnos WHERE Legajo_Alumno = @legajo", cnn);
            this._dataAdapterAlumnos.InsertCommand = new SqlCommand("INSERT INTO Alumnos(Legajo_Alumno, Apellido_Alumno, Curso_Alumno) VALUES (@legajo, @apellido, @curso)", cnn);

            this._dataAdapterAlumnos.SelectCommand.Parameters.Add("@legajo", SqlDbType.Int, 4, "Legajo");
            this._dataAdapterAlumnos.SelectCommand.Parameters.Add("@apellido", SqlDbType.VarChar, 25, "Apellido");
            this._dataAdapterAlumnos.SelectCommand.Parameters.Add("@curso", SqlDbType.Int, 1, "Curso");

            this._dataAdapterAlumnos.UpdateCommand.Parameters.Add("@legajo", SqlDbType.Int, 4, "Legajo");
            this._dataAdapterAlumnos.UpdateCommand.Parameters.Add("@apellido", SqlDbType.VarChar, 25, "Apellido");
            this._dataAdapterAlumnos.UpdateCommand.Parameters.Add("@curso", SqlDbType.Int, 1, "Curso");

            this._dataAdapterAlumnos.DeleteCommand.Parameters.Add("@legajo", SqlDbType.Int, 4, "Legajo");

            this._dataAdapterAlumnos.InsertCommand.Parameters.Add("@legajo", SqlDbType.Int, 4, "Legajo");
            this._dataAdapterAlumnos.InsertCommand.Parameters.Add("@apellido", SqlDbType.VarChar, 25, "Apellido");
            this._dataAdapterAlumnos.InsertCommand.Parameters.Add("@curso", SqlDbType.Int, 1, "Curso");

            this._dataAdapterAlumnos.Update(this._dataSetAlumnos_Cursos.Tables[0]);
        }

        public void TraerDatos()
        {
            SqlConnection cnn = new SqlConnection(Modelo.SegundoParcial.LabIII.Properties.Settings.Default.miConexion);
            using (cnn)
            {
                cnn.Open();
                SqlCommand cmm = new SqlCommand("SELECT Legajo_Alumno as Legajo, Apellido_Alumno as Apellido, Curso_Alumno as Curso FROM Alumnos", cnn);
                using (SqlDataReader dare = cmm.ExecuteReader())
                {
                    this._dataSetAlumnos_Cursos.Tables.Add("Alumnos");
                    this._dataSetAlumnos_Cursos.Tables["Alumnos"].Load(dare);
                }
                cnn.Close();
            }
            this._dataSetAlumnos_Cursos.Tables.Add(this.CrearDataTableCursos());
        }

        public void EstablecerRelacion()
        {
            DataRelation relacion = new DataRelation("CursoAlumno", this._dataSetAlumnos_Cursos.Tables["Cursos"].Columns[0], this._dataSetAlumnos_Cursos.Tables["Alumnos"].Columns[2]);
            this._dataSetAlumnos_Cursos.Relations.Add(relacion);
        }

        private void mostrarAlumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void altaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAlumno frmAlu = new frmAlumno();
            frmAlu.ShowDialog();
            if (frmAlu.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this._dataSetAlumnos_Cursos.Tables[0].Rows.Add(new Object[] { frmAlu.UnAlumno.Legajo, frmAlu.UnAlumno.Apellido, frmAlu.UnAlumno.Curso });
            }
        }

        private void bajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int leg = int.Parse(Interaction.InputBox("Ingrese Legajo"));
            for (int i = 0; i < this._dataSetAlumnos_Cursos.Tables[0].Rows.Count; i++)
            {
                if (leg == int.Parse(this._dataSetAlumnos_Cursos.Tables[0].Rows[i][1].ToString()))
                {
                    Alumno auxAlumno = new Alumno(int.Parse(this._dataSetAlumnos_Cursos.Tables[0].Rows[i][0].ToString()), this._dataSetAlumnos_Cursos.Tables[0].Rows[i][1].ToString(), int.Parse(this._dataSetAlumnos_Cursos.Tables[0].Rows[i][2].ToString()));

                    frmAlumno frmAluBaja = new frmAlumno(auxAlumno);
                    frmAluBaja.ShowDialog();

                    if (frmAluBaja.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        this._dataSetAlumnos_Cursos.Tables[0].Rows[i].Delete();
                    }
                }
            }
        }

        private void modificacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int leg = int.Parse(Interaction.InputBox("Ingrese Legajo"));

            for (int i = 0; i < this._dataSetAlumnos_Cursos.Tables[0].Rows.Count; i++)
            {
                if (leg == int.Parse(this._dataSetAlumnos_Cursos.Tables[0].Rows[i][0].ToString()))
                {
                    Alumno auxAlumno = new Alumno(int.Parse(this._dataSetAlumnos_Cursos.Tables[0].Rows[i][0].ToString()), this._dataSetAlumnos_Cursos.Tables[0].Rows[i][1].ToString(), int.Parse(this._dataSetAlumnos_Cursos.Tables[0].Rows[i][2].ToString()));
                    frmAlumno frmAluMod = new frmAlumno(auxAlumno);
                    frmAluMod.ShowDialog();

                    if (frmAluMod.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        this._dataSetAlumnos_Cursos.Tables[0].Rows[i][1] = frmAluMod.UnAlumno.Apellido;
                        this._dataSetAlumnos_Cursos.Tables[0].Rows[i][2] = frmAluMod.UnAlumno.Curso;
                    }
                }
            }
        }

        private void alumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nombre = "alumnos";
            frmMostrar Mostrar = new frmMostrar(this._dataSetAlumnos_Cursos, nombre);
            Mostrar.Show();
        }

        private void cursosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nombre = "curso";
            frmMostrar Mostrar = new frmMostrar(this._dataSetAlumnos_Cursos, nombre);
            Mostrar.Show();
        }

        private void alumnosConElNombreDelCursoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nombre = "aluCurso";
            frmMostrar Mostrar = new frmMostrar(this._dataSetAlumnos_Cursos, nombre);
            Mostrar.Show();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._dataAdapterAlumnos.Update(this._dataSetAlumnos_Cursos.Tables[0]);
        }




    }
}
