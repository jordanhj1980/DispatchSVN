using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DispatchApp
{
    public class propertyNodeItem
    {
        public int Tag { get; set; } 
        public string Icon { get; set; } 
        public string DisplayName { get; set; } 
        public string Name { get; set; } 
        public List<propertyNodeItem> Children { get; set; } 
        public propertyNodeItem() { Children = new List<propertyNodeItem>(); } 
    }
}
