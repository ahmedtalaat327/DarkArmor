using CliWrap;
using DarkArmor.Models;
using DarkArmor.Models.Skeleton;
using DarkArmor.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.Data
{
    public class ManARP
    {
        private string? url = null;
        private int interfIndex = 0;
        
        private int spoofed = 0;
        //we will get the gate address from this instance
        private NetworkDevice? sacrifiicedDevice = null;
        private string? ipv4Gateway = null;

        CancellationTokenSource cts;
        public ManARP(string _url,NICController _defInterface,int spf,NetworkDevice _victim) { 
        
            this.url = _url;
            this.interfIndex = (int)_defInterface.Nic_index;
            this.spoofed = spf;
            this.sacrifiicedDevice = _victim;
            this.ipv4Gateway = this.sacrifiicedDevice.Nic?.Gate?.ToString();

        }

        public async Task TrigAsyncProc()
        {
            await Task.Run(async () =>
            {
                 cts = new CancellationTokenSource();
                //index of interface (local) 
                string f_param = interfIndex.ToString();
                //gw
                string s_param = ipv4Gateway.ToString();
                //victim ipv4
                string t_param = sacrifiicedDevice._nic.Address.ToString();
                //spoof flag
                string fo_param = spoofed.ToString();

                try
                {
                    await Cli.Wrap("powershell.exe")
                         .WithArguments(new[] { $@"& '{url}\Processes\Mim.exe'" + " " + f_param + " " + s_param + " " + t_param + " " + fo_param })
                         // This can be simplified with `ExecuteBufferedAsync()`
                         .WithStandardOutputPipe(PipeTarget.ToDelegate(HandleLinesForMimRunning))
                         .WithStandardErrorPipe(PipeTarget.ToDelegate(Console.WriteLine))
                         .ExecuteAsync(cts.Token);

                   
                }
                catch (OperationCanceledException)
                {
                    // Command was canceled
                    cts.Cancel();
                }
            });
        }
        private async Task HandleLinesForMimRunning(string inp)
        {
            if (inp.Contains("spoof_packet"))
            {
                App.GetService<DashboardViewModel>().DataShowed[this.sacrifiicedDevice.DeviceIndex].Active = false;
            }
            if (inp.Contains("normal_packet"))
            {
                // Command was canceled
                cts.Cancel();
                //then run another process with 'ctr + c' parameter
            }
        }


     }
}
