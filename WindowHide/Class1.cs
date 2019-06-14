using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.IO;
using System.Diagnostics;

namespace WindowHide
{
	class Hide
	{
		public static bool exit = false;
		public static IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;

		public static WindowHider hider;
		public static TrayMgr tray;

		static void Main()
		{
			if (WindowFuncs.AlreadyOpen())
				return;

			WindowFuncs.HideWindow(hWnd);

			tray = new TrayMgr();
			var trayThread = new Thread(tray.StartTray);
			trayThread.Start();

			hider = new WindowHider(tray);
			var windowThread = new Thread(hider.BeginHiding);
			windowThread.Start();

			windowThread.Join();
			trayThread.Join();
		}
	}
}
