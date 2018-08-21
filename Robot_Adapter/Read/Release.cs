using System.Collections.Generic;
using RobotOM;
using BH.oM.Structural.Properties;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        private List<BarRelease> ReadBarRelease(List<string> ids = null)
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
 
    }

}

