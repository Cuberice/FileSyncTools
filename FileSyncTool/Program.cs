using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FileSyncTool
{
	static class Program
	{
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(params string[] args)
		{
			EnableTrace();
			Mutex mutex;
			try
			{
				bool created;
				mutex = new Mutex(false, "Directory Synchronizer", out created);
				if (!created)
				{
					//MessageBox.Show("Another instance of this application is already running");
					Process current = Process.GetCurrentProcess();
					foreach (Process process in Process.GetProcessesByName("Directory Synchronizer"))
					{
						if (process.Id != current.Id)
						{
							process.Kill();
						}
					}
				}

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new FileSyncForm(args));
			}
			catch (WaitHandleCannotBeOpenedException e)
			{
				Trace.WriteLine(e);
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex);
				MessageBox.Show("Error Occured: " + ex.Message);
			}
			finally
			{
				Trace.Flush();
			}
		}

		private static void EnableTrace()
		{
			Trace.Listeners.Clear();

			TextWriterTraceListener twtl = new TextWriterTraceListener(Path.Combine(Application.StartupPath, "DebugListener.log"))
			                               	{
			                               		Name = "DebugListener",
			                               		TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime
			                               	};

			ConsoleTraceListener ctl = new ConsoleTraceListener(false) {TraceOutputOptions = TraceOptions.DateTime};

			Trace.Listeners.Add(twtl);
			Trace.Listeners.Add(ctl);
			Trace.AutoFlush = true;
			Trace.WriteLine("\n\n**** STARTED LISTENER **** \n");
			Trace.Flush();
		}
	}
}
