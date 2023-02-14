using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Controls;

namespace АИС_Автосалон.Admin
{
    public partial class addUser : LostForm
    {
        // Создание объекта логера
        Logger logger = new Logger();

        // Переменная для подключения к БД
        string connectString = "Data Source=" + Properties.Resources.database + ";Version=3;";
        // Объявления класса для удобной работы с БД
        Sqlite sqlite;

        // Права доступа
        Dictionary<int, string> Accesses = new Dictionary<int, string>() {
            { 1, "Менеджер" },
            { 2, "Администратор" }
        };
        int idAccess = 0;
        public addUser()
        {
            logger.Log("Инициализация объекта sqlite");
            // Инициализация объекта sqlite
            sqlite = new Sqlite(connectString);
            // Инициализация всех компонентов на форме
            InitializeComponent();
            // Подключаемся к БД
            sqlite.Open();
            LoadingAccesses();
        }

        private bool CheckField()
        {
            bool result = false;

            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                if (ctrl.GetType() == typeof(CrownTextBox))
                {
                    if (string.IsNullOrEmpty(ctrl.Text))
                        result = true;
                }
            }

            return result;
        }

        private void LoadingAccesses()
        {
            crownComboBox1.DataSource = null;
            crownComboBox1.DataSource = Accesses.ToList();
            crownComboBox1.DisplayMember = "Value";
            crownComboBox1.ValueMember = "Key";
            crownComboBox1.SelectedValue = 0;
            crownComboBox1.Refresh();
        }

        private void addUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqlite.Close();
        }

        private void lostAcceptButton1_Click(object sender, EventArgs e)
        {
            logger.Log("Добавление нового пользователя в БД");
            if(CheckField())
            {
                sqlite.error("Не все поля заполнены!");
                return;
            }
            try
            {
                string query = string.Format(
                    "INSERT INTO users (login, name, surname, patronymic, password, access) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', {5})",
                    crownTextBox1.Text,
                    crownTextBox2.Text,
                    crownTextBox3.Text,
                    crownTextBox4.Text,
                    sqlite.GetMD5Hash(crownTextBox5.Text),
                    idAccess
                    );
                sqlite.Quary(query);
                sqlite.success("Пользователь успешно добавлен!");
            } catch(Exception ex)
            {
                sqlite.error(ex.Message);
            }

        }

        private void crownComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            idAccess = get_selected_id(crownComboBox1);
        }

        public int get_selected_id(ComboBox comboBox)
        {
            int id = Convert.ToInt32(comboBox.SelectedValue);

            return id;
        }
    }
}
