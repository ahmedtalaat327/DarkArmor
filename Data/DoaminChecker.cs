using DarkArmor.Helpers;
using DarkArmor.Models;
using DarkArmor.Models.Skeleton;
using DarkArmor.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.Data
{
    public class DoaminChecker
    {

        private NICController def_NICController = null;
        //we will get the device ip address from this instance
        private NetworkDevice? sacrifiicedDevice = null;
        //we will get the gate address from this instance
        private string? ipv4Gateway = null;


        CancellationTokenSource cts;




        public DoaminChecker(NetworkDevice _ipTobeChecked, NICController _nicController)
        {
            this.sacrifiicedDevice = _ipTobeChecked;
            this.def_NICController = _nicController;
            this.ipv4Gateway = this.sacrifiicedDevice.Nic?.Gate?.ToString();

        }

        public async Task TrigAsyncProc()
        {

            if (def_NICController == null)
            {
                return;
            }
            string parameter1Value = def_NICController.Nic_Index?.ToString();

            await Task.Run(async () =>
            {
                int processId = 1020202;
                var timeoutt = ((App.GetService<DataViewModel>().TimeOutVal/100) * 1000)*10;
                cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMilliseconds(timeoutt));
                string t_param = ipv4Gateway.ToString();
                string s_param = sacrifiicedDevice._nic.Address.ToString();
                try
                {
                    var task = CliWrap.Cli.Wrap("powershell.exe")
                              .WithArguments(new[] { $@"& '{DesktopAppOnly.PathFinder.GetApplicationRoot()}\Processes\DomainGrapper.exe'" + " " + parameter1Value + " " + s_param + " " + t_param })
                              // This can be simplified with `ExecuteBufferedAsync()`
                              .WithStandardOutputPipe(CliWrap.PipeTarget.ToDelegate(HandleLinesForDomainCheckerRunning))
                              .WithStandardErrorPipe(CliWrap.PipeTarget.ToDelegate(Console.WriteLine))
                              .ExecuteAsync(cts.Token);
                    // Get the process ID
                     processId = task.ProcessId;
                    App.GetService<DashboardViewModel>().ProcessesDomainCheckersIds.Add( processId );
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Domain checker process was cancelled.");
                    sacrifiicedDevice.DomainName = "Timeout"; // Update the domain name to indicate a timeout occurred

                }
                finally
                {
                    /*

                    await Task.Run(async () =>
                    {
                        await Task.Delay(App.GetService<DataViewModel>().TimeOutVal * 1000); // Wait for the timeout duration before attempting to kill the process
                        try
                        {
                            // Attempt to kill the process if it's still running
                            var process = System.Diagnostics.Process.GetProcessById(processId);
                            if (!process.HasExited)
                            {
                                process.Kill();
                                Console.WriteLine("Domain checker process was killed in the finally block.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error while trying to kill the domain checker process in the finally block: {ex.Message}");
                        }
                    });
                    */


                 
                }
            });
        }

        private async Task HandleLinesForDomainCheckerRunning(string arg1, CancellationToken token)
        {
            //throw new NotImplementedException();
                await Task.Run(() =>
                {
                    if (arg1.ToLower().Contains("domain"))
                    {
                        Console.WriteLine("Domain is up");
                        // You can also update the UI or perform other actions here
                        sacrifiicedDevice.DomainName = arg1.Split(':')[1].Trim(); // Assuming the output is in the format "Domain: domain_name"

                    }

                    else
                    {
                        Console.WriteLine($"Received output: {arg1}");
                        // Handle other output as needed
                    }
                }, token);
            

        }
    }
}
