using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.IO;

namespace WebApplication3._1.Models
{
    public class Command
    {
        /// <summary>
        /// singleton design pattern - we want multiple users of the same instance
        /// </summary>
        private static Command c_Instance = null;
        public TcpClient server;
        NetworkStream ns;
        public static Command Instance
        {
            get
            {
                if (c_Instance == null)
                {
                    c_Instance = new Command();
                }
                return c_Instance;
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="input">the command to be sent</param>
        /// <param name="flightServerIP">the ip address of the server - flightgear simulator </param>
        /// <param name="flightCommandPort">the port in which we will open a socket connection </param>

        /// <summary>
        /// the actual connection function
        /// when invoked - it will send to the server the actual command
        /// </summary>
        /// <param name="input"></param>
        /// <param name="flightServerIP"></param>
        /// <param name="flightCommandPort"></param>

        public void Start(string flightServerIP, int flightCommandPort)
        {
            byte[] data = new byte[1024];

            try
            {
                server = new TcpClient(flightServerIP, flightCommandPort);
            }
            catch (SocketException)
            {
                Console.WriteLine("Unable to connect to server");
                return;
            }
        }

        public string[] SendCommand()
        {
            ns = server.GetStream();
            string[] cmds = { "get /position/longitude-deg", "get /position/latitude-deg",
            "get /controls/flight/rudder", "get /controls/engines/current-engine/throttle"};
            foreach (string cmd in cmds)
            {
                string tmpCmd = cmd + "\r\n";
                ns.Write(Encoding.ASCII.GetBytes(tmpCmd), 0, Encoding.ASCII.GetBytes(tmpCmd).Length);
            }
            byte[] msg = new byte[1024];     // byte array
            ns.Read(msg, 0, msg.Length);   //the networkstream now reads what is being sent from the client
            char[] charsToTrim = { ' ', '?' };
            string phrase = Encoding.Default.GetString(msg).Trim(charsToTrim);
            cmds = phrase.Split(',', '\n');
            cmds[0] = cmds[0].Replace("/position/longitude-deg = '", "");
            cmds[1] = cmds[1].Replace("/> /position/latitude-deg = '", "");
            cmds[2] = cmds[2].Replace("/> /controls/flight/rudder = '", "");
            cmds[3] = cmds[3].Replace("/> /controls/engines/current-engine/throttle = '", "");
            for (int i = 0; i < 4; i++)
                cmds[i] = cmds[i].Replace("' (double)\r", "");
            return cmds;
        }

        public void CloseServer()
        {
            server.Close();
        }
    }
}