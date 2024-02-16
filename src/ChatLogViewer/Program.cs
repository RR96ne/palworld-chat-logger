using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatLogViewer
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var mutex = new Mutex(false, "palworld_chat_logger"))
			{
				try
				{
					if (mutex.WaitOne(0))
					{
						Run(mutex);
					}
					else
					{
						//別プロセス起動中
					}
				}
				catch (AbandonedMutexException)
				{
					Run(mutex);
				}
			}
		}

		static void Run(Mutex mutex)
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
			finally
			{
				mutex.ReleaseMutex();
			}
		}
	}
}
