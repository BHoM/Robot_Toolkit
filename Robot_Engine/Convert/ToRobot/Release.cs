using RobotOM;
using BH.oM.Structural.Properties;

namespace BH.Engine.Robot
{
    public static partial class Convert
    {
        /***************************************************/
        /****           Public Methods                  ****/
        /***************************************************/

        public static void RobotRelease(IRobotBarEndReleaseData rData, Constraint6DOF bhomRelease)
        {
            rData.UX = GetReleaseType(bhomRelease.TranslationX);
            rData.UY = GetReleaseType(bhomRelease.TranslationY);
            rData.UZ = GetReleaseType(bhomRelease.TranslationZ);
            rData.RX = GetReleaseType(bhomRelease.RotationX);
            rData.RY = GetReleaseType(bhomRelease.RotationY);
            rData.RZ = GetReleaseType(bhomRelease.RotationZ);

            rData.KX = bhomRelease.TranslationalStiffnessX;
            rData.KY = bhomRelease.TranslationalStiffnessY;
            rData.KZ = bhomRelease.TranslationalStiffnessZ;
            rData.HX = bhomRelease.RotationalStiffnessX;
            rData.HY = bhomRelease.RotationalStiffnessY;
            rData.HZ = bhomRelease.RotationalStiffnessZ;
        }

        /***************************************************/

        public static Constraint6DOF BHoMRelease(IRobotBarEndReleaseData rData)
        {
            Constraint6DOF bhomEndRelease = new Constraint6DOF();
            bhomEndRelease.TranslationX = GetReleaseType(rData.UX);
            bhomEndRelease.TranslationY = GetReleaseType(rData.UY);
            bhomEndRelease.TranslationZ = GetReleaseType(rData.UZ);
            bhomEndRelease.RotationX = GetReleaseType(rData.RX);
            bhomEndRelease.RotationY = GetReleaseType(rData.RY);
            bhomEndRelease.RotationZ = GetReleaseType(rData.RZ);

            bhomEndRelease.TranslationalStiffnessX = rData.KX;
            bhomEndRelease.TranslationalStiffnessY = rData.KY;
            bhomEndRelease.TranslationalStiffnessZ = rData.KZ;
            bhomEndRelease.RotationalStiffnessX = rData.HX;
            bhomEndRelease.RotationalStiffnessY = rData.HY;
            bhomEndRelease.RotationalStiffnessZ = rData.HZ;

            return bhomEndRelease;
        }

        /***************************************************/

        public static DOFType GetReleaseType(IRobotBarEndReleaseValue endRelease)
        {
            switch (endRelease)
            {
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC:
                    return DOFType.Spring;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_MINUS:
                    return DOFType.SpringNegative;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_PLUS:
                    return DOFType.SpringPositive;
                case IRobotBarEndReleaseValue.I_BERV_NONE:
                    return DOFType.Fixed;
                case IRobotBarEndReleaseValue.I_BERV_MINUS:
                    return DOFType.FixedNegative;
                case IRobotBarEndReleaseValue.I_BERV_PLUS:
                    return DOFType.FixedPositive;
                case IRobotBarEndReleaseValue.I_BERV_STD:
                    return DOFType.Free;
                case IRobotBarEndReleaseValue.I_BERV_NONLINEAR:
                    return DOFType.NonLinear;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED:
                    return DOFType.SpringRelative;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_MINUS:
                    return DOFType.SpringRelativeNegative;
                case IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_PLUS:
                    return DOFType.SpringRelativePositive;
                default:
                    return DOFType.Fixed;
            }
        }

        /***************************************************/

        public static IRobotBarEndReleaseValue GetReleaseType(DOFType endRelease)
        {
            switch (endRelease)
            {
                case DOFType.Spring:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC;
                case DOFType.SpringNegative:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_MINUS;
                case DOFType.SpringPositive:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_PLUS;
                case DOFType.Free:
                    return IRobotBarEndReleaseValue.I_BERV_STD;
                case DOFType.FixedNegative:
                    return IRobotBarEndReleaseValue.I_BERV_MINUS;
                case DOFType.FixedPositive:
                    return IRobotBarEndReleaseValue.I_BERV_PLUS;
                case DOFType.Fixed:
                    return IRobotBarEndReleaseValue.I_BERV_NONE;
                case DOFType.NonLinear:
                    return IRobotBarEndReleaseValue.I_BERV_NONLINEAR;
                case DOFType.SpringRelative:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED;
                case DOFType.SpringRelativeNegative:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_MINUS;
                case DOFType.SpringRelativePositive:
                    return IRobotBarEndReleaseValue.I_BERV_ELASTIC_REDUCED_PLUS;
                default:
                    return IRobotBarEndReleaseValue.I_BERV_STD;
            }
        }

        /***************************************************/

    }
}
