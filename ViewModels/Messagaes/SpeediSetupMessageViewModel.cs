using DarkArmor.Data;
using DarkArmor.Helpers;
using System.Collections.ObjectModel;
 
namespace DarkArmor.ViewModels.Messagaes
{
    public partial class SpeediSetupMessageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _unpackProcessStatus = "Sleep";

        [ObservableProperty]
        private bool _runUnpackingProcessButtonfloag = true;
       
        public async Task StartUnpacking()
        {
            UnpackProcessStatus = "Wait";
            RunUnpackingProcessButtonfloag = false;
            await new Unpacker(DesktopAppOnly.PathFinder.GetApplicationRoot()).TrigAsyncProc();
        }
    }
}
