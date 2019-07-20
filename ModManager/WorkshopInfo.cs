using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModManager
{
    public class WorkshopInfo
    {

        public string Uuid { get; set; }

        public int Order { get; set; }

        public bool Active { get; set; }

        public string Game { get; set; }

        public string Packfile { get; set; }

        public string Name { get; set; }

        public string Short { get; set; }

        public string Category { get; set; }

        public bool Owned { get; set; }
    }
}
