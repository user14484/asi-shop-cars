using ReaLTaiizor.Controls;
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

namespace АИС_Автосалон
{
    public partial class FormUser : Form
    {

        // Создание объекта логера
        Logger logger = new Logger();
        // Создание словаря для хранения данных о авторизированном пользователе
        Dictionary<string, string> DataUser = new Dictionary<string, string>();

        string print_string = "";

        // Переменная для подключения к БД
        string connectString = "Data Source=" + Properties.Resources.database + ";Version=3;";
        // Объявления класса для удобной работы с БД
        Sqlite sqlite;
        // Права доступа
        Dictionary<int, string> Accesses = new Dictionary<int, string>() {
            { 1, "Менеджер" },
            { 2, "Администратор" }
        };
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
        Dictionary<int, string> SelectionConditionsreferences = new Dictionary<int, string>();
        Dictionary<int, string> Brands = new Dictionary<int, string>();
        Dictionary<int, string> Models = new Dictionary<int, string>();
        Dictionary<int, string> TypesCars = new Dictionary<int, string>();
        Dictionary<int, string> Equipments = new Dictionary<int, string>();
        Dictionary<int, string> Engines = new Dictionary<int, string>();
        Dictionary<int, string> Colors = new Dictionary<int, string>();
        Dictionary<int, string> Countries = new Dictionary<int, string>();
        Dictionary<int, string> TypesDrives = new Dictionary<int, string>();
        Dictionary<int, string> Years = new Dictionary<int, string>();
        Dictionary<int, string> TypesTransmissions = new Dictionary<int, string>();
        BindingList<ClassAuto> Cars = new BindingList<ClassAuto>();

        string defaultNameComboBox = "";
        int CarId = 0;
        public FormUser(Dictionary<string, string> Data)
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
            logger.Log("Обновление информационной надписи внизу");
            InfoMessage();
            LoadBrands();
            LoadModels();
            LoadTypesСars();
            LoadEquipments();
            LoadEngine();
            LoadColors();
            LoadCountries();
            LoadTypesDrives();
            LoadYears();
            LoadTransmissions();
            LoadCars();
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
        }

        // Информация внизу
        private void InfoMessage()
        {
            string доступ = "error";
            // Задаём значение должности переменной
            switch (Convert.ToInt32(DataUser["access"]))
            {
                case 1:
                    доступ = Accesses[1];
                    break;
                case 2:
                    доступ = Accesses[2];
                    break;
            }
            // Формируем и обновляем надпись внизу
            label1.Text = string.Format(
                "Пользователь: {0} {1} {2} ({3})",
                DataUser["name"], DataUser["surname"], DataUser["patronymic"], доступ
                );
        }

        // Метод выполняеться при закрытии формы
        private void FormUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Закрываем соединение с БД
            sqlite.Close();
        }

        // Функция Загрузки списка марок автомобилей
        private void LoadBrands()
        {
            logger.Log("Загружаеться и обновляеться список марок");
            Dictionary<int, Dictionary<string, string>> temp;
            Brands.Clear();
            // Добавляем дефолтное название
            Brands.Add(0, defaultNameComboBox);
            // Отчищаем предыдущий список
            comboBox1.DataSource = null;
            // Формируем запрос
            string query = "SELECT * FROM brands";
            // Выполняем запрос
            temp = sqlite.QuaryMas(query);
            // Перебираем и заполняем наш словарь
            foreach (KeyValuePair<int, Dictionary<string, string>> brand in temp)
            {
                Brands.Add(Convert.ToInt32(brand.Value["id"]), brand.Value["brand"].ToString());
            }
            // Задаём обновлённый источник данных наш словарь
            comboBox1.DataSource = new BindingSource(Brands, null);
            // Задаём отображаемое значение combobox'a
            comboBox1.DisplayMember = "Value";
            // Задаём скрытое значение combobox'a
            comboBox1.ValueMember = "Key";
            // Задаём значение по умолчанию
            comboBox1.SelectedValue = 0;
            // Перезагружаем combobox
            comboBox1.Refresh();
        }

        // Функция загрузки моделей автомобилей
        private void LoadModels()
        {
            logger.Log("Загружаеться и обновляеться список моделей");
            Dictionary<int, Dictionary<string, string>> temp;
            Models.Clear();
            Models.Add(0, defaultNameComboBox);
            comboBox2.DataSource = null;
            string query = "SELECT * FROM models";
            temp = sqlite.QuaryMas(query);
            foreach (KeyValuePair<int, Dictionary<string, string>> model in temp)
            {
                Models.Add(Convert.ToInt32(model.Value["id"]), model.Value["model"].ToString());
            }
            comboBox2.DataSource = new BindingSource(Models, null);
            comboBox2.DisplayMember = "Value";
            comboBox2.ValueMember = "Key";
            comboBox2.SelectedValue = 0;
            comboBox2.Refresh();
        }

        // Функция загрузки типов автомобилей
        private void LoadTypesСars()
        {
            logger.Log("Загружаеться и обновляеться список типов автомобилей");
            Dictionary<int, Dictionary<string, string>> temp;
            TypesCars.Clear();
            TypesCars.Add(0, defaultNameComboBox);
            comboBox3.DataSource = null;
            string query = "SELECT * FROM types_сars";
            temp = sqlite.QuaryMas(query);
            foreach (KeyValuePair<int, Dictionary<string, string>> TypeCar in temp)
            {
                TypesCars.Add(Convert.ToInt32(TypeCar.Value["id"]), TypeCar.Value["type_name"].ToString());
            }
            comboBox3.DataSource = new BindingSource(TypesCars, null);
            comboBox3.DisplayMember = "Value";
            comboBox3.ValueMember = "Key";
            comboBox3.SelectedValue = 0;
            comboBox3.Refresh();
        }

        // Функция загрузки комплектаций автомобилей
        private void LoadEquipments()
        {
            logger.Log("Загружаеться и обновляеться список комплектаций автомобилей");
            Dictionary<int, Dictionary<string, string>> temp;
            Equipments.Clear();
            Equipments.Add(0, defaultNameComboBox);
            comboBox4.DataSource = null;
            string query = "SELECT * FROM equipments";
            temp = sqlite.QuaryMas(query);
            foreach (KeyValuePair<int, Dictionary<string, string>> equipment in temp)
            {
                Equipments.Add(Convert.ToInt32(equipment.Value["id"]), equipment.Value["equipment"].ToString());
            }
            comboBox4.DataSource = new BindingSource(Equipments, null);
            comboBox4.DisplayMember = "Value";
            comboBox4.ValueMember = "Key";
            comboBox4.SelectedValue = 0;
            comboBox4.Refresh();
        }

        // Функция загрузки двигателей автомобилей
        private void LoadEngine()
        {
            logger.Log("Загружаеться и обновляеться список двигателей автомобилей");
            Dictionary<int, Dictionary<string, string>> temp;
            Engines.Clear();
            Engines.Add(0, defaultNameComboBox);
            comboBox6.DataSource = null;
            string query = "SELECT * FROM engines";
            temp = sqlite.QuaryMas(query);
            foreach (KeyValuePair<int, Dictionary<string, string>> engine in temp)
            {
                string TypesEnginesName = "NULL";
                // Определяем тип двигателя
                switch(Convert.ToInt32(engine.Value["type_engine"]))
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
                Engines.Add(Convert.ToInt32(engine.Value["id"]),
                    engine.Value["number"].ToString() +
                    " | " +
                    engine.Value["power"].ToString() +
                    " л.с. | " +
                    engine.Value["capacity"].ToString() +
                    " куб. см. | Тип: " +
                    TypesEnginesName
                    );
            }
            comboBox6.DataSource = new BindingSource(Engines, null);
            comboBox6.DisplayMember = "Value";
            comboBox6.ValueMember = "Key";
            comboBox6.SelectedValue = 0;
            comboBox6.Refresh();
        }

        // Функция загрузки цветов автомобилей
        private void LoadColors()
        {
            logger.Log("Загружаеться и обновляеться список цветов автомобилей");
            Dictionary<int, Dictionary<string, string>> temp;
            Colors.Clear();
            Colors.Add(0, defaultNameComboBox);
            comboBox5.DataSource = null;
            string query = "SELECT * FROM colors";
            temp = sqlite.QuaryMas(query);
            foreach (KeyValuePair<int, Dictionary<string, string>> color in temp)
            {
                Colors.Add(Convert.ToInt32(color.Value["id"]), color.Value["color"].ToString());
            }
            comboBox5.DataSource = new BindingSource(Colors, null);
            comboBox5.DisplayMember = "Value";
            comboBox5.ValueMember = "Key";
            comboBox5.SelectedValue = 0;
            comboBox5.Refresh();
        }

        // Функция загрузки стран
        private void LoadCountries()
        {
            logger.Log("Загружаеться и обновляеться список стран");
            Dictionary<int, Dictionary<string, string>> temp;
            Countries.Clear();
            Countries.Add(0, defaultNameComboBox);
            comboBox7.DataSource = null;
            string query = "SELECT * FROM countries";
            temp = sqlite.QuaryMas(query);
            foreach (KeyValuePair<int, Dictionary<string, string>> country in temp)
            {
                Countries.Add(Convert.ToInt32(country.Value["id"]), country.Value["country"].ToString());
            }
            comboBox7.DataSource = new BindingSource(Countries, null);
            comboBox7.DisplayMember = "Value";
            comboBox7.ValueMember = "Key";
            comboBox7.SelectedValue = 0;
            comboBox7.Refresh();
        }

        // Функция загрузки типов приводов автомобилей
        private void LoadTypesDrives()
        {
            logger.Log("Загружаеться и обновляеться список типов приводов автомобилей");
            Dictionary<int, Dictionary<string, string>> temp;
            TypesDrives.Clear();
            TypesDrives.Add(0, defaultNameComboBox);
            comboBox10.DataSource = null;
            string query = "SELECT * FROM types_drives";
            temp = sqlite.QuaryMas(query);
            foreach (KeyValuePair<int, Dictionary<string, string>> type_drive in temp)
            {
                TypesDrives.Add(Convert.ToInt32(type_drive.Value["id"]), type_drive.Value["type_drive"].ToString());
            }
            comboBox10.DataSource = new BindingSource(TypesDrives, null);
            comboBox10.DisplayMember = "Value";
            comboBox10.ValueMember = "Key";
            comboBox10.SelectedValue = 0;
            comboBox10.Refresh();
        }

        // Функция загрузки годов
        private void LoadYears()
        {
            logger.Log("Загружаеться и обновляеться список годов");
            Dictionary<int, Dictionary<string, string>> temp;
            Years.Clear();
            Years.Add(0, defaultNameComboBox);
            comboBox8.DataSource = null;
            string query = "SELECT * FROM years";
            temp = sqlite.QuaryMas(query);
            foreach (KeyValuePair<int, Dictionary<string, string>> year in temp)
            {
                Years.Add(Convert.ToInt32(year.Value["id"]), year.Value["year"].ToString());
            }
            comboBox8.DataSource = new BindingSource(Years, null);
            comboBox8.DisplayMember = "Value";
            comboBox8.ValueMember = "Key";
            comboBox8.SelectedValue = 0;
            comboBox8.Refresh();
        }

        // Функция загрузки трансмиссии
        private void LoadTransmissions()
        {
            logger.Log("Загружаеться и обновляеться список трансмиссии");
            TypesTransmissions.Clear();
            TypesTransmissions.Add(0, defaultNameComboBox);
            comboBox9.DataSource = null;
            foreach (KeyValuePair<int, string> transmission in Transmissions)
            {
                TypesTransmissions.Add(Convert.ToInt32(transmission.Key), transmission.Value.ToString());
            }
            comboBox9.DataSource = new BindingSource(TypesTransmissions, null);
            comboBox9.DisplayMember = "Value";
            comboBox9.ValueMember = "Key";
            comboBox9.SelectedValue = 0;
            comboBox9.Refresh();
        }

        // Функция загрузки списка автомобилей
        private void LoadCars()
        {
            logger.Log("Загрузка списка автомобилей");
            try
            {
                // Отчищаем словарь автомобилей
                Cars.Clear();
                // Формируем запрос
                string query = "SELECT * FROM cars";
                // Проверяем есть ли у нас какие ли критерии
                if (!SelectionConditionsreferences.IsNullOrEmpty())
                {
                    string quary2 = string.Format(
                        " WHERE "
                        );
                    logger.Log("Формируеться условие отбора машин");
                    // Перебираем словарь с условиями и формируем запрос
                    foreach (string i in SelectionConditionsreferences.Values)
                    {
                        int temp1;
                        int.TryParse(string.Join("", i.Where(c => char.IsDigit(c))), out temp1);
                        if (temp1 != 0)
                        {
                            if (i == SelectionConditionsreferences.Values.First())
                                query += quary2;
                            if (i == SelectionConditionsreferences.Values.Last() & temp1 != 0)
                                query += i;
                            else
                                query += i + " AND ";
                        }
                    }
                }


                Dictionary<int, Dictionary<string, string>> temp;
                // Выполняем запрос для получения списка машин
                temp = sqlite.QuaryMas(query);
                foreach(KeyValuePair<int, Dictionary<string, string>> car in temp)
                {
                    string brand, model, type_car, equipment, engine, color, country, type_drive, year, type_transmission;
                    int status;

                    // Сопостовляем значения из таблицы с машинами с данными других таблиц
                    brand = Brands[Convert.ToInt32(car.Value["brand_id"])];
                    model = Models[Convert.ToInt32(car.Value["model_id"])];
                    type_car = TypesCars[Convert.ToInt32(car.Value["type_car_id"])];
                    equipment = Equipments[Convert.ToInt32(car.Value["equipment_id"])];
                    engine = Engines[Convert.ToInt32(car.Value["engine_id"])];
                    color = Colors[Convert.ToInt32(car.Value["color_id"])];
                    country = Countries[Convert.ToInt32(car.Value["country_id"])];
                    type_drive = TypesDrives[Convert.ToInt32(car.Value["type_drive_id"])];
                    year = Years[Convert.ToInt32(car.Value["year_id"])];
                    type_transmission = TypesTransmissions[Convert.ToInt32(car.Value["transmission_id"])];
                    status = Convert.ToInt32(car.Value["status"]);


                    /*label1.Text = reader["Цвет"].ToString();*/
                    // Добавляем автомобиль в источник данных
                    if(status != 0)
                        Cars.Add(new ClassAuto(
                            false,
                            Convert.ToInt32(car.Value["id"]),
                            brand.ToString(),
                            model.ToString(),
                            type_car.ToString(),
                            equipment.ToString(),
                            color.ToString(),
                            engine.ToString(),
                            country.ToString(),
                            Convert.ToInt32(year),
                            type_transmission.ToString(),
                            type_drive.ToString(),
                            car.Value["price"] + " ₽",
                            car.Value["mass"] + " кг."
                            ));
                }
                // Задаём источник данных для грида
                dataGridView1.DataSource = Cars;
                // Скрываем колону с айди авто
                dataGridView1.Columns["id"].Visible = false;
                // Задаём всем checkbox'ам режим только чтение
                foreach (DataGridViewColumn i in dataGridView1.Columns)
                {
                    if (i.Name != "Check")
                    {
                        i.ReadOnly = true;
                    }
                }
                // Корректируем колонки
                dataGridView1.Columns["Двигатель"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns["Комплектация"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            catch (Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }

        // Функция убирает выделение строк грида
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        // Функция выделения автомобиля
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            // Получаем айди автомобиля и записиваем в переменную
            CarId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
            foreach (DataGridViewRow i in dataGridView1.Rows)
            {
                // Убираем выделение с checkbox'а
                if (i.Index != e.RowIndex)
                {
                    i.Cells["Check"].Value = false;
                }
            }
        }

        // Функция перекидывает на шаг подтверждения покупки
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            logger.Log("Выбран автомобиль");
            // Ещё раз получаем айди автомобиля
            CarId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
            // Добавляем 2 страницу
            tabControl1.TabPages.Add(tabPage2);
            // Делаем её основной
            tabControl1.SelectedTab = tabPage2;
            // Выполняем функцию оформления заказа
            UploadingReportForm();
            //label1.Text = dataGridView1.Rows[e.RowIndex].Cells["Марка"].Value.ToString();
        }

        // Функция возращает айди checkbox'а
        public int get_selected_id(ComboBox comboBox)
        {
            int id = Convert.ToInt32(comboBox.SelectedValue);

            return id;
        }

        // Функция добавления условия
        private void AddingCondition(ComboBox sender, int Condition, string quary)
        {
            switch (Convert.ToInt32(sender.SelectedValue))
            {
                // Если у нас ничего не выбрано удаляем условие
                case 0:
                    logger.Log("Условие "+ quary + " удалено");
                    SelectionConditionsreferences.Remove(Condition);
                    break;
                // Если что то выбрано то добавляем в условия
                default:
                    // Если условие пустое то удаляем его
                    if (!SelectionConditionsreferences.IsNullOrEmpty())
                    {
                        logger.Log("Условие " + quary + " удалено");
                        SelectionConditionsreferences.Remove(Condition);
                    }
                    logger.Log("Условие " + quary + " Добавлено");
                    // Добавляем условие
                    SelectionConditionsreferences.Add(Condition, quary + "=" + get_selected_id(sender).ToString());
                    break;

            }
            // Обновляем список автомобилей
            LoadCars();
        }

        // Обработчик выбора combobox'а
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox1, 1, "brand_id");
        }
        // Обработчик выбора combobox'а
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox2, 2, "model_id");
        }
        // Обработчик выбора combobox'а
        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox3, 3, "type_car_id");
        }
        // Обработчик выбора combobox'а
        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox4, 4, "equipment_id");
        }
        // Обработчик выбора combobox'а
        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox5, 5, "color_id");
        }
        // Обработчик выбора combobox'а
        private void comboBox6_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox6, 6, "engine_id");
        }
        // Обработчик выбора combobox'а
        private void comboBox7_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox7, 7, "country_id");
        }
        // Обработчик выбора combobox'а
        private void comboBox8_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox8, 8, "year_id");
        }
        // Обработчик выбора combobox'а
        private void comboBox9_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox9, 9, "transmission_id");
        }
        // Обработчик выбора combobox'а
        private void comboBox10_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AddingCondition(comboBox10, 10, "type_drive_id");
        }
        // Обработчик нажатия кнопки оформить заказ
        private void lostAcceptButton1_Click(object sender, EventArgs e)
        {
            logger.Log("Нажата кнопки оформить заказ");
            // Добавляем 3 страницу 
            tabControl1.TabPages.Add(tabPage3);
            // Делаем её активной
            tabControl1.SelectedTab = tabPage3;
        }

        // Оброботчик отмены заказа
        private void lostCancelButton1_Click(object sender, EventArgs e)
        {
            logger.Log("Нажата кнопка отмены заказа");
            // Делаем активной 1 страницу
            tabControl1.SelectedTab = tabPage1;
            // Удаляем 2 и 3 страницы
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
            // Обновляем список автомобилей
            LoadCars();
        }

        // Загружаем данные выбранного автомобиля
        private void UploadingReportForm()
        {
            logger.Log("Загрузка данных выбранного автомобиля");
            // Устанавляем значение label'ей на данные выбранного автомобиля
            foreach (ClassAuto i in Cars.Where(n => n.id == CarId))
            {
                label25.Text = i.марка;
                label26.Text = i.модель;
                label27.Text = i.тип;
                label28.Text = i.комплектация;
                label29.Text = i.цвет;
                label30.Text = i.двигатель;
                label31.Text = i.страна;
                label32.Text = i.год.ToString();
                label33.Text = i.типКПП;
                label34.Text = i.типПривода;
                label35.Text = i.цена.ToString();
                label36.Text = i.вес.ToString();
            }
        }

        // Оброботчик кнопки конечного оформления заказа
        private void foxButton1_Click(object sender, EventArgs e)
        {
            logger.Log("Нажата кнопка добавления заказа");
            CompletionOrderForm();

        }

        // Функция добавления покупки
        private void CompletionOrderForm()
        {
            try
            {
                // Проверяем все ли поля заполнены
                foreach (Control ctrl in tableLayoutPanel5.Controls)
                {
                    if (ctrl.GetType() == typeof(BigTextBox))
                    {
                        if (string.IsNullOrEmpty(ctrl.Text) || string.IsNullOrWhiteSpace(ctrl.Text))
                        {
                            sqlite.error("Не все поля заполнены!");
                            return;
                        }
                    }
                }
                // Получаем текущую дату
                DateTime dateTime = DateTime.UtcNow.Date;

                print_string = $"ФИО: {bigTextBox1.Text}\nПаспорт: {bigTextBox2.Text}\nВремя: {dateTime.ToString("dd/MM/yyyy")}";

                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }

                //Формируем запрос
                string query = string.Format(
                    "INSERT INTO sales (car_id, name, passport, date_time, user_id) VALUES ({0}, '{1}', '{2}', '{3}', {4})",
                    CarId,
                    bigTextBox1.Text,
                    bigTextBox2.Text,
                    dateTime.ToString("dd/MM/yyyy"),
                    DataUser["id"]
                    );
                // Выполняем запрос с БД
                sqlite.Quary(query);

                sqlite.Quary($"UPDATE cars SET status=0 WHERE id={CarId}");
                // Выводим уведомление об успешном добавлении
                sqlite.success("Покупка успешно завершена!");
                // Возращаем обратно на 1 страницу
                tabControl1.SelectedTab = tabPage1;
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);
                LoadCars();
                bigTextBox1.Text = "";
                bigTextBox2.Text = "";

            }
            catch (OleDbException ex)
            {
                sqlite.error(ex.Message);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(print_string, new Font("Arial", 14), Brushes.Black, 0, 0);
        }
    }

    // Изменяем класс словаря для удобной проверки его на пустоту
    static class DictionaryExtentions
    {
        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict == null || dict.Count == 0;
        }
    }
}
