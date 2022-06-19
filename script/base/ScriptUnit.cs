using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using lib;

namespace script
{
    public class ScriptUnit
    {
        /// <summary>
        /// 图像宽高-固定
        /// </summary>
        protected const int ImgW = 1184, ImgH = 666;
        /// <summary>
        /// 窗口 宽高
        /// </summary>
        protected const int W = 1200, H = 705;
        /// <summary>
        /// 单元完成后睡眠时间
        /// </summary>
        protected const int SleepQuestMinutes = 9;
        public delegate void OnMsg(String msg);
        public static OnMsg onMsg; 
        protected String GameWinName;
        protected IntPtr GPR; 
        protected static Dictionary<string, GamePage> GamePageDic;
        public ScriptUnit() { }
        public ScriptUnit(String GameName)
        {
            this.GameWinName = GameName;
        }
          
        public virtual void Run() { throw new NotImplementedException(); }
        public virtual void RunTest() { throw new NotImplementedException(); }
        public virtual void Stop()
        {
           // thread?.Abort();
        }
        protected void waitSec(int sec)
        {
            var end=DateTime.Now.AddSeconds(sec);
            while(true)
            {
                if(DateTime.Now>=end)
                {
                    return;
                }
                Thread.Sleep(100);
            }
        }
        protected void wait(int ms)
        {
            if (ms > 1000)
            {
                int t = ms / 1000;
                for (int i = 0; i < t; i++)
                {

                    Thread.Sleep(ms);

                }
                if (ms - t * 1000 > 0)
                    Thread.Sleep(ms - t * 1000);
            }
            else
                Thread.Sleep(ms);
        }

        protected virtual void StopScript(String msg)
        {

            onMsg?.Invoke(msg);
            ////..
            /// 
           // thread?.Abort(); 

        }
        public void SleepQuest(int sec = 60 * SleepQuestMinutes)
        {
            onMsg?.Invoke($"脚本完成一轮,将休眠等待{sec}秒后继续进行脚本");
            var first = DateTime.Now;
            while (true)

            {
                Thread.Sleep(1000);
                if ((DateTime.Now - first).TotalSeconds > sec)
                {
                    break;
                }
            }
        }
        protected virtual bool WaitToNotCurPage(String PageName,int TimeOut_ms)
        {
            var first = DateTime.Now;
            while(true)
            {
                if((DateTime.Now-first).TotalMilliseconds>TimeOut_ms)
                {
                    return true;
                }
                if (GamePageDic[PageName].IsCurPage())
                {
                    wait(300);
                    continue;
                }else
                {
                    return false;
                }
            }
        }

        protected virtual bool WaitToCurPage(String PageName, int TimeOut_ms,  int interval_ms = 300)
        {
            var first = DateTime.Now;
            while (true)
            {
                if ((DateTime.Now - first).TotalMilliseconds > TimeOut_ms)
                {
                    return true;
                }
                if (GamePageDic[PageName].IsCurPage())
                { 
                    return false;
                }
                else
                {
                    wait(interval_ms);
                }
            }
        }
        protected virtual bool PageClick(String PageName, String ClickName,Boolean Valid=true)
        {
            if(Valid)
            {
                if (GamePageDic[PageName].IsCurPage())
                {
                    var CNP = SearchClickName(GamePageDic[PageName].NextPages, ClickName);
                    if (CNP == null) { { StopScript($" 找不到{PageName} {ClickName}点击数据"); } }
                    ClickPage(CNP);
                    wait(200);
                    return true;
                }
                else
                {
                    return false;
                }
            }else
            {
                var CNP = SearchClickName(GamePageDic[PageName].NextPages, ClickName);
                if (CNP == null) { { StopScript($" 找不到{PageName} {ClickName}点击数据"); } }
                ClickPage(CNP);
                wait(200);
                return true;
            }
            
             
        }
        protected virtual ClickToNextPage SearchClickName(List<ClickToNextPage> list, String Name)
        {
            foreach (var cnp in list)
            {
                if (cnp.ClickName == Name)
                {
                    return cnp;
                }
            }
            return null;
        }
        protected virtual void ClickPage(ClickToNextPage cnp)
        {
            Win.SetForegroundWindow(GPR);
            wait(250);
            var p = cnp.ClickPoint;
            Win.MouseClick(GPR, p.X, p.Y);
            wait(250); 
        }
    }
    public class CheckState
    {
        public Boolean StopScript;
        public Boolean Success; 
    }
}
