using System;
using SettingsHelper;
using System.IO;

namespace DZ_7_3
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryStream ms = new MemoryStream();
            //Записываем настройки консоли в поток
            ApplicationSettingsHelper.SaveCurrentSettings(ms);
            ConsoleSettings set;
            ms.Seek(0, SeekOrigin.Begin);
            //Загружаем из потока настройки консоли в структуру
            set = ApplicationSettingsHelper.LoadSettings(ms);
            //Показываем загруженные настройки
            Console.WriteLine(set);
            //Меняем настройки в структуре
            set.BackgroundColor = ConsoleColor.DarkGreen;
            set.CursorSize = 50;
            set.ForegroundColor = ConsoleColor.Cyan;
            set.Title = "Console settings application";
            set.WindowHeight = 30;
            set.WindowWidth = 60;
            ms.Close();
            ms = new MemoryStream();
            //Записываем в поток настройки из структуры
            ApplicationSettingsHelper.SaveSettings(ms, set);
            ms.Seek(0, SeekOrigin.Begin);
            //Загружаем из потока новые настройки консоли
            ApplicationSettingsHelper.LoadCurrentSettings(ms);
            //Показываем новые настройки консоли
            Console.WriteLine(set);
            Console.ReadKey();
        }
    }
}
