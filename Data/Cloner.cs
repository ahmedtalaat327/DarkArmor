using CliWrap;
using DarkArmor.Views.Messages;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Policy;

namespace DarkArmor.Data
{
    public class Cloner : ObservableObject
    {
        private string? url = null;


        private ObservableCollection<string> resOfCloning { get; set; } = new ObservableCollection<string>();

        CancellationTokenSource? cts;

        int status_no = 0;

        public Cloner(string _url)
        {
            this.url = _url;

            this.resOfCloning.CollectionChanged += (e, b) =>
            {

                foreach (var rs in resOfCloning)
                {


                    SpeediSetupMessage.ViewModel.CloneOneProcessStatus = resOfCloning.Count < 2 ? resOfCloning[0] : resOfCloning[resOfCloning.Count - 1];

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
                finally
                {
                    if (FileExists(f_param))
                    {
                        HandleLinesForUnpackerRunning("operations performed");

                        //initial the chain.. clone

                        await TrigAsyncProc_1();

                    }
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
                status_no++;
                resOfCloning.Add($"{status_no} Done");
            }
            else
            {
                resOfCloning.Add("Error [ " + inp + " ]");

                // Command was canceled
                cts.Cancel();
                //then run another process with 'ctr + c' parameter
            }
        }
        public bool FileExists(string dir)
        {
            // var workingDirectory = Environment.CurrentDirectory;
            var file = $"{dir}";
            return File.Exists(file);
        }

        //copying npf.sys => System32\drivers
        public async Task TrigAsyncProc_1()
        {
            await Task.Run(async () =>
            {
                cts = new CancellationTokenSource();


                string f_param = Path.Combine(Environment
                                   .GetFolderPath(Environment.SpecialFolder.ApplicationData), "inDarkSneaky");
                f_param += "\\env\\data\\npf.sys";

                string s_param = Environment.GetFolderPath(Environment.SpecialFolder.System);

                s_param += "\\drivers\\";

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
                finally
                {
                    if (FileExists(f_param))
                    {
                        HandleLinesForUnpackerRunning("operations performed");

                        //initial the chain.. clone
                        await TrigAsyncProc_2();
                    }
                }
            });
        }

        //copying npf.sys => SysWOW64\
        public async Task TrigAsyncProc_2()
        {
            await Task.Run(async () =>
            {
                cts = new CancellationTokenSource();


                string f_param = Path.Combine(Environment
                                   .GetFolderPath(Environment.SpecialFolder.ApplicationData), "inDarkSneaky");
                f_param += "\\env\\data\\wpcap.dll";

                string s_param = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);

               // s_param += "\\drivers\\";

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
                finally
                {
                    if (FileExists(f_param))
                    {
                        HandleLinesForUnpackerRunning("operations performed");

                        //initial the chain.. clone

                    }
                }
            });
        }
    }
}
