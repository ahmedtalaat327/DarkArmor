using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.Data
{
    public class Cloner : ObservableObject
    {
        private string? url = null;


        private ObservableCollection<string> resOfCloning { get; set; } = new ObservableCollection<string>();

        CancellationTokenSource? cts;
    }
}
