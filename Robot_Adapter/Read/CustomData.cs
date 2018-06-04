using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.Engine.Serialiser;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Base;
using BH.oM.Common.Materials;
using BH.oM.Structural.Design;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {         
        
        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/

        
        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        public List<BHoMObject> ReadCustomData(List<string> ids = null)
        {

            List<BHoMObject> obj_list = new List<BHoMObject>();
            obj_list.AddRange(ReadFramingElementDesignProperties());

            return obj_list;
        }

      

        /***************************************************/

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

