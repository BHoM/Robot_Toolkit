using System.Collections.Generic;
using RobotOM;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected bool Update(IEnumerable<FramingElementDesignProperties> framingElementDesignPropertiesList)
        {

            foreach (FramingElementDesignProperties framEleDesProps in framingElementDesignPropertiesList)
            {
               
                IRobotLabel memberType = m_RobotApplication.Project.Structure.Labels.Get(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name);

                string steelMembersCodeType = m_RobotApplication.Project.Preferences.GetActiveCode(IRobotCodeType.I_CT_STEEL_STRUCTURES);

                IRDimMembDef memberDef = memberType.Data;
                
                if (steelMembersCodeType == BHE.Query.GetStringFromEnum(DesignCode_Steel.BS_EN_1993_1_2005_NA_2008_A1_2014))
                {
                    IRDimMembParamsE32 memberDesignParams_EC3 = memberDef.CodeParams;
                    memberDesignParams_EC3.BuckLengthCoeffY = framEleDesProps.EulerBucklingLengthCoefficientY;
                    memberDesignParams_EC3.BuckLengthCoeffZ = framEleDesProps.EulerBucklingLengthCoefficientZ;
                    memberDef.CodeParams = memberDesignParams_EC3;
                }

                if (steelMembersCodeType == BHE.Query.GetStringFromEnum(DesignCode_Steel.BS5950))
                {
                    IRDimMembParamsBS59 memberDesignParams_BS5950 = memberDef.CodeParams;
                    memberDesignParams_BS5950.BuckLengthCoeffY = framEleDesProps.EulerBucklingLengthCoefficientY;
                    memberDesignParams_BS5950.BuckLengthCoeffZ = framEleDesProps.EulerBucklingLengthCoefficientZ;
                    memberDef.CodeParams = memberDesignParams_BS5950;
                }

                if (steelMembersCodeType == BHE.Query.GetStringFromEnum(DesignCode_Steel.BS5950_2000))
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

        /***************************************************/
    }
}
