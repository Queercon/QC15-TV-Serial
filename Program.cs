using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace QC15_TV_Serial
{
    class Program
    {
        private static string sqlcon = "";
        static void Main(string[] args)
        {

            Console.WriteLine("COM Port (COM6):");
            String comm = Console.ReadLine();
            if(comm == null || comm == "")
            {
                comm = "COM6";
            }
            Console.WriteLine(comm);

            Console.WriteLine("IP (127.0.0.1):");
            String ip = Console.ReadLine();
            if (ip == null || ip == "")
            {
                ip = "127.0.0.1";
            }
            Console.WriteLine(ip);

            Console.WriteLine("Debug (0):");
            String debug = Console.ReadLine();
            if (debug == null || debug == "")
            {
                debug = "0";
            }
            Console.WriteLine(debug);

            sqlcon = "server = " + ip + "; database = badge; UID = tv; password = 2az8wA4LxuQRIIH9";

                SerialPort port = new SerialPort(comm, 57600, Parity.None, 8, StopBits.One);


                
                if (debug == "0")
                {
                    port.DataReceived += onReceiveData;
                    port.Open();
                }

                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();

                if (debug == "0")
                {
                    port.Close();
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
                String[] data = createSplitData(line);

                if (charLine[0] == '3')
                {
                    updateFileData(data);
                }
                if (charLine[0] == '4')
                {
                    updateBadgeStats(data);
                }
            }
            catch (Exception es)
            {
                Console.WriteLine(charLine[0] + ": " + es.Message);
            }


        }

        private static String[] createSplitData(String txt)
        {
            String[] splitData = new string[15];

            Regex rgx = new Regex("[^a-zA-Z0-9 -$,0x960x9d?@]");
            txt = rgx.Replace(txt, "");

            Char delimiter = ',';
            splitData = txt.Split(delimiter);

            //Console.WriteLine("S0: " + splitData[0]);
            //Console.WriteLine("S1: " + splitData[1]);
            //Console.WriteLine("S2: " + splitData[2]);
            //Console.WriteLine("S3: " + splitData[3]);
            //Console.WriteLine("S4: " + splitData[4]);
            return splitData;
        }

        private static void updateFileData(String[] fileData)
        {
            string sqlQuery = "UPDATE badges SET [" + fileData[2] + "] = convert(binary(10), '" + fileData[3] + "', 1), [lastseen] = SYSDATETIME() WHERE[id0] = " + fileData[1];
            //Console.WriteLine(sqlQuery);
            //Console.WriteLine("F2: " + fileData[2]);
            //Console.WriteLine("F3: " + fileData[3]);
            //Console.WriteLine("F1: " + fileData[1]);


            using (SqlConnection con = new SqlConnection(sqlcon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                            //Console.WriteLine("Writen");

                    }
                }
            }

            
        }

        private static void updateBadgeStats(String[] badgeData)
        {
            string sqlQuery = "UPDATE[dbo].[badgestats] SET [lastseen] = SYSDATETIME() ,[badges_seen] = " + badgeData[2] + " ,[badges_connected] = " + badgeData[3] + " ,[badges_uploaded] = " + badgeData[4] + " ,[ubers_seen] = " + badgeData[5] + " ,[ubers_connected] = " + badgeData[6] + "  ,[ubers_uploaded] = " + badgeData[7] + " ,[handlers_seen] = " + badgeData[8] + " ,[handlers_connected] = " + badgeData[9] + " ,[handlers_uploaded] = " + badgeData[10] + " WHERE[id0] = " + badgeData[1];

            using (SqlConnection con = new SqlConnection(sqlcon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        //Console.WriteLine("Writen");

                    }
                }
            }


        }
    }
}
