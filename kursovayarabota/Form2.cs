using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kursovayarabota
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

        }
        public delegate void EmployeeCodeHandler(string fullName);
        public event EmployeeCodeHandler EmployeeCodeEvent;

        public string GetEmployeeFullName(int кодСотрудника)
        {

            string query1 = "SELECT Фамилия + ' ' + Имя + ' ' + Отчество AS ПолноеИмя FROM Сотрудники WHERE КодСотрудника = @КодСотрудника";
            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query1, connection))
                {
                    command.Parameters.AddWithValue("@КодСотрудника", кодСотрудника);

                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        string fio = (string)result;
                        return fio;

                    }
                    else return null;
                }
            }


        }
        public void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            SqlConnection conn1 = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False");

            SqlDataAdapter adapter1 = new SqlDataAdapter();
            DataTable dt = new DataTable();

            string query1 = $"select КодСотрудника, Логин, Пароль, Роль from Сотрудники where Логин = '{login}' and Пароль = '{password}'";

            SqlCommand command1 = new SqlCommand(query1, conn1);
            adapter1.SelectCommand = command1;
            adapter1.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                // Извлекаем КодСотрудника из первой строки DataTable
                int кодСотрудника = Convert.ToInt32(dt.Rows[0]["КодСотрудника"]);


                string fullName = GetEmployeeFullName(кодСотрудника); // Получаем ФИО
                if (EmployeeCodeEvent != null)
                {
                    EmployeeCodeEvent(fullName); // Вызываем событие с ФИО в качестве аргумента
                }


            }
            else
            {
                // Обработка ситуации, когда сотрудник с таким логином и паролем не найден
                MessageBox.Show("Сотрудник с такими учетными данными не найден.");
            }


            if (dt.Rows.Count == 1)
            {
                MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string newlogin = textBox4.Text;
            string newpassword = textBox3.Text;
            int employeecode = Convert.ToInt32(textBox5.Text);
            SqlConnection conn1 = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False");

            try
            {
                conn1.Open();
                // Проверяем, существует ли уже логин и пароль для данного сотрудника
                string checkQuery = "SELECT COUNT(*) FROM Сотрудники WHERE КодСотрудника = @employeecode AND Логин IS NOT NULL AND Пароль IS NOT NULL";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, conn1))
                {
                    checkCommand.Parameters.AddWithValue("@employeecode", employeecode);
                    int userExists = (int)checkCommand.ExecuteScalar();

                    if (userExists > 0)
                    {
                        // Если запись существует, предупреждаем пользователя
                        MessageBox.Show("У данного сотрудника уже есть логин и пароль.");
                    }
                    else
                    {
                        // Если записи нет, обновляем данные
                        string updateQuery = "UPDATE Сотрудники SET Логин = @newlogin, Пароль = @newpassword WHERE КодСотрудника = @employeecode";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, conn1))
                        {
                            updateCommand.Parameters.AddWithValue("@newlogin", newlogin);
                            updateCommand.Parameters.AddWithValue("@newpassword", newpassword);
                            updateCommand.Parameters.AddWithValue("@employeecode", employeecode);

                            int result = updateCommand.ExecuteNonQuery();

                            // Проверка успешности выполнения запроса
                            if (result > 0)
                            {
                                MessageBox.Show("Данные успешно обновлены.");
                            }
                            else
                            {
                                MessageBox.Show("Данные не были обновлены. Проверьте код сотрудника.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                conn1.Close();
            }
        }

    }
}
