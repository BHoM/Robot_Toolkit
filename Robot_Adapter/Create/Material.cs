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

        private bool CreateCollection(IEnumerable<Material> mat)
        {
            IRobotLabel label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_MATERIAL, "");
            IRobotMaterialData matData = label.Data;

            foreach (Material m in mat)
            {
                string match = BH.Engine.Robot.Convert.Match(m_dbMaterialNames, m);
                if (match != null)
                {
                    matData.LoadFromDBase(match);
                    m_RobotApplication.Project.Structure.Labels.StoreWithName(label, match);
                }
                else
                {
                    BH.Engine.Robot.Convert.RobotMaterial(matData, m);
                    m_RobotApplication.Project.Structure.Labels.StoreWithName(label, m.Name);
                }
            }
            return true;
        }

        /***************************************************/

      
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

