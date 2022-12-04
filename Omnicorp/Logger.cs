using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicorp
{
    internal class Logger
    { /*
    * CLASS NAME	:   Logger
    * DESCRIPTION	:   The purpose of this class is to act as a static class to log any content to a file
    * 
    * DATA MEMBERS  :   None
    */
            private static Logger _instance = new Logger();
            public static Logger Instance { get { return _instance; } }




            /*
            * METHOD		:   Log
            * DESCRIPTION	:   to log the given arguments into a pre-defined file
            * PARAMETERS    :
            *                   - string    action, as the action wich is loggin the message
            *                   - string    message, as the message to be logged
            * RETURNS       :
            *                   - None
            */
            public static void Log(string action, string message, string logFile)
            {
                StreamWriter file = null;
                DateTime now = DateTime.Now;
                string formated = $"{now.ToString()} [{action}] <{message}>";

                try
                {
                    file = new StreamWriter(logFile, true);
                    file.WriteLine(formated);
                }
                catch (Exception)
                {
                    file = null;
                }
                finally
                {
                    if (file != null)
                    {
                        file.Close();
                    }
                }

                return;
            }
        }
    }

