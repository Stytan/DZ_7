using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleExplorer
{
    class Explorer
    {
        static readonly string[] menuMain =
        {
            "\nГлавное меню",
            "1. Показать содержимое текущей папки",
            "2. Найти файл по имени, размеру, дате...",
            "3. Найти папку по имени, дате...",
            "4. Найти текстовый файл по содержимому",
            "5. Сменить папку...",
            "6. Показать текущий выбранный список",
            "7. Меню действий..."
        };
        static readonly string[] menuFileFind =
        {
            "\nМеню поиска файла. Найти файл по:",
            "1. имени",
            "2. размеру",
            "3. дате создания",
            "4. дате доступа",
            "5. дате модификации"
        };
        static readonly string[] menuDirFind =
        {
            "\nМеню поиска директории. Найти директорию по:",
            "1. имени",
            "2. дате создания",
            "3. дате доступа",
            "4. дате модификации"
        };
        static readonly string[] menuAction =
        {
            "\nЧто нужно сделать с найденным списком?",
            "1. скопировать",
            "2. переместить",
            "3. удалить",
            "4. заменить подстроку в текстовых файлах"
        };
        static readonly string[] menuExit =
        {
            "0. Выход",
            "0. Назад"
        };
        private enum MenuState
        {
            MenuMain, menuFileFind, menuDirFind, menuAction,
            fileByName, fileBySize, fileByCreate, fileByAcсess, fileByModify, fileByText,
            dirByName, dirByCreate, dirByAcсess, dirByModify,
            dir, copy, move, delete, replaceText, changeDir, currentList,
            Exit
        };
        //Хранит текущее состояние
        private MenuState State = MenuState.MenuMain;

        private DirectoryInfo currentDir; //Текущая папка

        private FileSystemInfo[] currentList; //Список выбранных файлов и папок

        public Explorer()
        {
            currentDir = new DirectoryInfo("./");
        }
        /// <summary>
        /// Выводит на экран текущее меню
        /// </summary>
        public void ShowMenu()
        {
            string[] currentMenu;
            //Показать меню соответствующее настоящему состоянию State
            switch (State)
            {
                case MenuState.MenuMain:
                    { currentMenu = menuMain; break; }
                case MenuState.menuFileFind:
                    { currentMenu = menuFileFind; break; }
                case MenuState.menuDirFind:
                    { currentMenu = menuDirFind; break; }
                case MenuState.menuAction:
                    { currentMenu = menuAction; break; }
                default:
                    { currentMenu = new string[0]; break; }
            }
            System.Collections.IEnumerator enumer = currentMenu.GetEnumerator();
            while (enumer.MoveNext())
            {
                Console.WriteLine(enumer.Current);
            }
            //Если мы в главном меню показываем Выход иначе Назад
            if (currentMenu.Equals(menuMain))
                Console.WriteLine(menuExit[0]);
            else
                Console.WriteLine(menuExit[1]);
        }
        /// <summary>
        /// Запрос выбора пункта меню
        /// </summary>
        /// <param name="N">Количество пунктов в меню</param>
        /// <returns>Выбранный пользователем пункт</returns>
        private int Select(int N)
        {
            int res = -1;
            do
            {
                Console.Write("Сделайте ваш выбор: ");
                try
                {
                    res = Convert.ToInt32(Console.ReadKey(false).KeyChar.ToString());
                    if (res < 0 || res > N) throw new OverflowException();
                    Console.WriteLine();
                }
                catch (Exception)
                {
                    Console.WriteLine("\nНе верный выбор. Попробуйте снова.");
                }
            } while (res < 0 || res > N);
            return res;
        }
        /// <summary>
        /// Запрашивает выбор пользователя и соответственно меняет статус State
        /// </summary>
        public void GetSelect()
        {
            switch (State)
            {
                case MenuState.MenuMain:
                    {
                        switch (Select(7))
                        {
                            case 1:
                                { State = MenuState.dir; break; }
                            case 2:
                                { State = MenuState.menuFileFind; break; }
                            case 3:
                                { State = MenuState.menuDirFind; break; }
                            case 4:
                                { State = MenuState.fileByText; break; }
                            case 5:
                                { State = MenuState.changeDir; break; }
                            case 6:
                                { State = MenuState.currentList; break; }
                            case 7:
                                { State = MenuState.menuAction; break; }
                            case 0:
                                { State = MenuState.Exit; break; }
                        }
                        break;
                    }
                case MenuState.menuFileFind:
                    {
                        switch (Select(5))
                        {
                            case 1:
                                { State = MenuState.fileByName; break; }
                            case 2:
                                { State = MenuState.fileBySize; break; }
                            case 3:
                                { State = MenuState.fileByCreate; break; }
                            case 4:
                                { State = MenuState.fileByAcсess; break; }
                            case 5:
                                { State = MenuState.fileByModify; break; }
                            case 0:
                                { State = MenuState.MenuMain; break; }
                        }
                        break;
                    }
                case MenuState.menuDirFind:
                    {
                        switch (Select(4))
                        {
                            case 1:
                                { State = MenuState.dirByName; break; }
                            case 2:
                                { State = MenuState.dirByCreate; break; }
                            case 3:
                                { State = MenuState.dirByAcсess; break; }
                            case 4:
                                { State = MenuState.dirByModify; break; }
                            case 0:
                                { State = MenuState.MenuMain; break; }
                        }
                        break;
                    }
                case MenuState.menuAction:
                    {
                        switch (Select(4))
                        {
                            case 1:
                                { State = MenuState.copy; break; }
                            case 2:
                                { State = MenuState.move; break; }
                            case 3:
                                { State = MenuState.delete; break; }
                            case 4:
                                { State = MenuState.replaceText; break; }
                            case 0:
                                { State = MenuState.MenuMain; break; }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Метод отображает текущее меню запрашивает выбор пользователя
        /// и вызывает соответствующие методы в зависимости от текущего состояния State
        /// </summary>
        public void StartMenu()
        {
            ClearList();
            while (State != MenuState.Exit)
            {
                ShowMenu(); //Показали текущее меню
                GetSelect(); //Запросили выбор у пользователя
                switch (State)
                {
                    case MenuState.dir:
                        {
                            ReadDir();
                            ShowCurrentList();
                            break;
                        }
                    case MenuState.copy:
                        {
                            CopyList();
                            ClearList();
                            break;
                        }
                    case MenuState.delete:
                        {
                            DeleteList();
                            ClearList();
                            break;
                        }
                    case MenuState.move:
                        {
                            MoveList();
                            ClearList();
                            break;
                        }
                    case MenuState.dirByAcсess:
                        { DirByAccess(); break; }
                    case MenuState.dirByCreate:
                        { DirByCreate(); break; }
                    case MenuState.dirByModify:
                        { DirByModify(); break; }
                    case MenuState.dirByName:
                        { DirByName(); break; }
                    case MenuState.fileByAcсess:
                        { FileByAccess(); break; }
                    case MenuState.fileByCreate:
                        { FileByCreate(); break; }
                    case MenuState.fileByModify:
                        { FileByModify(); break; }
                    case MenuState.fileByName:
                        { FileByName(); break; }
                    case MenuState.fileBySize:
                        { FileBySize(); break; }
                    case MenuState.fileByText:
                        { FileByText(); break; }
                    case MenuState.replaceText:
                        { ReplaceText(); break; }
                    case MenuState.changeDir:
                        { ChangeDir(); break; }
                    case MenuState.currentList:
                        {
                            ShowCurrentList();
                            State = MenuState.MenuMain;
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Очистить текущий список выбора, заполняем его содержимым текущей папки
        /// </summary>
        private void ClearList()
        {
            try
            {
                currentList = currentDir.GetFileSystemInfos();
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("\n" + e.Message);
                currentDir = new DirectoryInfo(@".\");
                currentList = currentDir.GetFileSystemInfos();
                State = MenuState.MenuMain;
            }
        }
        /// <summary>
        /// Заменяет текст в файлах
        /// </summary>
        private void ReplaceText()
        {
            Console.Write("Введите искомый текст для замены в файлах: ");
            string searchText = Console.ReadLine();
            Console.Write("Введите новый текст для замены в файлах: ");
            string replaceText = Console.ReadLine();
            var tempList = new List<FileSystemInfo>();
            //Перебираем все файлы из текущей папки
            foreach (var file in currentDir.GetFiles())
            {
                string str = File.ReadAllText(file.FullName, Encoding.UTF8);
                //Если найдено совпадение заменяем и перезаписываем файл
                if (str.Contains(searchText))
                {
                    str = str.Replace(searchText, replaceText);
                    File.WriteAllText(file.FullName, str);
                    //Запоминаем изменённый файл во временном списке
                    tempList.Add(file);
                }
            }
            //Если были замены - показываем в каких файлах
            if (tempList.Any())
            {
                currentList = tempList.ToArray<FileSystemInfo>();
                Console.WriteLine("Текст заменён в следующих файлах: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            //Возвращаемся в главное меню
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Поиск файлов по содержимому
        /// </summary>
        private void FileByText()
        {
            Console.Write("Введите текст для поиска файлов по содержимому: ");
            string text = Console.ReadLine();
            var tempList = new List<FileSystemInfo>();
            foreach (var file in currentDir.GetFiles())
            {
                string str = File.ReadAllText(file.FullName, Encoding.Default);
                if (str.Contains(text))
                    tempList.Add(file);
            }
            if (tempList.Any())
            {
                currentList = tempList.ToArray<FileSystemInfo>();
                Console.WriteLine("Найдены следующие файлы: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Поиск файлов по размеру
        /// </summary>
        private void FileBySize()
        {
            Console.Write("Введите размер для поиска файла (Mb): ");
            long size;
            do
            {
                try
                {
                    size = Convert.ToInt64(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                { Console.Write("Неверный формат числа. Попробуйте снова: "); }
            } while (true);
            var tempList = new List<FileSystemInfo>();
            foreach (FileInfo info in currentDir.GetFiles())
            {
                if (info.Length / 1024 / 1024 == size)
                    tempList.Add(info);
            }
            if (tempList.Any())
            {
                currentList = tempList.ToArray<FileSystemInfo>();
                Console.WriteLine("Найдены следующие файлы: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Поиск файлов по имени
        /// </summary>
        private void FileByName()
        {
            Console.Write("Введите имя для поиска файла: ");
            string name = Console.ReadLine();
            var tmp = currentDir.GetFiles(name);
            if (tmp.Any())
            {
                currentList = tmp;
                Console.WriteLine("Найдены следующие файлы: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Поиск файлов по дате изменения
        /// </summary>
        private void FileByModify()
        {
            Console.Write("Введите дату последнего изменения для поиска файла: ");
            DateTime date = GetData();
            var tempList = new List<FileSystemInfo>();
            foreach (FileInfo info in currentDir.GetFiles())
            {
                if (info.LastWriteTime.Date == date)
                    tempList.Add(info);
            }
            if (tempList.Any())
            {
                currentList = tempList.ToArray<FileSystemInfo>();
                Console.WriteLine("Найдены следующие файлы: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Поиск файлов по дате создания
        /// </summary>
        private void FileByCreate()
        {
            Console.Write("Введите дату создания для поиска файла: ");
            DateTime date = GetData();
            var tempList = new List<FileSystemInfo>();
            foreach (FileInfo info in currentDir.GetFiles())
            {
                if (info.CreationTime.Date == date)
                    tempList.Add(info);
            }
            if (tempList.Any())
            {
                currentList = tempList.ToArray<FileSystemInfo>();
                Console.WriteLine("Найдены следующие файлы: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Посик файлов по дате доступа
        /// </summary>
        private void FileByAccess()
        {
            Console.Write("Введите дату последнего доступа для поиска файла: ");
            DateTime date = GetData();
            var tempList = new List<FileSystemInfo>();
            foreach (FileInfo info in currentDir.GetFiles())
            {
                if (info.LastAccessTime.Date == date)
                    tempList.Add(info);
            }
            if (tempList.Any())
            {
                currentList = tempList.ToArray<FileSystemInfo>();
                Console.WriteLine("Найдены следующие файлы: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Поиск папок по имени
        /// </summary>
        private void DirByName()
        {
            Console.Write("Введите имя для поиска папки: ");
            string name = Console.ReadLine();
            var tmp = currentDir.GetDirectories(name);
            if (tmp.Any())
            {
                currentList = tmp;
                Console.WriteLine("Найдены следующие папки: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Поиск папок по дате изменения
        /// </summary>
        private void DirByModify()
        {
            Console.Write("Введите дату последнего изменения для поиска папки: ");
            DateTime date = GetData();
            var tempList = new List<FileSystemInfo>();
            foreach (DirectoryInfo info in currentDir.GetDirectories())
            {
                if (info.LastWriteTime.Date == date)
                    tempList.Add(info);
            }
            if (tempList.Any())
            {
                currentList = tempList.ToArray<FileSystemInfo>();
                Console.WriteLine("Найдены следующие папки: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Поиск папок по дате создания
        /// </summary>
        private void DirByCreate()
        {
            Console.Write("Введите дату создания для поиска папки: ");
            DateTime date = GetData();
            var tempList = new List<FileSystemInfo>();
            foreach (DirectoryInfo info in currentDir.GetDirectories())
            {
                if (info.CreationTime.Date == date)
                    tempList.Add(info);
            }
            if (tempList.Any())
            {
                currentList = tempList.ToArray<FileSystemInfo>();
                Console.WriteLine("Найдены следующие папки: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Поиск папок по дате доступа
        /// </summary>
        private void DirByAccess()
        {
            Console.Write("Введите дату последнего доступа для поиска папки: ");
            DateTime date = GetData();
            var tempList = new List<FileSystemInfo>();
            foreach (DirectoryInfo info in currentDir.GetDirectories())
            {
                if (info.LastAccessTime.Date == date)
                    tempList.Add(info);
            }
            if (tempList.Any())
            {
                currentList = tempList.ToArray<FileSystemInfo>();
                Console.WriteLine("Найдены следующие папки: ");
                ShowCurrentList();
            }
            else
            {
                Console.WriteLine("Ничего не найдено.");
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Вспомогательная функция для запроса даты у пользователя 
        /// </summary>
        /// <returns>дата введённая пользователем</returns>
        private DateTime GetData()
        {
            DateTime date;
            do
            {
                try
                {
                    date = Convert.ToDateTime(Console.ReadLine());
                    break;
                }
                catch (FormatException)
                { Console.Write("Неверный формат даты. Попробуйте снова: "); }
            } while (true);
            return date;
        }
        /// <summary>
        /// Перемещает файлы и папки из текущего списка в указанную папку
        /// </summary>
        private void MoveList()
        {
            Console.Write("Введите путь куда нужно переместить выбранные файлы: ");
            string newPath = Console.ReadLine();
            //Создаём новую папку если её нет
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            List<DirectoryInfo> dirs;
            List<FileInfo> files;
            GetLists(out dirs, out files);
            foreach (DirectoryInfo info in dirs)
            {
                try
                {
                    info.MoveTo(Path.Combine(newPath, info.Name));
                    Console.WriteLine("Папка " + info.Name + " перемещена");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Папка " + info.Name + " не перемещена.\nВозникла ошибка: " + e.Message);
                }
            }
            foreach (FileInfo info in files)
            {
                try
                {
                    info.MoveTo(Path.Combine(newPath, info.Name));
                    Console.WriteLine("Файл " + info.Name + " перемещен");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Файл " + info.Name + " не перемещен.\nВозникла ошибка: " + e.Message);
                }
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Выбирает из текущего списка папки и файлы
        /// </summary>
        /// <param name="dirs">Список выбранных папок</param>
        /// <param name="files">Список выбранных файлов</param>
        private void GetLists(out List<DirectoryInfo> dirs, out List<FileInfo> files)
        {
            dirs = new List<DirectoryInfo>();
            files = new List<FileInfo>();
            foreach (FileSystemInfo info in currentList)
            {
                if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    dirs.Add(new DirectoryInfo(info.FullName));
                else
                    files.Add(new FileInfo(info.FullName));
            }
        }
        /// <summary>
        /// Удаляет файлы и папки из выбранного списка
        /// </summary>
        private void DeleteList()
        {
            Console.Write("Вы уверены что хотите удалить выбранные файлы и папки? (y) ");
            if (Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                List<DirectoryInfo> dirs;
                List<FileInfo> files;
                GetLists(out dirs, out files);
                foreach (DirectoryInfo info in dirs)
                {
                    try
                    {
                        info.Delete(true);
                        Console.WriteLine("Папка " + info.Name + " удалена");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Папка " + info.Name + " не удалена.\nВозникла ошибка: " + e.Message);
                    }
                }
                foreach (FileInfo info in files)
                {
                    try
                    {
                        info.Delete();
                        Console.WriteLine("Файл " + info.Name + " удален");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Файл " + info.Name + " не удален.\nВозникла ошибка: " + e.Message);
                    }
                }
            }
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Копирует файлы и папки из выбранного списка
        /// </summary>
        private void CopyList()
        {
            Console.Write("Введите путь куда нужно скопировать выбранные файлы: ");
            string newPath = Console.ReadLine();
            //Создаём новую папку если её нет
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            //Выбираем все папки и файлы из текущего списка
            List<DirectoryInfo> dirs;
            List<FileInfo> files;
            GetLists(out dirs, out files);
            //Копируем файлы
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(newPath, file.Name);
                try
                {
                    file.CopyTo(temppath, false);
                }
                catch (IOException)
                {
                    Console.WriteLine("Файл " + file.Name + " уже есть впапке назначения и не будет скопирован");
                }
            }
            //Если есть подпапки копируем их
            if (dirs.Any())
                foreach (DirectoryInfo subdir in dirs)
                    DirectoryCopy(subdir.FullName, Path.Combine(newPath, subdir.Name));
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Вспомогательная функция для копирования папок
        /// </summary>
        private void DirectoryCopy(string sourceDirName, string destDirName)
        {
            var dir = new DirectoryInfo(sourceDirName);
            //Создаём папку если её нет
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);
            //Копируем файлы
            foreach (FileInfo file in dir.GetFiles())
            {
                try
                {
                    file.CopyTo(Path.Combine(destDirName, file.Name), false);
                }
                catch (IOException)
                {
                    Console.WriteLine("Файл " + file.Name + " уже есть впапке назначения и не будет скопирован");
                }
            }
            //Копируем подпапки
            foreach (DirectoryInfo subdir in dir.GetDirectories())
                DirectoryCopy(subdir.FullName, Path.Combine(destDirName, subdir.Name));
        }
        /// <summary>
        /// Показывает список выбранных папок и файлов
        /// </summary>
        private void ShowCurrentList()
        {
            //Показываем папки
            foreach (FileSystemInfo info in currentList)
                if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    Console.WriteLine("->[DIR] {0}", info.Name);
            //Показываем файлы
            foreach (FileSystemInfo info in currentList)
                if ((info.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
                    Console.WriteLine("->     {0}", info.Name);
        }
        /// <summary>
        /// Читает в список содержимое текущей папки
        /// </summary>
        private void ReadDir()
        {
            ClearList();
            Console.WriteLine("Текущая папка: " + currentDir.FullName);
            State = MenuState.MenuMain;
        }
        /// <summary>
        /// Переходит в указанную пользователем папку
        /// </summary>
        private void ChangeDir()
        {
            Console.Write("Введите новую папку: ");
            try
            {
                var newDir = new DirectoryInfo(Console.ReadLine());
                if (newDir.Exists)
                {
                    Directory.SetCurrentDirectory(currentDir.FullName);
                    currentDir = newDir;
                }
                else
                    Console.WriteLine("Заданный путь не найден.");
            }
            catch (IOException e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            Console.WriteLine("Текущая папка изменена на " + currentDir.FullName);
            State = MenuState.MenuMain;
        }
    }
}
