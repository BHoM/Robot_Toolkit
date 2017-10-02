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
    
    #region Read Available Property Names
    public static List<string> ReadSectionPropertyNames(RobotApplication robotapp)
        {
            RobotNamesArray names = robotapp.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_BAR_SECTION);

            List<string> outPut = new List<string>();
            
            for (int i = 1; i <= names.Count; i++)
                outPut.Add(names.Get(i));

            return outPut;
        }

        public static List<string> ReadSupportNames(RobotApplication robotapp)
        {
            RobotNamesArray names = robotapp.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_SUPPORT);

            List<string> outPut = new List<string>();

            for (int i = 1; i <= names.Count; i++)
                outPut.Add(names.Get(i));

            return outPut;
        }

        public static List<string> ReadBarReleaseNames(RobotApplication robotapp)
        {
            RobotNamesArray names = robotapp.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_BAR_RELEASE);

            List<string> outPut = new List<string>();

            for (int i = 1; i <= names.Count; i++)
                outPut.Add(names.Get(i));

            return outPut;
        }

        public static List<string> ReadMaterialNames(RobotApplication robotapp)
        {
            RobotNamesArray names = robotapp.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_MATERIAL);

            List<string> outPut = new List<string>();

            for (int i = 1; i <= names.Count; i++)
                outPut.Add(names.Get(i));

            return outPut;
        }

        public static List<string> ReadMemberTypeNames(RobotApplication robotapp)
        {
            RobotNamesArray names = robotapp.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_MEMBER_TYPE);

            List<string> outPut = new List<string>();
                            
            for (int i = 1; i <= names.Count; i++)
                outPut.Add(names.Get(i));

            return outPut;
        }

        #endregion

        /***************************************************/
        /**** Adapter Methods                           ****/
        /***************************************************/

        protected override IEnumerable<BHoMObject> Read(Type type, List<object> indices = null)
        {
            // Define the dictionary of Read methods
            if (m_ReadMethods == null)
            {
                m_ReadMethods = new Dictionary<Type, Func<List<string>, IList>>()
                {
                    {typeof(Material), ReadMaterials },
                    {typeof(SectionProperty),  ReadSectionProperties },
                    {typeof(Node), ReadNodes },
                    {typeof(Bar), ReadBars }
                };
            }

            // Get the objects based on indices
            if (m_ReadMethods.ContainsKey(type))
            {
                return m_ReadMethods[type](indices as dynamic);
            }
            else
                return new List<BHoMObject>();
        }

        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        public List<Bar> ReadBars(List<string> ids = null)
        {
            IRobotCollection robotBars = this.RobotApplication.Project.Structure.Bars.GetAll();
            
            List<Bar> bhomBars = new List<Bar>();
            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.Name.ToString());

            this.RobotApplication.Project.Structure.Bars.BeginMultiOperation();
            for (int i = 1; i <= robotBars.Count; i++)
            {
                bhomBars.Add(Robot.Convert.ToBHoMObject(robotBars.Get(i) as dynamic, bhomNodes as dynamic));
            }

            this.RobotApplication.Project.Structure.Bars.EndMultiOperation();

            return bhomBars;
        }

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public List<Material> ReadMaterials(List<string> ids = null)
        {
            return null;
        }

        /***************************************************/
            
        public List<Node> ReadNodes(List<string> ids = null)
        {
            IRobotCollection robotNodes = this.RobotApplication.Project.Structure.Nodes.GetAll();
            List<Node> bhomNodes = new List<Node>();

            for (int i = 1;i<= robotNodes.Count; i++)
            {
                bhomNodes.Add(Robot.Convert.ToBHoMObject(robotNodes.Get(i)));
            }
            return bhomNodes;
        }

        /***************************************/

        public List<SectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            return null;
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private Dictionary<Type, Func<List<string>, IList>> m_ReadMethods = null;

        }

}

