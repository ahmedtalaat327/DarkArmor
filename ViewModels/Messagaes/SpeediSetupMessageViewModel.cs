using DarkArmor.Data;
using DarkArmor.Helpers;
using DarkArmor.Models.Skeleton;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.ViewModels.Messagaes
{
    public partial class SpeediSetupMessageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _unpackProcessStatus = "Sleep";

        [ObservableProperty]
        public ObservableCollection<string> _discoveredStatusForUnpacking = new ObservableCollection<string>();

        public async Task StartUnpacking()
        {
            UnpackProcessStatus = "Wait";
            await new Unpacker(DesktopAppOnly.PathFinder.GetApplicationRoot()).TrigAsyncProc();
        }
    }
}
