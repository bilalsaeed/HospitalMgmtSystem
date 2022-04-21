using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Attributes
{
    public class ColumnNameAttribute : Attribute
    {

        // Provides name of the member
        private string name;

        // Constructor
        public ColumnNameAttribute(string name)
        {
            this.name = name;
        }

        // property to get name
        public string Name
        {
            get { return name; }
        }
    }
}
