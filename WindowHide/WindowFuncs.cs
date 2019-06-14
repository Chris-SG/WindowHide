using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WindowHide
{
	static class WindowFuncs
	{
		private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

		[DllImport("User32", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern int ShowWindowAsync(IntPtr hwnd, int nCmdShow);

		[DllImport("USER32.DLL")]
		private static extern IntPtr GetShellWindow();

		[DllImport("USER32.DLL")]
		private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("USER32.DLL")]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("USER32.DLL")]
		private static extern int GetWindowTextLength(IntPtr hWnd);

		private const int SW_HIDE = 0;
		private const int SW_MAX = 2;
        private const int SW_MINIMIZE = 6;
		private const int SW_SHOW = 1;

		public static void ShowWindow(IntPtr hWnd)
		{
            if(!IsWindowVisible(hWnd))
            {
                ShowWindowAsync(hWnd, SW_SHOW);
                Console.WriteLine(DateTime.Now.ToString("'['yyyy'-'MM'-'dd'T'HH':'mm':'ss']'") + " Window shown: " + GetWindowTitle(hWnd) + " (" + hWnd + ")");
            }
		}

        public static void MinimizeWindow(IntPtr hWnd)
        {
            if(!IsWindowVisible(hWnd))
            {
                ShowWindow(hWnd);
            }
            if(!IsIconic(hWnd))
            {
                ShowWindowAsync(hWnd, SW_MINIMIZE);
                Console.WriteLine(DateTime.Now.ToString("'['yyyy'-'MM'-'dd'T'HH':'mm':'ss']'") + " Window minimized: " + GetWindowTitle(hWnd) + " (" + hWnd + ")");
            }
        }

		public static void HideWindow(IntPtr hWnd)
		{
			if(IsWindowVisible(hWnd))
			{
				ShowWindowAsync(hWnd, SW_HIDE);
				Console.WriteLine(DateTime.Now.ToString("'['yyyy'-'MM'-'dd'T'HH':'mm':'ss']'") + " Window hidden: " + GetWindowTitle(hWnd) + " (" + hWnd + ")");
			}
		}

		public static void ToggleVisibility(IntPtr hWnd)
		{
			if (IsWindowVisible(hWnd))
				HideWindow(hWnd);
			else
				ShowWindow(hWnd);
		}

		public static IntPtr GetMainHandle()
		{
			return Process.GetCurrentProcess().MainWindowHandle;
		}

		public static String GetWindowTitle(IntPtr hWnd)
		{
			StringBuilder sb = new StringBuilder(GetWindowTextLength(hWnd));
			GetWindowText(hWnd, sb, GetWindowTextLength(hWnd) + 1);

			return sb.ToString();
		}

		public static List<IntPtr> TitleOccurences(String title)
		{
			IntPtr shellWnd = GetShellWindow();

			List<IntPtr> results = new List<IntPtr>();

			EnumWindows(delegate (IntPtr hWnd, int lParam)
			{
				if (hWnd == shellWnd)
					return true;

				String wndTitle = GetWindowTitle(hWnd);

				if (wndTitle.Contains(title))
					results.Add(hWnd);

				return true;
			}, 0);
			
			return results;
		}

		public static void FreeList(List<IntPtr> list)
		{
			foreach (IntPtr hWnd in list)
				Marshal.FreeHGlobal(hWnd);
		}

		public static bool AlreadyOpen()
		{
			var occ = TitleOccurences("WindowHide.exe");
			foreach(var i in occ)
			{
				if (i.ToInt32() != Hide.hWnd.ToInt32())
					return true;
			}

			return false;
		}
	}
}
