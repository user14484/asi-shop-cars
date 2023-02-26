 using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ReaLTaiizor.Forms;

namespace АИС_Автосалон.Admin
{
    public partial class delUser : LostForm
    {
        // Создание объекта логера
        Logger logger = new Logger();

        // Переменная для подключения к БД
        string connectString = "Data Source=" + Properties.Resources.database + ";Version=3;";
        // Объявления класса для удобной работы с БД
        Sqlite sqlite;
        Dictionary<int, string> ListEditUsers = new Dictionary<int, string>();
        public delUser()
        {
            logger.Log("Инициализация объекта sqlite");
            // Инициализация объекта sqlite
            sqlite = new Sqlite(connectString);
            // Инициализация всех компонентов на форме
            InitializeComponent();
            // Подключаемся к БД
            sqlite.Open();
            LoadListUsers();
        }
        private void LoadListUsers()
        {
            try
            {
                logger.Log("Загрузка списка пользователей");
                ListEditUsers.Clear();
                crownComboBox1.DataSource = null;
                string quary = string.Format(
                    "SELECT * FROM users"
                    );
                Dictionary<int, Dictionary<string, string>> temp = sqlite.QuaryMas(quary);

                foreach (KeyValuePair<int, Dictionary<string, string>> i in temp)
                {
                    string nameUser = string.Format(
                        "Логин: {0} | ФИО: {1} {2} {3}",
                        i.Value["login"].ToString(),
                        i.Value["name"].ToString(),
                        i.Value["surname"].ToString(),
                        i.Value["patronymic"].ToString()
                        );
                    ListEditUsers.Add(Convert.ToInt32(i.Value["id"]), nameUser);
                }
                crownComboBox1.DataSource = ListEditUsers.ToList();
                crownComboBox1.DisplayMember = "Value";
                crownComboBox1.ValueMember = "Key";
                crownComboBox1.Refresh();
                crownComboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }

        private int get_selected_id(ComboBox comboBox)
        {
            int id = (int)comboBox.SelectedValue;

            return id;
        }

        private void lostCancelButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string quary = string.Format(
                    "DELETE FROM users WHERE id={0}",
                    get_selected_id(crownComboBox1)
                    );
                sqlite.Quary(quary);
                logger.Log("Удаления пользователя");
                sqlite.success("Пользователь успешно удалён!");
                LoadListUsers();
            } catch(Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }
    }
}
