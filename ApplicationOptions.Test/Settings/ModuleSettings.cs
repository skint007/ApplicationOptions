using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationOptions.Test.Settings
{
    public class ModuleSettings : BaseOptions<ModuleSettings>
    {
        public string BaseModulePath { get; set; }
        public List<string> Modules { get; set; } = new();
    }
}
