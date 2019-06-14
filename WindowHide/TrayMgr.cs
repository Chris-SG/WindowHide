using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;

enum HideMode
{
	VISIBLE,
	HIDDEN,
	MINIMIZED
};

namespace WindowHide
{
	class TrayMgr : ApplicationContext
	{
		private NotifyIcon tray;
		private ContextMenu trayMenu;

		private MenuItem minimize;
		private MenuItem hide;
		private MenuItem show;

		private HideMode hidemode;

		public TrayMgr()
		{
		}

		public void StartTray()
		{
			// Tray setup
			tray = new NotifyIcon();
			tray.Text = "Window Hide";
			tray.Icon = new System.Drawing.Icon(WindowHide.Properties.Resources.Tray, 40, 40);

			trayMenu = new ContextMenu();

			MenuItem mode = new MenuItem("Window Mode");

			minimize = new MenuItem("Minimize Windows", new EventHandler(this.minimizewindows_click));
			hide = new MenuItem("Hide Windows", new EventHandler(this.hidewindows_click));
			show = new MenuItem("Show Windows", new EventHandler(this.showwindows_click));

			mode.MenuItems.Add(hide);
			mode.MenuItems.Add(show);
			mode.MenuItems.Add(minimize);

			minimize.Checked = true;
			hidemode = HideMode.MINIMIZED;
			trayMenu.MenuItems.Add(mode);

			trayMenu.MenuItems.Add("Toggle Main Window", new EventHandler(this.togglehide_click));
			trayMenu.MenuItems.Add("Exit", new EventHandler(this.exit_click));

			tray.ContextMenu = trayMenu;
			tray.Visible = true;

			Application.Run(this);
		}

		public void ExitTray()
		{
			tray.Icon = null;
			trayMenu.Dispose();
			tray.Dispose();
			Hide.exit = true;

			Application.Exit();
		}

		public HideMode GetHideMode()
		{
			return hidemode;
		}

		private void togglehide_click(System.Object sender, System.EventArgs e)
		{
			WindowFuncs.ToggleVisibility(Hide.hWnd);
		}

		private void hidewindows_click(System.Object sender, System.EventArgs e)
		{
			remove_checks();
			hide.Checked = true;
			hidemode = HideMode.HIDDEN;
		}

		private void showwindows_click(System.Object sender, System.EventArgs e)
		{
			remove_checks();
			show.Checked = true;
			hidemode = HideMode.VISIBLE;
		}

		private void minimizewindows_click(System.Object sender, System.EventArgs e)
		{
			remove_checks();
			minimize.Checked = true;
			hidemode = HideMode.MINIMIZED;
		}

		private void exit_click(System.Object sender, System.EventArgs e)
		{
			ExitTray();
		}

		private void remove_checks()
		{
			minimize.Checked = false;
			hide.Checked = false;
			show.Checked = false;
		}
	   }
}
