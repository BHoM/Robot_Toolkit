using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.oM.Adapters.Robot
{
    public class DesignGroup : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public int Number { get; set; } = 0;

        public string MaterialName { get; set; } = "";

        public List<int> MemberIds { get; set; } = new List<int>();

        /***************************************************/
    }
}
