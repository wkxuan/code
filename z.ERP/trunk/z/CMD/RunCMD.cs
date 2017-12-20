using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace z.CMD
{
    public class RunCmd
    {
        /// <summary>
        /// 主进程
        /// </summary>
        Process CmdProcess;
        string _cmdProcessName = "";
        public RunCmd(string Process = "cmd.exe")
        {
            _cmdProcessName = Process;
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="text"></param>
        public void Run(string text)
        {
            CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = _cmdProcessName;
            CmdProcess.StartInfo.WorkingDirectory = @"c:\";
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = true;
            CmdProcess.StartInfo.RedirectStandardOutput = true;
            CmdProcess.StartInfo.RedirectStandardError = true;
            CmdProcess.OutputDataReceived += CmdProcess_OutputDataReceived;
            CmdProcess.ErrorDataReceived += CmdProcess_OutputDataReceived;
            CmdProcess.EnableRaisingEvents = true;
            CmdProcess.Exited += CmdProcess_Exited;
            CmdProcess.StartInfo.CreateNoWindow = true;
            CmdProcess.Start();
            CmdProcess.StandardInput.WriteLine(text);
            CmdProcess.BeginOutputReadLine();
            CmdProcess.BeginErrorReadLine();
            CmdProcess.StandardInput.Close();
        }


        void CmdProcess_Exited(object sender, EventArgs e)
        {
            Exit?.Invoke();
        }

        void CmdProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output?.Invoke(e.Data);
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        public delegate void OutputDelegate(string msg);
        public event OutputDelegate Output;

        /// <summary>
        /// 退出
        /// </summary>
        public delegate void ExitDelegate();
        public event ExitDelegate Exit;
    }
}
