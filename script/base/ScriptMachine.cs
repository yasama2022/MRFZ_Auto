using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace script
{
    public class ScriptMachine
    {
        public delegate void OnMsg(String msg);
        public OnMsg onMsg;
        protected   virtual void LoadConfig() { }
        
        public void RunInMainForm(Form mainForm)
        {
            throw new Exception();
        }
    }
}
