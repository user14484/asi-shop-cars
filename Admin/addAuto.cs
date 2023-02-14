using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Controls;

namespace АИС_Автосалон.Admin
{
    public partial class addAuto : LostForm
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
        public addAuto()
        {
            logger.Log("Инициализация объекта sqlite");
            // Инициализация объекта sqlite
            sqlite = new Sqlite(connectString);
            // Инициализация всех компонентов на форме
            InitializeComponent();
            // Подключаемся к БД
            sqlite.Open();
            // Заполняем combobox'ы
            LoadComboBox();
        }

        // Функция заполнения combobox'ов
        private void LoadComboBox()
        {
            Loading(Brands, "SELECT * FROM brands", comboBoxEdit1);
            Loading(Models, "SELECT * FROM models", comboBoxEdit2);
            Loading(TypesCars, "SELECT * FROM types_сars", comboBoxEdit3);
            Loading(Equipments, "SELECT * FROM equipments", comboBoxEdit4);
            Loading(Colors, "SELECT * FROM colors", comboBoxEdit5);
            LoadingEngine(Engines, "SELECT * FROM engines", comboBoxEdit6);
            Loading(Countries, "SELECT * FROM countries", comboBoxEdit7);
            Loading(Years, "SELECT * FROM years", comboBoxEdit8);
            LoadingTransmissions(TypesTransmissions, comboBoxEdit9);
            Loading(TypesDrives, "SELECT * FROM types_drives", comboBoxEdit10);
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

        // Проверка полей на пустоту;
        private bool CheckField(TableLayoutPanel tableLayoutPanel)
        {
            logger.Log("Проверка полей на пустоту");
            bool result = false;
            foreach (Control ctrl in tableLayoutPanel.Controls)
            {
                if (ctrl.GetType() == typeof(RichTextBoxEdit))
                {
                    if (string.IsNullOrEmpty(ctrl.Text) || string.IsNullOrWhiteSpace(ctrl.Text))
                    {
                        sqlite.error("Не все поля заполнены!");
                        return true;
                    }
                }
            }
            return result;
        }

        // Получение айди выбранного item'а combobox'а
        public int get_selected_id(ComboBox comboBox)
        {
            int id = Convert.ToInt32(comboBox.SelectedValue);

            return id;
        }

        private void lostAcceptButton1_Click(object sender, EventArgs e)
        {
            if (CheckField(tableLayoutPanel1))
            {
                return;
            }
            else
            {
                try
                {
                    // Формируем запрос
                    string quary = string.Format(
                        "INSERT INTO cars (brand_id, model_id, type_car_id, equipment_id, color_id, engine_id, country_id, year_id, transmission_id, type_drive_id, price, mass) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11})",
                        get_selected_id(comboBoxEdit1),
                        get_selected_id(comboBoxEdit2),
                        get_selected_id(comboBoxEdit3),
                        get_selected_id(comboBoxEdit4),
                        get_selected_id(comboBoxEdit5),
                        get_selected_id(comboBoxEdit6),
                        get_selected_id(comboBoxEdit7),
                        get_selected_id(comboBoxEdit8),
                        get_selected_id(comboBoxEdit9),
                        get_selected_id(comboBoxEdit10),
                        richTextBoxEdit1.Text,
                        richTextBoxEdit2.Text
                        );
                    sqlite.Quary(quary);

                    sqlite.success("Автомобиль успешно добавлен!");
                } catch(Exception ex)
                {
                    sqlite.error(ex.Message);
                }
            }
        }
    }
}
