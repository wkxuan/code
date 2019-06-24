using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using z.ATR.Services;
using z.ATR.WinService.Model;
using z.Extensions;
using z.LogFactory;
using z.WSTools.Txt;

namespace z.ATR.WinService
{
    public partial class AotuService : ServiceBase
    {
        public Encoding encoding = Encoding.Default;

        public AotuService()
        {
            InitializeComponent();
            service = new ServicesBase();
        }

        protected LogWriter Log
        {
            get
            {
                return new LogWriter("Info");
            }
        }

        protected ServicesBase service
        {
            get;
            set;
        }

        public int SleepSecond
        {
            get
            {
                int s = ConfigExtension.GetConfig("SleepSecond").ToInt();
                return s < 5 ? 5 : s;
            }
        }

        Thread t;
        protected override void OnStart(string[] args)
        {
            Log.Info("服务开始");
            t = new Thread(Do);
            t.IsBackground = true;
            t.Start();
        }

        public void Do()
        {
            while (true)
            {
                try
                {
                    string configFilePath = IOExtension.GetBaesDir() + @"\Settings.Json";
                    List<Settings> sets = TxtReader.ReadToModel<Settings>(new JsonReaderSettings()
                    {
                        FilePath = configFilePath,
                        Encoding = encoding
                    });
                    bool hasRun = false;
                    if (sets != null)
                    {
                        sets.ForEach(set =>
                        {
                            try
                            {
                                if (!set.CanDo())
                                    return;
                                ServicesBase sb = new ServicesBase();
                                MethodInfo m = sb.AutoService.GetType().GetMethod(set.MethodName);
                                if (m == null)
                                    throw new Exception($"找不到方法{set.MethodName}");
                                m.Invoke(sb.AutoService, null);
                                set.LastRunTime = DateTime.Now;
                                Log.Info($"已执行{set.Name}");
                                hasRun = true;
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"执行{set.Name}异常", ex.InnerMessage());
                            }
                        });
                    }
                    if (hasRun)
                        TxtWriter.Write(configFilePath, sets.ToJson(true), encoding);
                }
                catch (Exception ex)
                {
                    Log.Error("服务异常", ex.InnerMessage());
                }
                finally
                {
                    Thread.Sleep(SleepSecond * 1000);
                }
            }
        }

        protected override void OnStop()
        {
            Log.Info("服务停止");
        }
    }
}
