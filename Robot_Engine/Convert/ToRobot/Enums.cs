/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

using BH.oM.Structure.SectionProperties;
using RobotOM;
using BH.oM.Structure.Results;


namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static IRobotFeLayerType FromBHoMEnum(MeshResultLayer bhomMeshLayer)
        {
            IRobotFeLayerType robotLayerType = new IRobotFeLayerType();

            switch (bhomMeshLayer)
            {
                case (MeshResultLayer.AbsoluteMaximum):
                    robotLayerType = IRobotFeLayerType.I_FLT_ABSOLUTE_MAXIMUM;
                    break;
                case (MeshResultLayer.Lower):
                    robotLayerType = IRobotFeLayerType.I_FLT_LOWER;
                    break;
                case (MeshResultLayer.Maximum):
                    robotLayerType = IRobotFeLayerType.I_FLT_MAXIMUM;
                    break;
                case (MeshResultLayer.Middle):
                    robotLayerType = IRobotFeLayerType.I_FLT_MIDDLE;
                    break;
                case (MeshResultLayer.Minimum):
                    robotLayerType = IRobotFeLayerType.I_FLT_MINIMUM;
                    break;
                case (MeshResultLayer.Upper):
                    robotLayerType = IRobotFeLayerType.I_FLT_UPPER;
                    break;
                case (MeshResultLayer.Arbitrary):
                    robotLayerType = IRobotFeLayerType.I_FLT_ARBITRARY;
                    break;
            }
            return robotLayerType;
        }

        /***************************************************/

        public static IRobotFeResultSmoothing FromBHoMEnum(MeshResultSmoothingType bhomPanelSmoothingType)
        {
            IRobotFeResultSmoothing robotSmoothingType = new IRobotFeResultSmoothing();

            switch (bhomPanelSmoothingType)
            {
                case (MeshResultSmoothingType.None):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_NO_SMOOTHING;
                    break;
                case (MeshResultSmoothingType.ByPanel):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_SMOOTHING_WITHIN_A_PANEL;
                    break;
                case (MeshResultSmoothingType.BySelection):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_SMOOTHING_ACCORDING_TO_SELECTION;
                    break;
                case (MeshResultSmoothingType.Global):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_GLOBAL_SMOOTHING;
                    break;
                case (MeshResultSmoothingType.ByFiniteElementCentres):
                    robotSmoothingType = IRobotFeResultSmoothing.I_FRS_IN_ELEMENT_CENTER;
                    break;           
            }
            return robotSmoothingType;
        }

        /***************************************************/

    }
}
