using System.Collections.Generic;
using System.Linq;
using System;
using BH.Engine.Serialiser;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using RobotOM;

using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Index Adapter Interface                   ****/
        /***************************************************/
        
        /***************************************************/
        
        public bool CreateCollection<T>(IEnumerable<BH.oM.Base.BHoMGroup<T>> groups) where T : BH.oM.Base.IBHoMObject
        {

            RobotGroupServer rGroupServer = m_RobotApplication.Project.Structure.Groups;
            foreach (BHoMGroup<T> group in groups)
            {
                IRobotObjectType rType = BH.Engine.Robot.Convert.RobotObjectType(typeof(T));
                string members = group.Elements.Select(x => int.Parse(x.CustomData[BH.Engine.Robot.Convert.AdapterID].ToString())).GeterateIdString();
                rGroupServer.Create(rType, group.Name, members);
            }

            return true;
        }

        /***************************************************/

     
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

