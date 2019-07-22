using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Threading;

namespace WiiBalanceWalker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           

            Worker workerObject = new Worker();
            Thread workerThread = new Thread(workerObject.DoWork);

            // Start the worker thread.
            workerThread.Start();
            Console.WriteLine("main thread: Starting worker thread...");

            /*// Loop until worker thread activates.
            while (!workerThread.IsAlive) ;

            // Put the main thread to sleep for 1 millisecond to
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            // Request that the worker thread stop itself:
            workerObject.RequestStop();

            // Use the Join method to block the current thread 
            // until the object's thread terminates.
            workerThread.Join();
            Console.WriteLine("main thread: Worker thread has terminated.");*/

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());         
        }
    }

    public class Worker
    {
        // This method will be called when the thread is started.
        public void DoWork()
        {
            
            while (!_shouldStop)
            {
                Console.WriteLine("worker thread: working...");
                ProgramServer.LaunchServer();
            }
            Console.WriteLine("worker thread: terminating gracefully.");
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop;
    }
}
