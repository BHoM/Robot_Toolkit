namespace RobotToolkit.SectionProperties
{
    /// <summary>
    /// Section property class, the parent abstract class for all structural 
    /// sections (RC, steel, PT beams, columns, bracing). Properties defined in this 
    /// parent class are those that would populate a multi category section database only
    /// </summary>
    public abstract class SectionProperty
    {
        /// <summary>Mass per metre based on section properties</summary>
        public double MassPerMetre { get; set; }

        /// <summary>Name of section propert - a user defined, instance based parameter</summary>
        public string Name { get; set; }

        /// <summary>Section type</summary>
        public string Type { get; set; }

        /// <summary>Information regarding section property type for the user</summary>
        public string Description { get; set; }

        /// <summary>Section material</summary>
        public BHoM.Materials.Material Material { get; set; }

   
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_R;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_T;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_L;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_Z;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_P;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_C;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_CH;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_COL_CQ;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_I;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_RECT;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM_T;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CONCR_BEAM;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UNKNOWN;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAEP;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAI;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CAIP;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCEC;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCED;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCEP;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCIG;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DCIP;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEA;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEAA;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEB;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEC;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HEM;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HER;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HHEA;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HHEB;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_HHEM;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IIPE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPEA;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPEO;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPER;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPEV;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_IPN;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MHEA;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MHEB;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MHEM;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_MIPE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_PRS;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TCAR;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TEAE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TEAI;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_THEX;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TREC;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TRON;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UAP;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UPN;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UUAP;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UUPN;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_FRTG;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WOOD_RECT;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_UPAF;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_RECT_FILLED;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CIRC_FILLED;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CCL;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_URND;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TRND;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CUAP;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_ROUND_OPENINGS;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CASTELLATED_WEB_HEXAGONAL_OPENINGS_SHIFTED;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_SFB;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_IFBA;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_IFBB;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_BOX;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_BOX;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_RECT;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_RECT;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TUBE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_TUBE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_ISYM;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_I_BISYM;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_INSYM;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TUSER;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_T_SHAPE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_CUSER;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_C_SHAPE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_TBETC;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_DRECT;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WOOD_DRECT;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WOOD_CIRC;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_CIRC_FILLED;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_BOX_2;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_POLYGONAL;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_SPEC_CORRUGATED_WEB;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_BOX_3;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_WELD_CROSS;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_USER_CROSS;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_K;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_LH;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_KCS;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_DLH;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_SLH;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_G;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_VG;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_JOIST_BG;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA1;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA2;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_ZED1;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_U;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_CE1;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_ANGL;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_OMEGA;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SO1;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_RIVE1;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_C_PLUS;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA_SL;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_SIGMA;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_Z;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_L_LIPS;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COLD_Z_ROT;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_FACE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_BACK;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2I;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2LI;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_FACE;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_BACK;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_SHORT;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_LONG;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_CROSS;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_SHORT;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_LONG;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI_BACK;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_FACE_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2C_BACK_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2I_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2LI_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_FACE_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_4L_BACK_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_SHORT_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_LONG_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_CROSS_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_SHORT_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_2L_FACE_LONG_WELD;
        //sec_shape_type = IRobotBarSectionShapeType.I_BSST_COMP_CI_BACK_WELD;


        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_AX);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_AY);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_AZ);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IX);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IY);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IZ);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VY);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VPY);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VZ);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_VPZ);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_SURFACE);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WEIGHT);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_D);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TW);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RA);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_RI);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_S);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ZY);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ZZ);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WX);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WY);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_WZ);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_GAMMA);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_IOMEGA);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P1_LENGTH);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P1_THICKNESS);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P2_LENGTH);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P2_THICKNESS);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P3_LENGTH);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P3_THICKNESS);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P4_LENGTH);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_P4_THICKNESS);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_BF2);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_TF2);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_DIM1);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_DIM2);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_DIM3);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ANGLE1);
        //sec_data.GetValue(IRobotBarSectionDataValue.I_BSDV_ANGLE2);

        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_B1);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_2_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B1);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_B2);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TF2);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_3_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_BOX_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P1_L);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P1_T);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P2_L);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P2_T);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P3_L);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P3_T);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P4_L);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_CROSS_P4_T);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_C_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_DRECT_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_DRECT_D);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_DRECT_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_X);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_HOLE_Z);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_B1);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_H_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_I_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_L_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_D);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_D_IS_INT);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_N);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_POLYGONAL_T);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_RECT_T);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_D);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_TUBE_T);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_T_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_H1);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XI_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_H1);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_XT_TW);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_B);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_H);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_TF);
        //nonstd_sec_data.GetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_Z_TW);

    }
}