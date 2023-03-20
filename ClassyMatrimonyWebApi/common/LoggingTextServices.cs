using System;
using System.IO;
using System.Text;

namespace MohsyWebApi.common
{
    public class LoggingTextServices
    {
        static string path = AppDomain.CurrentDomain.BaseDirectory + @"\logs\peoplematrimony.txt";

        static FileInfo fi = new FileInfo(path);
        public static void LogError(Exception ex)
        {
            try
            {
                var theBody = new StringBuilder();
                theBody.Append("Message: " + ex.Message + "\n");
                theBody.Append("StackTrace: " + ex.StackTrace + "\n");
                theBody.Append("InnerException: " + ex.InnerException + "\n");
              

                if (!fi.Exists)
                {
                    fi.Create();
                }
                using (StreamWriter sw = fi.AppendText())
                {
                    sw.WriteLine(theBody.ToString() + " ........... " + DateTime.Now.ToString() + "\n");
                }

            }
            catch  
            {
                //Nothing
            }

        }
    }
}