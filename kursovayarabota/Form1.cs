using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kursovayarabota
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Form2 loginForm = new Form2();
            dataGridView3.CellEndEdit += DataGridView3_CellEndEdit;
            dataGridView4.CellEndEdit += DataGridView4_CellEndEdit;
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;

            this.StartPosition = FormStartPosition.CenterScreen;

            loginForm.EmployeeCodeEvent += UpdateFormTitle;

            // Показываем форму и проверяем результат
            if (loginForm.ShowDialog() != DialogResult.OK)
            {
               
                this.Close();
            }
            else
            {
                
                LoadProducts();
                UpdateTitles();
                LoadCB();
            }
        }



        // Метод в Form1 для обновления названия

        public static string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public void UpdateFormTitle(string fullName)
        {
            this.Text = "Привет, " + fullName; // Обновляем название формы
        }

        public void UpdateColumnHeaders(DataGridView dgv)
        {
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.HeaderText = AddSpacesToSentence(column.HeaderText, true);
            }
        }

        public void UpdateTitles() 
        {
            UpdateColumnHeaders(dataGridView1);
            UpdateColumnHeaders(dataGridView2);
            UpdateColumnHeaders(dataGridView3);
            UpdateColumnHeaders(dataGridView4);
           
            UpdateColumnHeaders(dataGridView6);
           
            UpdateColumnHeaders(dataGridView8);


        }


        public class Position //требуется для хранения кода должности и названия должности combobox c выбором должности
        {
            public int Code { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private void LoadCB()
        {

            List<Position> positions = new List<Position>();
            List<Position> positions2 = new List<Position>();
            List<Position> positions3 = new List<Position>();
            List<Position> positions4 = new List<Position>();
            List<Position> positions5 = new List<Position>();


            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT КодДолжности, Должность FROM Должности", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                positions.Add(new Position
                                {
                                    Code = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command2 = new SqlCommand("SELECT КодКлиента, ФИО FROM Клиенты", connection))
                {
                    using (SqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            positions2.Add(new Position
                            {
                                Code = reader2.GetInt32(0),
                                Name = reader2.GetString(1)
                            });
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command3 = new SqlCommand("SELECT КодПоставщика, НаименованиеКомпании FROM Поставщики", connection))
                {
                    using (SqlDataReader reader3 = command3.ExecuteReader())
                    {
                        while (reader3.Read())
                        {
                            positions3.Add(new Position
                            {
                                Code = reader3.GetInt32(0),
                                Name = reader3.GetString(1)
                            });
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command3 = new SqlCommand("SELECT Код, Роль FROM Роли", connection))
                {
                    using (SqlDataReader reader3 = command3.ExecuteReader())
                    {
                        while (reader3.Read())
                        {
                            positions5.Add(new Position
                            {
                                Code = reader3.GetInt32(0),
                                Name = reader3.GetString(1)
                            });
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command4 = new SqlCommand("SELECT КодПоступления, Дата FROM ПоступлениеТоваров", connection))
                {
                    using (SqlDataReader reader4 = command4.ExecuteReader())
                    {
                        while (reader4.Read())
                        {
                            positions4.Add(new Position
                            {
                                Code = reader4.GetInt32(0),
                                Name = Convert.ToString(reader4.GetDateTime(1))
                            });
                        }
                    }
                }
            }
            comboBox7.DataSource = positions5;
            comboBox7.DisplayMember = "Name"; // Отображаем названия
            comboBox7.ValueMember = "Code"; // Сохраняем коды
          
            comboBox4.DataSource = positions2;
            comboBox4.DisplayMember = "Name"; // Отображаем названия
            comboBox4.ValueMember = "Code"; // Сохраняем коды
          
            comboBox2.DataSource = positions2;
            comboBox2.DisplayMember = "Name"; // Отображаем названия
            comboBox2.ValueMember = "Code"; // Сохраняем коды
            comboBox1.DataSource = positions;
            comboBox1.DisplayMember = "Name"; // Отображаем названия
            comboBox1.ValueMember = "Code"; // Сохраняем коды
            
        }

        private void DeleteRow(DataGridView dataGridView, string procedureName, string cellname)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];
                int kod = Convert.ToInt32(selectedRow.Cells[cellname].Value);

                using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
                {
                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Код", kod);

                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Строка удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Перезагружаем данные
                        LoadProducts();

                    }
                }
            }

            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {

            // создаем подключение к базе данных
            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                // создаем команду для выборки товаров из базы данных



                string query1 = "SELECT * From Товары";
                string query2 = "SELECT * FROM Клиенты";

                string query4 = "SELECT ОстаткиТоваров.Код, Товары.НаименованиеТовара, ОстаткиТоваров.Количество FROM ОстаткиТоваров JOIN Товары ON ОстаткиТоваров.ТоварFK = Товары.КодТовара";
                
                string query6 = "SELECT КодТовара, НаименованиеТовара, Цена FROM Товары";


                //Сотрудники 
                using (SqlCommand command1 = new SqlCommand("ВыводСотрудников", connection))
                {
                    // открываем подключение
                    connection.Open();
                    command1.CommandType = CommandType.StoredProcedure;
                    // создаем адаптер данных и заполняем DataTable
                    SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
                    DataTable dataTable1 = new DataTable();
                    adapter1.Fill(dataTable1);

                    // устанавливаем DataTable как источник данных для первого DataGridView
                    dataGridView1.DataSource = dataTable1;
                }
                //Заказы
                using (SqlCommand getZakaz = new SqlCommand("ВыводЗаказов", connection))
                {
                    getZakaz.CommandType = CommandType.StoredProcedure;
                    DataTable datatable2 = new DataTable();

                    using (SqlDataAdapter dataadapter1 = new SqlDataAdapter(getZakaz))
                    {
                        // Заполняем таблицу данными из процедуры
                        dataadapter1.Fill(datatable2);
                        dataGridView2.DataSource = datatable2;
                    }
                }


                using (SqlCommand command2 = new SqlCommand(query2, connection))
                {

                    // создаем адаптер данных и заполняем DataTable
                    SqlDataAdapter adapter2 = new SqlDataAdapter(command2);
                    DataTable datatable4 = new DataTable();
                    adapter2.Fill(datatable4);

                    // устанавливаем DataTable как источник данных для первого DataGridView
                    dataGridView4.DataSource = datatable4;
                }



                using (SqlCommand getVozvratTovarov = new SqlCommand("ВыводВозврата", connection))
                {
                    getVozvratTovarov.CommandType = CommandType.StoredProcedure;
                    DataTable datatable6 = new DataTable();

                    using (SqlDataAdapter dataadapter4 = new SqlDataAdapter(getVozvratTovarov))
                    {
                        // Заполняем таблицу данными из процедуры
                        dataadapter4.Fill(datatable6);
                        dataGridView6.DataSource = datatable6;
                    }
                }
                using (SqlCommand command11 = new SqlCommand(query1, connection))
                {

                    // создаем адаптер данных и заполняем DataTable
                    SqlDataAdapter adapter11 = new SqlDataAdapter(command11);
                    DataTable datatable11 = new DataTable();
                    adapter11.Fill(datatable11);

                    // устанавливаем DataTable как источник данных для первого DataGridView
                    dataGridView3.DataSource = datatable11;
                }
                using (SqlCommand command4 = new SqlCommand(query4, connection))
                {

                    // создаем адаптер данных и заполняем DataTable
                    SqlDataAdapter adapter6 = new SqlDataAdapter(command4);
                    DataTable datatable8 = new DataTable();
                    adapter6.Fill(datatable8);

                    // устанавливаем DataTable как источник данных для первого DataGridView
                    dataGridView8.DataSource = datatable8;
                }

                using (SqlCommand command6 = new SqlCommand(query6, connection))
                {

                    // создаем адаптер данных и заполняем DataTable
                    SqlDataAdapter adapter7 = new SqlDataAdapter(command6);
                    DataTable datatable10 = new DataTable();
                    adapter7.Fill(datatable10);

                    // устанавливаем DataTable как источник данных для первого DataGridView
                    dataGridProducts.DataSource = datatable10;
                }




            }
            // задаем столбцы для второго DataGrid
            dataGridSelectedProducts.Columns.Add("КодТовара", "Код товара");
            dataGridSelectedProducts.Columns.Add("Наименование", "Наименование");
            dataGridSelectedProducts.Columns.Add("Цена", "Цена");
            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn();
            quantityColumn.HeaderText = "Количество";
            quantityColumn.Name = "Количество";
            dataGridSelectedProducts.Columns.Add(quantityColumn);




        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }
        //СОТРУДНИКИ
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                // открываем подключение
                connection.Open();

                // создаем команду для вызова процедуры
                using (SqlCommand command = new SqlCommand("ДобавлениеСотрудника", connection))
                {
                    Image img = pictureBox1.Image;
                    byte[] array;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Сохраняем изображение в потоке MemoryStream
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                        // Преобразуем поток в массив байтов
                        array = ms.ToArray();
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Имя", textBox1.Text.ToString()); // Наименование
                    command.Parameters.AddWithValue("@Фамилия", textBox2.Text.ToString()); // Наименование
                    command.Parameters.AddWithValue("@Отчество", textBox3.Text.ToString()); //Адрес
                    command.Parameters.AddWithValue("@ДолжностьFK", comboBox1.SelectedValue); // Наименование
                    command.Parameters.AddWithValue("@СерияПаспорта", textBox5.Text.ToString()); // Наименование
                    command.Parameters.AddWithValue("@НомерПаспорта", textBox4.Text.ToString()); // Наименование
                    command.Parameters.AddWithValue("@ДатаВыдачи", dateTimePicker1.Value); // Наименование
                    command.Parameters.AddWithValue("@КодПодразделения", textBox7.Text.ToString()); // Наименование
                    command.Parameters.AddWithValue("@КемВыдан", textBox8.Text.ToString());//Телефон
                    command.Parameters.AddWithValue("@АдресПрописки", textBox9.Text.ToString()); // Наименование
                    command.Parameters.AddWithValue("@ФактическийАдрес", textBox10.Text.ToString());//ЭлПочта
                    command.Parameters.AddWithValue("@Роль", comboBox7.SelectedValue);
                    command.Parameters.AddWithValue("@Фотография", array);
                    command.ExecuteNonQuery();
                    LoadProducts();
                    MessageBox.Show("Клиент успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
        //ДОБАВЛЕНИЕ ТОВАРОВ В ЗАКАЗ

        private void button2_Click_1(object sender, EventArgs e)
        {
            // проверяем, есть ли выбранные строки в первом DataGridView
            if (dataGridProducts.SelectedRows.Count > 0)
            {
                // получаем выбранные строки из первого DataGridView
                foreach (DataGridViewRow row in dataGridProducts.SelectedRows)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row.Cells[i].Value });
                    }
                    dataGridSelectedProducts.Rows.Add(newRow);
                }
            }
            else //если нет выводим ошибочку
            {
                MessageBox.Show("Выберите товары для добавления в заказ.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LoadProducts();
        }


        

        //ЗАКАЗ

        private void button3_Click_1(object sender, EventArgs e)
        {
            DataTable productsTable = new DataTable();
            productsTable.Columns.Add("ТоварFK", typeof(int)); // Код товара
            productsTable.Columns.Add("Количество", typeof(int)); // Количество
            productsTable.Columns.Add("Цена", typeof(decimal)); // Цена товара

            foreach (DataGridViewRow row in dataGridSelectedProducts.Rows)
            {
                if (row.Cells["КодТовара"].Value != null && row.Cells["Количество"].Value != null && row.Cells["Цена"].Value != null)
                {
                    int productID = Convert.ToInt32(row.Cells["КодТовара"].Value);
                    int quantity = Convert.ToInt32(row.Cells["Количество"].Value);
                    decimal price = Convert.ToDecimal(row.Cells["Цена"].Value);
                    productsTable.Rows.Add(productID, quantity, price);
                }
            }

            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();
                DateTime todaydate = DateTime.Now;

                using (SqlCommand CreateZakaz = new SqlCommand("СозданиеНовогоЗаказа", connection))
                {
                    CreateZakaz.CommandType = CommandType.StoredProcedure;
                    CreateZakaz.Parameters.AddWithValue("@КлиентFK", Convert.ToInt32(comboBox2.SelectedValue));
                    CreateZakaz.Parameters.AddWithValue("@Дата", todaydate);
                    CreateZakaz.Parameters.AddWithValue("@Статус", "Новый");
                    CreateZakaz.Parameters.AddWithValue("@ТоварыТаблица", productsTable);
                    CreateZakaz.ExecuteNonQuery();
                    MessageBox.Show("Заказ успешно создан.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProducts();
        }

        //КЛИЕНТ
        private void button6_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                // открываем подключение
                connection.Open();

                // создаем команду для вызова процедуры
                using (SqlCommand command = new SqlCommand("ДобавлениеКлиента", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Наименование", textBox6.Text.ToString()); // Наименование
                    command.Parameters.AddWithValue("@Адрес", textBox11.Text.ToString()); // Наименование
                    command.Parameters.AddWithValue("@Телефон", textBox12.Text.ToString()); //Адрес
                    command.Parameters.AddWithValue("@ЭлектроннаяПочта", textBox13.Text.ToString()); // Наименование

                    command.ExecuteNonQuery();
                    LoadProducts();
                    MessageBox.Show("Клиент успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProducts();
        }



        private void button8_Click(object sender, EventArgs e)
        {
            DateTime todaydate = DateTime.Now;
            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {
                // открываем подключение
                connection.Open();

                // создаем команду для вызова процедуры
                using (SqlCommand AddRefund = new SqlCommand("ДобавлениеВозврата", connection))
                {
                    AddRefund.CommandType = CommandType.StoredProcedure;
                    AddRefund.Parameters.AddWithValue("@ПричинаВозврата", richTextBox1.Text.ToString()); // Наименование
                    AddRefund.Parameters.AddWithValue("@Дата", todaydate);
                    AddRefund.Parameters.AddWithValue("@КлиентFK", Convert.ToInt32(comboBox2.SelectedValue)); // Наименование
                    AddRefund.Parameters.AddWithValue("@ЗаказFK", Convert.ToInt32(textBox20.Text)); // Наименование
                    AddRefund.ExecuteNonQuery();
                    LoadProducts();
                    MessageBox.Show("Возврат успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProducts();
        }

       

        private void UpdateProductQuantity(int storageId, int newQuantity)
        {
            string connectionString = "Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False";
            string query = "UPDATE ОстаткиТоваров SET Количество = @NewQuantity WHERE Код = @StorageId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewQuantity", newQuantity);
                    command.Parameters.AddWithValue("@StorageId", storageId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }



        private void button10_Click(object sender, EventArgs e) //товары
        {
            int selectedRow = dataGridView8.SelectedRows[0].Index;
            int storageId = (int)dataGridView8.Rows[selectedRow].Cells["Код"].Value;
            int newQuantity;

            if (int.TryParse(textBox21.Text, out newQuantity))
            {
                UpdateProductQuantity(storageId, newQuantity);
                LoadProducts();  // Обновление данных в DataGridView
                MessageBox.Show("Количество продукции на складе успешно обновлено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для количества.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Получаем значение ячейки
                DataGridViewCell cell = dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string columnName = dataGridView3.Columns[e.ColumnIndex].Name;
                object newValue = cell.Value;

                // Получаем код товара
                int productId = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells["КодТовара"].Value);

                // Обновляем базу данных
                UpdateProduct(productId, columnName, newValue);
                LoadProducts();
            }
        }

        private void UpdateProduct(int productId, string columnName, object newValue)
        {
            string connectionString = "Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False";
            string query = $"UPDATE Товары SET {columnName} = @NewValue WHERE КодТовара = @ProductId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewValue", newValue);
                    command.Parameters.AddWithValue("@ProductId", productId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DataGridView4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Получаем значение ячейки
                DataGridViewCell cell = dataGridView4.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string columnName = dataGridView4.Columns[e.ColumnIndex].Name;
                object newValue = cell.Value;

                // Получаем код клиента
                int clientId = Convert.ToInt32(dataGridView4.Rows[e.RowIndex].Cells["КодКлиента"].Value);

                // Обновляем базу данных
                UpdateClient(clientId, columnName, newValue);
            }
        }

        private void UpdateClient(int clientId, string columnName, object newValue)
        {
            string connectionString = "Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False";
            string query = $"UPDATE Клиенты SET {columnName} = @NewValue WHERE КодКлиента = @ClientId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewValue", newValue);
                    command.Parameters.AddWithValue("@ClientId", clientId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Получаем значение ячейки
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
                object newValue = cell.Value;

                // Получаем код сотрудника
                int employeeId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["КодСотрудника"].Value);

                // Обновляем базу данных
                UpdateEmployee(employeeId, columnName, newValue);
            }
        }

        private void UpdateEmployee(int employeeId, string columnName, object newValue)
        {
            string connectionString = "Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False";
            string query = $"UPDATE Сотрудники SET {columnName} = @NewValue WHERE КодСотрудника = @EmployeeId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewValue", newValue);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog of1 = new OpenFileDialog();
            of1.ShowDialog();
            string name1 = of1.FileName;
            Image img1 = new Bitmap(name1);
            pictureBox1.Image = img1;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DeleteRow(dataGridView1, "УдалитьСотрудника", "КодСотрудника");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DeleteRow(dataGridView2, "УдалитьЗаказ", "НомерЗаказа");
        }



        private void button16_Click(object sender, EventArgs e)
        {
            DeleteRow(dataGridView4, "УдалитьКлиента", "КодКлиента");
        }



        private void button18_Click(object sender, EventArgs e)
        {
            DeleteRow(dataGridView6, "УдалитьВозвратТК", "КодВозвратаТК");
        }



        private void button21_Click(object sender, EventArgs e)
        {
            DeleteRow(dataGridView8, "УдалитьОстаткиТоваров", "Код");
        }


        private void button4_Click(object sender, EventArgs e)
        {

            using (SqlConnection connection = new SqlConnection("Data Source=denitydg;Initial Catalog=KURSA4SHAROV;Integrated Security=True;Encrypt=False"))
            {   
                
                // открываем подключение
                connection.Open();

                // создаем команду для вызова процедуры
                using (SqlCommand AddRefund = new SqlCommand("ДобавлениеТовара", connection))
                {
                    AddRefund.CommandType = CommandType.StoredProcedure;
                    AddRefund.Parameters.AddWithValue("@Наименование", textBox14.Text.ToString()); // Наименование
                    AddRefund.Parameters.AddWithValue("@Цена", Convert.ToDecimal(textBox15.Text));
                    AddRefund.Parameters.AddWithValue("@КатегорияТовара", textBox16.Text.ToString()); // Наименование
                    AddRefund.Parameters.AddWithValue("@Описание", textBox17.Text.ToString()); // Наименование
                    AddRefund.ExecuteNonQuery();
                    LoadProducts();
                    MessageBox.Show("Товар/Услуга успешно добавлен(а).", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProducts();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DeleteRow(dataGridView3, "УдалитьТовар", "Код");
        }
    }
    }

