using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Data.SqlClient;

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

            String line = spL.ReadLine();
            char[] charLine = line.ToCharArray();

            try
            {
                if (charLine[0] == '3')
                {
                    String[] fileData = createFileData(line);
                    updateFileData(fileData);
                }
            }
            catch (Exception es)
            {
                Console.WriteLine(es.Message);
            }


        }

        private static String[] createFileData(String file)
        {
            String[] fileData = new string[3];

            Char delimiter = ',';
            fileData = file.Split(delimiter);

            return fileData;
        }

       private static void updateFileData(String[] fileData)
        {
            string sqlQuery = "UPDATE badges SET [" + fileData[2] + "] = convert(binary(10), '" + fileData[3] + "', 1), [lastseen] = SYSDATETIME() WHERE[id0] = " + fileData[1];

            using (SqlConnection con = new SqlConnection(sqlcon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                            Console.WriteLine("Writen");

                    }
                }
            }

            
        }
    }
}
