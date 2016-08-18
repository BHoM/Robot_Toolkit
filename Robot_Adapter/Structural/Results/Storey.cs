using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;

namespace Robot_Adapter.Results
{
    class Storey
    {
        public void GetStoreyResults(
            BHoM.Structural.Loads.Loadcase loadcase,
            BHoM.Structural.Elements.Storey storey,
            out BHoM.Structural.Results.StoreyResult storeyResult,  
            string FilePath = "LiveLink")
        {
            RobotApplication robot = null;
            if (FilePath == "LiveLink") robot = new RobotApplication();

            storeyResult = new BHoM.Structural.Results.StoreyResult(loadcase);
            RobotStoreyResultServer robotResult = robot.Project.Structure.Results.Storeys;
            //RobotStoreyDisplacements robotDisplacements = robotResult.Displacements(storey.Number, loadcase.Number);
            //RobotStoreyReducedForces robotForces = robotResult.ReducedForces(storey.Number, loadcase.Number);
            //RobotStoreyValues robotValues = robotResult.Values(storey.Number, loadcase.Number);

            //storeyResult.SetDrift(robotDisplacements.DrUX, robotDisplacements.DrUY);
            //storeyResult.SetDriftRatio(robotDisplacements.DrUX / storey.Height, robotDisplacements.DrUY / storey.Height);
            //storeyResult.SetMaximumNodalDisplacements(robotDisplacements.NodeMaxUX, robotDisplacements.NodeMaxUY);
            //storeyResult.SetMinimumNodalDisplacements(robotDisplacements.NodeMinUX, robotDisplacements.NodeMinUY);
            //storeyResult.SetShear(robotForces.FX, robotForces.FY);
            //storeyResult.SetShearDistribution(robotForces.FX_ToColumns, robotForces.FY_ToColumns, robotForces.FX_ToWalls, robotForces.FY_ToWalls);
            //storeyResult.SetAxialDistribution(robotForces.FZ_ToColumns, robotForces.FZ_ToWalls);
            //storeyResult.SetCentreOfGravity(robotValues.G.X, robotValues.G.Y, robotValues.G.Z);
            //storeyResult.SetCentreOfRigidity(robotValues.R.X, robotValues.R.Y, robotValues.R.Z);
            //storeyResult.SetMomentOfIntertia(robotValues.Ix, robotValues.Iy, robotValues.Iz);
            //storeyResult.SetSeismicMass(robotValues.Mass, robotValues.Mass, robotValues.Mass);
        }
    }
}
