using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationOptions.Test.Settings
{
    public class AppSettings : BaseOptions<AppSettings>
    {
        public DateTime LastLaunch { get; set; }
        public string ApiKey { get; set; }
        public string ApiEndpoint { get; set; }
    }
}
