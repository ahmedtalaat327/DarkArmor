 
using System.Net;
using System.Xml.Serialization;


namespace DarkArmor.Models.Skeleton
{
    public partial class NICController : ObservableObject
    {
        /// <summary>
        /// index depend on discovery order
        /// </summary>
        [ObservableProperty]
        public int? _nic_Index;
        /// <summary>
        /// interface name in freindly title
        /// </summary>
        [ObservableProperty]
        public string? _friendlyName;
        /// <summary>
        /// MAC address captured within ARP reply
        /// </summary>
        [ObservableProperty]
        public string? _physicalAdress;
        /// <summary>
        /// Man name gathered with http req through api from online service
        /// </summary>
        [ObservableProperty]
        public string? _manufacture;
        /// <summary>
        /// ipv4 within ARP reply
        /// </summary>
        [ObservableProperty]
        public IPAddress? _address;
        /// <summary>
        /// ipv4 mask
        /// </summary>
        [ObservableProperty]
        public IPAddress? _mask;
        /// <summary>
        /// ipv4 gate address
        /// </summary>
        [ObservableProperty]
        public IPAddress? _gate;
        /// <summary>
        /// ipv4 bc address
        /// </summary>
        [ObservableProperty]
        public IPAddress? _broadcast;
        /// <summary>
        /// etermines if this NIC is the real active one!
        /// </summary>
        [ObservableProperty]
        public bool? _Active;
    }
    public partial class NICControllerAsString
    {
        [XmlElement("Nic_Index")]
        public string Nic_Index { get; set; }
        [XmlElement("FriendlyName")]
        public string FriendlyName { get; set; }
        [XmlElement("Address")]
        public string Address { get; set; }
        [XmlElement("Mask")]
        public string Mask { get; set; }
        [XmlElement("Gate")]
        public string Gate { get; set; }
        [XmlElement("PhysicalAdress")]
        public string PhysicalAdress { get; set; }
        [XmlElement("Manufacture")]
        public string Manufacture { get; set; }
        [XmlElement("Broadcast")]
        public string Broadcast { get; set; }
        [XmlElement("Active")]
        public string Active { get; set; }



    }
}
