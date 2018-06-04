using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Base;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BHoMObject CutomData(this BHoMObject bhomObj, string key = "", object obj = null)
        {
            BHoMObject new_bhomObj = bhomObj.GetShallowClone() as BHoMObject;
            Dictionary<string, object> customData = bhomObj.CustomData;
            if (key != "" && obj != null)
            {
                customData[key] = obj;
            }
            return bhomObj;
        }
    }
}
