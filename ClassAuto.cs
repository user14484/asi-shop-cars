using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АИС_Автосалон
{
    class ClassAuto
    {
        public ClassAuto(bool g_check, int g_id, string g_марка, string g_модель, string g_тип, string g_комплектация, string g_цвет,
            string g_двигатель, string g_страна, int g_год, string g_типКПП, string g_типПривода, string g_цена, string g_вес)
        {
            id = g_id;
            Check = g_check;
            марка = g_марка;
            модель = g_модель;
            тип = g_тип;
            комплектация = g_комплектация;
            цвет = g_цвет;
            двигатель = g_двигатель;
            страна = g_страна;
            год = g_год;
            типКПП = g_типКПП;
            типПривода = g_типПривода;
            цена = g_цена;
            вес = g_вес;
        }

        [System.ComponentModel.DisplayName("id")]
        public int id { get; set; }
        [System.ComponentModel.DisplayName("Выбрать")]
        public bool Check { get; set; }
        [System.ComponentModel.DisplayName("Марка")]
        public string марка { get; set; }
        [System.ComponentModel.DisplayName("Модель")]
        public string модель { get; set; }
        [System.ComponentModel.DisplayName("Тип")]
        public string тип { get; set; }
        [System.ComponentModel.DisplayName("Комплектация")]
        public string комплектация { get; set; }
        [System.ComponentModel.DisplayName("Цвет")]
        public string цвет { get; set; }
        [System.ComponentModel.DisplayName("Двигатель")]
        public string двигатель { get; set; }
        [System.ComponentModel.DisplayName("Страна")]
        public string страна { get; set; }
        [System.ComponentModel.DisplayName("Год")]
        public int год { get; set; }
        [System.ComponentModel.DisplayName("Тип КПП")]
        public string типКПП { get; set; }
        [System.ComponentModel.DisplayName("Тип привода")]
        public string типПривода { get; set; }
        [System.ComponentModel.DisplayName("Цена")]
        public string цена { get; set; }
        [System.ComponentModel.DisplayName("Вес")]
        public string вес { get; set; }
    }
}
