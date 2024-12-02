using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _492989_Daniel_Anantadhirya_A.L._ResponsiJuniorProject.Utils
{
    internal abstract class IdMapper
    {
        public abstract int getId(string value);
    }

    internal class DepartmentMapper : IdMapper
    {
        public override int getId(string value)
        {
            switch (value)
            {
                case "HR": return 1;
                case "Engineer": return 2;
                case "Developer": return 3;
                case "Product M": return 4;
                case "Finance": return 5;
                default: return -1;
            }
        }
    }

    internal class JabatanMapper : IdMapper
    {
        public override int getId(string value)
        {
            switch (value)
            {
                case "Intern": return 1;
                case "Junior": return 2;
                case "Senior": return 3;
                default: return -1;
            }
        }
    }
}
