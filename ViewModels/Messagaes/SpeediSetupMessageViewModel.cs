using DarkArmor.Data;
using DarkArmor.Helpers;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace DarkArmor.ViewModels.Messagaes
{
    public partial class SpeediSetupMessageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isVisivblewindow = true;
        [ObservableProperty]
        private string _unpackProcessStatus = "Sleep";

        [ObservableProperty]
        private bool _runUnpackingProcessButtonfloag = true;


        [ObservableProperty]
        private string _cloneOneProcessStatus = "Sleep";

        [ObservableProperty]
        private bool _runCloneOneProcessButtonfloag = true;

        [ObservableProperty]
        private string _taskExecuterStatus = "Sleep";

        [ObservableProperty]
        private bool _runTaskExecuterProcessButtonfloag = true;


        public async Task StartUnpacking()
        {
            UnpackProcessStatus = "Wait";
            RunUnpackingProcessButtonfloag = false;
            await new Unpacker(DesktopAppOnly.PathFinder.GetApplicationRoot()).TrigAsyncProc();
        }

        public async Task StartCloning(int stepNo)
        {
            switch(stepNo)
            {
                case 1:
                    CloneOneProcessStatus = "Wait";
                    RunCloneOneProcessButtonfloag = false;
                    await new Cloner(DesktopAppOnly.PathFinder.GetApplicationRoot()).TrigAsyncProc();
                    break;
                case 2:
                    CloneOneProcessStatus = "Wait";
                    RunCloneOneProcessButtonfloag = false;
                    await new Cloner(DesktopAppOnly.PathFinder.GetApplicationRoot()).TrigAsyncProc_2();
                    break;
                case 3:
                    CloneOneProcessStatus = "Wait";
                    RunCloneOneProcessButtonfloag = false;
                    await new Cloner(DesktopAppOnly.PathFinder.GetApplicationRoot()).TrigAsyncProc_4();
                    break;
                default:
                    break;
            }
           
        }

        public async Task StartSceduler()
        {
            TaskExecuterStatus = "Wait";
            RunTaskExecuterProcessButtonfloag = false;
             await new ExeTaskSC().TrigAsyncProc();
        }

        public void OnNavigatedTo()
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }
    }
}
