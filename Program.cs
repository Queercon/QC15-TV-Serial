using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace QC15_TV_Serial
{
    class Program
    {
        private static string sqlcon = "";
        static void Main(string[] args)
        {


            if(args.Length == 3)
            {
                sqlcon = "server = " + args[1] + "; database = badge; UID = tv; password = 2az8wA4LxuQRIIH9";

                SerialPort port = new SerialPort(args[0], 9600, Parity.None, 8, StopBits.One);
                

                Console.WriteLine(args[0]);
                //Console.WriteLine(sqlcon);

                if (args[2] == "0")
                {
                    port.DataReceived += onReceiveData;
                    port.Open();
                }

                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();

                if (args[2] == "0")
                {
                    port.Close();
                }
            }
            else
            {
                Console.WriteLine("Your missing variables.. COM IP DEMO");
                Console.ReadLine();
            }



            

        }

        private static void onReceiveData(object sender,
                                   SerialDataReceivedEventArgs e)
        {
            SerialPort spL = (SerialPort)sender;

            Console.WriteLine(spL.ReadLine());

        }

       
    }
}
