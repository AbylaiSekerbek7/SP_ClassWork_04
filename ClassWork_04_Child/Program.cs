using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace ClassWork_04_Child
{
    internal class Program
    {
        static EventWaitHandle onExit = null;
        static void MyConsoleHandle(object sender, ConsoleCancelEventArgs e)
        {
            // if programm is closing - giving a signal of Exit
            if (onExit != null)
            {
                onExit.Set();
            }
        }
        static void Main(string[] args)
        {
            Console.CancelKeyPress += MyConsoleHandle;
            Console.WriteLine("Child is started!");
            bool IsCreate = false; // Если событие создано не нами.
            // Opening Global Event with name EventName
            string EventName = args[args.Length - 1];
            onExit = new EventWaitHandle(false, EventResetMode.ManualReset, EventName, out IsCreate);
            if (IsCreate)
            {
                Console.WriteLine($"Error - Global Event {EventName} absent!");
                onExit.Dispose();
                onExit = null;
            }
            else
            {
                Console.WriteLine($"OK: Event {EventName} is open!");
            }

            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"{i} : {args[i]}");
            }
            Process current = Process.GetCurrentProcess();

            Console.WriteLine($"Id current process: {current.Id}");
            
            Console.WriteLine("Good bye...");
            Console.ReadLine();
            if (onExit != null)
            { // If Global Event is exist
                onExit.Set(); // We giving signal about exit of programm
            }

        } // void Main();
    }
}