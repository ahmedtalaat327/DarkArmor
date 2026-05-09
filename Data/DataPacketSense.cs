using CliWrap;
using DarkArmor.Models.Skeleton;
using DarkArmor.ViewModels.Pages;
using System.Collections.ObjectModel;
using System.Security.Policy;


namespace DarkArmor.Data
{
    public class DataPacketSense : ObservableObject
    {

        private NICController def_NICController = null;
        private string url = null;
        private bool onlyBytsArray = false;
        private int caseCount = 0;
        private ObservableCollection<string> InfoSniffedPkts_list { get; set; } = new ObservableCollection<string>();


        private static List<CancellationTokenSource> ctcs = new List<CancellationTokenSource>();

        public DataPacketSense(NICController _nic, string _url, bool _onlyBytsArray)
        {
            def_NICController = _nic;
            url = _url;
            onlyBytsArray = _onlyBytsArray;

            this.InfoSniffedPkts_list.CollectionChanged += (s, e) =>
            {

                if (InfoSniffedPkts_list.ToList().Count < 8)
                    return;


                for(int i = 0;i<InfoSniffedPkts_list.ToList().Count;i++)
                {
                   if(InfoSniffedPkts_list.ToList()[i].ToLower().Contains("source"))
                    {

                        if (App.GetService<DashboardViewModel>().DiscoveredNICControllers.ToList().Count > 0)
                        {


                            for (int z = 0; z < App.GetService<DashboardViewModel>().DiscoveredNICControllers.ToList().Count; z++)
                            {
                                //  if ((InfoSniffedPkts_list.ToList()[(1) + (z * 8)]).Contains(App.GetService<DashboardViewModel>()?.DiscoveredNICControllers.ToList()[z].Address?.ToString()))
                                //{
                                //   (App.GetService<DashboardViewModel>().DataShowed[z].ReceivedBytes) = (InfoSniffedPkts_list.ToList()[(6) + (z * 8)]);
                                // }
                                if (InfoSniffedPkts_list.ToList()[i].ToLower().Contains(App.GetService<DashboardViewModel>()?.DiscoveredNICControllers.ToList()[z].Address?.ToString()))
                                {
                                    (App.GetService<DashboardViewModel>().DataShowed[z].ReceivedBytes) = InfoSniffedPkts_list.ToList()[i+5].Substring(14, InfoSniffedPkts_list.ToList()[i+5].Length - 14);

                                    InfoSniffedPkts_list.Clear();
                                    break;
                                }
                            }
                        }

                        break;


                    }
                }

                
                OnPropertyChanged("DataShowed");
            };
        }
        public async Task TrigProcAsync()
        {
            if(def_NICController == null || url == null)
            {
                return;
            }
            string parameter1Value = def_NICController.Nic_Index?.ToString();

            var countdevice = App.GetService<DashboardViewModel>().DiscoveredNICControllers.Count;

           
                string client = App.GetService<DashboardViewModel>().DiscoveredNICControllers[caseCount].Address.ToString();
                string gw = App.GetService<DashboardViewModel>().DiscoveredNICControllers[caseCount].Gate.ToString();

            if (client.Equals(gw))
            {
                caseCount++;

                if (caseCount < countdevice)
                    await TrigProcAsync();
            }
            else
            {

                await Task.Run(async () =>
                    {

                        var cts = new CancellationTokenSource();


                        try
                        {
                            var task = Cli.Wrap("powershell.exe")
                                 .WithArguments(new[] { $@"& '{url}\Processes\DataCapacity.exe'" + " " + parameter1Value + " " + client + " " + gw })
                                 // This can be simplified with `ExecuteBufferedAsync()`
                                 .WithStandardOutputPipe(PipeTarget.ToDelegate(HandleLinesForPacketSniffingBoardRunning))
                                 .WithStandardErrorPipe(PipeTarget.ToDelegate(Console.WriteLine))
                                 .ExecuteAsync(cts.Token);

                            ctcs.Add(cts);

                            // Get the process ID
                            var processId = task.ProcessId;
                            App.GetService<DashboardViewModel>().ProcessesDataSniffsIds.Add( processId );


                             await task;    
                        }
                        catch (OperationCanceledException)
                        {
                            // Command was canceled
                            cts.Cancel();
                        }
                        finally
                        {
                            caseCount++;

                            if (caseCount < countdevice)
                                await TrigProcAsync();
                        }



                    });
            }

            



        }
        private async Task HandleLinesForPacketSniffingBoardRunning(string inp)
        { 
            if (inp == null || inp.ToLower().Contains("wi")) return;
            else
            {
                InfoSniffedPkts_list.Add(inp);
            }
        }
    }
}
