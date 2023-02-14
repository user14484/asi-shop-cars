using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using System.Data.SQLite;

namespace АИС_Автосалон.Admin
{
    public partial class editAuto : LostForm
    {
        // Создание объекта логера
        Logger logger = new Logger();

        // Переменная для подключения к БД
        string connectString = "Data Source=" + Properties.Resources.database + ";Version=3;";
        // Объявления класса для удобной работы с БД
        Sqlite sqlite;

        // Типы двигателей
        Dictionary<int, string> TypesEngines = new Dictionary<int, string>() {
            { 1, "Бензин" },
            { 2, "Дизель" },
            { 3, "Электро" }
        };
        // Типы КПП
        Dictionary<int, string> Transmissions = new Dictionary<int, string>() {
            { 1, "Механика" },
            { 2, "Автомат" },
            { 3, "Электро" }
        };

        // Словари для хранения данных с БД
        Dictionary<int, string> Brands = new Dictionary<int, string>();
        Dictionary<int, string> Models = new Dictionary<int, string>();
        Dictionary<int, string> TypesCars = new Dictionary<int, string>();
        Dictionary<int, string> Equipments = new Dictionary<int, string>();
        Dictionary<int, string> Colors = new Dictionary<int, string>();
        Dictionary<int, string> Engines = new Dictionary<int, string>();
        Dictionary<int, string> Countries = new Dictionary<int, string>();
        Dictionary<int, string> Years = new Dictionary<int, string>();
        Dictionary<int, string> TypesTransmissions = new Dictionary<int, string>();
        Dictionary<int, string> TypesDrives = new Dictionary<int, string>();

        Dictionary<int, string> Cars = new Dictionary<int, string>();
        public editAuto()
        {
            logger.Log("Инициализация объекта sqlite");
            // Инициализация объекта sqlite
            sqlite = new Sqlite(connectString);
            // Инициализация всех компонентов на форме
            InitializeComponent();
            // Подключаемся к БД
            sqlite.Open();
            LoadComboBox();
            LoadingAutoSheet();
            UnloadingFullness(1);
        }



        private void LoadingAutoSheet()
        {
            Cars.Clear();
            comboBox1.DataSource = null;
            logger.Log("Загрузка списка автомобилей");
            string quary = string.Format(
                "SELECT * FROM cars"
                );

            Dictionary<int, Dictionary<string, string>> temp = sqlite.QuaryMas(quary);

            int i = 0, id = 0;

            foreach (KeyValuePair<int, Dictionary<string, string>> car in temp)
            {
                string brand = "NULL";
                string model = "NULL";

                brand = Brands[Convert.ToInt32(car.Value["brand_id"])];
                model = Models[Convert.ToInt32(car.Value["model_id"])];

                if (i == 0)
                {
                    id = Convert.ToInt32(car.Value["id"]);
                }
                Cars.Add(Convert.ToInt32(car.Value["id"]), brand + " " + model);
                i++;
            }

            comboBox1.DataSource = Cars.ToList();
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            comboBox1.SelectedIndex = 0;
            comboBox1.Refresh();
            UnloadingFullness(id);
        }

        public int get_selected_id(ComboBox comboBox)
        {
            int id = Convert.ToInt32(comboBox.SelectedValue);

            return id;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int id = get_selected_id(comboBox1);
            UnloadingFullness(id);

        }

        private void UnloadingFullness(int id)
        {
            logger.Log("Загрузка данных автомобилей");
            //SQLiteConnection myConnection = new SQLiteConnection(connectString);
            //myConnection.Open();
            string quary = string.Format(
                "SELECT * FROM cars WHERE id={0}",
                id
            );
            //SQLiteCommand cmd = new SQLiteCommand();
            //cmd.Connection = myConnection;
            //cmd.CommandText = quary;

            //SQLiteDataReader reader = cmd.ExecuteReader();
            //if (reader.Read())
            //{
            //    comboBox2.SelectedValue = reader["brand_id"];
            //    comboBox3.SelectedValue = reader["model_id"];
            //    comboBox4.SelectedValue = reader["type_car_id"];
            //    comboBox5.SelectedValue = reader["equipment_id"];
            //    comboBox6.SelectedValue = reader["color_id"];
            //    comboBox7.SelectedValue = reader["engine_id"];
            //    comboBox8.SelectedValue = reader["country_id"];
            //    comboBox9.SelectedValue = reader["year_id"];
            //    comboBox10.SelectedValue = reader["transmission_id"];
            //    comboBox11.SelectedValue = reader["type_drive_id"];
            //    textBox2.Text = reader["price"].ToString();
            //    textBox3.Text = reader["mass"].ToString();
            //}
            //reader.Close();
            //myConnection.Close();

            Dictionary<int, Dictionary<string, string>> temp = sqlite.QuaryMas(quary);
            comboBox2.SelectedValue = Convert.ToInt32(temp.Values.First()["brand_id"]);
            comboBox3.SelectedValue = Convert.ToInt32(temp.Values.First()["model_id"]);
            comboBox4.SelectedValue = Convert.ToInt32(temp.Values.First()["type_car_id"]);
            comboBox5.SelectedValue = Convert.ToInt32(temp.Values.First()["equipment_id"]);
            comboBox6.SelectedValue = Convert.ToInt32(temp.Values.First()["color_id"]);
            comboBox7.SelectedValue = Convert.ToInt32(temp.Values.First()["engine_id"]);
            comboBox8.SelectedValue = Convert.ToInt32(temp.Values.First()["country_id"]);
            comboBox9.SelectedValue = Convert.ToInt32(temp.Values.First()["year_id"]);
            comboBox10.SelectedValue = Convert.ToInt32(temp.Values.First()["transmission_id"]);
            comboBox11.SelectedValue = Convert.ToInt32(temp.Values.First()["type_drive_id"]);
            textBox2.Text = temp.Values.First()["price"].ToString();
            textBox3.Text = temp.Values.First()["mass"].ToString();
        }

        // Функция полчения и заполнения combobox'ов
        private void Loading(Dictionary<int, string> dict, string quary, ComboBox comboBox)
        {
            logger.Log("Выполняеться функция Loading()");
            Dictionary<int, Dictionary<string, string>> temp;
            dict.Clear();
            comboBox.DataSource = null;
            // Выполняем запрос
            temp = sqlite.QuaryMas(quary);
            // Перебираем и заполняем наш словарь
            foreach (KeyValuePair<int, Dictionary<string, string>> i in temp)
            {
                dict.Add(Convert.ToInt32(i.Value["id"]), i.Value.Last().ToString());
            }
            comboBox.DataSource = dict.ToList();
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "Key";
            comboBox.SelectedIndex = 0;
            comboBox.Refresh();
        }
        // Функция полчения и заполнения combobox'а с транисмиссией
        private void LoadingTransmissions(Dictionary<int, string> dict, ComboBox comboBox)
        {
            logger.Log("Выполняеться функция Loading()");
            dict.Clear();
            comboBox.DataSource = null;
            dict = Transmissions;
            comboBox.DataSource = dict.ToList();
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "Key";
            comboBox.SelectedIndex = 0;
            comboBox.Refresh();
        }
        // Функция полчения и заполнения combobox'а с двигателями
        private void LoadingEngine(Dictionary<int, string> dict, string quary, ComboBox comboBox)
        {
            logger.Log("Выполняеться функция LoadingEngine()");
            Dictionary<int, Dictionary<string, string>> temp;
            dict.Clear();
            comboBox.DataSource = null;
            string query = "SELECT * FROM engines";
            temp = sqlite.QuaryMas(query);
            foreach (KeyValuePair<int, Dictionary<string, string>> engine in temp)
            {
                string TypesEnginesName = "NULL";
                // Определяем тип двигателя
                switch (Convert.ToInt32(engine.Value["type_engine"]))
                {
                    case 1:
                        TypesEnginesName = TypesEngines[1];
                        break;
                    case 2:
                        TypesEnginesName = TypesEngines[2];
                        break;
                    case 3:
                        TypesEnginesName = TypesEngines[3];
                        break;
                }
                dict.Add(Convert.ToInt32(engine.Value["id"]),
                    engine.Value["number"].ToString() +
                    " | " +
                    engine.Value["power"].ToString() +
                    " л.с. | " +
                    engine.Value["capacity"].ToString() +
                    " куб. см. | Тип: " +
                    TypesEnginesName
                    );
            }
            comboBox.DataSource = dict.ToList();
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "Key";
            comboBox.SelectedIndex = 0;
            comboBox.Refresh();
        }

        private void LoadComboBox()
        {
            Loading(Brands, "SELECT * FROM brands", comboBox2);
            Loading(Models, "SELECT * FROM models", comboBox3);
            Loading(TypesCars, "SELECT * FROM types_сars", comboBox4);
            Loading(Equipments, "SELECT * FROM equipments", comboBox5);
            Loading(Colors, "SELECT * FROM colors", comboBox6);
            LoadingEngine(Engines, "SELECT * FROM engines", comboBox7);
            Loading(Countries, "SELECT * FROM countries", comboBox8);
            Loading(Years, "SELECT * FROM years", comboBox9);
            LoadingTransmissions(TypesTransmissions, comboBox10);
            Loading(TypesDrives, "SELECT * FROM types_drives", comboBox11);
        }

        private void lostAcceptButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string quary = string.Format(
                    "UPDATE cars SET brand_id={0}, model_id={1}, type_car_id={2}, equipment_id={3}, color_id={4}, engine_id={5}, country_id={6}, year_id={7}, transmission_id={8}, type_drive_id={9}, price={10}, mass={11} WHERE id={12}",
                    comboBox2.SelectedValue,
                    comboBox3.SelectedValue,
                    comboBox4.SelectedValue,
                    comboBox5.SelectedValue,
                    comboBox6.SelectedValue,
                    comboBox7.SelectedValue,
                    comboBox8.SelectedValue,
                    comboBox9.SelectedValue,
                    comboBox10.SelectedValue,
                    comboBox11.SelectedValue,
                    textBox2.Text,
                    textBox3.Text,
                    comboBox1.SelectedValue
                    );
                sqlite.Quary(quary);
                sqlite.success("Автомобиль успешно обновлён!");

            } catch(Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }
    }
}
