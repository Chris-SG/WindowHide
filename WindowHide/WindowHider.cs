using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowHide
{
	class WindowHider
	{
		private List<Tuple<String, int>> titles;
		private TrayMgr traymgr;

		public WindowHider(TrayMgr mgr)
		{
			traymgr = mgr;
			titles = new List<Tuple<String, int>>();

			string[] lines = System.IO.File.ReadAllLines("windowhider.ini");
			foreach(string line in lines)
			{
				titles.Add(Tuple.Create(line, 0));
			}
		}

		public void BeginHiding()
		{
			while(!Hide.exit)
			{
				for (int i = 0; i < titles.Count; ++i)
				{

					titles[i] = Tuple.Create(titles[i].Item1, 0);
					var results = WindowFuncs.TitleOccurences(titles[i].Item1);
					if (results.Count > 0)
					{
						foreach (IntPtr hWnd in results)
						{
							switch(traymgr.GetHideMode())
							{
								case HideMode.VISIBLE:
									WindowFuncs.ShowWindow(hWnd);
									break;
								case HideMode.MINIMIZED:
									WindowFuncs.MinimizeWindow(hWnd);
									break;
								case HideMode.HIDDEN:
									WindowFuncs.HideWindow(hWnd);
									break;
							}
						}
					}
				}
				
				Thread.Sleep(500);
			}
		}
	}
}
