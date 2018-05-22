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
using BH.oM.Adapters.Robot.Properties;
using BHE = BH.Engine.Adapters.Robot.Properties;

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
             
        /***************************************************/
        
        public List<BarRelease> ReadBarRelease(List<string> ids = null)
        {
            IRobotCollection releaseCollection = m_RobotApplication.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_RELEASE);
            List<BarRelease> bhomReleases = new List<BarRelease>();

            for (int i = 1; i <= releaseCollection.Count; i++)
            {
                IRobotLabel rReleaseLabel = releaseCollection.Get(i);
                IRobotBarReleaseData rReleaseData = rReleaseLabel.Data as IRobotBarReleaseData;
                BarRelease bhomMBarRelease = new BarRelease
                {
                    Name = rReleaseLabel.Name,
                    StartRelease = BH.Engine.Robot.Convert.BHoMRelease(rReleaseData.StartNode),
                    EndRelease = BH.Engine.Robot.Convert.BHoMRelease(rReleaseData.EndNode)
                };
                bhomMBarRelease.CustomData.Add(AdapterId, rReleaseLabel.Name);
                bhomReleases.Add(bhomMBarRelease);
            }
            return bhomReleases;
        }

        /***************************************************/        
        

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

