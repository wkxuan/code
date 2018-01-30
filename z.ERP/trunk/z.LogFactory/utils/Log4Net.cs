using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zlog4net;
using System.Data;
using zlog4net.Appender;
using zlog4net.Layout;
using zlog4net.Config;
using System.IO;
using System.Reflection;
using zlog4net.Repository;
using zlog4net.Repository.Hierarchy;

namespace z.LogFactory.utils
{
    public class Log4Net : LogUtilsBase
    {
        static void _log(string msg, ILog log, LogLevel loglevel)
        {

            switch (loglevel)
            {
                case LogLevel.Debug:
                    {
                        log.Debug(msg);
                        return;
                    }
                case LogLevel.Info:
                    {
                        log.Info(msg);
                        return;
                    }
                case LogLevel.Woring:
                    {
                        log.Warn(msg);
                        return;
                    }
                case LogLevel.Error:
                    {
                        log.Error(msg);
                        return;
                    }
                case LogLevel.Fatal:
                    {
                        log.Fatal(msg);
                        return;
                    }
            }
        }


        internal static void Log(LogConfigBase config, string LogName, LogLevel loglevel, string title, object[] obj)
        {
            InitLog4net(LogName, config);
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            string msg = FixLog(config, title, obj);
            _log(msg, log, loglevel);
        }

        private static void InitLog4net(string LogName, LogConfigBase config)
        {

            Hierarchy hier = LogManager.GetRepository() as Hierarchy;

            RollingFileAppender fileAppender = new RollingFileAppender();
            fileAppender.Name = LogName;
            fileAppender.File = Path.Combine(config.FilePath, LogName) + @"\";
            fileAppender.AppendToFile = true;
            fileAppender.DatePattern = config.FileName;
            fileAppender.StaticLogFileName = false;
            fileAppender.LockingModel = new FileAppender.MinimalLock();


            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = config.Pattern;
            patternLayout.ActivateOptions();
            fileAppender.Layout = patternLayout;

            //选择UTF8编码，确保中文不乱码。
            fileAppender.Encoding = Encoding.Default;

            fileAppender.ActivateOptions();

            hier.Root.RemoveAllAppenders();
            BasicConfigurator.Configure(fileAppender);
        }



    }
}
