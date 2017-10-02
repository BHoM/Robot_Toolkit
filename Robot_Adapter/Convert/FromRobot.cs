using BH.oM.Geometry;
using GeometryEngine = BH.Engine.Geometry;
using StructureEngine = BH.Engine.Structure;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using BH.oM.Materials;
using BH.oM.Base;

namespace BH.Adapter.Robot
{    
    public static partial class Convert
    {
        /***************************************/

        #region Geometry Converters

        public static Point ToBHoMGeometry(RobotGeoPoint3D point)
        {
            return new Point(point.X, point.Y, point.Z);
        }

        public static Circle ToBHoMGeometry(RobotGeoCircle circle)
        {
            return (GeometryEngine.Create.CreateCircle(ToBHoMGeometry(circle.P1 as dynamic), ToBHoMGeometry(circle.P2 as dynamic), ToBHoMGeometry(circle.P3 as dynamic)));
        }

        public static Arc ToBHoMGeometry(this RobotGeoArc arc)
        {
            return new Arc(ToBHoMGeometry(arc.P1 as dynamic), ToBHoMGeometry(arc.P2 as dynamic), ToBHoMGeometry(arc.P3 as dynamic));
        }

        public static Polyline ToBHoMGeometry(this RobotGeoPolyline polyline)
        {
            Polyline bhomPolyline = new Polyline();

            for (int j = 1; j <= polyline.Segments.Count; j++)
            {
                bhomPolyline.ControlPoints.Add(ToBHoMGeometry(polyline.Segments.Get(j).P1 as dynamic));
            }
            bhomPolyline.ControlPoints.Add(ToBHoMGeometry(polyline.Segments.Get(1).P1));

            return bhomPolyline;
        }

        public static PolyCurve ToBHoMGeometry(this RobotGeoContour contour)
        {            
            PolyCurve polycurve = new PolyCurve();
            RobotGeoSegment segment1, segment2;
            
            for (int j = 1; j <= contour.Segments.Count; j++)
            {
                segment1 = contour.Segments.Get(j) as RobotGeoSegment;
                segment2 = contour.Segments.Get(j % contour.Segments.Count + 1) as RobotGeoSegment;

                if (segment1.Type == IRobotGeoSegmentType.I_GST_ARC)
                {
                    polycurve.Curves.Add(ToBHoMGeometry(segment1 as dynamic));
                }
                else
                {
                    polycurve.Curves.Add(new Line(ToBHoMGeometry(segment1.P1 as dynamic), ToBHoMGeometry(segment2.P1 as dynamic)));
                }
            }
            return polycurve;
        }

        #endregion

        #region Object Converters

        public static Bar ToBHoMObject(this RobotBar robotBar, Dictionary<string,Node> bhomNodes)
        {
            Node startNode = null;  bhomNodes.TryGetValue(robotBar.StartNode.ToString(), out startNode);
            Node endNode = null; bhomNodes.TryGetValue(robotBar.EndNode.ToString(), out endNode);
            Bar bhomBar = new Bar(startNode, endNode, robotBar.Number.ToString());

            bhomBar.SectionProperty = null;
            bhomBar.OrientationAngle = robotBar.Gamma * 180 / Math.PI;
            bhomBar.Name = robotBar.Number.ToString();
            bhomBar.Tags.Add(robotBar.Name);

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

            bhomBar.CustomData.Add("Robot Name", robotBar.Name);
            bhomBar.CustomData.Add("Robot Guid", robotBar.UniqueId);
            return bhomBar;       
        }

        public static Node ToBHoMObject(this RobotNode robotNode)
        {
           return new Node(new Point(robotNode.X, robotNode.Y, robotNode.Z), robotNode.Number.ToString());
        }

        //public static SectionProperty ToBHoMSectionProperty(IRobotLabel sec_label)
        //{
        //    IRobotBarSectionData sec_data = sec_label.Data;
        //    SectionProperty property = null;
        //    if (sec_data.IsConcrete)
        //    {
        //        double b = 0;
        //        double h = 0;
        //        double T1 = 0;
        //        double T2 = 0;
        //        double T3 = 0;
        //        double b1 = 0;
        //        double b2 = 0;
        //        double b3 = 0;
        //        RobotBarSectionConcreteData concMember = sec_data.Concrete;
        //        switch (sec_data.ShapeType)
        //        {
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT:
        //                h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_H);
        //                b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_B);
        //                property = SectionProperty.CreateRectangularSection(MaterialType.Concrete, h, b);
                        
                       
        //                break;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I:
        //                b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B1);
        //                b2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B2);
        //                T2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF1);
        //                T3 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_HF2);
        //                h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_H);
        //                b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_I_B);
        //                property = SectionProperty.CreateISection(MaterialType.Concrete, b1, b2, h, T2, T3, b, 0, 0);
        //                break;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T:
        //                h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_H);
        //                b1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_B);
        //                T2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_HF);
        //                b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_BEAM_T_BF);
        //                property = SectionProperty.CreateTeeSection(MaterialType.Concrete, h, b, T2, b1);
        //                break;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM:
        //                return null;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_COL_C:
        //                b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_DE);
        //                h = b;
        //                property = SectionProperty.CreateCircularSection(MaterialType.Concrete, b);
        //                break;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_COL_R:
        //                b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
        //                h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
        //                property = SectionProperty.CreateRectangularSection(MaterialType.Concrete, h, b);
        //                break;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_COL_CQ:
        //                //quarter Cirlce de dimeter
        //                return null;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_COL_CH:
        //                //Half circle de dimeter total height
        //                return null;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_COL_L:
        //                b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
        //                h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
        //                T1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
        //                T2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
        //                property = SectionProperty.CreateAngleSection(MaterialType.Concrete, h, b, h - T2, b - T1, 0, 0);
        //                break;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_COL_P:
        //                //equal sided polygon de total height n angle
        //                break;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_COL_T:
        //                b = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_B);
        //                h = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H);
        //                T1 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_L1);
        //                T2 = concMember.GetValue(IRobotBarSectionConcreteDataValue.I_BSCDV_COL_H1);
        //                property = SectionProperty.CreateTeeSection(MaterialType.Concrete, h, b, h - T2, b - 2 * T1);
        //                property.Orientation = 180;
        //                break;
        //            case IRobotBarSectionShapeType.I_BSST_CONCR_COL_Z:
        //                return null;
        //        }
        //    }
        //    else
        //    {
        //        double d = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
        //        double b = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
        //        double Tf = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
        //        double Tw = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
        //        double r = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
        //        double ri = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RI);
        //        double s = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_S);
        //        double mass = sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WEIGHT);
        //        property = new SteelSection(GetShapeType(sec_data.ShapeType), d, b, Tw, Tf, r, ri, mass, s);
        //    }

        //    property.Orientation += sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_GAMMA);
        //    return property;
        //    #region BarTypes
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_R;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_T;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_L;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_Z;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_P;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_C;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_CH;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_CQ;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UNKNOWN;

        //    //        ///<summary>Equal Angle (xy axis, parallel to legs)</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAE;

        //    //        ///<summary>Equal Angle (uv main axis)</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAEP;

        //    //        ///<summary>Unequal Angles (xy axis, parallel to legs)</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAI;

        //    //        ///<summary>Unequal Angle (uv main axis) </summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAIP;

        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCEC;

        //    //        ///<summary>Compound Equal Angles Legs Back to Back</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCED;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCEP;

        //    //        ///<summary>Compound Unequal Angles Long Legs Back to Back</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCIG;

        //    //        ///<summary>Compound Unequal Angles Short Legs Back to Back</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCIP;

        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEAA;

        //    //        ///<summary>Universal bearing pile</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEB;

        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEC;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEM;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HER;


        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HHEA;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HHEB;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HHEM;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IIPE;



        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPEA;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPEO;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPER;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPEV;

        //    //        ///<summary>Rolled Steel Joists</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPN;

        //    //        ///<summary>Structural Tees cut from Universal Columns</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MHEA;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MHEB;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MHEM;

        //    //        ///<summary>Structural Tees cut from Universal Beams</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MIPE;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_PRS;

        //    //        ///<summary>Square hollow section</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TCAR;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TEAE;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TEAI;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_THEX;

        //    //        ///<summary>Rectangular hollow section</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TREC;

        //    //        ///<summary>Circular Hollow Section</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TRON;

        //    //        ///<summary>Parallel flange channel</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UAP;

        //    //        ///<summary>Rolled steel channel</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UPN;

        //    //        ///<summary>Two Parallel Flange Channel Back to Back</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UUAP;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UUPN;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_FRTG;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WOOD_RECT;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UPAF;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;

        //    //        ///<summary>Solid circular section</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CIRC_FILLED;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CCL;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_URND;

        //    //        ///<summary>Square and Rectangular Hollow Sections</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TRND;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CUAP;

        //    //        ///<summary>Castellated beam</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS;

        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_ROUND_OPENINGS;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS_SHIFTED;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_SFB;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_IFBA;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_IFBB;


        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_BOX;

        //    //        ///<summary>Rectangular section (solid or hollow)</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_RECT;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_RECT;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TUBE;

        //    //        ///<summary>User defined circular hollow section</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_TUBE;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_ISYM;

        //    //        ///<summary>I section with bi symmetry (flanges similar)</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_INSYM;

        //    //        ///<summary>I section with mono symmetry (flanges different)</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TUSER;

        //    //        ///<summary>User defined tee shape</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE;

        //    //        ///<summary>User defined channel</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CUSER;

        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TBETC;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DRECT;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WOOD_DRECT;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WOOD_CIRC;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_CIRC_FILLED;

        //    //        ///<summary>User defined fabricated box with offset webs and bi symmetry (flanges similar)</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_BOX_2;

        //    //        ///<summary>User defined polygonal</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_POLYGONAL;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CORRUGATED_WEB;

        //    //        ///<summary>User defined fabricated box with offset webs and mono symmetry (flanges different)</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;

        //    //        ///<summary>Cruciform section with flanges</summary>
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WELD_CROSS;

        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_CROSS;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_K;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_LH;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_KCS;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_DLH;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_SLH;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_G;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_VG;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_BG;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA1;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA2;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_ZED1;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_U;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_CE1;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_ANGL;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_OMEGA;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SO1;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_RIVE1;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_C_PLUS;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA_SL;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_Z;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_L_LIPS;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_Z_ROT;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_FACE;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_BACK;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2I;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2LI;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_FACE;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_BACK;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_SHORT;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_LONG;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_CROSS;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_SHORT;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_LONG;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI_BACK;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_FACE_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_BACK_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2I_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2LI_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_FACE_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_BACK_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_SHORT_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_LONG_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_CROSS_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_SHORT_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_LONG_WELD;
        //    //        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI_BACK_WELD;
        //    //    }

        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_AX);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_AY);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_AZ);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IX);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IY);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IZ);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VY);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VPY);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VZ);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VPZ);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_SURFACE);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WEIGHT);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RI);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_S);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ZY);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ZZ);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WX);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WY);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WZ);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_GAMMA);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IOMEGA);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P1_LENGTH);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P1_THICKNESS);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P2_LENGTH);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P2_THICKNESS);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P3_LENGTH);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P3_THICKNESS);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P4_LENGTH);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P4_THICKNESS);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF2);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF2);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_DIM1);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_DIM2);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_DIM3);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ANGLE1);
        //    //    //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ANGLE2);

        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B1);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P1_L);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P1_T);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P2_L);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P2_T);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P3_L);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P3_T);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P4_L);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P4_T);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_DRECT_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_DRECT_D);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_DRECT_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_X);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_Z);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_B1);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_D);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_D_IS_INT);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_N);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_T);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_H1);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_H1);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_TW);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_B);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_H);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_TF);
        //    //    //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_TW);

        //    //}
        //    #endregion
        //}

        //public static ShapeType ToBHoMEnum(IRobotBarSectionShapeType sType)
        //{
        //    switch (sType)
        //    {
        //        case IRobotBarSectionShapeType.I_BSST_TRND:
        //        case IRobotBarSectionShapeType.I_BSST_USER_BOX:
        //        case IRobotBarSectionShapeType.I_BSST_USER_BOX_2:
        //        case IRobotBarSectionShapeType.I_BSST_USER_BOX_3:
        //            return ShapeType.Box;
        //        case IRobotBarSectionShapeType.I_BSST_CONCR_COL_R:
        //        case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT:
        //            return ShapeType.Rectangle;
        //        case IRobotBarSectionShapeType.I_BSST_HEA:
        //        case IRobotBarSectionShapeType.I_BSST_HEB:
        //        case IRobotBarSectionShapeType.I_BSST_IPE:
        //        case IRobotBarSectionShapeType.I_BSST_UUPN:
        //        case IRobotBarSectionShapeType.I_BSST_DCIP:
        //        case IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS:
        //        case IRobotBarSectionShapeType.I_BSST_USER_I_BISYM:
        //            return ShapeType.ISection;
        //        case IRobotBarSectionShapeType.I_BSST_UUAP:
        //        case IRobotBarSectionShapeType.I_BSST_CONCR_COL_L:
        //            return ShapeType.Angle;
        //        case IRobotBarSectionShapeType.I_BSST_CONCR_COL_T:
        //        case IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T:
        //        case IRobotBarSectionShapeType.I_BSST_MHEA:
        //        case IRobotBarSectionShapeType.I_BSST_MHEB:
        //        case IRobotBarSectionShapeType.I_BSST_MHEM:
        //        case IRobotBarSectionShapeType.I_BSST_MIPE:
        //        case IRobotBarSectionShapeType.I_BSST_DCED:
        //        case IRobotBarSectionShapeType.I_BSST_IPN:
        //        case IRobotBarSectionShapeType.I_BSST_DCIG:
        //        case IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE:
        //            return ShapeType.Tee;
        //        case IRobotBarSectionShapeType.I_BSST_TRON:
        //        case IRobotBarSectionShapeType.I_BSST_USER_TUBE:
        //            return ShapeType.Tube;
        //        case IRobotBarSectionShapeType.I_BSST_CAE:
        //        case IRobotBarSectionShapeType.I_BSST_CAEP:
        //        case IRobotBarSectionShapeType.I_BSST_CAI:
        //        case IRobotBarSectionShapeType.I_BSST_CAIP:
        //        case IRobotBarSectionShapeType.I_BSST_UPN:
        //        case IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE:
        //            return ShapeType.Channel;
        //        case IRobotBarSectionShapeType.I_BSST_CONCR_COL_C:
        //        case IRobotBarSectionShapeType.I_BSST_CONCR_COL_CQ:
        //            return ShapeType.Circle;
        //    }
        //    return ShapeType.Rectangle;
        //}

        //public static PanelType ToBHoMEnum(IRobotObjectStructuralType robotSType)
        //{
        //    switch (robotSType)
        //    {
        //        case IRobotObjectStructuralType.I_OST_SLAB:
        //            return PanelType.Slab;
        //        case IRobotObjectStructuralType.I_OST_WALL:
        //            return PanelType.Wall;
        //        default:
        //            return PanelType.Undefined;
        //    }
        // }
        #endregion

    }
}
