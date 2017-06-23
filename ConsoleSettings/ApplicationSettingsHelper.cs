using System;
using System.IO;
using System.Text;

namespace SettingsHelper
{
    /// <summary>
    /// Структура хранящая настройки консоли
    /// </summary>
    public struct ConsoleSettings
    {
        public ConsoleColor BackgroundColor;
        public ConsoleColor ForegroundColor;
        public int BufferHeight;
        public int BufferWidth;
        public int CursorSize;
        public bool CursorVisible;
        public Encoding InputEncoding;
        public Encoding OutputEncoding;
        public string Title;
        public int WindowHeight;
        public int WindowLeft;
        public int WindowTop;
        public int WindowWidth;
        public override string ToString()
        {
            return string.Format(
                "BackgroundColor = {0}, ForegroundColor = {1}, BufferHeight = {2}, "
                + "BufferWidth = {3}, CursorSize = {4}, CursorVisible = {5}, "
                + "InputEncoding = {6}, OutputEncoding = {7}, Title = {8}, "
                + "WindowHeight = {9}, WindowLeft = {10}, WindowTop = {11}, WindowWidth = {12}",
                BackgroundColor, ForegroundColor, BufferHeight, BufferWidth,
                CursorSize, CursorVisible, InputEncoding, OutputEncoding,
                Title, WindowHeight, WindowLeft, WindowTop, WindowWidth);
        }
    }
    /// <summary>
    /// Класс позволяющий сохранять и загружать настройки консоли в потоке
    /// </summary>
    public class ApplicationSettingsHelper
	{
        /// <summary>
        /// Сохраняет текущие настройки консоли в поток
        /// </summary>
        /// <param name="ostream">Поток в который запишутся настройки</param>
		public static void SaveCurrentSettings(Stream ostream)
		{
			var bw = new BinaryWriter(ostream);
			bw.Write((int)Console.BackgroundColor);
			bw.Write((int)Console.ForegroundColor);
			bw.Write(Console.BufferHeight);
			bw.Write(Console.BufferWidth);
			bw.Write(Console.CursorSize);
			bw.Write(Console.CursorVisible);
			bw.Write(Console.InputEncoding.CodePage);
			bw.Write(Console.OutputEncoding.CodePage);
			bw.Write(Console.Title);
			bw.Write(Console.WindowHeight);
			bw.Write(Console.WindowLeft);
			bw.Write(Console.WindowTop);
			bw.Write(Console.WindowWidth);
			bw.Flush();
		}
        /// <summary>
        /// Записывает в поток настройки из структуры
        /// </summary>
        /// <param name="ostream">Поток для записи настроек</param>
        /// <param name="set">Структура хранящая настройки для сохранения</param>
		public static void SaveSettings(Stream ostream, ConsoleSettings set)
		{
			var bw = new BinaryWriter(ostream);
			bw.Write((int)set.BackgroundColor);
			bw.Write((int)set.ForegroundColor);
			bw.Write(set.BufferHeight);
			bw.Write(set.BufferWidth);
			bw.Write(set.CursorSize);
			bw.Write(set.CursorVisible);
			bw.Write(set.InputEncoding.CodePage);
			bw.Write(set.OutputEncoding.CodePage);
			bw.Write(set.Title);
			bw.Write(set.WindowHeight);
			bw.Write(set.WindowLeft);
			bw.Write(set.WindowTop);
			bw.Write(set.WindowWidth);
			bw.Flush();
		}
        /// <summary>
        /// Загружает настройки консоли из потока
        /// </summary>
        /// <param name="istream">Поток из которого загружаются насройки консоли</param>
		public static void LoadCurrentSettings(Stream istream)
		{
			var br = new BinaryReader(istream);
			Console.BackgroundColor = (ConsoleColor)br.ReadInt32();
            Console.ForegroundColor = (ConsoleColor)br.ReadInt32();
            Console.BufferHeight = br.ReadInt32();
            Console.BufferWidth = br.ReadInt32();
            Console.CursorSize = br.ReadInt32();
            Console.CursorVisible = br.ReadBoolean();
            Console.InputEncoding = Encoding.GetEncoding(br.ReadInt32());
            Console.OutputEncoding = Encoding.GetEncoding(br.ReadInt32());
            Console.Title = br.ReadString();
            Console.WindowHeight = br.ReadInt32();
            Console.WindowLeft = br.ReadInt32();
            Console.WindowTop = br.ReadInt32();
            Console.WindowWidth = br.ReadInt32();
            br.Dispose();
			br.Close();
		}
        /// <summary>
        /// Загружает настройки консоли из потока и возвращает их в виде структуры настроек
        /// </summary>
        /// <param name="istream">Поток из которого нужно загрузить настройки</param>
        /// <returns>Возвращает структуру ConsoleSettings содержащую загруженные настройки</returns>
		public static ConsoleSettings LoadSettings(Stream istream)
		{
			var br = new BinaryReader(istream);
			ConsoleSettings set;
			set.BackgroundColor = (ConsoleColor) br.ReadInt32();
			set.ForegroundColor = (ConsoleColor) br.ReadInt32();
			set.BufferHeight = br.ReadInt32();
			set.BufferWidth = br.ReadInt32();
			set.CursorSize = br.ReadInt32();
			set.CursorVisible = br.ReadBoolean();
			set.InputEncoding = Encoding.GetEncoding(br.ReadInt32());
			set.OutputEncoding = Encoding.GetEncoding(br.ReadInt32());
			set.Title = br.ReadString();
			set.WindowHeight = br.ReadInt32();
			set.WindowLeft = br.ReadInt32();
			set.WindowTop = br.ReadInt32();
			set.WindowWidth = br.ReadInt32();
			br.Dispose();
			br.Close();
			return set;
		}
	}
}
