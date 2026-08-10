using DarkArmor;
using DarkArmor.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DarkArmor.Data
{
    public class HardwareChecker : ObservableObject
    {
        private  readonly HttpClient client = new HttpClient();
        private string result = "no data";
        private string macAddress = "00:00:00:00:00:00";

        public HardwareChecker(string _macAddress, string _macManufacturer)
        {
            macAddress =_macAddress;
             result = _macManufacturer;
            
        }
        public async Task GetMacManufacturerAsync()
        {
            await Task.Run(async () =>
            {
                if (string.IsNullOrWhiteSpace(macAddress))
                    result = "Invalid MAC address.";

                // CORRECT URL STRUCTURE: 
                // Notice the 'api.' subdomain, the '/v2/macs/' path, and the trailing '/company/name'
                string apiUrl = $"https://api.maclookup.app/v2/macs/{Uri.EscapeDataString(macAddress)}/company/name";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Returns the clean, plain-text name of the vendor (e.g., "Intel Corporation")
                        result = await response.Content.ReadAsStringAsync();

                        foreach (var item in App.GetService<DashboardViewModel>().DiscoveredNICControllers)
                        {
                            if (item.PhysicalAdress.ToLower().Equals(macAddress))
                            {
                                item.Manufacture = result;
                            }
                        }
                        OnPropertyChanged("DiscoveredNICControllers");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        result = "Unknown Manufacturer.";
                    }

                    result = $"Error: Code {response.StatusCode}";
                }
                catch (Exception ex)
                {
                    result = $"Request failed: {ex.Message}";
                }


            }
            );
        }
    }
}
