using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Controls;

namespace АИС_Автосалон.Admin
{
    public partial class editUser : LostForm
    {
        int g_idUser, g_idPermission, g_idSearch;
        Dictionary<string, string> DataUser = new Dictionary<string, string>();
        // Создание объекта логера
        Logger logger = new Logger();

        // Переменная для подключения к БД
        string connectString = "Data Source=" + Properties.Resources.database + ";Version=3;";
        // Объявления класса для удобной работы с БД
        Sqlite sqlite;
        Dictionary<int, string> ListEditUsers = new Dictionary<int, string>();
        Dictionary<int, string> ListPermissions = new Dictionary<int, string>();

        // Права доступа
        Dictionary<int, string> Accesses = new Dictionary<int, string>() {
            { 1, "Менеджер" },
            { 2, "Администратор" }
        };

        public editUser(Dictionary<string, string> Data)
        {
            logger.Log("Инициализация объекта sqlite");
            // Инициализация объекта sqlite
            sqlite = new Sqlite(connectString);
            // Инициализация всех компонентов на форме
            InitializeComponent();
            // Подключаемся к БД
            sqlite.Open();
            DataUser.Clear();
            DataUser = Data;
            LoadListUsers();
            LoadUser();
        }

        private void UploadingAccess()
        {
            try
            {
                crownComboBox1.DataSource = null;
                crownComboBox1.DataSource = Accesses.ToList();
                crownComboBox1.DisplayMember = "Value";
                crownComboBox1.ValueMember = "Key";
                crownComboBox1.Refresh();
                crownComboBox1.SelectedValue = g_idPermission;

            }
            catch (Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }

        private void GetAccess()
        {
            g_idPermission = Convert.ToInt32(sqlite.QuaryStr($"SELECT access FROM users WHERE id={g_idUser}"));
        }

        private void LoadListUsers()
        {
            try
            {
                ListEditUsers.Clear();
                foreverComboBox1.DataSource = null;
                string quary = string.Format(
                    "SELECT * FROM users"
                    );
                Dictionary<int, Dictionary<string, string>> temp = sqlite.QuaryMas(quary);

                int i = 0;
                foreach (KeyValuePair<int, Dictionary<string, string>> car in temp)
                {
                    if (i == 0)
                    {
                        g_idUser = Convert.ToInt32(car.Value["id"]);
                        g_idPermission = Convert.ToInt32(car.Value["access"]);
                    }
                    string nameUser = string.Format(
                        "Логин: {0} | ФИО: {1} {2} {3}",
                        car.Value["login"].ToString(),
                        car.Value["name"].ToString(),
                        car.Value["surname"].ToString(),
                        car.Value["patronymic"].ToString()
                        );

                    ListEditUsers.Add(Convert.ToInt32(car.Value["id"]), nameUser);
                    i++;
                }
                foreverComboBox1.DataSource = ListEditUsers.ToList();
                foreverComboBox1.DisplayMember = "Value";
                foreverComboBox1.ValueMember = "Key";
                foreverComboBox1.Refresh();
                foreverComboBox1.SelectedIndex = 0;
                g_idUser = get_selected_id(foreverComboBox1);
                GetAccess();
            }
            catch (Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }

        private void foreverComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            g_idUser = get_selected_id(foreverComboBox1);
            LoadUser();
        }

        private void crownButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ListEditUsers.Clear();
                foreverComboBox1.DataSource = null;
                string quary = string.Format(
                    "SELECT * FROM users WHERE login='{0}' OR name='{0}' OR surname='{0}' OR patronymic='{0}'",
                    crownTextBox1.Text
                    );
                Dictionary<int, Dictionary<string, string>> temp = sqlite.QuaryMas(quary);
                int i = 0;
                foreach (KeyValuePair<int, Dictionary<string, string>> user in temp)
                {
                    if (i == 0)
                    {
                        g_idUser = Convert.ToInt32(user.Value["id"]);
                        g_idPermission = Convert.ToInt32(user.Value["access"]);
                    }
                    string nameUser = string.Format(
                        "Логин: {0} | ФИО: {1} {2} {3}",
                        user.Value["login"].ToString(),
                        user.Value["name"].ToString(),
                        user.Value["surname"].ToString(),
                        user.Value["patronymic"].ToString()
                        );

                    ListEditUsers.Add(Convert.ToInt32(user.Value["id"]), nameUser);
                    i++;
                }
                foreverComboBox1.DataSource = ListEditUsers.ToList();
                foreverComboBox1.DisplayMember = "Value";
                foreverComboBox1.ValueMember = "Key";
                foreverComboBox1.Refresh();
                foreverComboBox1.SelectedIndex = 0;
                g_idUser = get_selected_id(foreverComboBox1);
                GetAccess();
                LoadUser();
            }
            catch (Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }

        private void crownButton2_Click(object sender, EventArgs e)
        {
            LoadListUsers();
            LoadUser();
        }

        private void lostAcceptButton1_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                string quary;
                if (string.IsNullOrEmpty(crownTextBox6.Text))
                    quary = string.Format("UPDATE users SET login='{0}', name='{1}', surname='{2}', patronymic='{3}', access={4} WHERE id={5}",
                        crownTextBox2.Text,
                        crownTextBox3.Text,
                        crownTextBox4.Text,
                        crownTextBox5.Text,
                        g_idPermission,
                        g_idUser
                        );
                else
                    quary = string.Format("UPDATE users SET login='{0}', name='{1}', surname='{2}', patronymic='{3}', password='{4}', access={5} WHERE id={6}",
                        crownTextBox2.Text,
                        crownTextBox3.Text,
                        crownTextBox4.Text,
                        crownTextBox5.Text,
                        sqlite.GetMD5Hash(crownTextBox6.Text),
                        g_idPermission,
                        g_idUser
                        );
                sqlite.Quary(quary);
                id = g_idUser;
                LoadListUsers();
                g_idUser = id;
                foreverComboBox1.SelectedValue = g_idUser;
                LoadUser();
                sqlite.success("Пользователь успешно обновлён!");
            } catch(Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }

        private void crownComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            g_idPermission = get_selected_id(crownComboBox1);
        }

        private int get_selected_id(ComboBox comboBox)
        {
            int id = (int)comboBox.SelectedValue;

            return id;
        }

        private void LoadUser()
        {
            GetAccess();
            string quary = $"SELECT * FROM users WHERE id={g_idUser}";
            Dictionary<int, Dictionary<string, string>> temp = sqlite.QuaryMas(quary);

            foreach (KeyValuePair<int, Dictionary<string, string>> user in temp)
            {
                crownTextBox2.Text = user.Value["login"].ToString();
                crownTextBox3.Text = user.Value["name"].ToString();
                crownTextBox4.Text = user.Value["surname"].ToString();
                crownTextBox5.Text = user.Value["patronymic"].ToString();
            }
            crownTextBox6.Text = "";
            UploadingAccess();
        }
    }
}
