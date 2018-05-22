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
      
        public bool CreateCollection(IEnumerable<IProperty2D> properties)
        {
            RobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel lable = null;
            string name = "";
            foreach (IProperty2D property in properties)
            {
                if (property is LoadingPanelProperty)
                {
                    name = BH.Engine.Robot.Convert.ThicknessProperty(lable, property);
                    lable = labelServer.CreateLike(IRobotLabelType.I_LT_CLADDING, property.Name, name);
                }

                else
                {
                    lable = labelServer.Create(IRobotLabelType.I_LT_PANEL_THICKNESS, name);
                    name = BH.Engine.Robot.Convert.ThicknessProperty(lable, property);
                }

                labelServer.StoreWithName(lable, name);
            }

            return true;
        }

        /***************************************************/

       
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

