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

using RobotOM;
using BH.oM.Structure.Constraints;
using System.Collections.Generic;
using BH.oM.Base;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Offsets;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        //dependantObjects to be used for labels that require additional inner obejcts, such as Materials for SectionProeprties and SurfaceProeprties
        //The method will assume this to be of a specific type
        public List<IBHoMObject> ReadLabels(IRobotLabelType robotLabelType, List<string> ids, Dictionary<string, IMaterialFragment> bhomMaterials = null)
        {
            IRobotLabelServer robotLabelServer = m_RobotApplication.Project.Structure.Labels;
            List<IBHoMObject> objects = new List<IBHoMObject>();

            if (ids != null && ids.Count != 0)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    if (robotLabelServer.Exist(robotLabelType, ids[i]) == -1)
                    {
                        IBHoMObject obj = ReadLabel(robotLabelType, ids[i], robotLabelServer, bhomMaterials);
                        if (obj != null)
                            objects.Add(obj);
                    }

                }
            }
            else
            {
                IRobotNamesArray robotLabelNames = robotLabelServer.GetAvailableNames(robotLabelType);

                for (int i = 1; i <= robotLabelNames.Count; i++)
                {
                    string robotLabelName = robotLabelNames.Get(i);

                    if (string.IsNullOrEmpty(robotLabelName))
                        continue;

                    IBHoMObject obj = ReadLabel(robotLabelType, robotLabelName, robotLabelServer, bhomMaterials);
                    if (obj != null)
                        objects.Add(obj);

                }
            }
            return objects;
        }

        /***************************************************/

        private IBHoMObject ReadLabel(IRobotLabelType robotLabelType, string robotLabelName, IRobotLabelServer robotLabelServer, Dictionary<string, IMaterialFragment> bhomMaterials = null)
        {
            IRobotLabel robotLabel = null;
            IBHoMObject obj = null;

            if (robotLabelServer.IsUsed(robotLabelType, robotLabelName))
                robotLabel = robotLabelServer.Get(robotLabelType, robotLabelName) as dynamic;
            else
                robotLabel = robotLabelServer.CreateLike(robotLabelType, "", robotLabelName) as dynamic;

            if (robotLabel == null || robotLabel.Data == null)
            {
                BH.Engine.Base.Compute.RecordWarning("Failed to read label '" + robotLabelName);
                return null;
            }
            else
            {
                switch (robotLabelType)
                {
                    case IRobotLabelType.I_LT_NODE_COMPATIBILITY:
                        break;
                    case IRobotLabelType.I_LT_BAR_SECTION:
                        IRobotBarSectionData secData = robotLabel.Data as IRobotBarSectionData;

                        if (secData == null)
                        {
                            BH.Engine.Base.Compute.RecordWarning($"Failed to read section with name {robotLabelName}");
                            return null;
                        }

                        bhomMaterials = bhomMaterials ?? new Dictionary<string, IMaterialFragment>();
                        IMaterialFragment sectionMaterial;
                        string materialName = secData.MaterialName;
                        if (!bhomMaterials.TryGetValue(materialName, out sectionMaterial))
                        {
                            sectionMaterial = ReadMaterialByLabelName(materialName);
                            bhomMaterials[materialName] = sectionMaterial;
                        }
                        ISectionProperty section = secData.FromRobot(sectionMaterial, robotLabelName);
                        obj = section;
                        break;
                    case IRobotLabelType.I_LT_BAR_RELEASE:
                        BarRelease barRelease = robotLabel.FromRobot(robotLabel.Data as IRobotBarReleaseData, robotLabelName);
                        obj = barRelease;
                        break;
                    case IRobotLabelType.I_LT_BAR_OFFSET:
                        Offset offset = robotLabel.FromRobot(robotLabel.Data as IRobotBarOffsetData);
                        obj = offset;
                        break;
                    case IRobotLabelType.I_LT_BAR_CABLE:
                        break;
                    case IRobotLabelType.I_LT_BAREND_BRACKET:
                        break;
                    case IRobotLabelType.I_LT_EDGE_SUPPORT:
                        break;
                    case IRobotLabelType.I_LT_PANEL_THICKNESS:
                        IRobotThicknessData data = robotLabel.Data;

                        if (data == null)
                        {
                            BH.Engine.Base.Compute.RecordWarning($"Failed to read {nameof(ISurfaceProperty)} with name {robotLabelName}");
                            return null;
                        }

                        bhomMaterials = bhomMaterials ?? new Dictionary<string, IMaterialFragment>();
                        IMaterialFragment srfMat;
                        materialName = data.MaterialName;
                        if (!bhomMaterials.TryGetValue(materialName, out srfMat))
                        {
                            srfMat = ReadMaterialByLabelName(materialName);
                            bhomMaterials[materialName] = srfMat;
                        }

                        ISurfaceProperty prop = robotLabel.FromRobot(data, srfMat, robotLabelName);
                        obj = prop;
                        break;
                    case IRobotLabelType.I_LT_PANEL_REINFORCEMENT:
                        break;
                    case IRobotLabelType.I_LT_UNKNOWN:
                        break;
                    case IRobotLabelType.I_LT_SUPPORT:
                        Constraint6DOF support = Convert.FromRobot(robotLabel as RobotNodeSupport);
                        obj = support;
                        break;
                    case IRobotLabelType.I_LT_MATERIAL:
                        IMaterialFragment material = Convert.FromRobot(robotLabel, robotLabel.Data as RobotMaterialData, robotLabelName);
                        obj = material;
                        break;
                    case IRobotLabelType.I_LT_LINEAR_RELEASE:
                        Constraint4DOF linearRelease = Convert.FromRobot(robotLabel, robotLabel.Data as IRobotLinearReleaseData);
                        obj = linearRelease;
                        break;
                    case IRobotLabelType.I_LT_BAR_ELASTIC_GROUND:
                        break;
                    case IRobotLabelType.I_LT_NODE_RIGID_LINK:
                        break;
                    case IRobotLabelType.I_LT_MEMBER_TYPE:
                        break;
                    case IRobotLabelType.I_LT_VEHICLE:
                        break;
                    case IRobotLabelType.I_LT_SOLID_PROPERTIES:
                        break;
                    case IRobotLabelType.I_LT_BAR_GEO_IMPERFECTIONS:
                        break;
                    case IRobotLabelType.I_LT_BAR_NONLINEAR_HINGE:
                        break;
                    case IRobotLabelType.I_LT_CLADDING:
                        ISurfaceProperty cladding = robotLabel.FromRobot(robotLabel.Data as RobotCladdingData);
                        obj = cladding;
                        break;
                    case IRobotLabelType.I_LT_PANEL_CALC_MODEL:
                        break;
                    case IRobotLabelType.I_LT_MEMBER_REINFORCEMENT_PARAMS:
                        break;
                    case IRobotLabelType.I_LT_COUNT:
                        break;
                    default:
                        break;
                }
                if (obj != null)
                {
                    SetAdapterId(obj, robotLabelName);
                    obj.Name = robotLabelName;
                }
            }
            return obj;
        }
    

        /***************************************************/
    }
}



