﻿using System.Collections.Generic;
using RobotOM;
using BH.oM.Adapters.Robot;
using BHE = BH.Engine.Adapters.Robot;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
        
        private bool Create(FramingElementDesignProperties framEleDesProps) //TODO: move the label part to convert such that duplicate code in Update can be removed
        {

            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;
            IRobotLabel label = labelServer.CreateLike(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name, labelServer.GetDefault(IRobotLabelType.I_LT_MEMBER_TYPE));

            IRDimMembDef memberDef = label.Data;

            string steelMembersCodeType = m_RobotApplication.Project.Preferences.GetActiveCode(IRobotCodeType.I_CT_STEEL_STRUCTURES);

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

            if (steelMembersCodeType == BHE.Query.GetStringFromEnum(DesignCode_Steel.ANSI_AISC_360_10))
            {
                IRDimMembParamsANS memberDesignParams_AISC_360_10 = memberDef.CodeParams;
                memberDesignParams_AISC_360_10.BuckLenghtCoeffY = framEleDesProps.EulerBucklingLengthCoefficientY;
                memberDesignParams_AISC_360_10.BuckLenghtCoeffZ = framEleDesProps.EulerBucklingLengthCoefficientZ;
                memberDef.CodeParams = memberDesignParams_AISC_360_10;
            }

            labelServer.Store(label);
                    

            return true;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<FramingElementDesignProperties> framEleDesPropsList)
        {
            IRobotLabelServer labelServer = m_RobotApplication.Project.Structure.Labels;

            foreach (FramingElementDesignProperties framEleDesProps in framEleDesPropsList)
            {
                IRobotLabel label = labelServer.CreateLike(IRobotLabelType.I_LT_MEMBER_TYPE, framEleDesProps.Name, labelServer.GetDefault(IRobotLabelType.I_LT_MEMBER_TYPE));

                labelServer.Store(label);
            }
            return true;
        }

        /***************************************************/

    }

}

