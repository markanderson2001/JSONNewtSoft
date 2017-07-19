using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONNewtSoft
{
    public class person
    {
        public string firstName;
        public string lasttName;
        public string height;
        public List<string> Friends = new List<string>();
        public Dictionary<string, string> contact = new Dictionary<string, string>();

    }
}
