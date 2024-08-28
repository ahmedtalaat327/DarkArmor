using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.ViewModels.Messagaes
{
    public partial class SpeediSetupMessageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _unpackProcessStatus = "Sleep";
    }
}
