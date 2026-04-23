using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;



namespace DarkArmor.Data
{
    public class ExeTaskSC : ObservableObject
    {

        public bool ReplyFromFirstService { get; set; } = false;

        public bool ReplyFromSecondService { get; set; } = false;

        public async System.Threading.Tasks.Task CreateDriverForPacketFiltering()
        {
            await System.Threading.Tasks.Task.Run(()=>{ 
            if (!ReplyFromFirstService)
            {
                string s_param = Environment.GetFolderPath(Environment.SpecialFolder.System);

                s_param += "\\drivers\\npf.sys";

                   

                    TaskService ts = new TaskService();

                    var td = ts.GetTask("NetGroup Packet Filter Driver");

                    if (td == null)
                    {

                        // Run a program every day on the local machine
                        var res = TaskService.Instance.AddTask("NetGroup Packet Filter Driver", QuickTriggerType.Daily, s_param, "-a arg");
                        if (res.Enabled)
                        {
                            ReplyFromFirstService = true;
                        }
                    }
                    else
                    {
                        if(td.Enabled)
                            ReplyFromFirstService = true;
                    }
            }
            });

        }
        private readonly string _wu10TaskPath = "Wu10Man Admin Task";

        private Microsoft.Win32.TaskScheduler.Task CreateHelperTask(string toExecute, string fullPath)
        {
            var sid = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, WindowsIdentity.GetCurrent().User.AccountDomainSid);
            var account = sid.Translate(typeof(NTAccount));
            var username = account.Value;
 
            return TaskService.Instance.AddTask(
                    _wu10TaskPath,
                    new TimeTrigger() { StartBoundary = DateTime.Now, Enabled = false },
                    new ExecAction(
                        @"powershell",
                        $" -command \"& {{ {toExecute}  -TaskName '{fullPath}' }}\"",
                        Environment.CurrentDirectory),
                    userId: username,
                    logonType: TaskLogonType.ServiceAccount,
                    description: "Task used by Wu10Man to enable other tasks");
        }
    }
}
