using CliWrap;
using DarkArmor.Views.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.Data
{
    public class Cloner : ObservableObject
    {
        private string? url = null;


        private ObservableCollection<string> resOfCloning { get; set; } = new ObservableCollection<string>();

        CancellationTokenSource? cts;

        public Cloner(string _url)
        {
            this.url = _url;

            this.resOfCloning.CollectionChanged += (e, b) => {

                foreach (var rs in resOfCloning)
                {


                    SpeediSetupMessage.ViewModel.CloneOneProcessStatus = resOfCloning.Count < 2 ? resOfCloning[0] : resOfCloning[1];

                }

                //OnPropertyChanged(nameof());

            };
        }

        public async Task TrigAsyncProc()
        {
            await Task.Run(async () =>
            {
                cts = new CancellationTokenSource();


                string f_param = Path.Combine(Environment
                                   .GetFolderPath(Environment.SpecialFolder.ApplicationData), "inDarkSneaky");
                f_param += "\\env\\data\\Packet.dll";

                string s_param = Environment.GetFolderPath(Environment.SpecialFolder.System);

                try
                {
                    var task = Cli.Wrap("powershell.exe")
                              .WithArguments(new[] { $@"& '{url}\Processes\Cloner\Cloner.exe'" + " " + f_param + " " + s_param })
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
       
        ///***Unf. this method will not be trigged at all this is a special case related to the hacky way 
        ///when the powershell executed another console [cmd] or dos named with Cloner.exe will appear injecting a capture from the handle 
        ///would work in this case!! [future development]
        private async Task HandleLinesForUnpackerRunning(string inp)
        {
            if (inp.Contains("operations performed"))
            {
                resOfCloning.Add("Done");
            }
            else
            {
                resOfCloning.Add("Error [ " + inp + " ]");

                // Command was canceled
                cts.Cancel();
                //then run another process with 'ctr + c' parameter
            }
        }

    }
}
