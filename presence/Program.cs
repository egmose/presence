using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;
using AudioSwitcher.AudioApi.CoreAudio;

namespace presence
{
    class Program
    {

        enum Color
        {
            red,
            green,
            na
        }

        static int maxcount = 30;
        static int count = 0;


        static Color current = Color.na;
        static HttpClient client = new HttpClient();

        static void setColor(Color newcolor)
        {
            if(newcolor != current || count++ > maxcount)
            {
                count = 0;
                current = newcolor;
                switch(newcolor)
                {
                    case Color.red:
                        try
                        {
                            client.PostAsync("http://172.19.1.11/automation.php", new FormUrlEncodedContent(new Dictionary<string, string> { { "channel", "/presence" }, { "value", "red" } }));
                        } catch (Exception e)
                        {

                        }
                        
                        Console.Write("\rred   ");
                        break;

                    case Color.green:
                        try
                        {
                            client.PostAsync("http://172.19.1.11/automation.php", new FormUrlEncodedContent(new Dictionary<string, string> { { "channel", "/presence" }, { "value", "green" } }));
                        }
                        catch (Exception e)
                        {

                        }
                        Console.Write("\rgreen ");
                        break;

                }
            }
        }

        static void Main(string[] args)
        {
            CoreAudioController controller = new CoreAudioController();
            CoreAudioDevice device = controller.DefaultCaptureCommunicationsDevice;
            Console.WriteLine(device.FullName);

            while (true)
            {
                if(device.IsDefaultCommunicationsDevice)
                {
                    if(device.SessionController.ActiveSessions().GetEnumerator().MoveNext())
                    {
                        setColor(Color.red);
                        
                    }
                    else
                    {
                        setColor(Color.green);
                    }
                }

                Thread.Sleep(1000);
                if(!device.IsDefaultCommunicationsDevice)
                {
                    device = controller.DefaultCaptureCommunicationsDevice;
                    current = Color.na;
                    Console.WriteLine();
                    Console.WriteLine(device.FullName);
                }                
            }
        }
    }
}
