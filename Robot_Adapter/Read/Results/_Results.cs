using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BH.oM.Structure.Results;
using RobotOM;
using BH.oM.Common;
using System;
using System.Collections;

namespace BH.Adapter.Robot
{
    public partial class RobotAdapter
    {
        /***************************************************/
        /****           Protected Methods               ****/
        /***************************************************/

        protected override IEnumerable<IResult> ReadResults(Type type, IList ids = null, IList cases = null, int divisions = 5)
        {
            if (type == typeof(BarForce))
                return ReadBarForce(ids, cases, divisions);
            if (type == typeof(NodeDisplacement))
                return ReadNodeDisplacement(ids, cases, divisions);
            if (type == typeof(NodeReaction))
                return ReadNodeReactions(ids, cases, divisions);
            return base.ReadResults(type, ids, cases, divisions);
        }

        /***************************************************/

        /***************************************************/
        /****           Private Methods                 ****/
        /***************************************************/
        
        private List<int> CheckAndGetIds(IList ids)
        {
            if (ids is List<string>)
                return (ids as List<string>).Select(x => int.Parse(x)).ToList();
            else if (ids is List<int>)
                return ids as List<int>;
            else if (ids is List<double>)
                return (ids as List<double>).Select(x => (int)Math.Round(x)).ToList();
            else
            {
                List<int> idsOut = new List<int>();
                foreach (object o in ids)
                {
                    int id;
                    object idObj;
                    if (int.TryParse(o.ToString(), out id))
                    {
                        idsOut.Add(id);
                    }
                    else if (o is IBHoMObject && (o as IBHoMObject).CustomData.TryGetValue(AdapterId, out idObj) && int.TryParse(idObj.ToString(), out id))
                        idsOut.Add(id);
                }
                return idsOut;
            }
        }

        /***************************************************/

    }
}
