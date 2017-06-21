using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            MenuMain, menuFileFind, menuDirFind, menuAction, menuActionText,
            fileByName, fileBySize, fileByCreate, fileByAcсess, fileByModify, fileByText,
            dirByName, dirByCreate, dirByAcсess, dirByModify,
            dir, copy, move, delete, replaceText, changeDir, currentList,
            Exit
        };
        private MenuState State = MenuState.MenuMain;
        private DirectoryInfo currentDir;
        private FileSystemInfo[] currentList;
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
            switch (State)
            {
                case MenuState.MenuMain:
                    { currentMenu = menuMain; break; }
                case MenuState.menuFileFind:
                    { currentMenu = menuFileFind; break; }
                case MenuState.menuDirFind:
                    { currentMenu = menuDirFind; break; }
                /*case MenuState.menuActionText:
                    { currentMenu = menuAction; break; }*/
                case MenuState.menuAction:
                    {
                        currentMenu = menuAction;
                        //Array.Copy(menuAction, 0, currentMenu, 0, 4);
                        break;
                    }
                default:
                    { currentMenu = new string[0]; break; }
            }
            System.Collections.IEnumerator enumer = currentMenu.GetEnumerator();
            while (enumer.MoveNext())
            {
                Console.WriteLine(enumer.Current);
            }
            if (currentMenu.Equals(menuMain))
                Console.WriteLine(menuExit[0]);
            else
                Console.WriteLine(menuExit[1]);
        }
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
                        switch(Select(7))
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
								{ State = MenuState.menuAction;	break;}
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
                                {
                                    //ClearList();
                                    State = MenuState.MenuMain;
                                    break;
                                }
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
                                {
                                    //ClearList();
                                    State = MenuState.MenuMain;
                                    break;
                                }
                        }
                        break;
                    }
                case MenuState.menuActionText:
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
                                {
                                    //ClearList();
                                    State = MenuState.MenuMain;
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Метод отображает текущее меню запрашивает выбор пользователя
        /// и вызывает соответствующие методы в зависимости от текущего состояния State
        /// если выбран пункт конкретного действия, а не перехода в другое меню
        /// </summary>
        public void StartMenu()
        {
        	ClearList();
            while (State != MenuState.Exit)
            {
                ShowMenu();
                GetSelect();
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
                            CopyList();
                            DeleteList();
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
                        { ShowCurrentList();
							State = MenuState.MenuMain;
							break; }
                }
            }
        }
        private void ClearList()
        {
			try{
				currentList = currentDir.GetFileSystemInfos();
			} catch (DirectoryNotFoundException e) {
				Console.WriteLine("\n" + e.Message);
				currentDir = new DirectoryInfo(@".\");
				currentList = currentDir.GetFileSystemInfos();
				State = MenuState.MenuMain;
			}
        }
        private void ReplaceText()
        {
            throw new NotImplementedException();
        }

        private void FileByText()
        {
            throw new NotImplementedException();
        }

        private void FileBySize()
        {
            throw new NotImplementedException();
        }

        private void FileByName()
        {
            throw new NotImplementedException();
        }

        private void FileByModify()
        {
            throw new NotImplementedException();
        }

        private void FileByCreate()
        {
            throw new NotImplementedException();
        }

        private void FileByAccess()
        {
            throw new NotImplementedException();
        }

        private void DirByName()
        {
            throw new NotImplementedException();
        }

        private void DirByModify()
        {
            throw new NotImplementedException();
        }

        private void DirByCreate()
        {
            throw new NotImplementedException();
        }

        private void DirByAccess()
        {
            throw new NotImplementedException();
        }

        private void DeleteList()
        {
            throw new NotImplementedException();
        }

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
			var dirs = new List<DirectoryInfo>();
			var files = new List<FileInfo>();
			foreach(FileSystemInfo info in currentList)
			{
				if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
					dirs.Add(new DirectoryInfo(info.FullName));
				else
					files.Add(new FileInfo(info.FullName));
			}
			//Копируем файлы
	        foreach (FileInfo file in files)
        	{
            	string temppath = Path.Combine(newPath, file.Name);
            	try{
            	file.CopyTo(temppath, false);
            	}catch(IOException)
            	{ Console.WriteLine("Файл " + file.Name + " уже есть впапке назначения и не будет скопирован");}
            	
        	}
	        //Если есть подпапки копируем их
			if (dirs.Count>0)
        	{
	            foreach (DirectoryInfo subdir in dirs)
    	        {
    	            string temppath = Path.Combine(newPath, subdir.Name);
    	            DirectoryCopy(subdir.FullName, temppath);
    	        }
    	    }
			State = MenuState.MenuMain;
        }
		
        private void DirectoryCopy(string sourceDirName, string destDirName)
        {
        	DirectoryInfo dir = new DirectoryInfo(sourceDirName);

	        DirectoryInfo[] dirs = dir.GetDirectories();
        	if (!Directory.Exists(destDirName))
        	{
            	Directory.CreateDirectory(destDirName);
        	}

	        FileInfo[] files = dir.GetFiles();
    	    foreach (FileInfo file in files)
    	    {
    	        string temppath = Path.Combine(destDirName, file.Name);
    	        try{
    	        	file.CopyTo(temppath, false);
    	        }catch(IOException)
            	{ Console.WriteLine("Файл " + file.Name + " уже есть впапке назначения и не будет скопирован");}
    	    }

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }
        private void ShowCurrentList()
        {
            foreach(FileSystemInfo info in currentList)
            {
            	if((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                	Console.WriteLine("->[DIR] {0}",info.Name);
            }
            foreach(FileSystemInfo info in currentList)
            {
            	if((info.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
                	Console.WriteLine("->     {0}",info.Name);
            }
        }

        private void ReadDir()
        {
			ClearList();
			Console.WriteLine("Текущая папка: " + currentDir.FullName);
            State = MenuState.MenuMain;
        }
        
        private void ChangeDir()
        {
			Console.Write("Введите новую папку: ");
			try{
				var newDir = new DirectoryInfo(Console.ReadLine());
				newDir.Refresh();
				currentDir = newDir;
				Directory.SetCurrentDirectory(currentDir.FullName);
			} catch (IOException e) {
				Console.WriteLine("\n" + e.Message);
			}
			Console.WriteLine("Текущая папка изменена на " + currentDir.FullName);
			State = MenuState.MenuMain;
        }
    }
}
