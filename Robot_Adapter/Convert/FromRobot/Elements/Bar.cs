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

using BH.oM.Structure.MaterialFragments;
using RobotOM;
using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Adapters.Robot;


namespace BH.Adapter.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static Bar FromRobot( this RobotBar robotBar, 
                                        Dictionary<string,Node> bhomNodes, 
                                        Dictionary<string, ISectionProperty> bhomSections, 
                                        Dictionary<string, IMaterialFragment> bhomMaterials, 
                                        Dictionary<string, BarRelease> barReleases,
                                        Dictionary<string, FramingElementDesignProperties> bhomFramEleDesPropList,
                                        ref Dictionary<string, Dictionary<string, ISectionProperty>> sectionWithMaterial)
        {
            Node startNode = null;  bhomNodes.TryGetValue(robotBar.StartNode.ToString(), out startNode);
            Node endNode = null; bhomNodes.TryGetValue(robotBar.EndNode.ToString(), out endNode);
            Bar bhomBar = new Bar { StartNode = startNode, EndNode = endNode, Name = robotBar.Name };
            ISectionProperty secProp = null;
            IMaterialFragment barMaterial = null;
            BarRelease bhomBarRel = null;
            FramingElementDesignProperties bhomFramEleDesignProps = null;

            if (robotBar.HasLabel(IRobotLabelType.I_LT_BAR_SECTION) == -1)
            {
                //Get section name
                string secName = robotBar.GetLabelName(IRobotLabelType.I_LT_BAR_SECTION);

                if (robotBar.HasLabel(IRobotLabelType.I_LT_MATERIAL) == -1)
                {
                    //Get material name and material
                    string matName = robotBar.GetLabelName(IRobotLabelType.I_LT_MATERIAL);


                    if (!bhomMaterials.TryGetValue(matName, out barMaterial))
                    {
                        BH.Engine.Reflection.Compute.RecordWarning("Could not extract material with name" + matName + "null material will be provided for the crossection for bars with this material.");
                    }


                    Dictionary<string, ISectionProperty> innerDict;
                    //Check if a section of the specified type has allready been pulled
                    if (!sectionWithMaterial.TryGetValue(secName, out innerDict))
                    {
                        innerDict = new Dictionary<string, ISectionProperty>();
                    }

                    //Check if a section of the specified type with the material has allready been added
                    if (!innerDict.TryGetValue(matName, out secProp))
                    {
                        //If not, get out the section from the basic dictionary
                        if (bhomSections.TryGetValue(secName, out secProp))
                        {
                            //Construct and store a copy of the section, with new material
                            secProp = secProp.GetShallowClone(true) as ISectionProperty;
                            secProp.Material = barMaterial;
                            innerDict[matName] = secProp;
                            sectionWithMaterial[secName] = innerDict;
                        }
                        else
                        {
                            BH.Engine.Reflection.Compute.RecordEvent("Section property type " + secName + " is not supported", oM.Reflection.Debugging.EventType.Warning);
                        }
                    }
                }
                else
                {
                    //No material label appended to the bar. The default section is used, if found
                    if (bhomSections.TryGetValue(secName, out secProp))
                    {
                        Dictionary<string, ISectionProperty> innerDict;
                        if (!sectionWithMaterial.TryGetValue(secName, out innerDict))
                        {
                            innerDict = new Dictionary<string, ISectionProperty>();
                        }
                        if (!innerDict.ContainsKey(secProp.Material.Name))
                        {
                            innerDict[secProp.Material.Name] = secProp;
                            sectionWithMaterial[secName] = innerDict;
                        }
                    }
                    else
                    {
                        BH.Engine.Reflection.Compute.RecordEvent("Section property type " + secName + " is not supported", oM.Reflection.Debugging.EventType.Warning);
                    }
                }
            }            


            if (robotBar.HasLabel(IRobotLabelType.I_LT_MEMBER_TYPE) == -1)
            {
                string framEleDesPropsName = robotBar.GetLabelName(IRobotLabelType.I_LT_MEMBER_TYPE);
                if (bhomFramEleDesPropList.TryGetValue(framEleDesPropsName, out bhomFramEleDesignProps) && bhomFramEleDesignProps != null)
                {
                    bhomBar.CustomData["FramingElementDesignProperties"] = bhomFramEleDesignProps;
                }
                else
                { 
                    BH.Engine.Reflection.Compute.RecordEvent("Framing element design property" + framEleDesPropsName + "is not supported", oM.Reflection.Debugging.EventType.Warning);
                }
                    
            }

            if (robotBar.HasLabel(IRobotLabelType.I_LT_BAR_RELEASE) == -1)
            {

                string releaseName = robotBar.GetLabelName(IRobotLabelType.I_LT_BAR_RELEASE);
                if (barReleases.TryGetValue(releaseName, out bhomBarRel))
                    bhomBar.Release = bhomBarRel;
                else
                    BH.Engine.Reflection.Compute.RecordEvent("Bars with auto-generated releases will not have releases", oM.Reflection.Debugging.EventType.Warning);
            }

            bhomBar.SectionProperty = secProp;
            bhomBar.OrientationAngle = robotBar.Gamma * Math.PI / 180;
            bhomBar.CustomData[AdapterID] = robotBar.Number;

            if (robotBar.TensionCompression == IRobotBarTensionCompression.I_BTC_COMPRESSION_ONLY)
            {
                bhomBar.FEAType = BarFEAType.CompressionOnly;
            }
            if (robotBar.TensionCompression == IRobotBarTensionCompression.I_BTC_TENSION_ONLY)
            {
                bhomBar.FEAType = BarFEAType.TensionOnly;
            }
            if (robotBar.TrussBar == true)
            {
                bhomBar.FEAType = BarFEAType.Axial;
            }
            return bhomBar;       
        }

        /***************************************************/
    }

}

