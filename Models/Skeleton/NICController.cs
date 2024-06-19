﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.Models.Skeleton
{
    public partial class NICController : ObservableObject
    {
        /// <summary>
        /// index depend on discovery order
        /// </summary>
        [ObservableProperty]
        public int _nic_ndex;
        /// <summary>
        /// MAC address captured within ARP reply
        /// </summary>
        [ObservableProperty]
        public string _physicalAdress;
        /// <summary>
        /// Man name gathered with http req through api from online service
        /// </summary>
        [ObservableProperty]
        public string _manufacture;
        /// <summary>
        /// ipv4 within ARP reply
        /// </summary>
        [ObservableProperty]
        public IPAddress _address;
    }
}