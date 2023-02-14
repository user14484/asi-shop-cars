using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АИС_Автосалон
{
    public partial class FormLogin : Form
    {
        // Создание объекта логера
        Logger logger = new Logger();
        // Создание словаря для хранения данных о авторизированном пользователе
        Dictionary<string, string> DataUser = new Dictionary<string, string>();

        // Переменная для подключения к БД
        string connectString = "Data Source=" + Properties.Resources.database + ";Version=3;";
        // Объявления класса для удобной работы с БД
        Sqlite sqlite;
        public FormLogin()
        {
            logger.Log("Инициализация объекта sqlite");
            // Инициализация объекта sqlite
            sqlite = new Sqlite(connectString);
            // Инициализация всех компонентов на форме
            InitializeComponent();
            // Подключаемся к БД
            sqlite.Open();
        }

        // Функция регистрации
        private void registration()
        {
            string login, password, password_retry, name, surname, patronymic;

            logger.Log("Чтение полей регистрации");
            // Считываем содержимое полей в переменные
            login = textBox_reg_login.Text;
            password = textBox_reg_password.Text;
            password_retry = textBox_reg_password_retry.Text;
            name = textBox_reg_name.Text;
            surname = textBox_reg_surname.Text;
            patronymic = textBox_reg_patronymic.Text;

            logger.Log("Проверка полей регистрации");
            // Проверяем все ли поля заполнены
            if (!CheckingRegistrationField())
            {
                // Выводим сообщение если не все полня заполнены
                sqlite.error("Не все поля заполнены!");
                // Завершаем выполнение функции
                return;
            }
            // Проверяем совподают ли пароли
            else if (password != password_retry)
            {
                // Выводим сообщение если пароли не совподают
                sqlite.error("Пароли не совподают!");
                // Завершаем выполнение функции
                return;
            }

            // Создаём конструкцию оброботки исключений, что бы в случае чего можно было вывести ошибку пользователю
            try
            {
                // Формируем наш запрос для проверки существования логина
                string quary = $"SELECT count(*) FROM users WHERE login='{login}'";
                // Выполняем запрос
                string result = sqlite.QuaryStr(quary);
                //Проверяем есть ли совпадения
                if (Convert.ToInt32(result) > 0)
                {
                    //Выводим сообщение пользоватею и выходим из функции
                    sqlite.error("Пользователь с таким логином уже существует!");
                    return;
                }
                // Формируем новый запрос для записи его в бд
                quary = string.Format(
                    "INSERT INTO users (login, name, surname, patronymic, password, access) " +
                    "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', {5})",
                    login,
                    name,
                    surname,
                    patronymic,
                    sqlite.GetMD5Hash(password),//Получаем MD5-хеш пароля, что бы нельзя было узнать в бд пароль пользователя
                    1
                    );
                logger.Log("Регистрация пользователя в БД");
                // Выполняем запрос
                sqlite.Quary(quary);
                sqlite.success("Регистрация прошла успешно!");
            }
            // В случае если программа получила ошибку выводим это сообщение пользователю
            catch (Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }

        // Функция проверки полей на пусту
        private bool CheckingRegistrationField()
        {
            bool result = true;

            // Перебираем все контролы
            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                // Если тип TextBox
                if (ctrl.GetType() == typeof(TextBox))
                {
                    // Проверяем содержимое контрола
                    if (string.IsNullOrEmpty(ctrl.Text) || string.IsNullOrWhiteSpace(ctrl.Text))
                    {
                        result = false;
                    }
                }
            }
            // Возращаем результат
            return result;
        }

        // Оброботчик нажатия на кнопку регистрации
        private void button_reg_Click(object sender, EventArgs e)
        {
            // Выполняем функцию регистрации
            registration();
        }

        // Метод срабатывает при закрытии формы
        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Закрываем соединение с БД
            sqlite.Close();
        }

        // Функция авторизации
        private void authorization()
        {
            string login, password;
            // Временная переменная
            Dictionary<int, Dictionary<string, string>> temp;

            logger.Log("Считывание полей авторизации");
            // Считывание полей авторизации
            login = textBox_login.Text;
            password = sqlite.GetMD5Hash(textBox_password.Text);

            logger.Log("Проверка полей авторизации");
            // Проверка полей авторизации на пустоту
            if (!VerificationAuthorizationField())
            {
                sqlite.error("Не все поля заполнены!");
                return;
            }

            // Конструкция оброботки исключений
            try
            {
                // Формируем запрос для поиска пользователя в БД
                string quary = $"SELECT count(*) FROM users WHERE login='{login}' AND password='{password}'";
                // Выполняем запрос
                string result = sqlite.QuaryStr(quary);

                // Если данные пользователя совпадают
                if (Convert.ToInt32(result) > 0)
                {
                    // Формируем новый запрос для получения данных о пользователе
                    quary = $"SELECT * FROM users WHERE login='{login}' AND password='{password}'";
                    logger.Log("Получение данных о пользователе");
                    // Выполняем запрос
                    temp = sqlite.QuaryMas(quary);
                    // Отчищаем предыдущую информацию о пользователе
                    DataUser.Clear();
                    // Получем новую информацию о пользователе
                    DataUser = temp.Values.First();
                    logger.Log("Открытие формы для пользователя");
                    // Открываем форму в зависимости доступа пользователя
                    switch (Convert.ToInt32(DataUser["access"]))
                    {
                        case 1:
                            OpenFormUser();
                            break;
                        case 2:
                            OpenFormAdmin();
                            break;

                    }
                    // Выходим из функции
                    return;
                }
                // Если пароль неверный показываем ему надпись неверный логин или пароль
                label4.Visible = true;
            }
            // Выводим сообщение об ошибке пользователю
            catch (Exception ex)
            {
                sqlite.error(ex.Message);
            }
        }

        // Функция проверки полей авторизации на пустоту
        private bool VerificationAuthorizationField()
        {
            bool result = true;

            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                if (ctrl.GetType() == typeof(TextBox))
                {
                    if (string.IsNullOrEmpty(ctrl.Text) || string.IsNullOrWhiteSpace(ctrl.Text))
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        // Обработчик нажатия кнопки авторизации
        private void button_login_Click(object sender, EventArgs e)
        {
            authorization();
        }

        // Функция открытия формы пользователя
        private void OpenFormUser()
        {
            // Создаём форму
            FormUser FormUser = new FormUser(DataUser);
            // Если 2 форма закрыта то закрываем и форму входа
            FormUser.FormClosed += ((s, e) => { this.Close(); });
            // Показываем 2 форму
            FormUser.Show();
            // Скрываем форму входа
            this.Hide();
        }

        // Функция открытия формы админа
        private void OpenFormAdmin()
        {
            FormAdmin FormAdmin = new FormAdmin(DataUser);
            FormAdmin.FormClosed += ((s, e) => { this.Close(); });
            FormAdmin.Show();
            this.Hide();
        }
    }
}
