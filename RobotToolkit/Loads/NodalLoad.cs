using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    /// <summary>
    /// Nodal load objects
    /// </summary>
    public class NodalLoad
    {
        /// <summary>
        /// Create a nodal load displacement
        /// </summary>
        /// <param name="nodalLoads"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        //public static bool CreateNodalLoadDisplacement(BHoM.Structural.Loads.NodalLoad[] nodalLoads, string FilePath = "LiveLink")
        //{
        //    RobotApplication robot = null;
        //    if (FilePath == "LiveLink") robot = new RobotApplication();
        //    RobotCaseServer caseServer = robot.Project.Structure.Cases;

        //    Dictionary<int, BHoM.Structural.Loads.Loadcase> loadcases = new Dictionary<int, BHoM.Structural.Loads.Loadcase>();
        //    foreach (BHoM.Structural.Loads.NodalLoad nodalLoad in nodalLoads)
        //    {
        //        try { loadcases.Add(nodalLoad.Loadcase.Number, nodalLoad.Loadcase); }
        //        catch { }
        //    }
        //    RobotToolkit.Loadcase.CreateLoadcases(loadcases.Values.ToArray());
        //    caseServer.BeginMultiOperation();

        //    foreach (BHoM.Structural.Loads.NodalLoad nodalLoad in nodalLoads)
        //    {

        //        IRobotSimpleCase loadcase = (IRobotSimpleCase)robot.Project.Structure.Cases.Get(nodalLoad.Loadcase.Number);
        //        for (int i = 0; i < loadcase.Records.Count; i++)
        //        {
        //            loadcase.Records.Delete(i);
        //        }
        //        RobotLoadRecord loadRecord = null;
        //        //try
        //        //{
        //        //    loadcase.Records.Get(nodalLoad.RobotLoadRecordNumber);
        //        //}
        //        //catch
        //        //{ }

        //        if (loadRecord == null) loadRecord = (RobotLoadRecord)loadcase.Records.Create(IRobotLoadRecordType.I_LRT_NODE_DISPLACEMENT);

        //        if (nodalLoad.Translation != null)
        //        {
        //            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_FX, nodalLoad.Translation.X);
        //            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_FY, nodalLoad.Translation.Y);
        //            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_FZ, nodalLoad.Translation.Z);
        //        }

        //        if (nodalLoad.Rotation != null)
        //        {
        //            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_CX, nodalLoad.Rotation.X);
        //            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_CY, nodalLoad.Rotation.Y);
        //            loadRecord.SetValue((int)IRobotNodeForceRecordValues.I_NFRV_CZ, nodalLoad.Rotation.Z);
        //        }

        //        foreach (int loadCaseNumber in nodalLoad.NodeNumbers)
        //        {
        //            loadRecord.Objects.AddOne(loadCaseNumber);
        //        }
                
        //    }
        //    return true;
        //}

        //public static bool CreateNodalLoad(RobotApplication robot, int loadCaseId, string selString, LoadAxis axis, double value)
        //{
        //    double fx = axis == LoadAxis.X ? value : 0;
        //    double fy = axis == LoadAxis.Y ? value : 0;
        //    double fz = axis == LoadAxis.Z ? value : 0;
        //    double mx = axis == LoadAxis.XX ? value : 0;
        //    double my = axis == LoadAxis.YY ? value : 0;
        //    double mz = axis == LoadAxis.ZZ ? value : 0;

        //    return CreateNodalLoad(robot, loadCaseId, selString, fx, fy, fz, mx, my, mz);
        //}

    }
}
