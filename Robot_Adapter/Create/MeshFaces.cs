using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using RobotOM;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/

        /***************************************************/

        private bool CreateCollection(IEnumerable<MeshFace> meshFaces)
        {
            int nbOfDistinctProps = meshFaces.Select(x => x.Property).Distinct(Comparer<IProperty2D>()).Count();
            if (nbOfDistinctProps == 1)
            {
                string faceList = "";
                IRobotStructure objServer = m_RobotApplication.Project.Structure;
                RobotObjObject mesh = null;
                foreach (MeshFace face in meshFaces)
                {
                    IRobotNumbersArray ptarray = new RobotNumbersArray();
                    if (face.Nodes.Count == 3)
                    {
                        ptarray.SetSize(3);
                        ptarray.Set(1, System.Convert.ToInt32(face.Nodes[0].CustomData[AdapterId]));
                        ptarray.Set(2, System.Convert.ToInt32(face.Nodes[1].CustomData[AdapterId]));
                        ptarray.Set(3, System.Convert.ToInt32(face.Nodes[2].CustomData[AdapterId]));
                    }
                    else
                    {
                        ptarray.SetSize(4);
                        ptarray.Set(1, System.Convert.ToInt32(face.Nodes[0].CustomData[AdapterId]));
                        ptarray.Set(2, System.Convert.ToInt32(face.Nodes[1].CustomData[AdapterId]));
                        ptarray.Set(3, System.Convert.ToInt32(face.Nodes[2].CustomData[AdapterId]));
                        ptarray.Set(4, System.Convert.ToInt32(face.Nodes[3].CustomData[AdapterId]));
                    }
                    int faceNum = System.Convert.ToInt32(face.CustomData[AdapterId]);
                    RobotNumbersArray ptArray = ptarray as RobotNumbersArray;
                    faceList = faceList + faceNum.ToString() + ",";

                    objServer.FiniteElems.Create(faceNum, ptArray);
                }
                faceList.TrimEnd(',');

                objServer.Objects.CreateOnFiniteElems(faceList, objServer.Objects.FreeNumber);
                mesh = objServer.Objects.Get(objServer.Objects.FreeNumber - 1) as RobotObjObject;
                //mesh.Main.Attribs.Meshed = 1;
                //mesh.Update();
                if (meshFaces.First().Property is LoadingPanelProperty)
                    mesh.SetLabel(IRobotLabelType.I_LT_CLADDING, meshFaces.First().Property.Name);

                else
                    mesh.SetLabel(IRobotLabelType.I_LT_PANEL_THICKNESS, meshFaces.First().Property.Name);

                return true;
            }

            else
            {
                throw new Exception("All meshFaces should have the same property");
            }
        }

        /***************************************************/

    }

}

