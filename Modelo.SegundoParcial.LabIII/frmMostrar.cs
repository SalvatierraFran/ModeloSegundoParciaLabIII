using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modelo.SegundoParcial.LabIII
{
    public partial class frmMostrar : Form
    {
        public frmMostrar()
        {
            InitializeComponent();
        }

        public frmMostrar(DataSet MiDS, string Nombre):this()
        {
            this.CargarDataGrid(MiDS, Nombre);
            this.ConfigurarDataGrid(MiDS, Nombre);
        }

        private void CargarDataGrid(DataSet MiDS, string tabla)
        {
            this.dgvAlumnos.Rows.Clear();

            if (tabla == "alumnos")
            {
                foreach (DataRow fila in MiDS.Tables[0].Rows)
                {
                    this.dgvAlumnos.Rows.Add(fila.ItemArray);
                }
            }

            if (tabla == "curso")
            {
                foreach (DataRow fila in MiDS.Tables["Cursos"].Rows)
                {
                    if (fila.RowState != DataRowState.Deleted)
                    {
                        this.dgvAlumnos.Rows.Add(fila.ItemArray);
                    }
                }
            }

            if (tabla == "aluCurso")
            {
                foreach (DataRow fila in MiDS.Tables[0].Rows)
                {
                    if (fila.RowState != DataRowState.Deleted)
                    {
                        foreach (DataRow filaCurso in fila.GetParentRows("CursoAlumno"))
                        {
                            this.dgvAlumnos.Rows.Add(fila[0], fila[1], fila[2], filaCurso[2]);
                        }
                    }
                }
            }
        }

        private void ConfigurarDataGrid(DataSet MiDS, string tabla)
        {
            if (tabla == "alumnos")
            {
                int cantCol = MiDS.Tables[0].Columns.Count;
                List<string> nombreCol = new List<string>();

                foreach (DataColumn columna in MiDS.Tables[0].Columns)
                {
                    nombreCol.Add(columna.ColumnName);
                }

                this.dgvAlumnos.ColumnCount = cantCol;
                this.dgvAlumnos.GridColor = Color.Black;

                for (int index = 0; index < nombreCol.Count; index++)
                {
                    this.dgvAlumnos.Columns[index].Name = nombreCol[index];
                }

                this.dgvAlumnos.MultiSelect = false;
                this.dgvAlumnos.AllowUserToAddRows = false;
            }

            else if (tabla == "curso")
            {
                int cantCol = MiDS.Tables[0].Columns.Count;
                List<string> nombreCol = new List<string>();

                foreach (DataColumn columna in MiDS.Tables[1].Columns)
                {
                    nombreCol.Add(columna.ColumnName);
                }

                this.dgvAlumnos.ColumnCount = cantCol;
                this.dgvAlumnos.GridColor = Color.Black;
                this.dgvAlumnos.MultiSelect = false;
                this.dgvAlumnos.AllowUserToAddRows = false;
            }

            else if (tabla == "aluCurso")
            {
                int cantCol = MiDS.Tables[0].Columns.Count;
                List<string> nombreCol = new List<string>();

                foreach (DataColumn columna in MiDS.Tables[1].Columns)
                {
                    nombreCol.Add(columna.ColumnName);
                }

                nombreCol.Add("Nombre Curso");
                this.dgvAlumnos.ColumnCount = cantCol;
                this.dgvAlumnos.GridColor = Color.Black;

                for (int index = 0; index < nombreCol.Count; index++)
                {
                    this.dgvAlumnos.Columns[index].Name = nombreCol[index];
                }

                this.dgvAlumnos.MultiSelect = false;
                this.dgvAlumnos.AllowUserToAddRows = false;
            }
        }
    }
}
