using DarkArmor.Models.Skeleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DarkArmor.Models
{
    public partial class NetworkDevice :  ObservableObject 
    {
        [ObservableProperty]
        public int _deviceIndex = 0;
        [ObservableProperty]
        public string _domainName = "UserName";
        [ObservableProperty]
        [Browsable(false)]
        public string _oSName = "Unknown"; 
        [ObservableProperty]
        [Browsable(false)]
        public DeviceType _type = DeviceType.UDevice;
        [ObservableProperty]
        [Browsable(false)]
        public bool _active = true;
        [ObservableProperty]
        [Browsable(false)]
        public NICController _nic = new NICController() { Nic_Index = 0,Address = IPAddress.Parse("192.168.0.0"), Mask = IPAddress.Parse("255.255.0.0") ,Manufacture = "some trading manf", PhysicalAdress = "00:00:00:00"};
         
      
    }
}
