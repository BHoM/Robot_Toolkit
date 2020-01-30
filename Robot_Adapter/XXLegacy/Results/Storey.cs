/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using RobotOM;

//namespace Robot_Adapter.Results
//{
//    class Storey
//    {
//        public void GetStoreyResults(
//            BH.oM.Structure.Loads.Loadcase loadcase,
//            BH.oM.Structure.Elements.Storey storey,
//            out BH.oM.Structure.Results.StoreyResult storeyResult,  
//            string FilePath = "LiveLink")
//        {
//            RobotApplication robot = null;
//            if (FilePath == "LiveLink") robot = new RobotApplication();

//            storeyResult = new BH.oM.Structure.Results.StoreyResult(loadcase);
//            RobotStoreyResultServer robotResult = robot.Project.Structure.Results.Storeys;
//            //RobotStoreyDisplacements robotDisplacements = robotResult.Displacements(storey.Number, loadcase.Number);
//            //RobotStoreyReducedForces robotForces = robotResult.ReducedForces(storey.Number, loadcase.Number);
//            //RobotStoreyValues robotValues = robotResult.Values(storey.Number, loadcase.Number);

//            //storeyResult.SetDrift(robotDisplacements.DrUX, robotDisplacements.DrUY);
//            //storeyResult.SetDriftRatio(robotDisplacements.DrUX / storey.Height, robotDisplacements.DrUY / storey.Height);
//            //storeyResult.SetMaximumNodalDisplacements(robotDisplacements.NodeMaxUX, robotDisplacements.NodeMaxUY);
//            //storeyResult.SetMinimumNodalDisplacements(robotDisplacements.NodeMinUX, robotDisplacements.NodeMinUY);
//            //storeyResult.SetShear(robotForces.FX, robotForces.FY);
//            //storeyResult.SetShearDistribution(robotForces.FX_ToColumns, robotForces.FY_ToColumns, robotForces.FX_ToWalls, robotForces.FY_ToWalls);
//            //storeyResult.SetAxialDistribution(robotForces.FZ_ToColumns, robotForces.FZ_ToWalls);
//            //storeyResult.SetCentreOfGravity(robotValues.G.X, robotValues.G.Y, robotValues.G.Z);
//            //storeyResult.SetCentreOfRigidity(robotValues.R.X, robotValues.R.Y, robotValues.R.Z);
//            //storeyResult.SetMomentOfIntertia(robotValues.Ix, robotValues.Iy, robotValues.Iz);
//            //storeyResult.SetSeismicMass(robotValues.Mass, robotValues.Mass, robotValues.Mass);
//        }
//    }
//}

