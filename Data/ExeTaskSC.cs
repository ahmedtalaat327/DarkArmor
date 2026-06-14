using CliWrap;
using DarkArmor.Views.Messages;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Policy;


namespace DarkArmor.Data
{
    public class ExeTaskSC : ObservableObject
    {

        private ObservableCollection<string> resOfScripting { get; set; } = new ObservableCollection<string>();

        CancellationTokenSource? cts;

        bool flagThisDoen = false;


        public ExeTaskSC() {

            this.resOfScripting.CollectionChanged += (e, b) =>
            {

                foreach (var rs in resOfScripting)
                {


                    SpeediSetupMessage.ViewModel.TaskExecuterStatus = resOfScripting.Count < 2 ? resOfScripting[0] : resOfScripting[resOfScripting.Count - 1];

                }

                //OnPropertyChanged(nameof());

            };

        }

        public async Task TrigAsyncProc()
        {
            await Task.Run(async () =>
            {


                string _pathrpcainroaming = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "inDarkSneaky\\env\\data\\rpcapd.exe");
                string _pathrpcainroamingini = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "inDarkSneaky\\env\\data\\rpcapd.ini");


                string _pathnpf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers\\npf.sys");
               


                cts = new CancellationTokenSource();

                string option_param = $"sc.exe create rpcapd type= own start= demand binPath= {_pathrpcainroaming} DisplayName= \"Remote Package Capture Protocol...\"\r\n" +
                $"sc.exe create npf binPath= {_pathnpf} type= kernel start= auto error= normal tag= no DisplayName= \"NetGroup Packet Filter Driver\"\r\n" +
                "sc.exe start npf";

                try
                {
                    var task = Cli.Wrap("sc.exe")
                                    .WithArguments(new[] {
                                        "create",
                                        "rpcapd",
                                        $"binPath=\"{_pathrpcainroaming}\" -d -f \"{_pathrpcainroamingini}\"",
                                        "DisplayName=Remote Package Capture Protocol...",
                                        "start=demand",
                                        "type=own",

                                    })
                              //Cli.Wrap("cmd")
                              //.WithArguments($"-Command Start-Process {option_param} -Verb RunAs")

                              //.WithArguments(new[] { $@"&  {option_param} "})
                              // This can be simplified with `ExecuteBufferedAsync()`
                              //.WithStandardInputPipe(PipeSource.FromString($"{option_param}"))
                              .WithStandardOutputPipe(PipeTarget.ToDelegate(HandleLinesForUnpackerRunning))
                              .WithStandardErrorPipe(PipeTarget.ToDelegate(Console.WriteLine))
                              //.WithCredentials(new Credentials("codinglap", "ahmed hassan", "EGYX@720p"))
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
                

                try
                {
                    var task_1 = Cli.Wrap("sc.exe")
                                    .WithArguments(new[] {
                                        "create",
                                        "npf",
                                        $"binPath={_pathnpf}",
                                        "DisplayName=NetGroup Packet Filter Driver",
                                        "start=auto",
                                        "type=kernel",
                                        "error=normal",
                                        "tag=no",

                                    })
                              //Cli.Wrap("cmd")
                              //.WithArguments($"-Command Start-Process {option_param} -Verb RunAs")

                              //.WithArguments(new[] { $@"&  {option_param} "})
                              // This can be simplified with `ExecuteBufferedAsync()`
                              //.WithStandardInputPipe(PipeSource.FromString($"{option_param}"))
                              .WithStandardOutputPipe(PipeTarget.ToDelegate(HandleLinesForUnpackerRunning))
                              .WithStandardErrorPipe(PipeTarget.ToDelegate(Console.WriteLine))
                              //.WithCredentials(new Credentials("codinglap", "ahmed hassan", "EGYX@720p"))
                              .ExecuteAsync(cts.Token);



                    // Get the process ID
                    //   var processId = task.ProcessId;
                    //   App.GetService<DashboardViewModel>().ProcessesMimsIds.Add(new System.Collections.ObjectModel.ObservableCollection<int> { inKey, processId });
                    //async exec
                    await task_1;
                }
                catch (OperationCanceledException)
                {
                    // Command was canceled
                    cts.Cancel();
                }

                
                try
                {
                    var task_2 = Cli.Wrap($"sc.exe")

                              //Cli.Wrap("cmd")
                              .WithArguments(new[] { 
                                  "start npf"
                                   })
                              

                              //.WithArguments(new[] { $@"&  {option_param} "})
                              // This can be simplified with `ExecuteBufferedAsync()`
                              //.WithStandardInputPipe(PipeSource.FromString($"{option_param}"))
                              .WithStandardOutputPipe(PipeTarget.ToDelegate(HandleLinesForUnpackerRunning))
                              .WithStandardErrorPipe(PipeTarget.ToDelegate(Console.WriteLine))
                              //.WithCredentials(new Credentials("codinglap", "ahmed hassan", "EGYX@720p"))
                               
                              .ExecuteAsync(cts.Token);



                    // Get the process ID
                    //   var processId = task.ProcessId;
                    //   App.GetService<DashboardViewModel>().ProcessesMimsIds.Add(new System.Collections.ObjectModel.ObservableCollection<int> { inKey, processId });
                    //async exec
                    await task_2;
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
            if (inp.ToLower().Contains("createservice")|| inp.ToLower().Contains("already"))
            {
                resOfScripting.Add("Done");

                if(!flagThisDoen)
                flagThisDoen = true;
            }
            else
            {
                if (!flagThisDoen)
                {
                    resOfScripting.Add("Error [ " + inp + " ]");

                    // Command was canceled
                    cts.Cancel();
                    //then run another process with 'ctr + c' parameter
                }
            }
        }
        /*
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
        */

    }
}
