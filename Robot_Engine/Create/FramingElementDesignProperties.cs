using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapters.Robot;

namespace BH.Engine.Adapters.Robot
{
    public static partial class Create
    {
        public static FramingElementDesignProperties FramingElementDesignProperties(string name)
        {
            FramingElementDesignProperties framingElementDesignProperties = new FramingElementDesignProperties();
            framingElementDesignProperties.Name = name;

            return framingElementDesignProperties;
        }

        public static FramingElementDesignProperties FramingElementDesignProperties(string name,
                                                                                    double eulerBucklingLengthCoeffY = 1,
                                                                                    double eulerBucklingLengthCoeffZ = 1)
        {
            FramingElementDesignProperties framEleDesignProps = new FramingElementDesignProperties();
            framEleDesignProps.Name = name;
            framEleDesignProps.EulerBucklingLengthCoefficientY = eulerBucklingLengthCoeffY;
            framEleDesignProps.EulerBucklingLengthCoefficientZ = eulerBucklingLengthCoeffZ;

            return framEleDesignProps;
        }
    }
}
