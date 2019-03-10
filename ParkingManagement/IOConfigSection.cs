using System;
using System.Configuration;

namespace ParkingManagement
{
    public class IOConfigSection : System.Configuration.ConfigurationSection
    {
        public const string SECTION_NAME = "IOSectionConfig";
        
        [ConfigurationProperty("InputFile")]
        public FileConfigElement InputFile
        {
            get
            {
                return base["InputFile"] as FileConfigElement;
            }
        }
        [ConfigurationProperty("OutputFile")]
        public FileConfigElement OutputFile
        {
            get
            {
                return base["OutputFile"] as FileConfigElement;
            }
        }
    }
}
