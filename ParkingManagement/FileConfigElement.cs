using System;
using System.Configuration;

namespace ParkingManagement
{
    public class FileConfigElement : System.Configuration.ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name {
            get
            {
                return base["Name"] as string;
            }
        }
    }
}
