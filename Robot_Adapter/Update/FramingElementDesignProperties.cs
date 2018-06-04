using System.Collections.Generic;
using System;
using BH.oM.Base;
using BH.oM.Structural.Elements;
using BH.oM.Geometry;
using BH.oM.Structural.Properties;
using BH.oM.Structural.Loads;
using BH.oM.Common.Materials;
using BH.Engine.Serialiser;
using RobotOM;
using BH.Engine.Robot;
using BHEG = BH.Engine.Geometry;
using BH.oM.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        protected bool Update(IEnumerable<FramingElementDesignProperties> framingElementDesignPropertiesList)
        {

            foreach (FramingElementDesignProperties framEleDesProps in framingElementDesignPropertiesList)
            {
               
                IRobotLabel memberType = m_RobotApplication.Project.Structure.Labels.Get(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name);

                string steelMembersCodeType = m_RobotApplication.Project.Preferences.GetActiveCode(IRobotCodeType.I_CT_STEEL_STRUCTURES);

                IRDimMembDef memberDef = memberType.Data;
                
                if (steelMembersCodeType == "BS-EN 1993-1:2005/NA:2008/A1:2014")
                {
                    IRDimMembParamsE32 memberDesignParams_EC3 = memberDef.CodeParams;
                    memberDesignParams_EC3.BuckLengthCoeffY = framEleDesProps.EulerBucklingLengthCoefficientY;
                    memberDesignParams_EC3.BuckLengthCoeffZ = framEleDesProps.EulerBucklingLengthCoefficientZ;
                    memberDef.CodeParams = memberDesignParams_EC3;
                }

                if (steelMembersCodeType == "BS5950")
                {
                    IRDimMembParamsBS59 memberDesignParams_BS5950 = memberDef.CodeParams;
                    memberDesignParams_BS5950.BuckLengthCoeffY = framEleDesProps.EulerBucklingLengthCoefficientY;
                    memberDesignParams_BS5950.BuckLengthCoeffZ = framEleDesProps.EulerBucklingLengthCoefficientZ;
                    memberDef.CodeParams = memberDesignParams_BS5950;
                }

                if (steelMembersCodeType == "BS 5950:2000")
                {
                    IRDimMembParamsBS59_2000 memberDesignParams_BS5950_2000 = memberDef.CodeParams;
                    memberDesignParams_BS5950_2000.BuckLengthCoeffY = framEleDesProps.EulerBucklingLengthCoefficientY;
                    memberDesignParams_BS5950_2000.BuckLengthCoeffZ = framEleDesProps.EulerBucklingLengthCoefficientZ;
                    memberDef.CodeParams = memberDesignParams_BS5950_2000;
                }
                
                m_RobotApplication.Project.Structure.Labels.Store(memberType);                
   }
            
            return true;
    }
      
    }
}
