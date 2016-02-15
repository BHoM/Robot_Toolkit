using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using BHoM.Structural;

namespace RobotToolkit
{
    /// <summary>
    /// Robot access types
    /// </summary>
    public enum RobotAccessType
    {
        /// <summary>Live link to Robot</summary>
        Live = 0,
        /// <summary>Use a Robot file</summary>
        FromFile,

    }
}