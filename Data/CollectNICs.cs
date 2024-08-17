using CliWrap;
using DarkArmor.Models.Skeleton;

namespace DarkArmor.Data
{
    /// <summary>
    /// this class made for trigging the [NIC Collector.exe] Process to run in Async-way.
    /// </summary>
    public class CollectNICS
    {
        //static array to save in specifi location [old approach]
        private static string[] linesOfDNIC = { null, null, null, null, null, null, null, null };
        //counter of NICx
        private static int counter = 0;
        //real list will be returned
        public static List<NICController> NICs = new List<NICController>();

        /*
        /// <summary>
        /// Async Method ...
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<List<NIC>> TrigProcAsync(string url)
        {
            NICs.Clear(); counter = 0;
            await Task.Run(async () =>
            {

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = @"powershell.exe",
                    Arguments = $@"& '{url}\Processes\NIC Collector.exe'",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    Verb = "runas",
                };
                try
                {

                    var processResults = await ProcessEx.RunAsync(processStartInfo);
                    foreach (var output in processResults.StandardOutput)
                    {
                        Console.WriteLine("Output line: " + output);
                        OutputHandler(output.ToString());
                    }


                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Timeout of {0} hit while trying to run {1}", "powershell", "NIc Collector");
                }

            });
            return NICs;
        }

        */

        private void OutputHandler(string outLine)
        {



            //check if line is not empty from Print of execution
            if (!String.IsNullOrEmpty(outLine))
            {

                // MessageBox.Show(outLine.Data);

                linesOfDNIC[0] = (counter.ToString());


                if (String.IsNullOrEmpty(linesOfDNIC[1]))
                {
                    string namechuncs = null;
                    try
                    {
                        namechuncs = GetBetween(outLine, "(", ")");
                        linesOfDNIC[1] = (namechuncs.ToString());

                    }
                    catch
                    {

                    }
                }
                if (String.IsNullOrEmpty(linesOfDNIC[2]))
                {
                    string ipreservedchuncs = null;
                    try
                    {
                        ipreservedchuncs = GetBetween(outLine, "*", "*");
                        linesOfDNIC[2] = (ipreservedchuncs.ToString());

                    }
                    catch
                    {

                    }
                }
                if (string.IsNullOrEmpty(linesOfDNIC[3]))
                {
                    string macchuncs = null;
                    try
                    {
                        macchuncs = GetBetween(outLine, ">", ">");
                        linesOfDNIC[3] = (macchuncs.ToString());

                    }
                    catch
                    {

                    }
                }
                if (string.IsNullOrEmpty(linesOfDNIC[4]))
                {
                    string macchuncs = null;
                    try
                    {
                        macchuncs = GetBetween(outLine, "!", "!");
                        linesOfDNIC[4] = (macchuncs.ToString());

                    }
                    catch
                    {

                    }
                }
                if (string.IsNullOrEmpty(linesOfDNIC[5]))
                {
                    string netmaskchuncs = null;
                    try
                    {
                        netmaskchuncs = GetBetween(outLine, "#", "#");
                        linesOfDNIC[5] = (netmaskchuncs.ToString());

                    }
                    catch
                    {

                    }
                }
                if (string.IsNullOrEmpty(linesOfDNIC[6]))
                {
                    string broadcstchuncs = null;
                    try
                    {
                        broadcstchuncs = GetBetween(outLine, "-", "-");
                        linesOfDNIC[6] = (broadcstchuncs.ToString());

                    }
                    catch
                    {

                    }
                }
                if (string.IsNullOrEmpty(linesOfDNIC[7]))
                {
                    string gatewaychuncs = null;
                    try
                    {
                        gatewaychuncs = GetBetween(outLine, "<", "<");
                        linesOfDNIC[7] = (gatewaychuncs.ToString());

                    }
                    catch
                    {

                    }
                }
                if (!String.IsNullOrEmpty(linesOfDNIC[1]) && !String.IsNullOrEmpty(linesOfDNIC[2]) && !String.IsNullOrEmpty(linesOfDNIC[3])
                    && !String.IsNullOrEmpty(linesOfDNIC[4]) && !String.IsNullOrEmpty(linesOfDNIC[5]) && !String.IsNullOrEmpty(linesOfDNIC[6])
                    && !String.IsNullOrEmpty(linesOfDNIC[7]))
                {
                    NICs.Add(new NICController() { Nic_Index = counter, FriendlyName = linesOfDNIC[1], Address = System.Net.IPAddress.Parse(linesOfDNIC[2]), PhysicalAdress = linesOfDNIC[3], Active = linesOfDNIC[4] == "0" ? false : true, Mask = System.Net.IPAddress.Parse(linesOfDNIC[5]), Broadcast = System.Net.IPAddress.Parse(linesOfDNIC[6]), Gate = System.Net.IPAddress.Parse(linesOfDNIC[7]) });

                    counter++;

                    linesOfDNIC = new string[] { null, null, null, null, null, null, null, null };
                }
            }


        }
        /// <summary>
        /// helper
        /// </summary>
        /// <param name="content"></param>
        /// <param name="startString"></param>
        /// <param name="endString"></param>
        /// <returns></returns>
        private static string GetBetween(string content, string startString, string endString)
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



        public async Task<List<NICController>> TrigExpProcAsync(string url)
        {
            NICs.Clear(); counter = 0;

            await Task.Run(async () =>
            {

                try
                {
                    await Cli.Wrap("powershell.exe")
                        .WithArguments(new[] { $@"& '{url}\Processes\NIC Collector.exe'" })
                     // This can be simplified with `ExecuteBufferedAsync()`
                     .WithStandardOutputPipe(PipeTarget.ToDelegate(OutputHandler))
                     .WithStandardErrorPipe(PipeTarget.ToDelegate(Console.WriteLine))
                     .WithValidation(CommandResultValidation.None)
                        .ExecuteAsync();
                }
                catch (OperationCanceledException)
                {
                    // Command was canceled

                }

            });
            return NICs;
        }



    }

}
