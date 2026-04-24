using System.Collections.ObjectModel;
using CliWrap;
using DarkArmor.Views.Messages;


namespace DarkArmor.Data
{
    public class ExeTaskSC : ObservableObject
    {

        private ObservableCollection<string> resOfScripting { get; set; } = new ObservableCollection<string>();

        CancellationTokenSource? cts;



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
            
            
            
            
            
            
            });


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
