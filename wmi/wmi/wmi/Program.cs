using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;

namespace wmi
{
    class Program
    {
        static void Main(string[] args)
        {
            string user = getUser();
            if (user == "e")
            {
                Environment.Exit(1);
            }
            Console.Write(new string(' ', (Console.WindowWidth - 36) / 2));
            Console.WriteLine("####################################");
            Console.Write(new string(' ', (Console.WindowWidth - 36) / 2));
            Console.WriteLine("############# lol-wmi ##############");
            Console.Write(new string(' ', (Console.WindowWidth - 36) / 2));
            Console.WriteLine("###### v1.0 12/21/2016 sychan ######");
            Console.Write(new string(' ', (Console.WindowWidth - 36) / 2));
            Console.WriteLine("####################################");
            Console.Write(new string(' ', (Console.WindowWidth - (17 + user.Length)) / 2));
            Console.WriteLine("Configured USER: " + user);
            ManagementEventWatcher startWatch = new ManagementEventWatcher(
      new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
            startWatch.Start();
            ManagementEventWatcher stopWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(stopWatch_EventArrived);
            stopWatch.Start();
            //Console.WriteLine("Press any key to exit");
            //while (!Console.KeyAvailable)
            Console.WriteLine("\nWaiting for game to start...\n");
            int elapsedTime = 0;
            while (true)
            {
                if (elapsedTime > 3000)
                {
                    Ping pingClass = new Ping();
                    PingReply pingReply = pingClass.Send("104.160.131.3");
                    Console.WriteLine(" " + pingReply.RoundtripTime.ToString() + "ms\n");
                    elapsedTime = 0;
                }
                System.Threading.Thread.Sleep(50);
                elapsedTime += 50;

            }
            //startWatch.Stop();
            //stopWatch.Stop();
        }

        static void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            //Console.WriteLine("Process stopped: {0}", e.NewEvent.Properties["ProcessName"].Value);
            if (e.NewEvent.Properties["ProcessName"].Value.ToString().ToLower() == "leagueoflegends.exe")
            {
                Console.WriteLine("[GAME ENDED]");
                Console.WriteLine("Waiting for game to start...\n");
            }
        }

        static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string user = getUser();

            //Console.WriteLine("Process started: {0}", e.NewEvent.Properties["ProcessName"].Value);
            if (e.NewEvent.Properties["ProcessName"].Value.ToString().ToLower() == "leagueoflegends.exe")
            {
                Console.WriteLine("[GAME STARTING] " + user + " is now in game!");
                Console.WriteLine("Opening lolnexus...\n");
                Process.Start("chrome", @"http://www.lolnexus.com/NA/search?name=" + user + "&region=NA");
            }
        }
        static string getUser()
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\user_config.txt";
            try
            {
                using (StreamReader sr = File.OpenText(filepath))
                {
                    string s = String.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        return s;
                    }
                }
                Console.WriteLine("Error reading user file. Check the file and restart the program. \nPress any key to exit.");
                Console.ReadKey();
                return "e";
            }
            catch
            {
                Console.WriteLine("Error reading user file. Check the file and restart the program. Press any key to exit.");
                Console.ReadKey();
                return "e";
            }
            Console.WriteLine("Error reading user file. Check the file and restart the program. Press any key to exit.");
            Console.ReadKey();
            return "e";
        }
    }
}
