using CliWrap;
using DarkArmor.Models.Skeleton;
using DarkArmor.ViewModels.Messagaes;
using DarkArmor.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.Data
{
    public  class Unpacker : ObservableObject
    {
        private string? url = null;


        private ObservableCollection<string> resOfUnpacking { get; set; } = new ObservableCollection<string>();

        CancellationTokenSource? cts;

        public Unpacker(string _url)
        {
            this.url = _url;

            this.resOfUnpacking.CollectionChanged += (e, b) => {

                foreach(var  rs in resOfUnpacking) {


                    App.GetService<SpeediSetupMessageViewModel>().DiscoveredStatusForUnpacking.Add(rs);


                }

             

                OnPropertyChanged("DiscoveredStatusForUnpacking");

               
            };
           
        }

        public async Task TrigAsyncProc()
        {
            await Task.Run(async () =>
            {
                cts = new CancellationTokenSource();

                string f_param = $"{url}\\Processes\\Unpacker\\env.pack";
                
                string s_param = Path.Combine(Environment
                                   .GetFolderPath(Environment.SpecialFolder.ApplicationData), "inDarkSneaky");


                try
                {
                    var task = Cli.Wrap("powershell.exe")
                              .WithArguments(new[] { $@"& '{url}\Processes\Unpacker\Unpacker.exe'" + " " + f_param + " " + s_param })
                              // This can be simplified with `ExecuteBufferedAsync()`
                              .WithStandardOutputPipe(PipeTarget.ToDelegate(HandleLinesForUnpackerRunning))
                              .WithStandardErrorPipe(PipeTarget.ToDelegate(Console.WriteLine))
                              .ExecuteAsync(cts.Token);



                    // Get the process ID
                 //   var processId = task.ProcessId;
                 //   App.GetService<DashboardViewModel>().ProcessesMimsIds.Add(new System.Collections.ObjectModel.ObservableCollection<int> { inKey, processId });
                    //async exec
                    await task;
                }
                catch (OperationCanceledException)
                {
                    // Command was canceled
                    cts.Cancel();
                }

            });
        }

        private async Task HandleLinesForUnpackerRunning(string inp)
        {
            if (inp.Contains("done"))
            {
                resOfUnpacking.Add("Done unpacking");
            }
            else
            {
                resOfUnpacking.Add("Error [ " + inp + " ]");

                // Command was canceled
                cts.Cancel();
                //then run another process with 'ctr + c' parameter
            }
        }


    }
}
