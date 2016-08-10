using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace Robot_Adapter.Base
{
    /// <summary>
    /// Robot label object - to manage all label objects to/from Robot
    /// </summary>
    public class Label
    {
        /// <summary>
        /// Get all section property label names from a Robot model
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        public static string[] GetAllSectionPropertyNames(RobotApplication robot)
        {
            RobotNamesArray names = robot.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_BAR_SECTION);

            string[] outPut = new string[names.Count];

            for (int i = 1; i <= names.Count; i++)
                outPut[i - 1] = names.Get(i);

            return outPut;
        }

        /// <summary>
        /// Get all support label names from a Robot model
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        public static string[] GetAllSupportNames(RobotApplication robot)
        {
            RobotNamesArray names = robot.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_SUPPORT);

            string[] outPut = new string[names.Count];

            for (int i = 1; i <= names.Count; i++)
                outPut[i - 1] = names.Get(i);

            return outPut;
        }

        /// <summary>
        /// Get all release label names from a Robot model
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        public static string[] GetAllReleaseNames(RobotApplication robot)
        {
            RobotNamesArray names = robot.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_BAR_RELEASE);

            string[] outPut = new string[names.Count];

            for (int i = 1; i <= names.Count; i++)
                outPut[i - 1] = names.Get(i);

            return outPut;
        }

        /// <summary>
        /// Get all material label names from a Robot model
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        public static string[] GetAllMaterialNames(RobotApplication robot)
        {
            RobotNamesArray names = robot.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_MATERIAL);

            string[] outPut = new string[names.Count];

            for (int i = 1; i <= names.Count; i++)
                outPut[i - 1] = names.Get(i);

            return outPut;
        }

        /// <summary>
        /// Get all member type names from a Robot model
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        public static string[] GetAllBarMemberTypeNames(RobotApplication robot)
        {
            RobotNamesArray names = robot.Project.Structure.Labels.GetAvailableNames(IRobotLabelType.I_LT_MEMBER_TYPE);

            string[] outPut = new string[names.Count];

            for (int i = 1; i <= names.Count; i++)
                outPut[i - 1] = names.Get(i);

            return outPut;
        }

        /// <summary>
        /// Get all section properties from a Robot model
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="uniqueIds"></param>
        /// <param name="names"></param>
        /// <param name="materialNames"></param>
        /// <param name="types"></param>
        /// <param name="shapeTypes"></param>
        /// <param name="D"></param>
        /// <param name="BF"></param>
        /// <param name="BF2"></param>
        /// <param name="tw"></param>
        /// <param name="tf"></param>
        /// <param name="tf2"></param>
        /// <param name="Ax"></param>
        /// <param name="Ixx"></param>
        /// <param name="Iyy"></param>
        /// <param name="Izz"></param>
        /// <param name="mass"></param>
        /// <param name="aliases"></param>
        /// <returns></returns>
        public static bool GetAllSectionProperties(RobotApplication robot, out int[] uniqueIds, out string[] names, out string[] materialNames,
           out IRobotBarSectionType[] types, out IRobotBarSectionShapeType[] shapeTypes, out double[] D, out double[] BF, out double[] BF2,
            out double[] tw, out double[] tf, out double[] tf2, out double[] Ax, out double[] Ixx, out double[] Iyy, out double[] Izz, out double[] mass, out string[][] aliases)
        {
            IRobotCollection collection = robot.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_BAR_SECTION);
            int n = collection.Count;

            uniqueIds = new int[n];
            names = new string[n];
            materialNames = new string[n];
            types = new IRobotBarSectionType[n];
            shapeTypes = new IRobotBarSectionShapeType[n];
            D = new double[n];
            BF = new double[n];
            BF2 = new double[n];
            tw = new double[n];
            tf = new double[n];
            tf2 = new double[n];
            Ax = new double[n];
            Ixx = new double[n];
            Iyy = new double[n];
            Izz = new double[n];
            mass = new double[n];
            aliases = new string[n][];



            for (int i = 0; i < n; i++)
            {
                IRobotLabel section = (IRobotLabel)collection.Get(i + 1);
                IRobotBarSectionData data = (IRobotBarSectionData)section.Data;

                uniqueIds[i] = section.UniqueId;
                names[i] = section.Name;
                materialNames[i] = data.MaterialName;

                types[i] = data.Type;
                shapeTypes[i] = data.ShapeType;


                D[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
                BF[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
                BF2[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF2);
                tw[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
                tf[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
                tf2[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF2);
                Ax[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_AX);
                Ixx[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_IX);
                Iyy[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_IY);
                Izz[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_IZ);
                mass[i] = data.GetValue(IRobotBarSectionDataValue.I_BSDV_WEIGHT);



                string[] tempAlias = new string[data.AliasCount];
                for (int j = 0; j < data.AliasCount; j++)
                {
                    tempAlias[j] = data.GetAlias(j + 1);
                }

                aliases[i] = tempAlias;

            }

            return true;
        }

        /// <summary>
        /// Get all material properties from a Robot model
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="ids"></param>
        /// <param name="names"></param>
        /// <param name="types"></param>
        /// <param name="E"></param>
        /// <param name="nu"></param>
        /// <param name="G"></param>
        /// <param name="density"></param>
        /// <param name="alpha"></param>
        /// <param name="dampingRatio"></param>
        /// <param name="fy"></param>
        /// <param name="fu"></param>
        /// <returns></returns>
        public static bool GetAllMaterials(RobotApplication robot, out int[] ids, out string[] names, out IRobotMaterialType[] types, out double[] E, out double[] nu,
            out double[] G, out double[] density, out double[] alpha, out double[] dampingRatio, out double[] fy, out double[] fu)
        {
            IRobotCollection collection = robot.Project.Structure.Labels.GetMany(IRobotLabelType.I_LT_MATERIAL);

            int n = collection.Count;

            ids = new int[n];
            names = new string[n];
            types = new IRobotMaterialType[n];
            E = new double[n];
            nu = new double[n];
            G = new double[n];
            density = new double[n];
            alpha = new double[n];
            dampingRatio = new double[n];
            fu = new double[n];
            fy = new double[n];


            for (int i = 0; i < n; i++)
            {
                IRobotLabel label = (IRobotLabel)collection.Get(i + 1); // 1-based index
                IRobotMaterialData data = (IRobotMaterialData)label.Data;

                ids[i] = label.UniqueId;
                names[i] = label.Name;
                types[i] = data.Type;
                E[i] = data.E;
                nu[i] = data.NU;
                G[i] = data.Kirchoff; // i'm not sure which property is the right one for this, but I do know the formula for it. #GG 20150220 
                density[i] = data.RO;
                alpha[i] = data.LX;
                dampingRatio[i] = data.DumpCoef; // hehehe, silly polish guys. #GG 20150220 
                fy[i] = data.RE;
                fu[i] = data.RT;

            }

            return true;
        }

        /// <summary>
        /// Create a Robot section property label from a section database
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="name"></param>
        /// <param name="mat"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static bool CreateSectionPropertyFromDatabase(RobotApplication robot, string name, string mat, string searchString)
        {

            IRobotLabel section = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, name);

            IRobotBarSectionData data = (IRobotBarSectionData)section.Data;

            bool success = Convert.ToBoolean(data.LoadFromDBase(searchString));
            data.MaterialName = mat;

            if (success)
                robot.Project.Structure.Labels.Store(section);

            return success;
        }

        /// <summary>
        /// Create an RHS section property label 
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="name"></param>
        /// <param name="mat"></param>
        /// <param name="D"></param>
        /// <param name="B"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool CreateSectionPropertyRHS(RobotApplication robot, string name, string mat, double D, double B, double t)
        {
            IRobotLabel section = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, name);
            IRobotBarSectionData sectionData = (IRobotBarSectionData)section.Data;


            sectionData.Type = IRobotBarSectionType.I_BST_NS_RECT;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_RECT;

            sectionData.MaterialName = mat;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H, D);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B, B);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T, t);

            sectionData.CalcNonstdGeometry();

            robot.Project.Structure.Labels.Store(section);

            return true;
        }

        /// <summary>
        /// Create a CHS section property label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="name"></param>
        /// <param name="mat"></param>
        /// <param name="D"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool CreateSectionPropertyCHS(RobotApplication robot, string name, string mat, double D, double t)
        {
            IRobotLabel section = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, name);
            IRobotBarSectionData sectionData = (IRobotBarSectionData)section.Data;


            sectionData.Type = IRobotBarSectionType.I_BST_NS_TUBE;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_TUBE;

            sectionData.MaterialName = mat;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D, D);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T, t);

            sectionData.CalcNonstdGeometry();

            robot.Project.Structure.Labels.Store(section);

            return true;
        }

        /// <summary>
        /// Create a tee section property label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="name"></param>
        /// <param name="mat"></param>
        /// <param name="D"></param>
        /// <param name="B"></param>
        /// <param name="tw"></param>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static bool CreateSectionPropertyT(RobotApplication robot, string name, string mat, double D, double B, double tw, double tf)
        {
            IRobotLabel section = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, name);
            IRobotBarSectionData sectionData = (IRobotBarSectionData)section.Data;


            sectionData.Type = IRobotBarSectionType.I_BST_NS_T;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE;

            sectionData.MaterialName = mat;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B, B);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H, D);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF, tw);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW, tf);

            sectionData.CalcNonstdGeometry();

            robot.Project.Structure.Labels.Store(section);

            return true;
        }

        /// <summary>
        /// Create a box section property label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="name"></param>
        /// <param name="mat"></param>
        /// <param name="D"></param>
        /// <param name="B"></param>
        /// <param name="tw"></param>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static bool CreateSectionPropertyBox(RobotApplication robot, string name, string mat, double D, double B, double tw, double tf)
        {
            IRobotLabel section = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, name);
            IRobotBarSectionData sectionData = (IRobotBarSectionData)section.Data;


            sectionData.Type = IRobotBarSectionType.I_BST_NS_BOX;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_BOX;

            sectionData.MaterialName = mat;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_B, B);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H, D);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW, tw);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TF, tf);

            sectionData.CalcNonstdGeometry();

            robot.Project.Structure.Labels.Store(section);

            return true;
        }

        /// <summary>
        /// Create an I section property label with bi-symmetry
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="name"></param>
        /// <param name="mat"></param>
        /// <param name="D"></param>
        /// <param name="B"></param>
        /// <param name="tw"></param>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static bool CreateSectionPropertyIBiSymmetric(RobotApplication robot, string name, string mat, double D, double B, double tw, double tf)
        {
            IRobotLabel section = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_SECTION, name);
            IRobotBarSectionData sectionData = (IRobotBarSectionData)section.Data;


            sectionData.Type = IRobotBarSectionType.I_BST_NS_I;
            sectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;

            sectionData.MaterialName = mat;

            IRobotBarSectionNonstdData nonStdData = sectionData.CreateNonstd(0);

            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B, B);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H, D);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW, tw);
            nonStdData.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF, tf);

            sectionData.CalcNonstdGeometry();

            robot.Project.Structure.Labels.Store(section);

            return true;
        }

        /// <summary>
        /// Create a release label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="startRelease"></param>
        /// <param name="endRelease"></param>
        /// <returns></returns>
        public static bool CreateReleaseLabel(RobotApplication robot, BHoM.Structural.Properties.BarRelease startRelease, BHoM.Structural.Properties.BarRelease endRelease)
        {
            IRobotLabel robot_release = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_BAR_RELEASE, startRelease.Name + "-" + endRelease.Name);
            if (robot.Project.Structure.Labels.Exist(IRobotLabelType.I_LT_SUPPORT, startRelease.Name + "-" + endRelease.Name) == 1)
            {
                robot_release = robot.Project.Structure.Labels.Get(IRobotLabelType.I_LT_SUPPORT, startRelease.Name + "-" + endRelease.Name);
            }

            
            //data.StartNode.UX = startRelease.X;
            //data.StartNode.UY = startRelease.Y;
            //data.StartNode.UZ = startRelease.Z;

            //data.StartNode.UX = (startRelease.X == -1)? IRobotBarEndReleaseValue.I_BERV_STD : (startRelease.X == 0)? IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.StartNode.UY = startReleases[1] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.StartNode.UZ = startReleases[2] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.StartNode.RX = startReleases[3] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.StartNode.RY = startReleases[4] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.StartNode.RZ = startReleases[5] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;

            //data.EndNode.UX = endReleases[0] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.EndNode.UY = endReleases[1] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.EndNode.UZ = endReleases[2] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.EndNode.RX = endReleases[3] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.EndNode.RY = endReleases[4] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;
            //data.EndNode.RZ = endReleases[5] ? IRobotBarEndReleaseValue.I_BERV_STD : IRobotBarEndReleaseValue.I_BERV_NONE;

            //robot.Project.Structure.Labels.Store(release);

            return true;
        }

        /// <summary>
        /// Create a material label
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="name"></param>
        /// <param name="materialType"></param>
        /// <param name="E"></param>
        /// <param name="nu"></param>
        /// <param name="G"></param>
        /// <param name="density"></param>
        /// <param name="alpha"></param>
        /// <param name="dampingRatio"></param>
        /// <param name="fy"></param>
        /// <param name="fu"></param>
        /// <returns></returns>
        public static bool CreateMaterialLabel(RobotApplication robot, string name, IRobotMaterialType materialType,
     double E, double nu, double G, double density, double alpha, double dampingRatio, double fy, double fu)
        {
            IRobotLabel material = robot.Project.Structure.Labels.Create(IRobotLabelType.I_LT_MATERIAL, name);
            IRobotMaterialData data = (IRobotMaterialData)material.Data;

            data.Type = materialType;
            data.E = E;
            data.NU = nu;
            data.Kirchoff = G;
            data.RO = density * 9.81;
            data.LX = alpha;
            data.DumpCoef = dampingRatio;
            data.RE = fy;
            data.RT = fu;

            robot.Project.Structure.Labels.Store(material);

            return true;
        }

    }
}
