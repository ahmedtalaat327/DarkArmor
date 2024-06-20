using CliWrap;
using DarkArmor.Models.Skeleton;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;



namespace DarkArmor.Data
{
    public class ARPRequest
    {
        private NICController def_NICController = null;
        private ObservableCollection<NICController> NICController_list = new ObservableCollection<NICController>();
        private string Url = null;
        private int index = 0;
        private double timeOut = 0.49;

        bool key_to_start_req_proc = false;

        //[important] this option for choosing between real interface value & customized only with broadcast.
        private bool custom_range_scan = false;
        private List<string> ips_to_scan = new List<string>();
        private string artifial_broadcast = null;
        public ARPRequest(NICController currentInterface, ObservableCollection<NICController> refrenceredColls,double timeForARPReq = 0.49, bool shortScan = false, string someNewBroadcast = null)
        {

            this.def_NICController = currentInterface;
            this.timeOut = timeForARPReq;
            this.custom_range_scan = shortScan;
            this.artifial_broadcast = someNewBroadcast;
            this.NICController_list = refrenceredColls;
        }
        public async Task TrigProcAsync(string url)
        {
            Url = url;
            await Task.Run(async () =>
            {

                string parameter1Value = def_NICController.Nic_index.ToString();

                // string parameters =  parameter1Value;


                var stdOutBuffer = new StringBuilder();
                var stdErrBuffer = new StringBuilder();

                var cts = new CancellationTokenSource();

                IPNetwork ipnetwork = IPNetwork.Parse(def_NICController.Address, def_NICController.Mask);


                if (custom_range_scan)
                {


                    for (int p = 0; p < ipnetwork.ListIPAddress().Count; p++)
                    {
                        ips_to_scan.Add(ipnetwork.ListIPAddress()[p].ToString());
                    }

                    ips_to_scan = NewRangeForCustomScan(ips_to_scan, artifial_broadcast);

                    var totalTime = timeOut * (1 + Int32.Parse(ips_to_scan.Count.ToString()));
                    // Cancel after a timeout of 10 seconds
                    cts.CancelAfter(TimeSpan.FromSeconds(totalTime));
                }
                else
                {



                    var totalTime = timeOut * (1 + Int32.Parse(ipnetwork.ListIPAddress().Count.ToString()));
                    // Cancel after a timeout of 10 seconds
                    cts.CancelAfter(TimeSpan.FromSeconds(totalTime));
                }

                try
                {
                    await Cli.Wrap("powershell.exe")
                        .WithArguments(new[] { $@"& '{url}\Processes\Router Scanner.exe'" + " " + parameter1Value })
                        // This can be simplified with `ExecuteBufferedAsync()`
                        .WithStandardOutputPipe(PipeTarget.ToDelegate(HandleLinesForScnnerBoardRunning))
                        .WithStandardErrorPipe(PipeTarget.ToDelegate(Console.WriteLine))
                        .ExecuteAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    // Command was canceled
                    cts.Cancel();
                }
                #region old approach using usetaskasasyncprosses
                //     ProcessStartInfo processStartInfo = new ProcessStartInfo
                //     {
                //         FileName = @"powershell.exe",
                //         Arguments = $@"& '{url}\Processes\Router Scanner.exe'"+" "+parameter1Value,
                //         RedirectStandardOutput = true,
                //         RedirectStandardError = true,
                //         RedirectStandardInput = true,
                //         UseShellExecute = true,
                //         CreateNoWindow = true,
                //         Verb = "runas",
                //
                //     };
                //     try
                //     {
                //
                //         var processResults = await ProcessEx.RunAsync(processStartInfo);
                //         foreach (var output in processResults.StandardOutput)
                //         {
                //             Console.WriteLine("Output line: " + output);
                //          //   OutputHandler(output.ToString());
                //         }
                //
                //
                //     }
                //     catch (OperationCanceledException)
                //     {
                //         Console.WriteLine("Timeout of {0} hit while trying to run {1}", "powershell", "Router Scanner");
                //     }
                #endregion

            });
           
        }
        private async Task HandleLinesForScnnerBoardRunning(string inp)
        {
            await Task.Run(async () =>
            {
                string line_used = "";
                if (inp != null)
                {
                    Console.WriteLine($"reply :{inp}");
                    if (index > 0)
                        NICController_list.Add(new NICController
                        {
                            Nic_index = index,
                            Manufacture = "not set yet"/*await new DNSResolver().TrigProcAsync(Url, GetBetween(inp, "*", "*"), (timeOut * 1000).ToString())*/,
                            Address = IPAddress.Parse(GetBetween(inp, "#", "#")),
                            PhysicalAdress = GetBetween(inp, "*", "*"),
                            Gate = def_NICController.Gate,
                            Broadcast = def_NICController.Broadcast,
                            Mask = def_NICController.Mask
                             
                        });


                    index++;


                }
                //////////////////////////////RUN OLNY ONE TIME//////////////////////////////////////////////
                //fire arp req process
                if (!key_to_start_req_proc)
                {
                    if (custom_range_scan)
                    {
                        for (int x = 0; x < ips_to_scan.Count; x++)
                        {

                            line_used += ips_to_scan[x];
                            line_used += "+";
                            // Console.WriteLine(line_used);

                        }
                        await TrigProcAsync_Inner(Url, new string[] { def_NICController.Nic_index.ToString(), line_used, ips_to_scan.Count.ToString() });

                    }
                    else
                    {
                        IPNetwork ipnetwork = IPNetwork.Parse(def_NICController.Address, def_NICController.Mask);
                        for (int x = 0; x < ipnetwork.ListIPAddress().Count; x++)
                        {

                            line_used += ipnetwork.ListIPAddress()[x];
                            line_used += "+";
                            // Console.WriteLine(line_used);

                        }
                        await TrigProcAsync_Inner(Url, new string[] { def_NICController.Nic_index.ToString(), line_used, ipnetwork.ListIPAddress().Count.ToString() });
                    }

                    key_to_start_req_proc = true;
                }
                //////////////////////////////////////////////////////////////////////////////////////////////
            });
        }
        public async Task TrigProcAsync_Inner(string url, string[] paramsForReq)
        {
            await Task.Run(async () =>
            {

                // string parameter1Value = def_NICController.Index.ToString();

                // string parameters = parameter1Value;
                var cts = new CancellationTokenSource();

                // Cancel after a timeout of 10 seconds
                cts.CancelAfter(TimeSpan.FromSeconds(timeOut * Int32.Parse(paramsForReq[2])));

                var stdOutBuffer = new StringBuilder();
                var stdErrBuffer = new StringBuilder();
                try
                {
                    await Cli.Wrap("powershell.exe")
                        .WithArguments(new[] { $@"& '{url}\Processes\Request.exe'" + " " + paramsForReq[0] + " " + paramsForReq[1] })
                        // This can be simplified with `ExecuteBufferedAsync()`
                        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                        .ExecuteAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    // Command was canceled
                    cts.Cancel();
                }
                #region old approach using usetaskasasyncprosses
                //     ProcessStartInfo processStartInfo = new ProcessStartInfo
                //     {
                //         FileName = @"powershell.exe",
                //         Arguments = $@"& '{url}\Processes\Router Scanner.exe'"+" "+parameter1Value,
                //         RedirectStandardOutput = true,
                //         RedirectStandardError = true,
                //         RedirectStandardInput = true,
                //         UseShellExecute = true,
                //         CreateNoWindow = true,
                //         Verb = "runas",
                //
                //     };
                //     try
                //     {
                //
                //         var processResults = await ProcessEx.RunAsync(processStartInfo);
                //         foreach (var output in processResults.StandardOutput)
                //         {
                //             Console.WriteLine("Output line: " + output);
                //          //   OutputHandler(output.ToString());
                //         }
                //
                //
                //     }
                //     catch (OperationCanceledException)
                //     {
                //         Console.WriteLine("Timeout of {0} hit while trying to run {1}", "powershell", "Router Scanner");
                //     }
                #endregion

            });

        }
        /// <summary>
        /// helper
        /// </summary>
        /// <param name="content"></param>
        /// <param name="startString"></param>
        /// <param name="endString"></param>
        /// <returns></returns>
        public static string GetBetween(string content, string startString, string endString)
        {
            int Start = 0, End = 0;
            if (content.Contains(startString) && content.Contains(endString))
            {
                Start = content.IndexOf(startString, 0) + startString.Length;
                End = content.IndexOf(endString, Start);
                return content.Substring(Start, End - Start);
            }
            else
                return string.Empty;
        }

        private List<string> NewRangeForCustomScan(List<string> oldList, string threeSholdValue)
        {
            List<string> newList = new List<string>();
            foreach (string item in oldList)
            {

                if (item.Contains(threeSholdValue))
                {
                    oldList.RemoveRange(oldList.IndexOf(threeSholdValue) + 1, oldList.Count - (oldList.IndexOf(threeSholdValue) + 1));

                    newList = oldList;

                    break;

                }


            }
            return newList;
        }
    }

}
