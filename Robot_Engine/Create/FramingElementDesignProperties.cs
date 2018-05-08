using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapters.Robot;
using BH.oM.Adapters.Robot.Properties;

namespace BH.Engine.Adapters.Robot.Properties
{
    public static partial class Create
    {
        public static FramingElementDesignProperties FramingElementDesignProperties(string name)
        {
            FramingElementDesignProperties framingElementDesignProperties = new FramingElementDesignProperties();
            framingElementDesignProperties.Name = name;

            return framingElementDesignProperties;
        }     
    }
}
