using Newtonsoft.Json;
using System;
using System.IO.Ports;
using System.Threading;

namespace RemoteControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Remote Control.");

            var port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
            try
            {
                port.Open();
            }
            catch(Exception ex)
            { 
                Console.WriteLine(ex.Message);
                return;
            }

            using (var reader = new SensorReader())
            {
                while (true)
                {
                    var data = reader.GetData();
                    var json = JsonConvert.SerializeObject(data);
                    port.WriteLine(json);
                    Thread.Sleep(500);
                }
            }

        }
    }
}
