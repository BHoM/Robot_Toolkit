/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using System;
using System.IO;
using System.Collections.Generic;
using RobotOM;
using System.Diagnostics;
using BH.oM.Adapters.Robot;
using BH.Engine.Adapters.Robot;
using BH.oM.Data.Requests;
using BH.oM.Adapter;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {

        /***************************************************/
        /**** Public  Methods                           ****/
        /***************************************************/

        public override List<object> Push(IEnumerable<object> objects, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
        {
            if (m_RobotApplication == null)
            {
                Engine.Base.Compute.RecordWarning("The link to the Robot application is not established. Please make sure the RobotAdapter is activated!");
                return new List<object>();
            }

            ClearCashedTags();

            //Get out the interactive settings, and make nullchecks
            InteractiveSettings interactiveSettings = RobotConfig?.InteractiveSettings ?? new InteractiveSettings();

            //Check interactive
            if (!interactiveSettings.IsInteractive)
                m_RobotApplication.Interactive = 0;

            //Check vissible
            if (!interactiveSettings.IsVisible)
                m_RobotApplication.Visible = 0;

            try
            {
                List<object> pushedObjects = base.Push(objects, tag, pushType, actionConfig);
                //After push is done, ensure that all groups are correctly updated to match the tags
                ApplyTagsAsGroups();
                return pushedObjects;
            }
            finally
            {
                //Make sure the application is visible and interactive independant if the code throws an expection or not
                if (m_RobotApplication != null)
                {
                    m_RobotApplication.Interactive = 1;
                    m_RobotApplication.Visible = 1;
                }
                ClearCashedTags();
            }
        }

        /***************************************************/
    }
}





