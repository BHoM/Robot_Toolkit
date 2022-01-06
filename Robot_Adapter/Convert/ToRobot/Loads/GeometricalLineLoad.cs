/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using BH.Engine.Geometry;
using RobotOM;

namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/       
       
        public static void ToRobot(this oM.Structure.Loads.GeometricalLineLoad load, RobotSimpleCase sCase, RobotGroupServer rGroupServer)
        {
            //Replace null vectors with empty vector
            load.ForceA = load.ForceA ?? new oM.Geometry.Vector();
            load.ForceB = load.ForceB ?? new oM.Geometry.Vector();
            load.MomentA = load.MomentA ?? new oM.Geometry.Vector();
            load.MomentB = load.MomentB ?? new oM.Geometry.Vector();

            if (load.ForceA.Length() == 0 && load.ForceB.Length() == 0 && load.MomentA.Length() == 0 && load.MomentB.Length() == 0)
            {
                Engine.Reflection.Compute.RecordWarning("Zero geometrical forces and moments are not pushed to Robot");
                return;
            }
            IRobotLoadRecordLinear3D loadLin3D = sCase.Records.Create(IRobotLoadRecordType.I_LRT_LINEAR_3D) as IRobotLoadRecordLinear3D;

            loadLin3D.SetPoint(1, load.Location.Start.X, load.Location.Start.Y, load.Location.Start.Z);
            loadLin3D.SetPoint(2, load.Location.End.X, load.Location.End.Y, load.Location.End.Z);

            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PX1, load.ForceA.X);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PY1, load.ForceA.Y);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PZ1, load.ForceA.Z);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PX2, load.ForceB.X);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PY2, load.ForceB.Y);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_PZ2, load.ForceB.Z);

            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MX1, load.MomentA.X);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MY1, load.MomentA.Y);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MZ1, load.MomentA.Z);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MX2, load.MomentB.X);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MY2, load.MomentB.Y);
            loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_MZ2, load.MomentB.Z);

            if(load.Axis == oM.Structure.Loads.LoadAxis.Local)
                loadLin3D.SetValue((short)IRobotLinear3DRecordValues.I_L3DRV_LOCAL, 0);

        }

        /***************************************************/
    }
}



