using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using RobotOM;
using BH.oM.Structural.Properties;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.Materials;
using BH.Adapter.Queries;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/


        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;
            if (typeof(BH.oM.Structural.Design.DesignGroup).IsAssignableFrom(typeof(T)))
            {
                foreach (T obj in objects)
                {
                    CreateSteelDesignGroup(obj as BH.oM.Structural.Design.DesignGroup);
                }
            }
            if (typeof(BH.oM.Structural.Elements.Bar).IsAssignableFrom(typeof(T)))
            {
                Create(objects.ToList() as List<BH.oM.Structural.Elements.Bar>, replaceAll);
            }
            return success;
        }

        public void Create(List<Node> bhomNodes, bool replaceAll = false)
        {
            Convert.FromBHoMObject(this, bhomNodes);
        }

        public void Create(List<Bar> bhomBars )
        {
            //List<BH.oM.Structural.Elements.Node> bhomNodes = new List<Node>();
            //foreach(Bar bhomBar in bhomBars)
            //{                
            //    bhomNodes.Add(bhomBar.StartNode);
            //    bhomNodes.Add(bhomBar.EndNode);
            //}
            //Create(bhomNodes);

            //RobotApplication robot = this.RobotApplication;
            //RobotStructureCache rcache = robot.Project.Structure.CreateCache();
            
            //foreach (Bar bhomBar in bhomBars)
            //{
                
            //    rcache.AddBar(bhomBar.Name, bh)
            // }
        }

        public void CreateSteelDesignGroup(BH.oM.Structural.Design.DesignGroup bhomdesignGroup)
        {
            RobotApplication robot = this.RobotApplication;
            RDimServer RDServer = this.RobotApplication.Kernel.GetExtension("RDimServer");
            RDServer.Mode = RobotOM.IRDimServerMode.I_DSM_STEEL;
            RDimStream RDStream = RDServer.Connection.GetStream();
            RDimGroups RDGroups = RDServer.GroupsService;
            RDimGrpProfs RDGroupProfs = RDServer.Connection.GetGrpProfs();
            RDimGroup designGroup =  RDGroups.New(0, bhomdesignGroup.Number);
            designGroup.Name = bhomdesignGroup.Name;
            designGroup.Material = bhomdesignGroup.MaterialName;
            RDStream.Clear();
            RDStream.WriteText(Convert.FromSelectionList(bhomdesignGroup.MemberIds));
            designGroup.SetMembList(RDStream);      
            designGroup.SetProfs(RDGroupProfs);
            RDGroups.Save(designGroup);        
        }
     
        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

    }

}

