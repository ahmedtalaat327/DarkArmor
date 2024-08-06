using DarkArmor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.Data
{
    public class ManARP
    {
        private string? url = null;
        private int index = 0;
        
        private int spoofed = 0;
        //we will get the gate fron this innstance
        private NetworkDevice? sacrifiicedDevice = null;


        public ManARP() { 
        
        }
    }
}
