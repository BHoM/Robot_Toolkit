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

using RobotOM;
using BH.oM.Structure.Elements;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using System;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void SetFEAType(IRobotBar rBar, Bar bhomBar)
        {
            if (bhomBar.FEAType == BarFEAType.CompressionOnly)
            {
                rBar.TensionCompression = IRobotBarTensionCompression.I_BTC_COMPRESSION_ONLY;
            }
            if (bhomBar.FEAType == BarFEAType.TensionOnly)
            {
                rBar.TensionCompression = IRobotBarTensionCompression.I_BTC_TENSION_ONLY;
            }
            if (bhomBar.FEAType == BarFEAType.Axial)
            {
                rBar.TrussBar = true;
            }
        }

        /***************************************************/

        public static double ToRobotOrientationAngle(this Bar bhomBar)
        {

            if (bhomBar.StartNode == null || bhomBar.StartNode.Position == null || bhomBar.EndNode == null || bhomBar.EndNode.Position == null)
            {
                Engine.Reflection.Compute.RecordWarning("At least one of a bars end nodes or end nodes positions are null. Could not check orientation angle convention between Robot and BHoM. BHoM value will be used as is.");
                return bhomBar.OrientationAngle * 180 / Math.PI;
            }

            //Check vertical status
            bool bhomVertical = bhomBar.IsVertical();
            bool robotVertical = bhomBar.IsVerticalRobot();

            double orientationAngle;
            if (bhomVertical == robotVertical)
            {
                orientationAngle = bhomBar.OrientationAngle;
            }
            else
            {
                //BHoM and Robot use slightly different conditions for verticality. Calculate orientation angle that fits Robot
                Vector normal = bhomBar.Normal();
                Vector tan = bhomBar.Tangent(true);

                Vector reference;

                if (robotVertical)
                    reference = -Vector.XAxis;
                else
                    reference = Vector.ZAxis;

                orientationAngle = reference.Angle(normal, new Plane { Normal = tan });

                if (robotVertical && tan.Z < 0)
                {
                    orientationAngle += Math.PI;
                    orientationAngle = orientationAngle % (2 * Math.PI);
                }
            }

            return orientationAngle * 180 / Math.PI;
        }

        /***************************************************/

        [Description("Checks whether a bar is deemed vertical in Robot.")]
        [Input("bar", "The bar to check.")]
        [Output("isVertical", "Returns true if the bar is deemed vertical in Robot.")]
        public static bool IsVerticalRobot(this Bar bar)
        {
            if (bar.StartNode == null || bar.StartNode.Position == null || bar.EndNode == null || bar.EndNode.Position == null)
            {
                Engine.Reflection.Compute.RecordError("At least one of a bars end nodes or end nodes positions are null. Could not calculate verticality.");
                return false;
            }

            double length = bar.Length();

            double dx = bar.StartNode.Position.X - bar.EndNode.Position.X;
            double dy = bar.StartNode.Position.Y - bar.EndNode.Position.Y;

            double projLength = Math.Sqrt(dx * dx + dy * dy);

            return projLength < (length * 0.001);
        }

        /***************************************************/

    }
}
