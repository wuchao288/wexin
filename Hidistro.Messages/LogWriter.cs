using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Hidistro.Messages
{
    public class LogWriter
    {
        #region 保存日志
        public static void SaveLog(string msg)
        {
            SaveLog(msg, false);
        }
        #endregion

        #region 保存日志
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isClear">是否清空</param>
        public static void SaveLog(string msg, bool isClear)
        {
            string xmlPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

            string folder = Directory.GetParent(xmlPath).FullName + "\\debug";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string file = folder + "\\log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

            if (!File.Exists(file))
            {
                FileStream fc = File.Create(file);
                fc.Close();
            }

            FileStream fs = null;
            StreamWriter writer = null;
            try
            {
                if (isClear)
                {
                    fs = new FileStream(file, FileMode.Create);
                }
                else
                {
                    fs = new FileStream(file, FileMode.Append);
                }
                writer = new StreamWriter(fs, Encoding.UTF8);
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg);
                writer.Flush();
                writer.Close();
                fs.Close();
            }
            catch (Exception)
            {
                if (writer != null)
                {
                    writer.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
        #endregion
    }
}