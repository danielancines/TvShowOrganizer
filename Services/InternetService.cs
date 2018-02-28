using Labs.WPF.TvShowOrganizer.Services.Contracts;
using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Labs.WPF.TvShowOrganizer.Services
{
    public class InternetService : IInternetService
    {
        public bool HasConnection()
        {
            bool stats;
            if (NetworkInterface.GetIsNetworkAvailable() == true)
            {
                stats = true;
            }
            else
            {
                stats = false;
            }
            return stats;
        }

        public bool HasInternetConnection()
        {
            try
            {
                byte[] result;
                using (var client = new WebClient())
                {
                    result = client.DownloadData("https://www.google.com");
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
