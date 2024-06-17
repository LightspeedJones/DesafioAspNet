using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace DesafioAspNet
{
    public partial class _Default : Page
    {
        private readonly string connectionString = @"Data Source=LAPTOP-KJA9K60V;Initial Catalog=AtletasDB;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        private void BindGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                DataTable dataTable = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter("select * from atletas", conn);
                adapter.Fill(dataTable);

                GvData.DataSource = dataTable;
                GvData.DataBind();

            }
        }

        private void ExecuteNonQuery(string command, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }

            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            string command = "INSERT INTO atletas (id, nome, apelido, nascimento, altura,peso,posicao,camisa) VALUES(0, @Nome, @Apelido, @Nascimento, @Altura, @Peso, @Posicao, @Camisa)";
            var nasc = new SqlParameter("Nascimento", DateTime.ParseExact(Nascimento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            var altura = new SqlParameter("Altura", float.Parse(Altura.Text, CultureInfo.InvariantCulture.NumberFormat));
            var camisa = new SqlParameter("Camisa", Int32.Parse(Camisa.Text));

            SqlParameter[] parameters = {
                new SqlParameter("Nome", Nome.Text),
                new SqlParameter("Apelido", Apelido.Text),
                new SqlParameter("Nascimento", DateTime.ParseExact(Nascimento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                new SqlParameter("Altura", float.Parse(Altura.Text, CultureInfo.InvariantCulture.NumberFormat)),
                new SqlParameter("Peso", float.Parse(Peso.Text, CultureInfo.InvariantCulture.NumberFormat)),
                new SqlParameter("Posicao", Posicao.Text),
                new SqlParameter("Camisa", Int32.Parse(Camisa.Text)),
            };


            ExecuteNonQuery(command, parameters);
            BindGridView();
        }

        protected void GvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(GvData.DataKeys[e.RowIndex].Value);
            DeleteData(userId);
            BindGridView();
        }

        private void DeleteData(int id)
        {
            string commandText = "DELETE FROM atleta WHERE Id = @Id";
            SqlParameter[] parameters = { new SqlParameter("@Id", id) };
            ExecuteNonQuery(commandText, parameters);
        }

        protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvData.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void GvData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CancelEditing();
        }

        private void CancelEditing()
        {
            GvData.EditIndex = -1;
            BindGridView();
        }

        protected void GvData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GvData.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void Gvdata_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(GvData.DataKeys[e.RowIndex].Value);

            GridViewRow row = GvData.Rows[e.RowIndex];
            TextBox nome = (TextBox)row.Cells[1].Controls[0];
            TextBox apelido = (TextBox)row.Cells[2].Controls[0];
            TextBox nascimento = (TextBox)row.Cells[3].Controls[0];
            TextBox altura = (TextBox)row.Cells[4].Controls[0];
            TextBox peso = (TextBox)row.Cells[5].Controls[0];
            TextBox posicao = (TextBox)row.Cells[6].Controls[0];
            TextBox camisa = (TextBox)row.Cells[7].Controls[0];

            UpdateData(userId, nome.Text, apelido.Text, nascimento.Text, altura.Text, peso.Text, posicao.Text, camisa.Text);
            CancelEditing();
        }

        private void UpdateData(int id, string nome, string apelido, string nascimento, string altura, string peso, string posicao, string camisa)
        {
            string commandText = "UPDATE data SET Name = @Name, Email = @Email, Phone = @Phone, Age = @Age WHERE Id = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("Nome", nome),
                new SqlParameter("Apelido", apelido),
                new SqlParameter("Nascimento", nascimento),
                new SqlParameter("Altura", altura),
                new SqlParameter("Peso", peso),
                new SqlParameter("Posicao", posicao),
                new SqlParameter("Camisa", camisa),
                new SqlParameter("Id", id)
            };

            ExecuteNonQuery(commandText, parameters);
        }
    }

    
}