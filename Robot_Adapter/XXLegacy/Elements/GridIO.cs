/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

//using BH.oM.Structure.Elements;
//using RobotOM;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Robot_Adapter.Structural.Elements
//{
//    public class GridIO
//    {
//        public static bool SetGrids(RobotApplication Robot, List<Grid> grid, out List<string> ids)
//        {
//            RobotStructuralAxisGridMngr gm;
//            IRobotStructuralAxisGrid sag;
//            RobotStructuralAxisGridCartesian cartesianGrid = null;

//            gm = Robot.Project.AxisMngr;

//            if (gm.Count == 0)
//            {
//                gm.Create(IRobotStructuralAxisGridType.I_SAGT_CARTESIAN, "Structural Cartesian Axis");
//            }

//            sag = gm.Get(1);
//            if (sag.Type == IRobotStructuralAxisGridType.I_SAGT_CARTESIAN)
//            {
//                cartesianGrid = gm.Get(1) as RobotStructuralAxisGridCartesian;
//            }
//            int xCounter = 1;
//            int yCounter = 1;
//            double xPrevious = 0;
//            double yPrevious = 0;
//            string xLabel = "";
//            string yLabel = "";
//            grid.Sort(delegate (Grid g1, Grid g2)
//            {
//                double d1 = g1.Plane.Normal.X * 1000 + g1.Plane.Origin.X + g1.Plane.Normal.Y + g1.Plane.Origin.Y;
//                double d2 = g2.Plane.Normal.X * 1000 + g2.Plane.Origin.X + g2.Plane.Normal.Y + g2.Plane.Origin.Y;
//                return d1.CompareTo(d2);
//            });
//            for (int i = 0; i < grid.Count; i++)
//            {
//                try
//                {
//                    if (Math.Abs(grid[i].Plane.Normal.X) >= 0.99)
//                    {
//                        if (xCounter == 1)
//                        {
//                            cartesianGrid.X.StartPosition = grid[i].Plane.Origin.X;
//                            xPrevious = grid[i].Plane.Origin.X;
//                            xCounter++;
//                            xLabel = grid[i].Name;
//                        }
//                        else
//                        {
//                            cartesianGrid.X.AddSequence(grid[i].Plane.Origin.X - xPrevious, 1);
//                            cartesianGrid.X.SetAxisLabel(xCounter++, grid[i].Name);
//                            xPrevious = grid[i].Plane.Origin.X;
//                        }
//                    }
//                    else if (Math.Abs(grid[i].Plane.Normal.Y) >= 0.99)
//                    {
//                        if (yCounter == 1)
//                        {
//                            cartesianGrid.Y.StartPosition = grid[i].Plane.Origin.Y;
//                            yPrevious = grid[i].Plane.Origin.Y;
//                            yCounter++;
//                            yLabel = grid[i].Name;
//                        }
//                        else
//                        {
//                            cartesianGrid.Y.AddSequence(grid[i].Plane.Origin.Y - yPrevious, 1);
//                            cartesianGrid.Y.SetAxisLabel(yCounter++, grid[i].Name);
//                            yPrevious = grid[i].Plane.Origin.Y;
//                        }

//                    }
//                }
//                catch
//                {

//                }
//            }
//            if (grid.Count > 0)
//            {
//                cartesianGrid.X.SetAxisLabel(1, xLabel);
//                cartesianGrid.Y.SetAxisLabel(1, yLabel);
//                cartesianGrid.Save();
//            }

//            ids = null;
//            return true;
//        }

//        public static List<string> GetGrids(RobotApplication robot, out List<Grid> grids, List<string> ids)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}






