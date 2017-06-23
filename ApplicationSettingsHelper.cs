using System;
using System.IO;
using System.Text;

namespace SettingsHelper
{
	/// <summary>
	/// Description of ApplicationSettingsHelper.
	/// </summary>
	public class ApplicationSettingsHelper
	{
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
		}
		public static void SaveCurrentSettings(Stream ostream)
		{
			ConsoleSettings set;
			set.BackgroundColor = Console.BackgroundColor;
			set.ForegroundColor = Console.ForegroundColor;
			set.BufferHeight = Console.BufferHeight;
			set.BufferWidth = Console.BufferWidth;
			set.CursorSize = Console.CursorSize;
			set.CursorVisible = Console.CursorVisible;
			set.InputEncoding = Console.InputEncoding;
			set.OutputEncoding = Console.OutputEncoding;
			set.Title = Console.Title;
			set.WindowHeight = Console.WindowHeight;
			set.WindowLeft = Console.WindowLeft;
			set.WindowTop = Console.WindowTop;
			set.WindowWidth = Console.WindowWidth;
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
			bw.Dispose();
			bw.Close();
		}
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
			bw.Dispose();
			bw.Close();
		}
		public static void LoadCurrentSettings(Stream istream)
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
			Console.BackgroundColor = set.BackgroundColor;
			Console.ForegroundColor = set.ForegroundColor;
			Console.BufferHeight = set.BufferHeight;
			Console.BufferWidth = set.BufferWidth;
			Console.CursorSize = set.CursorSize;
			Console.CursorVisible = set.CursorVisible;
			Console.InputEncoding = set.InputEncoding;
			Console.OutputEncoding = set.OutputEncoding;
			Console.Title = set.Title;
			Console.WindowHeight = set.WindowHeight;
			Console.WindowLeft = set.WindowLeft;
			Console.WindowTop = set.WindowTop;
			Console.WindowWidth = set.WindowWidth;
			br.Dispose();
			br.Close();
		}
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
