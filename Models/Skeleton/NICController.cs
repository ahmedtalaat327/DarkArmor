 
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
        public int? _nic_index;
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
    }
    public partial class NICControllerAsString
    {
        [XmlElement("Nic_index")]
        public string Nic_index { get; set; }
        [XmlElement("Address")]
        public string Address { get; set; }
        [XmlElement("Mask")]
        public string Mask { get; set; }
        [XmlElement("Gate")]
        public string Gate { get; set; }
        [XmlElement("PhysicalAdress")]
        public string PhysicalAdress { get; set; }  

    }
}
