using System;
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

                GridAtletas.DataSource = dataTable;
                GridAtletas.DataBind();

            }

            LimparCampos();
            IMC();
        }

        private int GetId()
        {
            var query = "select max(id) from atletas";
            int id = 0;
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = reader.GetInt32(0);
                        }
                    }
                }
            }

            if(id == 0)
            {
                return 1;
            }

            return id + 1;
        }

        private void IMC()
        {
            foreach(GridViewRow gr in GridAtletas.Rows)
            {
                string peso = gr.Cells[5].Text;
                string altura = gr.Cells[4].Text;
                string imc = GetImc(peso, altura);

                gr.Cells[8].Text = imc;
                gr.Cells[9].Text = GetClassificacao(imc);
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

        private void LimparCampos()
        {
            Nome.Text = string.Empty;
            Apelido.Text = string.Empty;
            Nascimento.Text = string.Empty;
            Altura.Text = string.Empty;
            Peso.Text = string.Empty;
            Posicao.Text = string.Empty;
            Camisa.Text = string.Empty;

        }

        protected void Inserir(object sender, EventArgs e)
        {
            string command = "INSERT INTO atletas (id, nome, apelido, nascimento, altura,peso,posicao,camisa) VALUES(@Id, @Nome, @Apelido, @Nascimento, @Altura, @Peso, @Posicao, @Camisa)";
            var coiso = Nascimento.Text;
            SqlParameter[] parameters = {
                new SqlParameter("Nome", Nome.Text),
                new SqlParameter("Apelido", Apelido.Text),
                new SqlParameter("Nascimento", DateTime.ParseExact(Nascimento.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture)),
                new SqlParameter("Altura", float.Parse(Altura.Text, CultureInfo.InvariantCulture.NumberFormat)),
                new SqlParameter("Peso", float.Parse(Peso.Text, CultureInfo.InvariantCulture.NumberFormat)),
                new SqlParameter("Posicao", Posicao.Text),
                new SqlParameter("Camisa", Int32.Parse(Camisa.Text)),
                new SqlParameter("Id", GetId())
            };


            ExecuteNonQuery(command, parameters);
            BindGridView();
        }

        protected void GridAtletas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(GridAtletas.DataKeys[e.RowIndex].Value);
            DeleteData(userId);
            BindGridView();
        }

        private void DeleteData(int id)
        {
            string commandText = "DELETE FROM atletas WHERE Id = @Id";
            SqlParameter[] parameters = { new SqlParameter("@Id", id) };
            ExecuteNonQuery(commandText, parameters);
        }

        protected void GridAtletas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridAtletas.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void GridAtletas_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CancelEditing();
        }

        private void CancelEditing()
        {
            GridAtletas.EditIndex = -1;
            BindGridView();
        }

        protected void GridAtletas_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridAtletas.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void GridAtletas_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(GridAtletas.DataKeys[e.RowIndex].Value);

            GridViewRow row = GridAtletas.Rows[e.RowIndex];
            TextBox nome = (TextBox)row.Cells[1].Controls[0];
            TextBox apelido = (TextBox)row.Cells[2].Controls[0];
            TextBox nascimento = (TextBox)row.Cells[3].Controls[0];
            TextBox altura = (TextBox)row.Cells[4].Controls[0];
            TextBox peso = (TextBox)row.Cells[5].Controls[0];
            TextBox posicao = (TextBox)row.Cells[6].Controls[0];
            TextBox camisa = (TextBox)row.Cells[7].Controls[0];

            UpdateData(userId, nome.Text, apelido.Text, nascimento.Text.Split(' ')[0], altura.Text, peso.Text, posicao.Text, camisa.Text);
            CancelEditing();
        }

        private void UpdateData(int id, string nome, string apelido, string nascimento, string altura, string peso, string posicao, string camisa)
        {
            var teste = new SqlParameter("Nascimento", DateTime.ParseExact(nascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture));

            string commandText = "UPDATE atletas SET Nome = @Nome, Apelido = @Apelido, Nascimento = @Nascimento, Altura = @Altura, Peso = @Peso, Posicao = @Posicao, Camisa = @Camisa WHERE Id = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("Nome", nome),
                new SqlParameter("Apelido", apelido),
                new SqlParameter("Nascimento", DateTime.ParseExact(nascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                new SqlParameter("Altura", float.Parse(altura, CultureInfo.InvariantCulture.NumberFormat)),
                new SqlParameter("Peso", float.Parse(peso, CultureInfo.InvariantCulture.NumberFormat)),
                new SqlParameter("Posicao", posicao),
                new SqlParameter("Camisa", Int32.Parse(camisa)),
                new SqlParameter("Id", id)
            };

            ExecuteNonQuery(commandText, parameters);
        }

        public string GetImc(string peso, string altura)
        {
            float imc = float.Parse(peso) / (float.Parse(altura) * 2);

            return imc.ToString();
        }

        public string GetClassificacao(string imc)
        {
            var x = float.Parse(imc);

            string result = "";

            if(x < 18.5)
            {
                result = "Abaixo do peso normal";
            }

            if(x > 18.5 && x < 24.9)
            {
                result = "Peso normal";
            }

            if(x > 25 && x < 29.9)
            {
                result = "Excesso de peso";
            }

            if (x > 30 && x < 34.9)
            {
                result = "Obesidade classe I";
            }

            if (x > 35 && x < 39.9)
            {
                result = "Obesidade classe II";
            }

            if (x >=  40)
            {
                result = "Obsidade classe III";
            }

            return result;
        }
    }

    
    
}