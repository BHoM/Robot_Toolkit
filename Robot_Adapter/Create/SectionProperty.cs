﻿using System.Collections.Generic;
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

        private bool CreateCollection(IEnumerable<ISectionProperty> secProp)
        {
            foreach (ISectionProperty p in secProp)
            {
                IRobotLabel label;
                IRobotBarSectionData secData;
                string match = BH.Engine.Robot.Convert.Match(m_dbSecPropNames, p);
                if (match != null)
                {
                    label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, match);
                    secData = label.Data;
                    string matName = BH.Engine.Robot.Convert.Match(m_dbMaterialNames, p.Material);
                    if (matName == null)
                        matName = p.Material.Name;
                    
                    secData.LoadFromDBase(match);
                    secData.MaterialName = matName;
                    m_RobotApplication.Project.Structure.Labels.Store(label);
                }

                else
                {
                    label = m_RobotApplication.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, p.Name);
                    secData = label.Data;
                    BH.Engine.Robot.Convert.ISectionProperty(p, secData);
                    m_RobotApplication.Project.Structure.Labels.Store(label);
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

