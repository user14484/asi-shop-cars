using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using System.Data.SQLite;

namespace АИС_Автосалон
{
    public partial class FormAdmin : LostForm
    {
        // Создание объекта логера
        Logger logger = new Logger();
        // Создание словаря для хранения данных о авторизированном пользователе
        Dictionary<string, string> DataUser = new Dictionary<string, string>();

        // Переменная для подключения к БД
        static string connectString = "Data Source=" + Properties.Resources.database + ";Version=3;";
        // Объявления класса для удобной работы с БД
        Sqlite sqlite;

        DataSet Brands = new DataSet();
        DataSet Models = new DataSet();
        DataSet TypesCars = new DataSet();
        DataSet Equipments = new DataSet();
        DataSet Colors = new DataSet();
        DataSet Engines = new DataSet();
        DataSet Countries = new DataSet();
        public FormAdmin(Dictionary<string, string> Data)
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
            LoadTables();
        }

        private void LoadTables()
        {
            LoadTable("SELECT * FROM brands", dataGridView1, Brands, "brands");
            LoadTable("SELECT * FROM models", dataGridView2, Models, "models");
            LoadTable("SELECT * FROM types_сars", dataGridView3, TypesCars, "types_сars");
            LoadTable("SELECT * FROM equipments", dataGridView4, Equipments, "equipments");
            LoadTable("SELECT * FROM colors", dataGridView5, Colors, "colors");
            LoadTable("SELECT * FROM engines", dataGridView6, Engines, "engines");
            LoadTable("SELECT * FROM countries", dataGridView7, Countries, "countries");
        }

        private void LoadTable(string quary, DataGridView dataGridView, DataSet data, string table)
        {
            logger.Log("Загрузка таблиц из БД");
            SQLiteConnection myConnection = new SQLiteConnection(connectString);
            myConnection.Open();
            SQLiteDataAdapter dataAdapter;
            dataGridView.DataSource = null;
            data.Clear();
            dataAdapter = new SQLiteDataAdapter(quary, myConnection);
            dataAdapter.Fill(data, table);
            dataGridView.DataSource = data.Tables[0].DefaultView;
            myConnection.Close();
        }

        private void UpdateTable(DataGridView dataGridView, DataSet data)
        {
            logger.Log("Обновление таблиц");
            dataGridView.DataSource = null;
            data.Clear();
            dataGridView.Refresh();
        }

        private void UpgradeTable(string quary, DataSet data)
        {
            logger.Log("Применение изменений к БД");
            SQLiteConnection myConnection = new SQLiteConnection(connectString);
            myConnection.Open();
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(quary, myConnection);
            SQLiteCommandBuilder cb = new SQLiteCommandBuilder(dataAdapter);
            dataAdapter.InsertCommand = cb.GetInsertCommand(true);
            dataAdapter.Update(data.Tables[0]);
            myConnection.Close();
        }

        private void FormAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqlite.Close();
        }

        private void lostButton1_Click(object sender, EventArgs e)
        {
            UpdateTable(dataGridView1, Brands);
            UpdateTable(dataGridView2, Models);
            UpdateTable(dataGridView3, TypesCars);
            UpdateTable(dataGridView4, Equipments);
            UpdateTable(dataGridView5, Colors);
            UpdateTable(dataGridView6, Engines);
            UpdateTable(dataGridView7, Countries);
            LoadTables();
        }

        private void lostButton2_Click(object sender, EventArgs e)
        {
            UpgradeTable("SELECT * FROM brands", Brands);
            UpgradeTable("SELECT * FROM models", Models);
            UpgradeTable("SELECT * FROM types_сars", TypesCars);
            UpgradeTable("SELECT * FROM equipments", Equipments);
            UpgradeTable("SELECT * FROM colors", Colors);
            UpgradeTable("SELECT * FROM engines", Engines);
            UpgradeTable("SELECT * FROM countries", Countries);
            lostButton1_Click(null, new EventArgs());
            sqlite.success("База данных успешно обновлена!");
        }

        private void AddUsers()
        {
            /* Создаём объект формы входа в систему */
            Admin.addUser adduser = new Admin.addUser();
            /* При закрытии формы открываем основную форму */
            adduser.FormClosed += ((s, e) => { this.Show(); });
            /* Открываем форму входа в систему */
            adduser.Show();
            /* Скрываем основную форму */
            this.Hide();
        }

        private void lostAcceptButton1_Click(object sender, EventArgs e)
        {
            AddUsers();
        }

        private void lostButton3_Click(object sender, EventArgs e)
        {
            if (crownTextBox1.Text == "")
            {
                sqlite.error("Поле поиска пустое!");
            }
            else
            {
                string search = crownTextBox1.Text;
                LoadTable(string.Format(
                    "SELECT * FROM brands WHERE brand LIKE '%{0}%'",
                    search
                    ), dataGridView1, Brands, "brands");

                LoadTable(string.Format(
                    "SELECT * FROM models WHERE model LIKE '%{0}%'",
                    search
                    ), dataGridView2, Models, "models");
                LoadTable(string.Format(
                    "SELECT * FROM types_сars WHERE type_name LIKE '%{0}%'",
                    search
                    ), dataGridView3, TypesCars, "types_сars");
                LoadTable(string.Format(
                    "SELECT * FROM equipments WHERE equipment LIKE '%{0}%'",
                    search
                    ), dataGridView4, Equipments, "equipments");
                LoadTable(string.Format(
                    "SELECT * FROM colors WHERE color LIKE '%{0}%'",
                    search
                    ), dataGridView5, Colors, "colors");
                LoadTable(string.Format(
                    "SELECT * FROM engines WHERE number LIKE '%{0}%' OR power LIKE '%{0}%' OR capacity LIKE '%{0}%'",
                    search
                    ), dataGridView6, Engines, "engines");
                LoadTable(string.Format(
                    "SELECT * FROM countries WHERE country LIKE '%{0}%'",
                    search
                    ), dataGridView7, Countries, "countries");
            }
        }

        private void lostAcceptButton3_Click(object sender, EventArgs e)
        {
            EditUsers();
        }
        private void EditUsers()
        {
            /* Создаём объект формы входа в систему */
            Admin.editUser editUser = new Admin.editUser(DataUser);
            /* При закрытии формы открываем основную форму */
            editUser.FormClosed += ((s, e) => { this.Show(); });
            /* Открываем форму входа в систему */
            editUser.Show();
            /* Скрываем основную форму */
            this.Hide();
        }

        private void lostCancelButton1_Click(object sender, EventArgs e)
        {
            DelUsers();
        }

        private void DelUsers()
        {
            /* Создаём объект формы входа в систему */
            Admin.delUser delUser = new Admin.delUser();
            /* При закрытии формы открываем основную форму */
            delUser.FormClosed += ((s, e) => { this.Show(); });
            /* Открываем форму входа в систему */
            delUser.Show();
            /* Скрываем основную форму */
            this.Hide();
        }

        private void lostAcceptButton2_Click(object sender, EventArgs e)
        {
            AddCar();
        }

        private void AddCar()
        {
            /* Создаём объект формы входа в систему */
            Admin.addAuto addAuto = new Admin.addAuto();
            /* При закрытии формы открываем основную форму */
            addAuto.FormClosed += ((s, e) => { this.Show(); });
            /* Открываем форму входа в систему */
            addAuto.Show();
            /* Скрываем основную форму */
            this.Hide();
        }

        private void lostAcceptButton4_Click(object sender, EventArgs e)
        {
            EditCars();
        }

        private void EditCars()
        {
            /* Создаём объект формы входа в систему */
            Admin.editAuto editAuto = new Admin.editAuto();
            /* При закрытии формы открываем основную форму */
            editAuto.FormClosed += ((s, e) => { this.Show(); });
            /* Открываем форму входа в систему */
            editAuto.Show();
            /* Скрываем основную форму */
            this.Hide();
        }
    }
}
