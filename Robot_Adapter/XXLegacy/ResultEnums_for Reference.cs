/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

enum T_DATA_TYPES
{
    /*    */
    T_DATA_TYPES_NUL = -1,
    /*    */        //N O D E S
                    /*0000*/
    T_NODE_COORD_CART_X,          //node's coordinates X (cartesian)
                                  /*0001*/
    T_NODE_COORD_CART_Y,          //node's coordinates Y (cartesian)
                                  /*0002*/
    T_NODE_COORD_CART_Z,          //node's coordinates Z (cartesian)
                                  /*0003*/
    T_NODE_COORD_POLA_X,          //node's coordinates X (polar)
                                  /*0004*/
    T_NODE_COORD_POLA_Y,          //node's coordinates Y (polar)
                                  /*0005*/
    T_NODE_COORD_POLA_Z,          //node's coordinates Z (polar)
                                  /*0006*/
    T_NODE_COORD_CYL_X,           //node's coordinates X (cylindrical)
                                  /*0007*/
    T_NODE_COORD_CYL_Y,           //node's coordinates Y (cylindrical)
                                  /*0008*/
    T_NODE_COORD_CYL_Z,           //node's coordinates Z (cylindrical)
                                  /*0009*/
    T_NODE_COORD_SPHE_X,          //node's coordinates X (spherical)
                                  /*0010*/
    T_NODE_COORD_SPHE_Y,          //node's coordinates Y (spherical)
                                  /*0011*/
    T_NODE_COORD_SPHE_Z,          //node's coordinates Z (spherical)
                                  /*0012*/
    T_NODE_COMPAT,                //nodes compatibles
                                  /*0013*/
    T_NODE_RIGID,                 //nodes rigid links
                                  /*0014*/
    T_NODE_SUPP,                  //nodes supports
                                  /*    */        //E L E M E N T S
                                                  /*0015*/
    T_ELEM_NODES_1,               //element's node 1
                                  /*0016*/
    T_ELEM_NODES_2,               //element's node 2
                                  /*0017*/
    T_ELEM_LENGHT,                //element's lenght
                                  /*0018*/
    T_ELEM_MATERIAL,              //element's material
                                  /*0019*/
    T_ELEM_RELACH,
    /*0020*/
    T_ELEM_GAMMA,
    /*0021*/
    T_ELEM_SECT,
    /*0022*/
    T_ELEM_OFFSET,
    /*    */        //S E C T I O N S
                    /*0023*/
    T_SECT_ELEMLIST,              //elements list
                                  /*0024*/
    T_SECT_NAME,
    /*0025*/
    T_SECT_SUR_EP,                //surface
                                  /*0026*/
    T_SECT_SUR_KZ,
    /*0027*/
    T_SECT_SX,                    //basic
                                  /*0028*/
    T_SECT_SY,
    /*0029*/
    T_SECT_SZ,
    /*0030*/
    T_SECT_IX,
    /*0031*/
    T_SECT_IY,
    /*0032*/
    T_SECT_IZ,
    /*0033*/
    T_SECT_HY,                    //dimensions
                                  /*0034*/
    T_SECT_HZ,
    /*0035*/
    T_SECT_VY,
    /*0036*/
    T_SECT_VZ,
    /*0037*/
    T_SECT_VPY,
    /*0038*/
    T_SECT_VPZ,
    /*0039*/
    T_SECT_AX,                    //coefficients
                                  /*0040*/
    T_SECT_AY,
    /*0041*/
    T_SECT_AZ,
    /*0042*/
    T_SECT_SURF,                  //surface & mass
                                  /*0043*/
    T_SECT_MAS,
    /*0044*/
    T_SECT_ELA_KX,                //elastic
                                  /*0045*/
    T_SECT_ELA_KY,
    /*0046*/
    T_SECT_ELA_KZ,
    /*0047*/
    T_SECT_ELA_HX,
    /*0048*/
    T_SECT_ELA_HY,
    /*0049*/
    T_SECT_ELA_HZ,
    /*0050*/
    T_SECT_TP_CAI_H,              //caisson
                                  /*0051*/
    T_SECT_TP_CAI_B,
    /*0052*/
    T_SECT_TP_CAI_ES,
    /*0053*/
    T_SECT_TP_CAI_EA,
    /*0054*/
    T_SECT_TP_I_H,                //ISym
                                  /*0055*/
    T_SECT_TP_I_B,
    /*0056*/
    T_SECT_TP_I_ES,
    /*0057*/
    T_SECT_TP_I_EA,
    /*0058*/
    T_SECT_TP_II_H,              //INSym
                                 /*0059*/
    T_SECT_TP_II_EA,
    /*0060*/
    T_SECT_TP_II_ES,
    /*0061*/
    T_SECT_TP_II_ES2,
    /*0062*/
    T_SECT_TP_II_B,
    /*0063*/
    T_SECT_TP_II_B2,
    /*0064*/
    T_SECT_TP_TUBE_DI,           //Tube
                                 /*0065*/
    T_SECT_TP_TUBE_EP,
    /*0066*/
    T_SECT_TP_RECT_B,             //Rectangle
                                  /*0067*/
    T_SECT_TP_RECT_H,
    /*0068*/
    T_SECT_TP_RECT_EP,
    /*0069*/
    T_SECT_CB_SX,                //Cables
                                 /*0070*/
    T_SECT_CB_H,
    /*0071*/
    T_SECT_CB_LO,
    /*0072*/
    T_SECT_CB_DILABS,
    /*0073*/
    T_SECT_CB_DILREL,
    /*    */        //M A T E R I A L S
                    /*0074*/
    T_MAT_ELEMLIST,               //elements list
                                  /*0075*/
    T_MAT_NAME,
    /*0076*/
    T_MAT_YOUNG,
    /*0077*/
    T_MAT_G,
    /*0078*/
    T_MAT_POISSON,
    /*0079*/
    T_MAT_TERMI,
    /*0080*/
    T_MAT_DENS,
    /*0081*/
    T_MAT_ELAST,
    /*    */        //S U P P O R T S
                    /*0082*/
    T_SUPP_NODELIST,
    /*0083*/
    T_SUPP_NAME,
    /*0084*/
    T_SUPP_DESCR,
    /*0085*/
    T_SUPP_DOF_CODE,
    /*0086*/
    T_SUPP_UX,
    /*0087*/
    T_SUPP_UY,
    /*0088*/
    T_SUPP_UZ,
    /*0089*/
    T_SUPP_RX,
    /*0090*/
    T_SUPP_RY,
    /*0091*/
    T_SUPP_RZ,
    /*0092*/
    T_SUPP_ALFA,
    /*0093*/
    T_SUPP_BETA,
    /*0094*/
    T_SUPP_GAMMA,
    /*    */        //L O A D S
                    /*0095*/
    T_LOAD_ELENOELIST,
    /*0096*/
    T_LOAD_NAME,
    /*0097*/
    T_LOAD_DESCRIPTION,
    /*    */        //M A S S E S
                    /*0098*/
    T_MASS_UX,
    /*0099*/
    T_MASS_UY,
    /*0100*/
    T_MASS_UZ,
    /*0101*/
    T_MASS_RX,
    /*0102*/
    T_MASS_RY,
    /*0103*/
    T_MASS_RZ,
    /*    */        //C O M B I N A T I O N S
                    /*    */        //P O N D E R A T I O N S
                                    /*    */        //M O B I L E  L O A D S
                                                    /*    */        //M E T R E
                                                                    /*0104*/
    T_MET_LEN,   //elements length
                 /*0105*/
    T_MET_WUN,   //Uni.weight
                 /*0106*/
    T_MET_WPC,   //Obj.weight
                 /*0107*/
    T_MET_WTO,   //Tot.weight
                 /*0108*/
    T_MET_PTO,   //Tot. area
                 /*    */        //R E A C T I O N S
                                 /*0109*/
    T_REAC_FX,
    /*0110*/
    T_REAC_FY,
    /*0111*/
    T_REAC_FZ,
    /*0112*/
    T_REAC_MX,
    /*0113*/
    T_REAC_MY,
    /*0114*/
    T_REAC_MZ,
    /*0115*/
    T_REAC_SVAL_FX,
    /*0116*/
    T_REAC_SVAL_FY,
    /*0117*/
    T_REAC_SVAL_FZ,
    /*0118*/
    T_REAC_SVAL_MX,
    /*0119*/
    T_REAC_SVAL_MY,
    /*0120*/
    T_REAC_SVAL_MZ,
    /*0121*/
    T_REAC_SREA_FX,
    /*0122*/
    T_REAC_SREA_FY,
    /*0123*/
    T_REAC_SREA_FZ,
    /*0124*/
    T_REAC_SREA_MX,
    /*0125*/
    T_REAC_SREA_MY,
    /*0126*/
    T_REAC_SREA_MZ,
    /*0127*/
    T_REAC_SEFF_FX,
    /*0128*/
    T_REAC_SEFF_FY,
    /*0129*/
    T_REAC_SEFF_FZ,
    /*0130*/
    T_REAC_SEFF_MX,
    /*0131*/
    T_REAC_SEFF_MY,
    /*0132*/
    T_REAC_SEFF_MZ,
    /*0133*/
    T_REAC_VERI_FX,
    /*0134*/
    T_REAC_VERI_FY,
    /*0135*/
    T_REAC_VERI_FZ,
    /*0136*/
    T_REAC_VERI_MX,
    /*0137*/
    T_REAC_VERI_MY,
    /*0138*/
    T_REAC_VERI_MZ,
    /*0139*/
    T_REAC_COEF1,
    /*0140*/
    T_REAC_COEF2,
    /*    */        //D I S P L A C E M E N T S
                    /*0141*/
    T_DISP_NODE_UX,           //displacements of nodes
                              /*0142*/
    T_DISP_NODE_UY,
    /*0143*/
    T_DISP_NODE_UZ,
    /*0144*/
    T_DISP_NODE_RX,
    /*0145*/
    T_DISP_NODE_RY,
    /*0146*/
    T_DISP_NODE_RZ,
    /*0147*/
    T_DISP_ELEM_UX,           //displacements of elements
                              /*0148*/
    T_DISP_ELEM_UY,
    /*0149*/
    T_DISP_ELEM_UZ,
    /*0150*/
    T_DISP_ELEM_RX,
    /*0151*/
    T_DISP_ELEM_RY,
    /*0152*/
    T_DISP_ELEM_RZ,
    /*0153*/
    T_DISP_DEFL_UX,           //deflection
                              /*0154*/
    T_DISP_DEFL_UY,
    /*0155*/
    T_DISP_DEFL_UZ,
    /*0156*/
    T_DISP_DEFL_RX,
    /*0157*/
    T_DISP_DEFL_RY,
    /*0158*/
    T_DISP_DEFL_RZ,
    /*0159*/
    T_DISP_DEFL_MAX_UX,       //maximal deflection
                              /*0160*/
    T_DISP_DEFL_MAX_UY,
    /*0161*/
    T_DISP_DEFL_MAX_UZ,
    /*0162*/
    T_DISP_DEFL_MAX_RX,
    /*0163*/
    T_DISP_DEFL_MAX_RY,
    /*0164*/
    T_DISP_DEFL_MAX_RZ,
    /*    */        //F O R C E S
                    /*0165*/
    T_FORC_FX,
    /*0166*/
    T_FORC_FY,
    /*0167*/
    T_FORC_FZ,
    /*0168*/
    T_FORC_MX,
    /*0169*/
    T_FORC_MY,
    /*0170*/
    T_FORC_MZ,
    /*    */        //S T R E S S E S
                    /*0171*/
    T_CONT_SMAX,
    /*0172*/
    T_CONT_SMIN,
    /*0173*/
    T_CONT_SFX,
    /*0174*/
    T_CONT_SMAXMY,
    /*0175*/
    T_CONT_SMAXMZ,
    /*0176*/
    T_CONT_SMINMY,
    /*0177*/
    T_CONT_SMINMZ,
    /*0178*/
    T_CONT_E1,
    /*0179*/
    T_CONT_C1,
    /*0180*/
    T_CONT_C2,
    /*0181*/
    T_CONT_E2,
    /*0182*/
    T_CONT_TO,
    /*0183*/
    T_CONT_E3,
    /*    */        //D Y N A M I C
                    /*0184*/
    T_CURMASS_UX,
    /*0185*/
    T_CURMASS_UY,
    /*0186*/
    T_CURMASS_UZ,
    /*0187*/
    T_CURMASS_RX,
    /*0188*/
    T_CURMASS_RY,
    /*0189*/
    T_CURMASS_RZ,
    /*0190*/
    T_SUMMASS_UX,
    /*0191*/
    T_SUMMASS_UY,
    /*0192*/
    T_SUMMASS_UZ,
    /*0193*/
    T_SUMMASS_RX,
    /*0194*/
    T_SUMMASS_RY,
    /*0195*/
    T_SUMMASS_RZ,
    /*0196*/
    T_TOTMASS_UX,
    /*0197*/
    T_TOTMASS_UY,
    /*0198*/
    T_TOTMASS_UZ,
    /*0199*/
    T_TOTMASS_RX,
    /*0200*/
    T_TOTMASS_RY,
    /*0201*/
    T_TOTMASS_RZ,
    /*0202*/
    T_PART_FACT1_UX,
    /*0203*/
    T_PART_FACT1_UY,
    /*0204*/
    T_PART_FACT1_UZ,
    /*0205*/
    T_PART_FACT1_RX,
    /*0206*/
    T_PART_FACT1_RY,
    /*0207*/
    T_PART_FACT1_RZ,
    /*0208*/
    T_DYNM_FREQ,
    /*0209*/
    T_DYNM_PREC,
    /*    */        //SEISMIC
                    /*0210*/
    T_SPESIS_MULT_UX,
    /*0211*/
    T_SPESIS_MULT_UY,
    /*0212*/
    T_SPESIS_MULT_UZ,
    /*0213*/
    T_SPESIS_MULT_RX,
    /*0214*/
    T_SPESIS_MULT_RY,
    /*0215*/
    T_SPESIS_MULT_RZ,
    /*0216*/
    T_PART_FACT2_UX,
    /*0217*/
    T_PART_FACT2_UY,
    /*0218*/
    T_PART_FACT2_UZ,
    /*0219*/
    T_PART_FACT2_RX,
    /*0220*/
    T_PART_FACT2_RY,
    /*0221*/
    T_PART_FACT2_RZ,
    /*0222*/
    T_MOD_COEF_UX,
    /*0223*/
    T_MOD_COEF_UY,
    /*0224*/
    T_MOD_COEF_UZ,
    /*0225*/
    T_MOD_COEF_RX,
    /*0226*/
    T_MOD_COEF_RY,
    /*0227*/
    T_MOD_COEF_RZ,
    /*    */        //EIGENVALUES
                    /*0228*/
    T_EIGEN_UX,
    /*0229*/
    T_EIGEN_UY,
    /*0230*/
    T_EIGEN_UZ,
    /*0231*/
    T_EIGEN_RX,
    /*0232*/
    T_EIGEN_RY,
    /*0233*/
    T_EIGEN_RZ,
    /*0234*/
    T_EIGEN_UX_1,
    /*0235*/
    T_EIGEN_UY_1,
    /*0236*/
    T_EIGEN_UZ_1,
    /*0237*/
    T_EIGEN_RX_1,
    /*0238*/
    T_EIGEN_RY_1,
    /*0239*/
    T_EIGEN_RZ_1,
    /*    */        //B U C K L I N G
                    /*0240*/
    T_BUCK_PCR,
    /*0241*/
    T_BUCK_LFY,
    /*0242*/
    T_BUCK_LFZ,
    /*0243*/
    T_BUCK_ELY,
    /*0244*/
    T_BUCK_ELZ,
    /*    */        //MODES
                    /*0245*/
    T_MODES_UX,
    /*0246*/
    T_MODES_UY,
    /*0247*/
    T_MODES_UZ,
    /*0248*/
    T_MODES_RX,
    /*0249*/
    T_MODES_RY,
    /*0250*/
    T_MODES_RZ,
    /*    */        //P S E U D O - S T A T I C A L  F O R C E S
                    /*0251*/
    T_PSFO_FX,
    /*0252*/
    T_PSFO_FY,
    /*0253*/
    T_PSFO_FZ,
    /*0254*/
    T_PSFO_MX,
    /*0255*/
    T_PSFO_MY,
    /*0256*/
    T_PSFO_MZ,
    /*    */        //T E M P O R A L   A N A L S E
                    /*0257*/
    T_VEL_UX,
    /*0258*/
    T_VEL_UY,
    /*0259*/
    T_VEL_UZ,
    /*0260*/
    T_VEL_RX,
    /*0261*/
    T_VEL_RY,
    /*0262*/
    T_VEL_RZ,
    /*0263*/
    T_ACC_UX,
    /*0264*/
    T_ACC_UY,
    /*0265*/
    T_ACC_UZ,
    /*0266*/
    T_ACC_RX,
    /*0267*/
    T_ACC_RY,
    /*0268*/
    T_ACC_RZ,
    /*    */        //M E M B E R S
                    /*0269*/
    T_MEMB_NODES_1,
    /*0270*/
    T_MEMB_NODES_2,
    /*0271*/
    T_MEMB_LENGHT,
    /*0272*/
    T_MEMB_MATERIAL,
    /*0273*/
    T_MEMB_RELACH,
    /*0274*/
    T_MEMB_GAMMA,
    /*0275*/
    T_MEMB_SECT,
    /*0276*/
    T_MEMB_OFFSET,
    /*0277*/
    T_MEMB_TYPE,
    /*    */        //L O A D S  D E F I N I N G
                    /*0278*/
    T_LOAD_RECNO,     // RLIST internal
                      /*0279*/
    T_LOAD_CASNO,     // RLIST internal
                      /*0280*/
    T_LOAD_CASTXT,
    /*0281*/
    T_LOAD_TYPE,      // RLIST internal
                      /*0282*/
    T_LOAD_LABEL,     // RLIST internal
                      /*0283*/
    T_LOAD_LIST,      // RLIST internal
                      /*0284*/
    T_LOAD_NATURE,    // RLIST internal
                      /*0285*/
    T_LOAD_GROUP,     // RLIST internal
                      /*0286*/
    T_LOAD_EDIT,      // RLIST internal
                      /*    */
                      /*0287*/
    T_NOEFOR_LABEL,
    /*0288*/
    T_NOEFOR_LIST,
    /*0289*/
    T_NOEFOR_FX,
    /*0290*/
    T_NOEFOR_FY,
    /*0291*/
    T_NOEFOR_FZ,
    /*0292*/
    T_NOEFOR_CX,
    /*0293*/
    T_NOEFOR_CY,
    /*0294*/
    T_NOEFOR_CZ,
    /*0295*/
    T_NOEFOR_ALFA,
    /*0296*/
    T_NOEFOR_BETA,
    /*0297*/
    T_NOEFOR_GAMMA,
    /*0298*/
    T_NOEFOR_T,
    /*0299*/
    T_NOEFOR_LS,
    /*    */
    /*0300*/
    T_UNIFORM_LABEL,
    /*0301*/
    T_UNIFORM_LIST,
    /*0302*/
    T_UNIFORM_PX,
    /*0303*/
    T_UNIFORM_PY,
    /*0304*/
    T_UNIFORM_PZ,
    /*0305*/
    T_UNIFORM_ALFA,
    /*0306*/
    T_UNIFORM_BETA,
    /*0307*/
    T_UNIFORM_GAMMA,
    /*0308*/
    T_UNIFORM_LOC,
    /*0309*/
    T_UNIFORM_PROJ,
    /*0310*/
    T_UNIFORM_REL,
    /*0311*/
    T_UNIFORM_T,
    /*0312*/
    T_UNIFORM_LS,
    /*0313*/
    T_UNIFORM_UD,
    /*    */
    /*0314*/
    T_POIPRO_LABEL,
    /*0315*/
    T_POIPRO_LIST,
    /*0316*/
    T_POIPRO_X,
    /*0317*/
    T_POIPRO_Y,
    /*0318*/
    T_POIPRO_Z,
    /*0319*/
    T_POIPRO_XYZ,
    /*    */
    /*0320*/
    T_NOEDIS_LABEL,
    /*0321*/
    T_NOEDIS_LIST,
    /*0322*/
    T_NOEDIS_UX,
    /*0323*/
    T_NOEDIS_UY,
    /*0324*/
    T_NOEDIS_UZ,
    /*0325*/
    T_NOEDIS_RX,
    /*0326*/
    T_NOEDIS_RY,
    /*0327*/
    T_NOEDIS_RZ,
    /*0328*/
    T_NOEDIS_ALFA,
    /*0329*/
    T_NOEDIS_BETA,
    /*0330*/
    T_NOEDIS_GAMMA,
    /*    */
    /*0331*/
    T_DILAT_LABEL,
    /*0332*/
    T_DILAT_LIST,
    /*0333*/
    T_DILAT_DIL,
    /*0334*/
    T_DILAT_REL,
    /*    */
    /*0335*/
    T_FORCON_LABEL,
    /*0336*/
    T_FORCON_LIST,
    /*0337*/
    T_FORCON_FX,
    /*0338*/
    T_FORCON_FY,
    /*0339*/
    T_FORCON_FZ,
    /*0340*/
    T_FORCON_MX,
    /*0341*/
    T_FORCON_MY,
    /*0342*/
    T_FORCON_MZ,
    /*0343*/
    T_FORCON_X,
    /*0344*/
    T_FORCON_ALFA,
    /*0345*/
    T_FORCON_BETA,
    /*0346*/
    T_FORCON_GAMMA,
    /*0347*/
    T_FORCON_LOC,
    /*0348*/
    T_FORCON_REL,
    /*0349*/
    T_FORCON_T,
    /*0350*/
    T_FORCON_LS,
    /*0351*/
    T_FORCON_UD,
    /*    */
    /*0352*/
    T_MOMENT_LABEL,
    /*0353*/
    T_MOMENT_LIST,
    /*0354*/
    T_MOMENT_MX1,
    /*0355*/
    T_MOMENT_MY1,
    /*0356*/
    T_MOMENT_MZ1,
    /*0357*/
    T_MOMENT_MX2,
    /*0358*/
    T_MOMENT_MY2,
    /*0359*/
    T_MOMENT_MZ2,
    /*0360*/
    T_MOMENT_X1,
    /*0361*/
    T_MOMENT_X2,
    /*0362*/
    T_MOMENT_ALFA,
    /*0363*/
    T_MOMENT_BETA,
    /*0364*/
    T_MOMENT_GAMMA,
    /*0365*/
    T_MOMENT_LOC,
    /*0366*/
    T_MOMENT_PROJ,
    /*0367*/
    T_MOMENT_REL,
    /*    */
    /*0368*/
    T_PRETRA_LABEL,
    /*0369*/
    T_PRETRA_LIST,
    /*0370*/
    T_PRETRA_PX1,
    /*0371*/
    T_PRETRA_PY1,
    /*0372*/
    T_PRETRA_PZ1,
    /*0373*/
    T_PRETRA_PX2,
    /*0374*/
    T_PRETRA_PY2,
    /*0375*/
    T_PRETRA_PZ2,
    /*0376*/
    T_PRETRA_X1,
    /*0377*/
    T_PRETRA_X2,
    /*0378*/
    T_PRETRA_ALFA,
    /*0379*/
    T_PRETRA_BETA,
    /*0380*/
    T_PRETRA_BAMMA,
    /*0381*/
    T_PRETRA_LOC,
    /*0382*/
    T_PRETRA_PROJ,
    /*0383*/
    T_PRETRA_REL,
    /*0384*/
    T_PRETRA_T,
    /*0385*/
    T_PRETRA_LS,
    /*0386*/
    T_PRETRA_UD,
    /*    */
    /*0387*/
    T_THERMAL_LABEL,
    /*0388*/
    T_THERMAL_LIST,
    /*0389*/
    T_THERMAL_TX,
    /*0390*/
    T_THERMAL_TY,
    /*0391*/
    T_THERMAL_TZ,
    /*    */
    /*0392*/
    T_DEFORM_NORM,
    /*0393*/
    T_DEFORM_EXAC,
    /*    */
    /*    */        //LOADS FOR BEAM DEFINITION
                    /*0394*/
    T_BEAM_TYPE,
    /*0395*/
    T_BEAM_LIST,
    /*0396*/
    T_BEAM_NATURE,
    /*0397*/
    T_BEAM_POSITION,
    /*0398*/
    T_BEAM_X0,
    /*0399*/
    T_BEAM_XE,
    /*0400*/
    T_BEAM_PZ0,
    /*0401*/
    T_BEAM_PZE,
    /*0402*/
    T_BEAM_FZ,
    /*0403*/
    T_BEAM_MY,
    /*    */
    /*    */        //ADDITIONAL VALUES FOR DYNAMIC
                    /*0404*/
    T_DYNM_OFTEN,
    /*0405*/
    T_DYNM_PERIODE,
    /*0406*/
    T_DYNM_PULSAT,
    /*    */
    /*    */        //DEFINITION OF COMBINATIONS TABLE
                    /*0407*/
    T_COMB_NAME,
    /*0408*/
    T_COMB_TYPE,
    /*0409*/
    T_COMB_DEFINITION,
    /*    */
    /*    */        //ADDED MASSES
                    /*0410*/
    T_NOEFOR_MASS_LABEL,
    /*0411*/
    T_NOEFOR_MASS_LIST,
    /*0412*/
    T_NOEFOR_MASS_FX,
    /*0413*/
    T_NOEFOR_MASS_FY,
    /*0414*/
    T_NOEFOR_MASS_FZ,
    /*0415*/
    T_NOEFOR_MASS_CX,
    /*0416*/
    T_NOEFOR_MASS_CY,
    /*0417*/
    T_NOEFOR_MASS_CZ,
    /*0418*/
    T_NOEFOR_MASS_ALFA,
    /*0419*/
    T_NOEFOR_MASS_BETA,
    /*0420*/
    T_NOEFOR_MASS_GAMMA,
    /*0421*/
    T_NOEFOR_MASS_T,
    /*0422*/
    T_NOEFOR_MASS_LS,
    /*    */
    /*0423*/
    T_UNIFORM_MASS_LABEL,
    /*0424*/
    T_UNIFORM_MASS_LIST,
    /*0425*/
    T_UNIFORM_MASS_PX,
    /*0426*/
    T_UNIFORM_MASS_PY,
    /*0427*/
    T_UNIFORM_MASS_PZ,
    /*0428*/
    T_UNIFORM_MASS_ALFA,
    /*0429*/
    T_UNIFORM_MASS_BETA,
    /*0430*/
    T_UNIFORM_MASS_GAMMA,
    /*0431*/
    T_UNIFORM_MASS_LOC,
    /*0432*/
    T_UNIFORM_MASS_PROJ,
    /*0433*/
    T_UNIFORM_MASS_REL,
    /*0434*/
    T_UNIFORM_MASS_T,
    /*0435*/
    T_UNIFORM_MASS_LS,
    /*0436*/
    T_UNIFORM_MASS_UD,
    /*    */
    /*0437*/
    T_FORCON_MASS_LABEL,
    /*0438*/
    T_FORCON_MASS_LIST,
    /*0439*/
    T_FORCON_MASS_FX,
    /*0440*/
    T_FORCON_MASS_FY,
    /*0441*/
    T_FORCON_MASS_FZ,
    /*0442*/
    T_FORCON_MASS_MX,
    /*0443*/
    T_FORCON_MASS_MY,
    /*0444*/
    T_FORCON_MASS_MZ,
    /*0445*/
    T_FORCON_MASS_X,
    /*0446*/
    T_FORCON_MASS_ALFA,
    /*0447*/
    T_FORCON_MASS_BETA,
    /*0448*/
    T_FORCON_MASS_GAMMA,
    /*0449*/
    T_FORCON_MASS_LOC,
    /*0450*/
    T_FORCON_MASS_REL,
    /*0451*/
    T_FORCON_MASS_T,
    /*0452*/
    T_FORCON_MASS_LS,
    /*0453*/
    T_FORCON_MASS_UD,
    /*    */
    /*0454*/
    T_PRETRA_MASS_LABEL,
    /*0455*/
    T_PRETRA_MASS_LIST,
    /*0456*/
    T_PRETRA_MASS_PX1,
    /*0457*/
    T_PRETRA_MASS_PY1,
    /*0458*/
    T_PRETRA_MASS_PZ1,
    /*0459*/
    T_PRETRA_MASS_PX2,
    /*0460*/
    T_PRETRA_MASS_PY2,
    /*0461*/
    T_PRETRA_MASS_PZ2,
    /*0462*/
    T_PRETRA_MASS_X1,
    /*0463*/
    T_PRETRA_MASS_X2,
    /*0464*/
    T_PRETRA_MASS_ALFA,
    /*0465*/
    T_PRETRA_MASS_BETA,
    /*0466*/
    T_PRETRA_MASS_BAMMA,
    /*0467*/
    T_PRETRA_MASS_LOC,
    /*0468*/
    T_PRETRA_MASS_PROJ,
    /*0469*/
    T_PRETRA_MASS_REL,
    /*0470*/
    T_PRETRA_MASS_T,
    /*0471*/
    T_PRETRA_MASS_LS,
    /*0472*/
    T_PRETRA_MASS_UD,
    /*    */
    /*    */        //PONDERATIONS
                    /*0473*/
    T_POND_COMPNO,        //internal RLIST
                          /*0474*/
    T_POND_DEFINITION,    //internal RLIST
                          /*0475*/
    T_POND_COMPOS_MAX,
    /*0476*/
    T_POND_COMPOS_MIN,
    /*    */
    /*0477*/
    T_MET_NUM,            //number of elements with pointed section
                          /*    */
                          /*    */        //B U C K L I N G
                                          /*0478*/
    T_BUCK_COEFCR,
    /*0479*/
    T_BUCK_PRECCR,
    /*    */
    /*    */        //CASE INFO
                    /*0480*/
    T_CASE_NAME,
    /*0481*/
    T_CASE_NATURE,
    /*0482*/
    T_CASE_ANALTYPE,
    /*    */
    /*    */        //RESULTS FOR FINITE ELEMENTS
                    /*0483*/
    T_FE_S_XX,
    /*0484*/
    T_FE_S_YY,
    /*0485*/
    T_FE_S_XY,
    /*0486*/
    T_FE_S_ROUND,
    /*0487*/
    T_FE_S_MAX,
    /*0488*/
    T_FE_S_MIN,
    /*0489*/
    T_FE_S_ALPHA,
    /*0490*/
    T_FE_S_SHEA,
    /*0491*/
    T_FE_S_MISES,

    /*0492*/
    T_FE_N_XX,
    /*0493*/
    T_FE_N_YY,
    /*0494*/
    T_FE_N_XY,
    /*0495*/
    T_FE_N_ROUND,
    /*0496*/
    T_FE_N_MAX,
    /*0497*/
    T_FE_N_MIN,
    /*0498*/
    T_FE_N_ALPHA,
    /*0499*/
    T_FE_N_SHEA,
    /*0500*/
    T_FE_N_MISES,

    /*0501*/
    T_FE_M_XX,
    /*0502*/
    T_FE_M_YY,
    /*0503*/
    T_FE_M_XY,
    /*0504*/
    T_FE_M_ROUND,
    /*0505*/
    T_FE_M_MAX,
    /*0506*/
    T_FE_M_MIN,
    /*0507*/
    T_FE_M_ALPHA,
    /*0508*/
    T_FE_M_SHEA,
    /*0509*/
    T_FE_M_MISES,

    /*0510*/
    T_FE_T_XX,
    /*0511*/
    T_FE_T_YY,
    /*0512*/
    T_FE_T_XY,
    /*0513*/
    T_FE_T_ROUND,
    /*0514*/
    T_FE_T_MAX,
    /*0515*/
    T_FE_T_MIN,
    /*0516*/
    T_FE_T_ALPHA,
    /*0517*/
    T_FE_T_SHEA,
    /*0518*/
    T_FE_T_MISES,

    /*0519*/
    T_FE_Q_XX,
    /*0520*/
    T_FE_Q_YY,
    /*0521*/
    T_FE_Q_XY,
    /*0522*/
    T_FE_Q_ROUND,
    /*0523*/
    T_FE_Q_MAX,
    /*0524*/
    T_FE_Q_MIN,
    /*0525*/
    T_FE_Q_ALPHA,
    /*0526*/
    T_FE_Q_SHEA,
    /*0527*/
    T_FE_Q_MISES,

    /*0528*/
    T_FE_W_XX,
    /*0529*/
    T_FE_W_YY,
    /*0530*/
    T_FE_W_XY,
    /*0531*/
    T_FE_W_ROUND,
    /*0532*/
    T_FE_W_MAX,
    /*0533*/
    T_FE_W_MIN,
    /*0534*/
    T_FE_W_ALPHA,
    /*0535*/
    T_FE_W_SHEA,
    /*0536*/
    T_FE_W_MISES,

    /*0537*/
    T_FE_U_XX,
    /*0538*/
    T_FE_U_YY,
    /*0539*/
    T_FE_U_XY,
    /*0540*/
    T_FE_U_ROUND,
    /*0541*/
    T_FE_U_MAX,
    /*0542*/
    T_FE_U_MIN,
    /*0543*/
    T_FE_U_ALPHA,
    /*0544*/
    T_FE_U_SHEA,
    /*0545*/
    T_FE_U_MISES,

    /*0546*/
    T_FE_ROT_XX,
    /*0547*/
    T_FE_ROT_YY,
    /*0548*/
    T_FE_ROT_XY,
    /*0549*/
    T_FE_ROT_ROUND,
    /*0550*/
    T_FE_ROT_MAX,
    /*0551*/
    T_FE_ROT_MIN,
    /*0552*/
    T_FE_ROT_ALPHA,
    /*0553*/
    T_FE_ROT_SHEA,
    /*0554*/
    T_FE_ROT_MISES,

    /*0555*/
    T_FE_P_XX,
    /*0556*/
    T_FE_P_YY,
    /*0557*/
    T_FE_P_XY,
    /*0558*/
    T_FE_P_ROUND,
    /*0559*/
    T_FE_P_MAX,
    /*0560*/
    T_FE_P_MIN,
    /*0561*/
    T_FE_P_ALPHA,
    /*0562*/
    T_FE_P_SHEA,
    /*0563*/
    T_FE_P_MISES,
    /*    */
    /*    */        //DATA FOR FINITE ELEMENTS & PANELS
                    /*0564*/
    T_FE_NODES_1,
    /*0565*/
    T_FE_NODES_2,
    /*0566*/
    T_FE_NODES_3,
    /*0567*/
    T_FE_NODES_4,
    /*0568*/
    T_FE_NODES_5,
    /*0569*/
    T_FE_NODES_6,
    /*0570*/
    T_FE_NODES_7,
    /*0571*/
    T_FE_NODES_8,
    /*0572*/
    T_FE_MATERIAL,
    /*0573*/
    T_FE_SECT,
    /*    */
    /*0574*/
    T_PANEL_THICK,
    /*0575*/
    T_PANEL_MATERIAL,
    /*0576*/
    T_PANEL_MESH,
    /*    */
    /*    */        //RESULTS FOR REINFORCEMENT
                    /*0577*/
    T_FERA_ELE_LONG_UP,
    /*0578*/
    T_FERA_ELE_TRAN_UP,
    /*0579*/
    T_FERA_ELE_LONG_DOWN,
    /*0580*/
    T_FERA_ELE_TRAN_DOWN,
    /*    */
    /*0581*/
    T_FERA_NOD_LONG_UP,
    /*0582*/
    T_FERA_NOD_TRAN_UP,
    /*0583*/
    T_FERA_NOD_LONG_DOWN,
    /*0584*/
    T_FERA_NOD_TRAN_DOWN,
    /*    */
    /*    */        // L O A D S   F O R   F I N I T E   E L E M E N T S
                    /*0585*/
    T_EF_THERM_LABEL,
    /*0586*/
    T_EF_THERM_LIST_OBJ,
    /*0587*/
    T_EF_THERM_LIST_ELE,
    /*0588*/
    T_EF_THERM_TX1,
    /*0589*/
    T_EF_THERM_TY1,
    /*0590*/
    T_EF_THERM_TZ1,
    /*0591*/
    T_EF_THERM_TX2,
    /*0592*/
    T_EF_THERM_TY2,
    /*0593*/
    T_EF_THERM_TZ2,
    /*0594*/
    T_EF_THERM_TX3,
    /*0595*/
    T_EF_THERM_TY3,
    /*0596*/
    T_EF_THERM_TZ3,
    /*0597*/
    T_EF_THERM_N1X,
    /*0598*/
    T_EF_THERM_N1Y,
    /*0599*/
    T_EF_THERM_N1Z,
    /*0600*/
    T_EF_THERM_N2X,
    /*0601*/
    T_EF_THERM_N2Y,
    /*0602*/
    T_EF_THERM_N2Z,
    /*0603*/
    T_EF_THERM_N3X,
    /*0604*/
    T_EF_THERM_N3Y,
    /*0605*/
    T_EF_THERM_N3Z,
    /*    */
    /*0606*/
    T_3D_LIN_LABEL,
    /*0607*/
    T_3D_LIN_LIST_OBJ,
    /*0608*/
    T_3D_LIN_LIST_ELE,
    /*0609*/
    T_3D_LIN_PX1,
    /*0610*/
    T_3D_LIN_PY1,
    /*0611*/
    T_3D_LIN_PZ1,
    /*0612*/
    T_3D_LIN_MX1,
    /*0613*/
    T_3D_LIN_MY1,
    /*0614*/
    T_3D_LIN_MZ1,
    /*0615*/
    T_3D_LIN_PX2,
    /*0616*/
    T_3D_LIN_PY2,
    /*0617*/
    T_3D_LIN_PZ2,
    /*0618*/
    T_3D_LIN_MX2,
    /*0619*/
    T_3D_LIN_MY2,
    /*0620*/
    T_3D_LIN_MZ2,
    /*0621*/
    T_3D_LIN_N1X,
    /*0622*/
    T_3D_LIN_N1Y,
    /*0623*/
    T_3D_LIN_N1Z,
    /*0624*/
    T_3D_LIN_N2X,
    /*0625*/
    T_3D_LIN_N2Y,
    /*0626*/
    T_3D_LIN_N2Z,
    /*0627*/
    T_3D_LIN_LOC,
    /*    */
    /*0628*/
    T_CONTUR_LABEL,
    /*0629*/
    T_CONTUR_LIST_OBJ,
    /*0630*/
    T_CONTUR_LIST_ELE,
    /*0631*/
    T_CONTUR_PX1,
    /*0632*/
    T_CONTUR_PY1,
    /*0633*/
    T_CONTUR_PZ1,
    /*0634*/
    T_CONTUR_PX2,
    /*0635*/
    T_CONTUR_PY2,
    /*0636*/
    T_CONTUR_PZ2,
    /*0637*/
    T_CONTUR_PX3,
    /*0638*/
    T_CONTUR_PY3,
    /*0639*/
    T_CONTUR_PZ3,
    /*0640*/
    T_CONTUR_N1X,
    /*0641*/
    T_CONTUR_N1Y,
    /*0642*/
    T_CONTUR_N1Z,
    /*0643*/
    T_CONTUR_N2X,
    /*0644*/
    T_CONTUR_N2Y,
    /*0645*/
    T_CONTUR_N2Z,
    /*0646*/
    T_CONTUR_N3X,
    /*0647*/
    T_CONTUR_N3Y,
    /*0648*/
    T_CONTUR_N3Z,
    /*0649*/
    T_CONTUR_NPROJ,
    /*0650*/
    T_CONTUR_NP,
    /*    */
    /*0651*/
    T_LIN_LABEL,
    /*0652*/
    T_LIN_LIST_OBJ,
    /*0653*/
    T_LIN_LIST_ELE,
    /*0654*/
    T_LIN_MX1,
    /*0655*/
    T_LIN_MX2,
    /*0656*/
    T_LIN_PZ1,
    /*0657*/
    T_LIN_PZ2,
    /*0658*/
    T_LIN_MY1,
    /*0659*/
    T_LIN_MY2,
    /*0660*/
    T_LIN_N1X,
    /*0661*/
    T_LIN_N1Y,
    /*0662*/
    T_LIN_N1Z,
    /*0663*/
    T_LIN_N2X,
    /*0664*/
    T_LIN_N2Y,
    /*0665*/
    T_LIN_N2Z,
    /*0666*/
    T_LIN_LOC,
    /*    */
    /*0667*/
    T_ELE3P_LABEL,
    /*0668*/
    T_ELE3P_LIST_OBJ,
    /*0669*/
    T_ELE3P_LIST_ELE,
    /*0670*/
    T_ELE3P_PX1,
    /*0671*/
    T_ELE3P_PY1,
    /*0672*/
    T_ELE3P_PZ1,
    /*0673*/
    T_ELE3P_PX2,
    /*0674*/
    T_ELE3P_PY2,
    /*0675*/
    T_ELE3P_PZ2,
    /*0676*/
    T_ELE3P_PX3,
    /*0677*/
    T_ELE3P_PY3,
    /*0678*/
    T_ELE3P_PZ3,
    /*0679*/
    T_ELE3P_N1X,
    /*0680*/
    T_ELE3P_N1Y,
    /*0681*/
    T_ELE3P_N1Z,
    /*0682*/
    T_ELE3P_N2X,
    /*0683*/
    T_ELE3P_N2Y,
    /*0684*/
    T_ELE3P_N2Z,
    /*0685*/
    T_ELE3P_N3X,
    /*0686*/
    T_ELE3P_N3Y,
    /*0687*/
    T_ELE3P_N3Z,
    /*0688*/
    T_ELE3P_NPROJ,
    /*    */
    /*0689*/
    T_HYD_LABEL,
    /*0690*/
    T_HYD_LIST_OBJ,
    /*0691*/
    T_HYD_LIST_ELE,
    /*0692*/
    T_HYD_P,
    /*0693*/
    T_HYD_GAMMA,
    /*0694*/
    T_HYD_H,
    /*0695*/
    T_HYD_NDIR,
    /*    */
    /*0696*/
    T_LOAD_LIST_ELE,        //internal RLIST use
                            /*    */
                            /*0697*/
    T_FE_PANEL,
    /*0698*/
    T_FE_TYPE,
    /*    */
    /*    */        //RESULTS FOR REINFORCEMENT - LIMITS
                    /*0699*/
    T_FERA_ELE_SX_UP,
    /*0700*/
    T_FERA_ELE_SY_UP,
    /*0701*/
    T_FERA_ELE_SX_DOWN,
    /*0702*/
    T_FERA_ELE_SY_DOWN,
    /*    */
    /*0703*/
    T_FERA_NOD_SX_UP,
    /*0704*/
    T_FERA_NOD_SY_UP,
    /*0705*/
    T_FERA_NOD_SX_DOWN,
    /*0706*/
    T_FERA_NOD_SY_DOWN,
    /*    */
    /*0707*/
    T_FERA_ELE_AT,
    /*0708*/
    T_FERA_NOD_AT,
    /*    */
    /*    */        // L O A D S   F O R   F I N I T E   E L E M E N T S
                    /*0709*/
    T_UNIF_EF_LABEL,
    /*0710*/
    T_UNIF_EF_LIST,
    /*0711*/
    T_UNIF_EF_LIST_ELE,
    /*0712*/
    T_UNIF_EF_PX,
    /*0713*/
    T_UNIF_EF_PY,
    /*0714*/
    T_UNIF_EF_PZ,
    /*0715*/
    T_UNIF_EF_ALFA,
    /*0716*/
    T_UNIF_EF_BETA,
    /*0717*/
    T_UNIF_EF_GAMMA,
    /*0718*/
    T_UNIF_EF_LOC,
    /*0719*/
    T_UNIF_EF_PROJ,
    /*0720*/
    T_UNIF_EF_REL,
    /*0721*/
    T_UNIF_EF_T,
    /*0722*/
    T_UNIF_EF_LS,
    /*0723*/
    T_UNIF_EF_UD,
    /*    */
    /*0724*/
    T_NOEFOR_LIST_ELE,
    /*0725*/
    T_UNIFORM_LIST_ELE,
    /*0726*/
    T_POIPRO_LIST_ELE,
    /*0727*/
    T_NOEDIS_LIST_ELE,
    /*0728*/
    T_DILAT_LIST_ELE,
    /*0729*/
    T_FORCON_LIST_ELE,
    /*0730*/
    T_MOMENT_LIST_ELE,
    /*0731*/
    T_PRETRA_LIST_ELE,
    /*0732*/
    T_THERMAL_LIST_ELE,
    /*0733*/
    T_NOEFOR_MASS_LIST_ELE,
    /*0734*/
    T_FORCON_MASS_LIST_ELE,
    /*0735*/
    T_UNIFORM_MASS_LIST_ELE,
    /*0736*/
    T_PRETRA_MASS_LIST_ELE,
    /*    */
    /*0737*/
    T_PANEL_TYPE,
    /*    */        //PROPERTIES FOR PANELS
                    /*0738*/
    T_THICK_NAME,
    /*0739*/
    T_THICK_TYPE,
    /*0740*/
    T_THICK_MATER,
    /*0741*/
    T_THICK_KZ,
    /*0742*/
    T_THICK_GR,
    /*0743*/
    T_THICK_P1,
    /*0744*/
    T_THICK_P2,
    /*0745*/
    T_THICK_P3,
    /*0746*/
    T_THICK_G1,
    /*0747*/
    T_THICK_G2,
    /*0748*/
    T_THICK_G3,
    /*0749*/
    T_THICK_LIST,
    /*    */
    /*    */        //PROPERTIES FOR RDIM
                    /*0750*/
    T_RDIM_MNAME,        // Member name
                         /*0751*/
    T_RDIM_MLIST,        // Member components
                         /*0752*/
    T_RDIM_MGROUP,       // group member belongs to
                         /*0753*/
    T_RDIM_LY,
    /*0754*/
    T_RDIM_LZ,
    /*0755*/
    T_RDIM_GNAME,
    /*0756*/
    T_RDIM_GLIST,
    /*0757*/
    T_RDIM_GROUP,
    /*    */
    /*0758*/
    T_MEMB_TACOM,
    /*0759*/
    T_MEMB_SHEAR,
    /*    */
    /*0760*/
    T_CONTUR_DEFINITION,
    /*    */
    /*0761*/
    T_THICK_P1_X,
    /*0762*/
    T_THICK_P1_Y,
    /*0763*/
    T_THICK_P1_Z,
    /*0764*/
    T_THICK_P2_X,
    /*0765*/
    T_THICK_P2_Y,
    /*0766*/
    T_THICK_P2_Z,
    /*0767*/
    T_THICK_P3_X,
    /*0768*/
    T_THICK_P3_Y,
    /*0769*/
    T_THICK_P3_Z,
    /*    */
    /*0770*/
    T_SECT_CB_ELEMLIST,
    /*0771*/
    T_SECT_CB_NAME,
    /*0772*/
    T_SECT_CB_STRE,
    /*    */        //OFFSETS
                    /*0773*/
    T_MEMB_OFF_NAME,
    /*0774*/
    T_MEMB_OFF_BEG_UX,
    /*0775*/
    T_MEMB_OFF_BEG_UY,
    /*0776*/
    T_MEMB_OFF_BEG_UZ,
    /*0777*/
    T_MEMB_OFF_END_UX,
    /*0778*/
    T_MEMB_OFF_END_UY,
    /*0779*/
    T_MEMB_OFF_END_UZ,
    /*0780*/
    T_MEMB_OFF_COORD,
    /*    */        //JARRET
                    /*0781*/
    T_JARR_NAME,
    /*0782*/
    T_JARR_ELEMLIST_BEG,
    /*0783*/
    T_JARR_ELEMLIST_END,
    /*0784*/
    T_JARR_LENGTH,
    /*0785*/
    T_JARR_HIGHT,
    /*0786*/
    T_JARR_WIDTH,
    /*0787*/
    T_JARR_THICK1,
    /*0788*/
    T_JARR_THICK2,
    /*0789*/
    T_JARR_PLACEMENT,
    /*0790*/
    T_JARR_TYPE,
    /*0791*/
    T_MEMB_JARR_NAME_BEG,
    /*0792*/
    T_MEMB_JARR_NAME_END,
    /*    */        //GLOBAL ANALIZE
                    /*0793*/
    T_CONT_SENV,    //enveloppe of T_CONT_SMIN & T_CONT_SMAX
                    /*0794*/
    T_CONT_SENVMY,  //enveloppe of T_CONT_SMINMY & T_CONT_SMAXMY
                    /*0795*/
    T_CONT_SENVMZ,  //enveloppe of T_CONT_SMINMZ & T_CONT_SMAXMZ
                    /*    */
                    /*0796*/
    T_MEMB_RATIO,  //dimensioment ratio
                   /*    */        //MAXIMAL DEFLECTION - LOCAL COORDINATION
                                   /*0797*/
    T_DISP_DEFL_MAX_UX_COO,
    /*0798*/
    T_DISP_DEFL_MAX_UY_COO,
    /*0799*/
    T_DISP_DEFL_MAX_UZ_COO,
    /*    */
    /*0800*/
    T_MEMB_RELACH_CODE,
    /*    */
    /*0801*/
    T_REAC_FX_DDC,
    /*0802*/
    T_REAC_FY_DDC,
    /*0803*/
    T_REAC_FZ_DDC,
    /*0804*/
    T_REAC_MX_DDC,
    /*0805*/
    T_REAC_MY_DDC,
    /*0806*/
    T_REAC_MZ_DDC,
    /*0807*/
    T_REAC_SREA_FX_DDC,
    /*0808*/
    T_REAC_SREA_FY_DDC,
    /*0809*/
    T_REAC_SREA_FZ_DDC,
    /*0810*/
    T_REAC_SREA_MX_DDC,
    /*0811*/
    T_REAC_SREA_MY_DDC,
    /*0812*/
    T_REAC_SREA_MZ_DDC,
    /*    */        //SUPPORTS LABELS
                    /*0813*/
    T_SUPP_LABEL_NAME,
    /*0814*/
    T_SUPP_LABEL_NODELIST,
    /*0815*/
    T_SUPP_LABEL_UX,
    /*0816*/
    T_SUPP_LABEL_UY,
    /*0817*/
    T_SUPP_LABEL_UZ,
    /*0818*/
    T_SUPP_LABEL_RX,
    /*0819*/
    T_SUPP_LABEL_RY,
    /*0820*/
    T_SUPP_LABEL_RZ,
    /*0821*/
    T_SUPP_LABEL_ALFA,
    /*0822*/
    T_SUPP_LABEL_BETA,
    /*0823*/
    T_SUPP_LABEL_GAMMA,
    /*0824*/
    T_SUPP_LABEL_KX,
    /*0825*/
    T_SUPP_LABEL_KY,
    /*0826*/
    T_SUPP_LABEL_KZ,
    /*0827*/
    T_SUPP_LABEL_HX,
    /*0828*/
    T_SUPP_LABEL_HY,
    /*0829*/
    T_SUPP_LABEL_HZ,
    /*0830*/
    T_SUPP_LABEL_DESCR,
    /*    */
    /*0831*/
    T_MEMB_LAY,
    /*0832*/
    T_MEMB_LAZ,
    /*    */
    /*0833*/
    T_MAT_LABEL_ELEMLIST,
    /*0834*/
    T_MAT_LABEL_NAME,
    /*0835*/
    T_MAT_LABEL_YOUNG,
    /*0836*/
    T_MAT_LABEL_G,
    /*0837*/
    T_MAT_LABEL_POISSON,
    /*0838*/
    T_MAT_LABEL_TERMI,
    /*0839*/
    T_MAT_LABEL_DENS,
    /*0840*/
    T_MAT_LABEL_ELAST,
    /*    */
    /*0841*/
    T_FORC_FX_COMP,
    /*0842*/
    T_FORC_FX_TORS,
    /*    */        //EDITION OF COMBINATIONS
                    /*0843*/
    T_COMB_ED_NAME,
    /*0844*/
    T_COMB_ED_TYPE,
    /*0845*/
    T_COMB_ED_COMBNAT,
    /*0846*/
    T_COMB_ED_CASENAT,
    /*0847*/
    T_COMB_ED_SQRT,
    /*0848*/
    T_COMB_ED_CASE,
    /*0849*/
    T_COMB_ED_COEF,

    /*0850*/
    T_3D_LIN_GAMMA,

    /*0851*/
    T_MET_WOODVOL,          //volume for wooden sections

    /*0852*/
    T_LOAD_MEMO,

    /*0853*/
    T_REAC_SVAL_FX_DDC,
    /*0854*/
    T_REAC_SVAL_FY_DDC,
    /*0855*/
    T_REAC_SVAL_FZ_DDC,
    /*0856*/
    T_REAC_SVAL_MX_DDC,
    /*0857*/
    T_REAC_SVAL_MY_DDC,
    /*0858*/
    T_REAC_SVAL_MZ_DDC,
    /*    */        //LIASONS RIGIDE LABELS
                    /*0859*/
    T_LIARIG_LABEL_NAME,
    /*0860*/
    T_LIARIG_LABEL_NODEMAIN,
    /*0861*/
    T_LIARIG_LABEL_NODELIST,
    /*0862*/
    T_LIARIG_LABEL_UX,
    /*0863*/
    T_LIARIG_LABEL_UY,
    /*0864*/
    T_LIARIG_LABEL_UZ,
    /*0865*/
    T_LIARIG_LABEL_RX,
    /*0866*/
    T_LIARIG_LABEL_RY,
    /*0867*/
    T_LIARIG_LABEL_RZ,
    /*0868*/
    T_LIARIG_LABEL_DESCR,
    /*    */        //REACTIONS IN LOCAL COORDINATE SYSTEM
                    /*0869*/
    T_REAC_FX_LOC,
    /*0870*/
    T_REAC_FY_LOC,
    /*0871*/
    T_REAC_FZ_LOC,
    /*0872*/
    T_REAC_MX_LOC,
    /*0873*/
    T_REAC_MY_LOC,
    /*0874*/
    T_REAC_MZ_LOC,
    /*0875*/
    T_REAC_FX_DDC_LOC,
    /*0876*/
    T_REAC_FY_DDC_LOC,
    /*0877*/
    T_REAC_FZ_DDC_LOC,
    /*0878*/
    T_REAC_MX_DDC_LOC,
    /*0879*/
    T_REAC_MY_DDC_LOC,
    /*0880*/
    T_REAC_MZ_DDC_LOC,
    /*    */        //SUPPORTS - DAMPING
                    /*0881*/
    T_SUPP_LABEL_UX_DAMP,
    /*0882*/
    T_SUPP_LABEL_UY_DAMP,
    /*0883*/
    T_SUPP_LABEL_UZ_DAMP,
    /*0884*/
    T_SUPP_LABEL_RX_DAMP,
    /*0885*/
    T_SUPP_LABEL_RY_DAMP,
    /*0886*/
    T_SUPP_LABEL_RZ_DAMP,
    /*0887*/
    T_SUPP_LABEL_DAMP,
    /*    */        //NODES - EMITTERS
                    /*0888*/
    T_NODE_EMITTER,
    /*    */        //ELEMENTS FINITE - REDUCED MOMENTS
                    /*0889*/
    T_FE_RM_XX_UP_WAA,
    /*0890*/
    T_FE_RM_XX_DO_WAA,
    /*0891*/
    T_FE_RM_YY_UP_WAA,
    /*0892*/
    T_FE_RM_YY_DO_WAA,
    /*0893*/
    T_FE_RM_XX_UP_NEN,
    /*0894*/
    T_FE_RM_XX_DO_NEN,
    /*0895*/
    T_FE_RM_YY_UP_NEN,
    /*0896*/
    T_FE_RM_YY_DO_NEN,
    /*    */        //AUXILLIARY OFFSETS
                    /*0897*/
    T_MEMB_OFF_LABEL,
    /*0898*/
    T_MEMB_OFF_LIST,
    /*    */        //load concentrated in point
                    /*0899*/
    T_PNTAUX_LABEL,
    /*0900*/
    T_PNTAUX_LIST,
    /*0901*/
    T_PNTAUX_FX,
    /*0902*/
    T_PNTAUX_FY,
    /*0903*/
    T_PNTAUX_FZ,
    /*0904*/
    T_PNTAUX_CX,
    /*0905*/
    T_PNTAUX_CY,
    /*0906*/
    T_PNTAUX_CZ,
    /*0907*/
    T_PNTAUX_ALFA,
    /*0908*/
    T_PNTAUX_BETA,
    /*0909*/
    T_PNTAUX_GAMMA,
    /*0910*/
    T_PNTAUX_X,
    /*0911*/
    T_PNTAUX_Y,
    /*0912*/
    T_PNTAUX_Z,
    /*    */        //double rectangle section
                    /*0913*/
    T_SECT_TP_DRECT_B,
    /*0914*/
    T_SECT_TP_DRECT_H,
    /*0915*/
    T_SECT_TP_DRECT_D,
    /*    */        //cable results
                    /*0916*/
    T_CONT_CB_FORCE,
    /*0917*/
    T_CONT_CB_REGUL,
    /*    */
    /*0918*/
    T_MOVE_DEFINITION,    //internal RLIST
                          /*    */        //BEST RESULTS
                                          /*0919*/
    T_BEST_AS1,
    /*0920*/
    T_BEST_AS2,
    /*0921*/
    T_BEST_ROMIN,
    /*0922*/
    T_BEST_RO,
    /*0923*/
    T_BEST_ROMAX,
    /*0924*/
    T_BEST_CASEDIM,
    /*0925*/
    T_BEST_N,
    /*0926*/
    T_BEST_MY,
    /*0927*/
    T_BEST_MZ,
    /*0928*/
    T_BEST_COMBDEF,
    /*    */        //REINFORCEMENT - CALCULATION METHOD
                    /*0929*/
    T_FERA_SGN_LIST,
    /*0930*/
    T_FERA_SGU_LIST,
    /*0931*/
    T_FERA_AKC_LIST,
    /*0932*/
    T_FERA_CALC_MET,
    /*0933*/
    T_FERA_AVER_MET,
    /*    */        //MOVE LOADS RESULTS - POINT COORDINATES
                    /*0934*/
    T_MOVE_COORD_X,
    /*0935*/
    T_MOVE_COORD_Y,
    /*0936*/
    T_MOVE_COORD_Z,
    /*    */        // MOVING LOADS
                    /*0937*/
    T_MOVFORC_LABEL,
    /*0938*/
    T_MOVFORC_LIST,
    /*0939*/
    T_MOVFORC_X,
    /*0940*/
    T_MOVFORC_Y,
    /*0941*/
    T_MOVFORC_Z,
    /*0942*/
    T_MOVFORC_FX,
    /*0943*/
    T_MOVFORC_FY,
    /*0944*/
    T_MOVFORC_FZ,
    /*    */
    /*0945*/
    T_MOVFORC_P_LABEL,
    /*0946*/
    T_MOVFORC_P_LIST,
    /*0947*/
    T_MOVFORC_P_X,
    /*0948*/
    T_MOVFORC_P_Y,
    /*0949*/
    T_MOVFORC_P_Z,
    /*0950*/
    T_MOVFORC_P_PX,
    /*0951*/
    T_MOVFORC_P_PY,
    /*0952*/
    T_MOVFORC_P_PZ,
    /*0953*/
    T_MOVFORC_P_DX,
    /*0954*/
    T_MOVFORC_P_DY,
    /*0955*/
    T_MOVFORC_P_VX,
    /*0956*/
    T_MOVFORC_P_VY,
    /*0957*/
    T_MOVFORC_P_VZ,
    /*0958*/
    T_MOVFORC_P_WX,
    /*0959*/
    T_MOVFORC_P_WY,
    /*0960*/
    T_MOVFORC_P_WZ,
    /*    */        //ESTIMATION TABLE
                    /*0961*/
    T_ESTIM_S_GROUP,
    /*0962*/
    T_ESTIM_S_PROTECTION,
    /*0963*/
    T_ESTIM_S_WEIGHT,
    /*0964*/
    T_ESTIM_S_PRICE,
    /*0965*/
    T_ESTIM_S_TPRICE,
    /*0966*/
    T_ESTIM_S_PPRICE,
    /*0967*/
    T_ESTIM_S_TPPRICE,
    /*0968*/
    T_ESTIM_S_TOTALPRICE,
    /*0969*/
    T_ESTIM_S_WEIGHT_T,
    /*0970*/
    T_ESTIM_S_PRICE_T,
    /*0971*/
    T_ESTIM_S_TPRICE_T,
    /*0972*/
    T_ESTIM_S_PPRICE_T,
    /*0973*/
    T_ESTIM_S_TPPRICE_T,
    /*0974*/
    T_ESTIM_S_TOTALPRICE_T,
    /*    */
    /*0975*/
    T_ESTIM_G_WEIGHT,
    /*0976*/
    T_ESTIM_G_PRICE,
    /*0977*/
    T_ESTIM_G_TPRICE,
    /*0978*/
    T_ESTIM_G_TOTALPRICE,
    /*0979*/
    T_ESTIM_G_WEIGHT_T,
    /*0980*/
    T_ESTIM_G_PRICE_T,
    /*0981*/
    T_ESTIM_G_TPRICE_T,
    /*0982*/
    T_ESTIM_G_TOTALPRICE_T,
    /*    */
    /*0983*/
    T_MOVE_COORD_DIST,
    /*    */        //ADDITIONAL VALUES FOR DYNAMIC
                    /*0984*/
    T_DYNM_DAMPING,
    /*0985*/
    T_DYNM_ENERGY,
    /*0986*/
    T_DYNM_AVERCOEF,
    /*    */        //ADDED MASSES FOR FINITE ELEMENTS
                    /*0987*/
    T_3D_LIN_MASS_LABEL,
    /*0988*/
    T_3D_LIN_MASS_LIST_OBJ,
    /*0989*/
    T_3D_LIN_MASS_LIST_ELE,
    /*0990*/
    T_3D_LIN_MASS_PX1,
    /*0991*/
    T_3D_LIN_MASS_PY1,
    /*0992*/
    T_3D_LIN_MASS_PZ1,
    /*0993*/
    T_3D_LIN_MASS_MX1,
    /*0994*/
    T_3D_LIN_MASS_MY1,
    /*0995*/
    T_3D_LIN_MASS_MZ1,
    /*0996*/
    T_3D_LIN_MASS_PX2,
    /*0997*/
    T_3D_LIN_MASS_PY2,
    /*0998*/
    T_3D_LIN_MASS_PZ2,
    /*0999*/
    T_3D_LIN_MASS_MX2,
    /*1000*/
    T_3D_LIN_MASS_MY2,
    /*1001*/
    T_3D_LIN_MASS_MZ2,
    /*1002*/
    T_3D_LIN_MASS_N1X,
    /*1003*/
    T_3D_LIN_MASS_N1Y,
    /*1004*/
    T_3D_LIN_MASS_N1Z,
    /*1005*/
    T_3D_LIN_MASS_N2X,
    /*1006*/
    T_3D_LIN_MASS_N2Y,
    /*1007*/
    T_3D_LIN_MASS_N2Z,
    /*1008*/
    T_3D_LIN_MASS_LOC,
    /*1009*/
    T_3D_LIN_MASS_GAMMA,
    /*    */
    /*1010*/
    T_LIN_MASS_LABEL,
    /*1011*/
    T_LIN_MASS_LIST_OBJ,
    /*1012*/
    T_LIN_MASS_LIST_ELE,
    /*1013*/
    T_LIN_MASS_MX1,
    /*1014*/
    T_LIN_MASS_MX2,
    /*1015*/
    T_LIN_MASS_PZ1,
    /*1016*/
    T_LIN_MASS_PZ2,
    /*1017*/
    T_LIN_MASS_MY1,
    /*1018*/
    T_LIN_MASS_MY2,
    /*1019*/
    T_LIN_MASS_N1X,
    /*1020*/
    T_LIN_MASS_N1Y,
    /*1021*/
    T_LIN_MASS_N1Z,
    /*1022*/
    T_LIN_MASS_N2X,
    /*1023*/
    T_LIN_MASS_N2Y,
    /*1024*/
    T_LIN_MASS_N2Z,
    /*1025*/
    T_LIN_MASS_LOC,
    /*    */
    /*1026*/
    T_UNIF_EF_MASS_LABEL,
    /*1027*/
    T_UNIF_EF_MASS_LIST,
    /*1028*/
    T_UNIF_EF_MASS_LIST_ELE,
    /*1029*/
    T_UNIF_EF_MASS_PX,
    /*1030*/
    T_UNIF_EF_MASS_PY,
    /*1031*/
    T_UNIF_EF_MASS_PZ,
    /*1032*/
    T_UNIF_EF_MASS_ALFA,
    /*1033*/
    T_UNIF_EF_MASS_BETA,
    /*1034*/
    T_UNIF_EF_MASS_GAMMA,
    /*1035*/
    T_UNIF_EF_MASS_LOC,
    /*1036*/
    T_UNIF_EF_MASS_PROJ,
    /*1037*/
    T_UNIF_EF_MASS_REL,
    /*1038*/
    T_UNIF_EF_MASS_T,
    /*1039*/
    T_UNIF_EF_MASS_LS,
    /*1040*/
    T_UNIF_EF_MASS_UD,
    /*    */
    /*1041*/
    T_PNTAUX_MASS_LABEL,
    /*1042*/
    T_PNTAUX_MASS_LIST,
    /*1043*/
    T_PNTAUX_MASS_FX,
    /*1044*/
    T_PNTAUX_MASS_FY,
    /*1045*/
    T_PNTAUX_MASS_FZ,
    /*1046*/
    T_PNTAUX_MASS_CX,
    /*1047*/
    T_PNTAUX_MASS_CY,
    /*1048*/
    T_PNTAUX_MASS_CZ,
    /*1049*/
    T_PNTAUX_MASS_ALFA,
    /*1050*/
    T_PNTAUX_MASS_BETA,
    /*1051*/
    T_PNTAUX_MASS_GAMMA,
    /*1052*/
    T_PNTAUX_MASS_X,
    /*1053*/
    T_PNTAUX_MASS_Y,
    /*1054*/
    T_PNTAUX_MASS_Z,
    /*    */
    /*1055*/
    T_CONTUR_MASS_LABEL,
    /*1056*/
    T_CONTUR_MASS_LIST_OBJ,
    /*1057*/
    T_CONTUR_MASS_LIST_ELE,
    /*1058*/
    T_CONTUR_MASS_PX1,
    /*1059*/
    T_CONTUR_MASS_PY1,
    /*1060*/
    T_CONTUR_MASS_PZ1,
    /*1061*/
    T_CONTUR_MASS_PX2,
    /*1062*/
    T_CONTUR_MASS_PY2,

    /*1063*/
    T_CONTUR_MASS_PZ2,
    /*1064*/
    T_CONTUR_MASS_PX3,
    /*1065*/
    T_CONTUR_MASS_PY3,
    /*1066*/
    T_CONTUR_MASS_PZ3,
    /*1067*/
    T_CONTUR_MASS_N1X,
    /*1068*/
    T_CONTUR_MASS_N1Y,
    /*1069*/
    T_CONTUR_MASS_N1Z,
    /*1070*/
    T_CONTUR_MASS_N2X,
    /*1071*/
    T_CONTUR_MASS_N2Y,
    /*1072*/
    T_CONTUR_MASS_N2Z,
    /*1073*/
    T_CONTUR_MASS_N3X,
    /*1074*/
    T_CONTUR_MASS_N3Y,
    /*1075*/
    T_CONTUR_MASS_N3Z,
    /*1076*/
    T_CONTUR_MASS_NPROJ,
    /*1077*/
    T_CONTUR_MASS_NP,
    /*1078*/
    T_CONTUR_MASS_DEFINITION,
    /*    */
    /*1079*/
    T_ELE3P_MASS_LABEL,
    /*1080*/
    T_ELE3P_MASS_LIST_OBJ,
    /*1081*/
    T_ELE3P_MASS_LIST_ELE,
    /*1082*/
    T_ELE3P_MASS_PX1,
    /*1083*/
    T_ELE3P_MASS_PY1,
    /*1084*/
    T_ELE3P_MASS_PZ1,
    /*1085*/
    T_ELE3P_MASS_PX2,
    /*1086*/
    T_ELE3P_MASS_PY2,
    /*1087*/
    T_ELE3P_MASS_PZ2,
    /*1088*/
    T_ELE3P_MASS_PX3,
    /*1089*/
    T_ELE3P_MASS_PY3,
    /*1090*/
    T_ELE3P_MASS_PZ3,
    /*1091*/
    T_ELE3P_MASS_N1X,
    /*1092*/
    T_ELE3P_MASS_N1Y,
    /*1093*/
    T_ELE3P_MASS_N1Z,
    /*1094*/
    T_ELE3P_MASS_N2X,
    /*1095*/
    T_ELE3P_MASS_N2Y,
    /*1096*/
    T_ELE3P_MASS_N2Z,
    /*1097*/
    T_ELE3P_MASS_N3X,
    /*1098*/
    T_ELE3P_MASS_N3Y,
    /*1099*/
    T_ELE3P_MASS_N3Z,
    /*1100*/
    T_ELE3P_MASS_NPROJ,
    /*    */
    /*1101*/
    T_HYD_MASS_LABEL,
    /*1102*/
    T_HYD_MASS_LIST_OBJ,
    /*1103*/
    T_HYD_MASS_LIST_ELE,
    /*1104*/
    T_HYD_MASS_P,
    /*1105*/
    T_HYD_MASS_GAMMA,
    /*1106*/
    T_HYD_MASS_H,
    /*1107*/
    T_HYD_MASS_NDIR,
    /*    */        //CROSS SECTIONS
                    /*1108*/
    T_SECT_CROSS_P1_L,
    /*1109*/
    T_SECT_CROSS_P1_T,
    /*1110*/
    T_SECT_CROSS_P2_L,
    /*1111*/
    T_SECT_CROSS_P2_T,
    /*1112*/
    T_SECT_CROSS_P3_L,
    /*1113*/
    T_SECT_CROSS_P3_T,
    /*1114*/
    T_SECT_CROSS_P4_L,
    /*1115*/
    T_SECT_CROSS_P4_T,
    /*    */        //REINFORCEMENT - POLISH CODE
                    /*1116*/
    T_FERA_PL_ELE_AX,
    /*1117*/
    T_FERA_PL_ELE_AY,
    /*1118*/
    T_FERA_PL_ELE_F,
    /*1119*/
    T_FERA_PL_ELE_AMIN,
    /*1120*/
    T_FERA_PL_NOD_AX,
    /*1121*/
    T_FERA_PL_NOD_AY,
    /*1122*/
    T_FERA_PL_NOD_F,
    /*1123*/
    T_FERA_PL_NOD_AMIN,
    /*    */        //RESULTS FOR REINFORCEMENT - REAL RESULTS
                    /*1124*/
    T_FERA_ELE_LONG_UP_R,
    /*1125*/
    T_FERA_ELE_TRAN_UP_R,
    /*1126*/
    T_FERA_ELE_LONG_DOWN_R,
    /*1127*/
    T_FERA_ELE_TRAN_DOWN_R,
    /*    */
    /*1128*/
    T_FERA_NOD_LONG_UP_R,
    /*1129*/
    T_FERA_NOD_TRAN_UP_R,
    /*1130*/
    T_FERA_NOD_LONG_DOWN_R,
    /*1131*/
    T_FERA_NOD_TRAN_DOWN_R,
    /*    */
    /*1132*/
    T_FERA_ELE_SX_UP_R,
    /*1133*/
    T_FERA_ELE_SY_UP_R,
    /*1134*/
    T_FERA_ELE_SX_DOWN_R,
    /*1135*/
    T_FERA_ELE_SY_DOWN_R,
    /*    */
    /*1136*/
    T_FERA_NOD_SX_UP_R,
    /*1137*/
    T_FERA_NOD_SY_UP_R,
    /*1138*/
    T_FERA_NOD_SX_DOWN_R,
    /*1139*/
    T_FERA_NOD_SY_DOWN_R,
    /*    */
    /*1140*/
    T_FERA_PL_ELE_AMIN_R,
    /*1141*/
    T_FERA_PL_NOD_AMIN_R,
    /*    */        //more results for BEST view
                    /*1142*/
    T_BEST_RO_R,
    /*1143*/
    T_BEST_AS_MAX,
    /*1144*/
    T_BEST_AS_MIN,
    /*1145*/
    T_BEST_AS1_R,
    /*1146*/
    T_BEST_AS2_R,
    /*1147*/
    T_BEST_REINF_B,
    /*1148*/
    T_BEST_REINF_H,
    /*1149*/
    T_BEST_B_NOTES,
    /*1150*/
    T_BEST_STIR_SPACE,
    /*1151*/
    T_BEST_DIM_CASE,
    /*1152*/
    T_BEST_DIM_QY,
    /*1153*/
    T_BEST_DIM_QZ,
    /*1154*/
    T_BEST_DIM_MX,
    /*1155*/
    T_BEST_DIM_COMBDEF,
    /*1156*/
    T_BEST_PARAM,
    /*1157*/
    T_BEST_SGN,
    /*1158*/
    T_BEST_SGU,
    /*1159*/
    T_BEST_AKC,
    /*1160*/
    T_BEST_CALC_POINTS,
    /*1161*/
    T_BEST_COEFF_Y,
    /*1162*/
    T_BEST_COEFF_Z,
    /*1163*/
    T_BEST_LENGTH_Y,
    /*1164*/
    T_BEST_LENGTH_Z,
    /*1165*/
    T_BEST_BEND_MAX,
    /*1166*/
    T_BEST_SCRATCH,
    /*1167*/
    T_BEST_DISP_UY,
    /*1168*/
    T_BEST_DISP_UZ,
    /*1169*/
    T_BEST_NORM_ROMAX,
    /*1170*/
    T_BEST_NORM_ROMIN,
    /*1171*/
    T_BEST_ROAVER,
    /*1172*/
    T_BEST_P_NOTES,
    /*1173*/
    T_BEST_AS1_BEAM_T,     //only for RLIST
                           /*1174*/
    T_BEST_AS2_BEAM_T,     //only for RLIST
                           /*1175*/
    T_BEST_AS1_BEAM_R,     //only for RLIST
                           /*1176*/
    T_BEST_AS2_BEAM_R,     //only for RLIST
                           /*1177*/
    T_BEST_AS1_COLUMN_T,   //only for RLIST
                           /*1178*/
    T_BEST_AS2_COLUMN_T,   //only for RLIST
                           /*1179*/
    T_BEST_AS1_COLUMN_R,   //only for RLIST
                           /*1180*/
    T_BEST_AS2_COLUMN_R,   //only for RLIST
                           /*1181*/
    T_BEST_REINF_BEAM_B,   //only for RLIST
                           /*1182*/
    T_BEST_REINF_BEAM_H,   //only for RLIST
                           /*1183*/
    T_BEST_REINF_COLUMN_B, //only for RLIST
                           /*1184*/
    T_BEST_REINF_COLUMN_H, //only for RLIST
                           /*1185*/
    T_BEST_REINF_TYPE_LAYM,//only for RLIST
                           /*1186*/
    T_BEST_TRANS_TYPE,
    /*1187*/
    T_BEST_TRANS_LAYM,
    /*    */
    /*1188*/
    T_TEMP_DEFINITION,      //internal RLIST

    /*1189*/
    T_TEMP_LABEL,           //internal RLIST
                            /*1190*/
    T_TEMP_CASE,            //internal RLIST
                            /*1191*/
    T_TEMP_VAL_NAME_UNIT,   //internal RLIST
                            /*1192*/
    T_TEMP_VAL_MIN,         //internal RLIST
                            /*1193*/
    T_TEMP_VAL_MAX,         //internal RLIST
                            /*1194*/
    T_TEMP_OBJ,             //internal RLIST
                            /*1195*/
    T_TEMP_COLOR,           //internal RLIST
                            /*1196*/
    T_TEMP_POSITION,        //internal RLIST
                            /*1197*/
    T_TEMP_LAYER,           //internal RLIST
                            /*1198*/
    T_TEMP_DIRECTION,       //internal RLIST
                            /*1199*/
    T_TEMP_POS_MIN,         //internal RLIST
                            /*1200*/
    T_TEMP_POS_MAX,         //internal RLIST

    /*1201*/
    T_BEST_STIR_SPACE_R,
    /*1202*/
    T_BEST_AS1_Z,
    /*1203*/
    T_BEST_AS2_Z,
    /*1204*/
    T_BEST_AS1_R_Z,
    /*1205*/
    T_BEST_AS2_R_Z,
    /*1206*/
    T_BEST_REINF_B_Z,
    /*1207*/
    T_BEST_REINF_H_Z,

    /*1208*/
    T_MEMB_ELA_LABEL,

    /*1209*/
    T_UNIFORM_DY,
    /*1210*/
    T_UNIFORM_DZ,
    /*1211*/
    T_UNIFORM_MASS_DY,
    /*1212*/
    T_UNIFORM_MASS_DZ,
    /*1213*/
    T_FORCON_DY,
    /*1214*/
    T_FORCON_DZ,
    /*1215*/
    T_FORCON_MASS_DY,
    /*1216*/
    T_FORCON_MASS_DZ,
    /*    */        //REDUCED RESULTS FOR PANELS
                    /*1217*/
    T_PANEL_RR_NRX,
    /*1218*/
    T_PANEL_RR_MRZ,
    /*1219*/
    T_PANEL_RR_TRY,
    /*1220*/
    T_PANEL_RR_SRO,
    /*1221*/
    T_PANEL_RR_SRE,
    /*1222*/
    T_PANEL_RR_TR,
    /*1223*/
    T_PANEL_RR_TRZ,
    /*1224*/
    T_PANEL_RR_MRY,
    /*1225*/
    T_PANEL_RR_LENGTH,
    /*1226*/
    T_PANEL_RR_HEIGHT,
    /*    */
    /*1227*/
    T_THICK_ORTOTROP,
    /*    */
    /*1228*/
    T_LOAD_GEOMLIMIT,
    /*    */
    /*1229*/
    T_PANEL_RELEASE,
    /*    */
    /*1230*/
    T_VOLUME_NAME,
    /*1231*/
    T_VOLUME_LIST,
    /*1232*/
    T_VOLUME_MATER,
    /*1233*/
    T_VOLUME_E,
    /*1234*/
    T_VOLUME_NI,
    /*1235*/
    T_VOLUME_CW,
    /*1236*/
    T_VOLUME_LX,
    /*1237*/
    T_VOLUME_DAMP,
    /*1238*/
    T_VOLUME_LABEL,
    /*    */
    /*1239*/
    T_FE_NODES_9,
    /*1240*/
    T_FE_NODES_10,
    /*1241*/
    T_FE_NODES_11,
    /*1242*/
    T_FE_NODES_12,
    /*1243*/
    T_FE_NODES_13,
    /*1244*/
    T_FE_NODES_14,
    /*1245*/
    T_FE_NODES_15,
    /*1246*/
    T_FE_NODES_16,
    /*1247*/
    T_FE_NODES_17,
    /*1248*/
    T_FE_NODES_18,
    /*1249*/
    T_FE_NODES_19,
    /*1250*/
    T_FE_NODES_20,
    /*1251*/
    T_FE_VOLUME,
    /*1252*/
    T_FE_SOLID,
    /*    */        //RESULTS FOR FINITE ELEMENTS (SOLIDS)
                    /*1253*/
    T_VOLDET_STREX,
    /*1254*/
    T_VOLDET_STREY,
    /*1255*/
    T_VOLDET_STREZ,
    /*1256*/
    T_VOLDET_STREXY,
    /*1257*/
    T_VOLDET_STREXZ,
    /*1258*/
    T_VOLDET_STREYZ,

    /*1259*/
    T_VOLDET_STRAX,
    /*1260*/
    T_VOLDET_STRAY,
    /*1261*/
    T_VOLDET_STRAZ,
    /*1262*/
    T_VOLDET_STRAXY,
    /*1263*/
    T_VOLDET_STRAXZ,
    /*1264*/
    T_VOLDET_STRAYZ,

    /*1265*/
    T_VOLDET_DEPXX,
    /*1266*/
    T_VOLDET_DEPYY,
    /*1267*/
    T_VOLDET_DEPZZ,
    /*1268*/
    T_VOLDET_DEPX,
    /*1269*/
    T_VOLDET_DEPY,
    /*1270*/
    T_VOLDET_DEPZ,
    /*1271*/
    T_VOLDET_DEP,

    /*1272*/
    T_VOLMAX_STRE1,
    /*1273*/
    T_VOLMAX_STRE2,
    /*1274*/
    T_VOLMAX_STRE3,
    /*1275*/
    T_VOLMAX_STRE12,
    /*1276*/
    T_VOLMAX_STRE13,
    /*1277*/
    T_VOLMAX_STRE23,

    /*1278*/
    T_VOLMAX_STRA1,
    /*1279*/
    T_VOLMAX_STRA2,
    /*1280*/
    T_VOLMAX_STRA3,
    /*1281*/
    T_VOLMAX_STRA12,
    /*1282*/
    T_VOLMAX_STRA13,
    /*1283*/
    T_VOLMAX_STRA23,

    /*1284*/
    T_VOLMAX_STRE_HM,
    /*1285*/
    T_VOLMAX_STRE_I1,
    /*1286*/
    T_VOLMAX_STRA_HM,
    /*1287*/
    T_VOLMAX_STRA_I1,
    /*    */
    /*1288*/
    T_PANEL_SURF,
    /*1289*/
    T_VOLUME_VOL,
    /*    */
    /*1290*/
    T_LINEDGE_LIST_OBJ,
    /*1291*/
    T_LINEDGE_LIST_ELE,
    /*1292*/
    T_LINEDGE_PX,
    /*1293*/
    T_LINEDGE_PY,
    /*1294*/
    T_LINEDGE_PZ,
    /*1295*/
    T_LINEDGE_MX,
    /*1296*/
    T_LINEDGE_MY,
    /*1297*/
    T_LINEDGE_MZ,
    /*1298*/
    T_LINEDGE_GAMMA,
    /*1298*/
    T_LINEDGE_LOC,
    /*    */
    /*1300*/
    T_VOLUME_GRID,
    /*1301*/
    T_VOLUME_SIDES,
    /*    */        // GLOBAL DISPLACEMENTS FOR FINITE ELEMENTS
                    /*1302*/
    T_FE_DEPX,
    /*1303*/
    T_FE_DEPY,
    /*1304*/
    T_FE_DEPZ,
    /*1305*/
    T_FE_DEP,
    /*    */
    /*1306*/
    T_VOL_NODES_1,
    /*1307*/
    T_VOL_NODES_2,
    /*1308*/
    T_VOL_NODES_3,
    /*1309*/
    T_VOL_NODES_4,
    /*1310*/
    T_VOL_NODES_5,
    /*1311*/
    T_VOL_NODES_6,
    /*1312*/
    T_VOL_NODES_7,
    /*1313*/
    T_VOL_NODES_8,
    /*1314*/
    T_VOL_NODES_9,
    /*1315*/
    T_VOL_NODES_10,
    /*1316*/
    T_VOL_NODES_11,
    /*1317*/
    T_VOL_NODES_12,
    /*1318*/
    T_VOL_NODES_13,
    /*1319*/
    T_VOL_NODES_14,
    /*1320*/
    T_VOL_NODES_15,
    /*1321*/
    T_VOL_NODES_16,
    /*1322*/
    T_VOL_NODES_17,
    /*1323*/
    T_VOL_NODES_18,
    /*1324*/
    T_VOL_NODES_19,
    /*1325*/
    T_VOL_NODES_20,
    /*    */
    /*1326*/
    T_DYNM_RIG,
    /*1327*/
    T_DYNM_MASS,
    /*    */
    /*1328*/
    T_PSACC_GX,
    /*1329*/
    T_PSACC_GY,
    /*1330*/
    T_PSACC_GZ,
    /*1331*/
    T_PSACC_TX,
    /*1332*/
    T_PSACC_TY,
    /*1333*/
    T_PSACC_TZ,

    /*1334*/
    T_EF_THERM_LOC,
    /*1335*/
    T_CONTUR_LOC,
    /*1336*/
    T_ELE3P_LOC,
    /*1337*/
    T_EF_THERM_NP,

    /*1338*/
    T_THICK_ORTOTROP_DSC,

    /*1339*/
    T_POIPRO_MASS_LABEL,
    /*1340*/
    T_POIPRO_MASS_LIST,
    /*1341*/
    T_POIPRO_MASS_LIST_ELE,
    /*1342*/
    T_POIPRO_MASS_X,
    /*1343*/
    T_POIPRO_MASS_Y,
    /*1344*/
    T_POIPRO_MASS_Z,
    /*1345*/
    T_POIPRO_MASS_XYZ,

    /*1346*/
    T_OBJSURF_LABEL,
    /*1347*/
    T_OBJSURF_LIST_OBJ,
    /*1348*/
    T_OBJSURF_LIST_ELE,
    /*1349*/
    T_OBJSURF_PX,
    /*1350*/
    T_OBJSURF_PY,
    /*1351*/
    T_OBJSURF_PZ,
    /*1352*/
    T_OBJSURF_IZO,
    /*1353*/
    T_OBJSURF_LOC,

    /*1354*/
    T_BAR_REAC_KY,
    /*1355*/
    T_BAR_REAC_KZ,

    /*1356*/
    T_PUSHOVER_JOINT,

    /*1357*/
    T_MEMB_OFF_AUTO_BEG_UX,
    /*1358*/
    T_MEMB_OFF_AUTO_BEG_UY,
    /*1359*/
    T_MEMB_OFF_AUTO_BEG_UZ,
    /*1360*/
    T_MEMB_OFF_AUTO_END_UX,
    /*1361*/
    T_MEMB_OFF_AUTO_END_UY,
    /*1362*/
    T_MEMB_OFF_AUTO_END_UZ,
    /*1363*/
    T_MEMB_OFF_AUTO_DESCR, //internal RLIST use

    /*1364*/
    T_LOAD_POIPRO_ALLELE,

    /*1365*/
    T_PSH_SREA,
    /*1366*/
    T_PSH_DEPL,
    /*1367*/
    T_PSH_AB,
    /*1368*/
    T_PSH_BI0,
    /*1369*/
    T_PSH_I0LS,
    /*1370*/
    T_PSH_LSSS,
    /*1371*/
    T_PSH_SSC,
    /*1372*/
    T_PSH_CD,
    /*1373*/
    T_PSH_DE,
    /*1374*/
    T_PSH_E,

    /*1375*/
    T_PSH_SPEU,
    /*1376*/
    T_PSH_SPEV,

    /*1377*/
    T_PSH_DUMU,
    /*1378*/
    T_PSH_DUMV,

    /*1379*/
    T_PSH_DEMSPE_X,
    /*1380*/
    T_PSH_DEMSPE_Y,

    /*1381*/
    T_PSH_CONPER_X,
    /*1382*/
    T_PSH_CONPER_Y,

    /*1383*/
    T_PSH_REDSPE_X,
    /*1384*/
    T_PSH_REDSPE_Y,

    /*1385*/
    T_PSH_TEFF,
    /*1386*/
    T_PSH_BEFF,
    /*1387*/
    T_PSH_ALPHA,
    /*1388*/
    T_PSH_PF,
    /*1389*/
    T_PSH_EP_U,
    /*1390*/
    T_PSH_EP_V,
    /*1391*/
    T_PSH_EP_SD,
    /*1392*/
    T_PSH_EP_SA,
    /*1393*/
    T_PSH_EP_TEFF,
    /*1394*/
    T_PSH_EP_BEFF,

    /*1395*/
    T_PANEL_PLA_MESH,
    /*1396*/
    T_PANEL_PLA_SURF,
    /*1397*/
    T_PANEL_PLA_CARA,

    /*1398*/
    T_PANEL_PLA_NAME,
    /*1399*/
    T_PANEL_PLA_LIST,
    /*1400*/
    T_PANEL_PLA_E,
    /*1401*/
    T_PANEL_PLA_NI,
    /*1402*/
    T_PANEL_PLA_CW,
    /*1403*/
    T_PANEL_PLA_LX,
    /*1404*/
    T_PANEL_PLA_DAMP,

    /*1405*/
    T_FE_PANEL_PLA_CARA,

    /*1406*/
    T_MEMB_TRUSS,

    /*1407*/
    T_ADVAN_INCR,
    /*1408*/
    T_ADVAN_TIME,

    /*1409*/
    T_IMPERF_NAME,
    /*1410*/
    T_IMPERF_LIST,
    /*1411*/
    T_IMPERF_BEND_Z,
    /*1412*/
    T_IMPERF_BEND_Y,
    /*1413*/
    T_IMPERF_COEF_Z,
    /*1414*/
    T_IMPERF_COEF_Y,

    /*1415*/
    T_DISP_NODE_TOTAL,

    /*1416*/
    T_STRESS_LEVEL,

    /*1417*/
    T_VOLFORC_CUT_NAME,
    /*1418*/
    T_VOLFORC_N_PLUS,
    /*1419*/
    T_VOLFORC_N_MINUS,
    /*1420*/
    T_VOLFORC_N,
    /*1421*/
    T_VOLFORC_QT1,
    /*1422*/
    T_VOLFORC_QT2,
    /*1423*/
    T_VOLFORC_MN,
    /*1424*/
    T_VOLFORC_MT1,
    /*1425*/
    T_VOLFORC_MT2,
    /*1426*/
    T_VOLFORC_AREA,
    /*    */
    /*1427*/
    T_LOAD_POIPRO_MASCF,
    /*    */
    /*1428*/
    T_MSTA_INER_LABEL,
    /*1429*/
    T_MSTA_INER_LIST,
    /*1430*/
    T_MSTA_INER_LIST_ELE,
    /*1431*/
    T_MSTA_INER_AX,
    /*1432*/
    T_MSTA_INER_AY,
    /*1433*/
    T_MSTA_INER_AZ,
    /*1434*/
    T_MSTA_INER_REL,
    /*    */
    /*1435*/
    T_MSTA_ROTA_LABEL,
    /*1436*/
    T_MSTA_ROTA_LIST,
    /*1437*/
    T_MSTA_ROTA_LIST_ELE,
    /*1438*/
    T_MSTA_ROTA_VX,
    /*1439*/
    T_MSTA_ROTA_VY,
    /*1440*/
    T_MSTA_ROTA_VZ,
    /*1441*/
    T_MSTA_ROTA_AX,
    /*1442*/
    T_MSTA_ROTA_AY,
    /*1443*/
    T_MSTA_ROTA_AZ,
    /*1444*/
    T_MSTA_ROTA_X,
    /*1445*/
    T_MSTA_ROTA_Y,
    /*1446*/
    T_MSTA_ROTA_Z,

    /*1447*/
    T_MSTA_INER_MAS,
    /*1448*/
    T_MSTA_ROTA_MAS,

    /*1449*/
    T_POIPRO_COEFF,
    /*1450*/
    T_ELAPLA_COEF,
    /*1451*/
    T_ELAPLA_COORD_CART_X,

    /*1452*/
    T_FE_RF_XX_MIN_WAA,
    /*1453*/
    T_FE_RF_YY_MIN_WAA,
    /*1454*/
    T_FE_RF_XX_MAX_WAA,
    /*1455*/
    T_FE_RF_YY_MAX_WAA,
    /*1456*/
    T_FE_RF_XX_MIN_NEN,
    /*1457*/
    T_FE_RF_YY_MIN_NEN,
    /*1458*/
    T_FE_RF_XX_MAX_NEN,
    /*1459*/
    T_FE_RF_YY_MAX_NEN,

    /*    */        //MATERIAL PROPERTIES FOR ROBOT 'BOIS'
                    /*    */
                    /*    */
                    /*1460*/
    T_MAT_E,
    /*1461*/
    T_MAT_GMEAN,
    /*1462*/
    T_MAT_RO,
    /*1463*/
    T_MAT_RE_BENDING,
    /*1464*/
    T_MAT_RE_AXCOMP,
    /*1465*/
    T_MAT_RE_AXTENS,
    /*1466*/
    T_MAT_RE_TRCOMPR,
    /*1467*/
    T_MAT_RE_TRTENS,
    /*1468*/
    T_MAT_RE_SHEAR,
    /*1469*/
    T_MAT_LABEL_E,          // INTERNAL RLIST USE
                            /*1470*/
    T_MAT_LABEL_GMEAN,      // INTERNAL RLIST USE
                            /*1471*/
    T_MAT_LABEL_RO,         // INTERNAL RLIST USE
                            /*1472*/
    T_MAT_LABEL_RE_BENDING, // INTERNAL RLIST USE
                            /*1473*/
    T_MAT_LABEL_RE_AXCOMP,  // INTERNAL RLIST USE
                            /*1474*/
    T_MAT_LABEL_RE_AXTENS,  // INTERNAL RLIST USE
                            /*1475*/
    T_MAT_LABEL_RE_TRCOMPR, // INTERNAL RLIST USE
                            /*1476*/
    T_MAT_LABEL_RE_TRTENS,  // INTERNAL RLIST USE
                            /*1477*/
    T_MAT_LABEL_RE_SHEAR,   // INTERNAL RLIST USE
                            /*    */
                            /*1478*/
    T_MAT_CB71_HUMIDITY,
    /*1479*/
    T_MAT_CS_TIMBR,
    /*1480*/
    T_MAT_CS_STEEL,
    /*1481*/
    T_MAT_TYPE_TIMBR,
    /*1482*/
    T_MAT_LABEL_CB71_HUMIDITY,// INTERNAL RLIST USE
                              /*1483*/
    T_MAT_LABEL_CS_TIMBR,     // INTERNAL RLIST USE
                              /*1484*/
    T_MAT_LABEL_CS_STEEL,     // INTERNAL RLIST USE
                              /*1485*/
    T_MAT_LABEL_TYPE_TIMBR,   // INTERNAL RLIST USE

    /*1486*/
    T_OBJ_FROZEN,
    /*    */
    /*1487*/
    T_PRETRA_MP_LABEL,
    /*1488*/
    T_PRETRA_MP_LIST,
    /*1489*/
    T_PRETRA_MP_LIST_ELE,
    /*1490*/
    T_PRETRA_MP_ALFA,
    /*1491*/
    T_PRETRA_MP_BETA,
    /*1492*/
    T_PRETRA_MP_BAMMA,
    /*1493*/
    T_PRETRA_MP_LOC,
    /*1494*/
    T_PRETRA_MP_REL,
    /*1495*/
    T_PRETRA_MP_PX,
    /*1496*/
    T_PRETRA_MP_PY,
    /*1497*/
    T_PRETRA_MP_PZ,
    /*1498*/
    T_PRETRA_MP_X,
    /*    */        //ESTIMATION TABLE
                    /*1499*/
    T_ESTIM_S_SURF,
    /*1500*/
    T_ESTIM_S_VOL,
    /*1501*/
    T_ESTIM_S_SURF_T,
    /*1502*/
    T_ESTIM_S_VOL_T,
    /*1503*/
    T_ESTIM_G_SURF,
    /*1504*/
    T_ESTIM_G_VOL,
    /*1505*/
    T_ESTIM_G_SURF_T,
    /*1506*/
    T_ESTIM_G_VOL_T,

    /*1507*/
    T_GROUP_NAME,
    /*1508*/
    T_GROUP_LIST,
    /*1509*/
    T_GROUP_COLOR,

    /*1510*/
    T_SECT_R_IY,
    /*1511*/
    T_SECT_R_IZ,

    /*1512*/
    T_MEMB_RELACH_CODE_EX,

    /*1513*/
    T_DYNM_DAMP_TJ_TI,
    /*1514*/
    T_DYNM_DAMP_TJ_TI_LIMIT_PS92,
    /*1515*/
    T_DYNM_DAMP_TJ_TI_LIMIT_EC8,

    /*1516*/
    T_PRETRA_MPMAI_LIST,
    /*1517*/
    T_PRETRA_MPMAI_LIST_ELE,
    /*1518*/
    T_PRETRA_MPMAI_X1,
    /*1519*/
    T_PRETRA_MPMAI_PX1,
    /*1520*/
    T_PRETRA_MPMAI_PY1,
    /*1521*/
    T_PRETRA_MPMAI_PZ1,
    /*1522*/
    T_PRETRA_MPMAI_X2,
    /*1523*/
    T_PRETRA_MPMAI_PX2,
    /*1524*/
    T_PRETRA_MPMAI_PY2,
    /*1525*/
    T_PRETRA_MPMAI_PZ2,
    /*1526*/
    T_PRETRA_MPMAI_X3,
    /*1527*/
    T_PRETRA_MPMAI_PX3,
    /*1528*/
    T_PRETRA_MPMAI_PY3,
    /*1529*/
    T_PRETRA_MPMAI_PZ3,
    /*1530*/
    T_PRETRA_MPMAI_X4,
    /*1531*/
    T_PRETRA_MPMAI_PX4,
    /*1532*/
    T_PRETRA_MPMAI_PY4,
    /*1533*/
    T_PRETRA_MPMAI_PZ4,
    /*1534*/
    T_PRETRA_MPMAI_LOC,
    /*1535*/
    T_PRETRA_MPMAI_PROJ,
    /*1536*/
    T_PRETRA_MPMAI_REL,
    /*1537*/
    T_PRETRA_MPMAI_T,
    /*1538*/
    T_PRETRA_MPMAI_LS,
    /*1539*/
    T_PRETRA_MPMAI_UD,
    /*1540*/
    T_PRETRA_MPMAI_ALFA,
    /*1541*/
    T_PRETRA_MPMAI_BETA,
    /*1542*/
    T_PRETRA_MPMAI_GAMMA,
    /*1543*/
    T_SUPP_ROT,
    /*1544*/
    T_SUPP_LOC_GLO,
    /*1545*/
    T_SUPP_TYPE_ELA,
    /*1546*/
    T_SUPP_LABEL_ROT,
    /*1547*/
    T_SUPP_LABEL_LOC_GLO,
    /*1548*/
    T_SUPP_LABEL_TYPE_ELA,
    /*1549*/
    T_SECT_TP_TPOLY_D,           //TPoly
                                 /*1550*/
    T_SECT_TP_TPOLY_T,
    /*1551*/
    T_SECT_TP_TPOLY_N,
    /*1552*/
    T_SUPP_LABEL_EDGELIST,
    /*1553*/
    T_SUPP_LABEL_PARTLIST,

    /*1554*/
    T_STOREY_NAME,
    /*1555*/
    T_STOREY_LST_ELE,
    /*1556*/
    T_STOREY_LST_OBJ,
    /*1557*/
    T_STOREY_COLOR,
    /*1558*/
    T_STOREY_MASS,
    /*1559*/
    T_STOREY_G,
    /*1560*/
    T_STOREY_T,
    /*1561*/
    T_STOREY_SIZE_X,
    /*1562*/
    T_STOREY_SIZE_Y,
    /*1563*/
    T_STOREY_IX,
    /*1564*/
    T_STOREY_IY,
    /*1565*/
    T_STOREY_IZ,
    /*1566*/
    T_STOREY_EX0,
    /*1567*/
    T_STOREY_EY0,
    /*1568*/
    T_STOREY_EX1,
    /*1569*/
    T_STOREY_EY1,
    /*1570*/
    T_STOREY_EX2,
    /*1571*/
    T_STOREY_EY2,
    /*1572*/
    T_NOD_STOREY,
    /*1573*/
    T_OBJ_STOREY,
    /*1574*/
    T_FE_STOREY,

    /*1575*/
    T_PRESTRES_LABEL,
    /*1576*/
    T_PRESTRES_LIST,
    /*1577*/
    T_PRESTRES_N,
    /*1578*/
    T_PRESTRES_E1,
    /*1579*/
    T_PRESTRES_E2,
    /*1580*/
    T_PRESTRES_E3,
    /*1581*/
    T_PRESTRES_DIR,
    /*1582*/
    T_PRESTRES_LIST_ELE,

    /*1583*/
    T_STOREY_LST_OBJ_AUTO,

    /*1584*/
    T_SECT_R_IX,

    /*1585*/
    T_DISP_DEFL_TOTAL,
    /*1586*/
    T_DISP_DEFL_MAX_TOTAL,
    /*1587*/
    T_DISP_ELEM_TOTAL,

    /*1588*/
    T_MET_TH,
    /*1589*/
    T_MET_SURF,
    /*1590*/
    T_MET_WUN_S,
    /*1591*/
    T_MET_VOL,
    /*1592*/
    T_MET_WUN_V,
    /*1593*/
    T_RDIM_MNAME_EDIT,

    /*1594*/
    T_THICK_RED_I,
    /*1595*/
    T_THICK_UPLIFT,
    /*1596*/
    T_SECT_ELA_UPLIFT,

    /*1597*/
    T_BAR_REAC_ITG_KY,
    /*1598*/
    T_BAR_REAC_ITG_KZ,

    /*1599*/
    T_CONTUR_AUTO,

    /*1600*/
    T_FERA_SRNF_AS_MIN_LONG_UP,
    /*1601*/
    T_FERA_SRNF_AS_MIN_LONG_DOWN,
    /*1602*/
    T_FERA_SRNF_AS_MIN_TRAN_UP,
    /*1603*/
    T_FERA_SRNF_AS_MIN_TRAN_DOWN,
    /*1604*/
    T_FERA_SRNF_STIFF_LONG_UP,
    /*1605*/
    T_FERA_SRNF_STIFF_LONG_DOWN,
    /*1606*/
    T_FERA_SRNF_STIFF_TRAN_UP,
    /*1607*/
    T_FERA_SRNF_STIFF_TRAN_DOWN,
    /*1608*/
    T_FERA_SRNF_DFL_UP,
    /*1609*/
    T_FERA_SRNF_DFL_DOWN,
    /*1610*/
    T_FERA_SRNF_CRK_LT_LONG_DOWN,
    /*1611*/
    T_FERA_SRNF_CRK_LT_TRAN_UP,
    /*1612*/
    T_FERA_SRNF_CRK_LT_TRAN_DOWN,
    /*1613*/
    T_FERA_SRNF_CRK_ST_LONG_UP,
    /*1614*/
    T_FERA_SRNF_CRK_ST_LONG_DOWN,
    /*1615*/
    T_FERA_SRNF_CRK_ST_TRAN_UP,
    /*1616*/
    T_FERA_SRNF_CRK_ST_TRAN_DOWN,

    /*    */        //RESULTS FOR REINFORCEMENT
                    /*1617*/
    T_FERA_ELE_ASMIN_LONG_UP,
    /*1618*/
    T_FERA_ELE_ASMIN_TRAN_UP,
    /*1619*/
    T_FERA_ELE_ASMIN_LONG_DOWN,
    /*1620*/
    T_FERA_ELE_ASMIN_TRAN_DOWN,

    /*1621*/
    T_FERA_NOD_ASMIN_LONG_UP,
    /*1622*/
    T_FERA_NOD_ASMIN_TRAN_UP,
    /*1623*/
    T_FERA_NOD_ASMIN_LONG_DOWN,
    /*1624*/
    T_FERA_NOD_ASMIN_TRAN_DOWN,

    /*1625*/
    T_FERA_ELE_ASMIN_SX_UP,
    /*1626*/
    T_FERA_ELE_ASMIN_SY_UP,
    /*1627*/
    T_FERA_ELE_ASMIN_SX_DOWN,
    /*1628*/
    T_FERA_ELE_ASMIN_SY_DOWN,

    /*1629*/
    T_FERA_NOD_ASMIN_SX_UP,
    /*1630*/
    T_FERA_NOD_ASMIN_SY_UP,
    /*1631*/
    T_FERA_NOD_ASMIN_SX_DOWN,
    /*1632*/
    T_FERA_NOD_ASMIN_SY_DOWN,

    /*1633*/
    T_FERA_ELE_NUMX_UP,
    /*1634*/
    T_FERA_ELE_NUMY_UP,
    /*1635*/
    T_FERA_ELE_NUMX_DOWN,
    /*1636*/
    T_FERA_ELE_NUMY_DOWN,

    /*1637*/
    T_FERA_NOD_NUMX_UP,
    /*1638*/
    T_FERA_NOD_NUMY_UP,
    /*1639*/
    T_FERA_NOD_NUMX_DOWN,
    /*1640*/
    T_FERA_NOD_NUMY_DOWN,

    /*1641*/
    T_FERA_ELE_ASMIN_NUMX_UP,
    /*1642*/
    T_FERA_ELE_ASMIN_NUMY_UP,
    /*1643*/
    T_FERA_ELE_ASMIN_NUMX_DOWN,
    /*1644*/
    T_FERA_ELE_ASMIN_NUMY_DOWN,

    /*1645*/
    T_FERA_NOD_ASMIN_NUMX_UP,
    /*1646*/
    T_FERA_NOD_ASMIN_NUMY_UP,
    /*1647*/
    T_FERA_NOD_ASMIN_NUMX_DOWN,
    /*1648*/
    T_FERA_NOD_ASMIN_NUMY_DOWN,

    /*    */        //RESULTS FOR REINFORCEMENT - LIMITS

    /*1649*/
    T_FERA_ELE_CRK_LT_LONG_UP,
    /*1650*/
    T_FERA_ELE_CRK_LT_TRAN_UP,
    /*1651*/
    T_FERA_ELE_CRK_LT_LONG_DOWN,
    /*1652*/
    T_FERA_ELE_CRK_LT_TRAN_DOWN,
    /*1653*/
    T_FERA_NOD_CRK_LT_LONG_UP,
    /*1654*/
    T_FERA_NOD_CRK_LT_TRAN_UP,
    /*1655*/
    T_FERA_NOD_CRK_LT_LONG_DOWN,
    /*1656*/
    T_FERA_NOD_CRK_LT_TRAN_DOWN,

    /*1657*/
    T_FERA_ELE_CRK_ST_LONG_UP,
    /*1658*/
    T_FERA_ELE_CRK_ST_TRAN_UP,
    /*1659*/
    T_FERA_ELE_CRK_ST_LONG_DOWN,
    /*1660*/
    T_FERA_ELE_CRK_ST_TRAN_DOWN,
    /*1661*/
    T_FERA_NOD_CRK_ST_LONG_UP,
    /*1662*/
    T_FERA_NOD_CRK_ST_TRAN_UP,
    /*1663*/
    T_FERA_NOD_CRK_ST_LONG_DOWN,
    /*1664*/
    T_FERA_NOD_CRK_ST_TRAN_DOWN,

    /*1665*/
    T_FERA_ELE_STIFF_LONG_UP,
    /*1666*/
    T_FERA_ELE_STIFF_TRAN_UP,
    /*1667*/
    T_FERA_ELE_STIFF_LONG_DOWN,
    /*1668*/
    T_FERA_ELE_STIFF_TRAN_DOWN,
    /*1669*/
    T_FERA_NOD_STIFF_LONG_UP,
    /*1670*/
    T_FERA_NOD_STIFF_TRAN_UP,
    /*1671*/
    T_FERA_NOD_STIFF_LONG_DOWN,
    /*1672*/
    T_FERA_NOD_STIFF_TRAN_DOWN,

    /*1673*/
    T_FERA_ELE_DFL_UP,
    /*1674*/
    T_FERA_ELE_DFL_DOWN,
    /*1675*/
    T_FERA_NOD_DFL_UP,
    /*1676*/
    T_FERA_NOD_DFL_DOWN,

    /*1677*/
    T_PANEL_KZ_SUM,

    /*1678*/
    T_SECT_NLC_MSY,
    /*1679*/
    T_SECT_NLC_MSZ,

    /*1680*/
    T_LOA_CONV_CAS2CONV,
    /*1681*/
    T_LOA_CONV_DIR,
    /*1682*/
    T_LOA_CONV_COEFF,
    /*1683*/
    T_LOA_CONV_DIR_MASS,
    /*1684*/
    T_LOA_CONV_CAS,

    /*1685*/
    T_STOREY_RED_FX,
    /*1686*/
    T_STOREY_RED_FY,
    /*1687*/
    T_STOREY_RED_FZ,
    /*1688*/
    T_STOREY_RED_MX,
    /*1689*/
    T_STOREY_RED_MY,
    /*1690*/
    T_STOREY_RED_MZ,
    /*1691*/
    T_STOREY_RED_FXPC,
    /*1692*/
    T_STOREY_RED_FYPC,
    /*1693*/
    T_STOREY_RED_FZPC,
    /*1694*/
    T_STOREY_RED_FXPW,
    /*1695*/
    T_STOREY_RED_FYPW,
    /*1696*/
    T_STOREY_RED_FZPW,

    /*1697*/
    T_NOEVEL_LABEL,
    /*1698*/
    T_NOEVEL_LIST,
    /*1699*/
    T_NOEVEL_UX,
    /*1700*/
    T_NOEVEL_UY,
    /*1701*/
    T_NOEVEL_UZ,
    /*1702*/
    T_NOEVEL_RX,
    /*1703*/
    T_NOEVEL_RY,
    /*1704*/
    T_NOEVEL_RZ,
    /*1705*/
    T_NOEVEL_ALFA,
    /*1706*/
    T_NOEVEL_BETA,
    /*1707*/
    T_NOEVEL_GAMMA,
    /*1708*/
    T_NOEVEL_LIST_ELE,
    /*1709*/
    T_NOEACC_LABEL,
    /*1710*/
    T_NOEACC_LIST,
    /*1711*/
    T_NOEACC_UX,
    /*1712*/
    T_NOEACC_UY,
    /*1713*/
    T_NOEACC_UZ,
    /*1714*/
    T_NOEACC_RX,
    /*1715*/
    T_NOEACC_RY,
    /*1716*/
    T_NOEACC_RZ,
    /*1717*/
    T_NOEACC_ALFA,
    /*1718*/
    T_NOEACC_BETA,
    /*1719*/
    T_NOEACC_GAMMA,
    /*1720*/
    T_NOEACC_LIST_ELE,

    /*1721*/
    T_PANEL_KX_SUM,
    /*1722*/
    T_PANEL_KY_SUM,
    /*1723*/
    T_THICK_KX,
    /*1724*/
    T_THICK_KY,

    /*1725*/
    T_NODE_LIST_PIEC,
    /*1726*/
    T_NODE_LIST_ELE,
    /*1727*/
    T_NODE_LIST_DSC,

    /*1728*/
    T_CASE_PREFIX,

    /*1729*/
    T_FORC_FRX,
    /*1730*/
    T_FORC_FRZ,
    /*1731*/
    T_FORC_MRY,

    /*1732*/
    T_PLABEAM_DIM_PLTHI,
    /*1733*/
    T_PLABEAM_DIM_BMHY,
    /*1734*/
    T_PLABEAM_DIM_BMHZ,
    /*1735*/
    T_PLABEAM_DIM_E1,
    /*1736*/
    T_PLABEAM_DIM_E2,
    /*1737*/
    T_PLABEAM_DIM_BEFF,

    /*1738*/
    T_CAS_TYP_2ORD,

    /*1739*/
    T_JOIST_ELEMLIST,              //elements list
                                   /*1740*/
    T_JOIST_NAME,
    /*1741*/
    T_JOIST_TOTAL_LOAD,
    /*1742*/
    T_JOIST_LIVE_LOAD,
    /*1743*/
    T_JOIST_MOMENT_CAPACITY,
    /*1744*/
    T_JOIST_SHEAR_CAPACITY,
    /*1745*/
    T_JOIST_WEIGHT,
    /*1746*/
    T_JOIST_DEPTH,
    /*1747*/
    T_JOIST_IY,
    /*1748*/
    T_JOIST_MAX_SPAN_LENGTH,
    /*1749*/
    T_JOIST_MIN_SPAN_LENGTH,
    /*1750*/
    T_JOIST_DEFL_LIMIT,
    /*1751*/
    T_JOIST_DEFL_TYPE,             //std/envelope

    /*    */        //SUPPORTS - DAMPING COEFFS
                    /*1752*/
    T_SUPP_LABEL_CX_DAMP,
    /*1753*/
    T_SUPP_LABEL_CY_DAMP,
    /*1754*/
    T_SUPP_LABEL_CZ_DAMP,
    /*1755*/
    T_SUPP_LABEL_CRX_DAMP,
    /*1756*/
    T_SUPP_LABEL_CRY_DAMP,
    /*1757*/
    T_SUPP_LABEL_CRZ_DAMP,

    /*1758*/
    T_CASE_USRNUM,
    /*1759*/
    T_FORCON_GENNOD,

    /*1760*/
    T_FE_SURF,

    /*1761*/
    T_DRIFT_GLO_X,
    /*1762*/
    T_DRIFT_GLO_Y,
    /*1763*/
    T_DRIFT_GLO_XY,
    /*1764*/
    T_DRIFT_LOC_Y,
    /*1765*/
    T_DRIFT_LOC_Z,
    /*1766*/
    T_DRIFT_LOC_YZ,

    /*1767*/
    T_MASMOD_X,
    /*1768*/
    T_MASMOD_Y,
    /*1769*/
    T_MASMOD_Z,
    /*1770*/
    T_MASMOD_NOD_X,
    /*1771*/
    T_MASMOD_NOD_Y,
    /*1772*/
    T_MASMOD_NOD_Z,

    /*1773*/
    T_FINT_FX,
    /*1774*/
    T_FINT_FY,
    /*1775*/
    T_FINT_FZ,
    /*1776*/
    T_FINT_MX,
    /*1777*/
    T_FINT_MY,
    /*1778*/
    T_FINT_MZ,

    /*1779*/
    T_STRAIN_EPS,
    /*1780*/
    T_STRAIN_EPS_P,
    /*1781*/
    T_STRAIN_EPS_MIN,
    /*1782*/
    T_STRAIN_EPS_MAX,

    /*    */        //  Forced vibration in frequency domain (FRF)
                    /*1783*/
    T_FRF_VEL_UX,
    /*1784*/
    T_FRF_VEL_UY,
    /*1785*/
    T_FRF_VEL_UZ,
    /*1786*/
    T_FRF_VEL_RX,
    /*1787*/
    T_FRF_VEL_RY,
    /*1788*/
    T_FRF_VEL_RZ,
    /*1789*/
    T_FRF_ACC_UX,
    /*1790*/
    T_FRF_ACC_UY,
    /*1791*/
    T_FRF_ACC_UZ,
    /*1792*/
    T_FRF_ACC_RX,
    /*1793*/
    T_FRF_ACC_RY,
    /*1794*/
    T_FRF_ACC_RZ,

    /*1795*/
    T_FRF_DEFINITION,      //internal RLIST
                           /*1796*/
    T_FRF_LABEL,           //internal RLIST
                           /*1797*/
    T_FRF_CASE,            //internal RLIST
                           /*1798*/
    T_FRF_VAL_NAME_UNIT,   //internal RLIST
                           /*1799*/
    T_FRF_VAL_MIN,         //internal RLIST
                           /*1800*/
    T_FRF_VAL_MAX,         //internal RLIST
                           /*1801*/
    T_FRF_OBJ,             //internal RLIST
                           /*1802*/
    T_FRF_COLOR,           //internal RLIST
                           /*1803*/
    T_FRF_POSITION,        //internal RLIST
                           /*1804*/
    T_FRF_LAYER,           //internal RLIST
                           /*1805*/
    T_FRF_DIRECTION,       //internal RLIST
                           /*1806*/
    T_FRF_POS_MIN,         //internal RLIST
                           /*1807*/
    T_FRF_POS_MAX,         //internal RLIST

    /*1808*/
    T_STOREY_LEVEL,
    /*1809*/
    T_STOREY_HEIGHT,

    /*1810*/
    T_FRF_VEL_U,
    /*1811*/
    T_FRF_ACC_U,
    /*1812*/
    T_FRF_UX_F,
    /*1813*/
    T_FRF_UY_F,
    /*1814*/
    T_FRF_UZ_F,
    /*1815*/
    T_FRF_U_F,
    /*1816*/
    T_FRF_RX_M,
    /*1817*/
    T_FRF_RY_M,
    /*1818*/
    T_FRF_RZ_M,
    /*1819*/
    T_FRF_AX_F,
    /*1820*/
    T_FRF_AY_F,
    /*1821*/
    T_FRF_AZ_F,
    /*1822*/
    T_FRF_A_F,
    /*1823*/
    T_FRF_ARX_M,
    /*1824*/
    T_FRF_ARY_M,
    /*1825*/
    T_FRF_ARZ_M,
    /*1826*/
    T_FRF_VX_F,
    /*1827*/
    T_FRF_VY_F,
    /*1828*/
    T_FRF_VZ_F,
    /*1829*/
    T_FRF_V_F,
    /*1830*/
    T_FRF_VRX_M,
    /*1831*/
    T_FRF_VRY_M,
    /*1832*/
    T_FRF_VRZ_M,
    /*1833*/
    T_FRF_F_UX,
    /*1834*/
    T_FRF_F_UY,
    /*1835*/
    T_FRF_F_UZ,
    /*1836*/
    T_FRF_F_U,
    /*1837*/
    T_FRF_M_RX,
    /*1838*/
    T_FRF_M_RY,
    /*1839*/
    T_FRF_M_RZ,
    /*1840*/
    T_FRF_F_VX,
    /*1841*/
    T_FRF_F_VY,
    /*1842*/
    T_FRF_F_VZ,
    /*1843*/
    T_FRF_F_V,
    /*1844*/
    T_FRF_M_VRX,
    /*1845*/
    T_FRF_M_VRY,
    /*1846*/
    T_FRF_M_VRZ,
    /*1847*/
    T_FRF_F_AX,
    /*1848*/
    T_FRF_F_AY,
    /*1849*/
    T_FRF_F_AZ,
    /*1850*/
    T_FRF_F_A,
    /*1851*/
    T_FRF_M_ARX,
    /*1852*/
    T_FRF_M_ARY,
    /*1853*/
    T_FRF_M_ARZ,

    /*1854*/
    T_FF_MX_TOTACC,
    /*1855*/
    T_FF_MX_RFHARM,
    /*1856*/
    T_FF_MX_RFTRAN,
    /*1857*/
    T_FF_MX_RFTOTL,
    /*1858*/
    T_FF_V_RMS,
    /*1859*/
    T_FF_V_RMQ,

    /*1860*/
    T_FF_N_ANS,
    /*1861*/
    T_FF_N_FORC,
    /*1862*/
    T_FF_FRQ,
    /*1863*/
    T_FF_A_COEFF,

    //internal RLIST
    /*1864*/
    T_FF_GR_RF_F_1,
    /*1865*/
    T_FF_GR_RF_F_2,
    /*1866*/
    T_FF_GR_RF_F_3,
    /*1867*/
    T_FF_GR_RF_F_4,
    /*1868*/
    T_FF_GR_RF_F_TOT,

    /*1869*/
    T_FF_GR_A_F_1,
    /*1870*/
    T_FF_GR_A_F_2,
    /*1871*/
    T_FF_GR_A_F_3,
    /*1872*/
    T_FF_GR_A_F_4,
    /*1873*/
    T_FF_GR_A_F_TOT,

    /*1874*/
    T_FF_GR_DLF_F_1,
    /*1875*/
    T_FF_GR_DLF_F_2,
    /*1876*/
    T_FF_GR_DLF_F_3,
    /*1877*/
    T_FF_GR_DLF_F_4,
    /*1878*/
    T_FF_GR_DLF_F_TOT,

    /*1879*/
    T_FF_GR_TRAN_RF_F,
    /*1880*/
    T_FF_GR_TRAN_RF_F_TOT,

    /*1881*/
    T_FF_GR_TRAN_V_T,

    /*1882*/
    T_FF_GR_TRAN_VRMS,
    /*1883*/
    T_FF_GR_TRAN_VRMS_T,

    /*1884*/
    T_FF_GR_TRAN_VRMQ,

    /*1885*/
    T_FF_GR_FRQ,
    /*1886*/
    T_FF_GR_TIME,

    /*1887*/
    T_PANEL_CLMSF,

    /*1888*/
    T_OBJ_TYPE,

    /*1889*/
    T_BKGND_NAME,
    /*1890*/
    T_BKGND_SCALE,

    /*1891*/
    T_PANEL_ENV,
    /*1892*/
    T_PANEL_PNLCL,
    /*1893*/
    T_PANEL_PNLCL_ES,
    /*1894*/
    T_PANEL_PNLCL_LOA,
    /*1895*/
    T_PANEL_PNLCL_DIAFR,

    /*1896*/
    T_OBJ_ENV,
    /*1897*/
    T_BKGND_PATH,
    /*1898*/
    T_BKGND_VISIBLE,
    /*1899*/
    T_BKGND_COLOR,
    /*1900*/
    T_BKGND_PLANE,
    /*1901*/
    T_BKGND_INSPNT2D,
    /*1902*/
    T_BKGND_INSPNT1D,
    /*1903*/
    T_BKGND_ROT,
    /*1904*/
    T_BKGND_RANGE_TYPE,
    /*1905*/
    T_BKGND_FROM,
    /*1906*/
    T_BKGND_FROM_VAL,
    /*1907*/
    T_BKGND_TO,
    /*1908*/
    T_BKGND_TO_VAL,
    /*1909*/
    T_MERGE_LST_OBJ,

    /*1910*/
    T_FF_MX_ARMS,

    /*1881*/
    T_FF_GR_TRAN_A_T,

    /*1912*/
    T_FF_GR_TRAN_ARMS,
    /*1913*/
    T_FF_GR_TRAN_ARMS_T,

    /*1914*/
    T_NOTIONAL_CAS_CONV,
    /*1915*/
    T_NOTIONAL_COEFF,
    /*1916*/
    T_NOTIONAL_MASS_DIR,
    /*1917*/
    T_NOTIONAL_CAS,

    /*1918*/
    T_BEST_XPOS,

    /*1919*/
    T_CORWAL_RED_FX,
    /*1920*/
    T_CORWAL_RED_FY,
    /*1921*/
    T_CORWAL_RED_FZ,
    /*1922*/
    T_CORWAL_RED_MX,
    /*1923*/
    T_CORWAL_RED_MY,
    /*1924*/
    T_CORWAL_RED_MZ,
    /*1925*/
    T_CORWAL_NAME,
    /*1926*/
    T_CORWAL_LST_OBJ,
    /*1927*/
    T_CORWAL_COLOR,
    /*1928*/
    T_CORWAL_G,

    /*1929*/
    T_STOREY_F,
    /*1930*/
    T_STOREY_DRF_UX,
    /*1931*/
    T_STOREY_DRF_UY,
    /*1932*/
    T_STOREY_DRFR_UX,
    /*1933*/
    T_STOREY_DRFR_UY,
    /*1934*/
    T_STOREY_DPL_UX,
    /*1935*/
    T_STOREY_DPL_UY,
    /*1936*/
    T_STOREY_DPL_MAX_UX,
    /*1937*/
    T_STOREY_DPL_MAX_UY,
    /*1938*/
    T_STOREY_DPL_MIN_UX,
    /*1939*/
    T_STOREY_DPL_MIN_UY,
    /*1940*/
    T_STOREY_MASS_X,
    /*1941*/
    T_STOREY_MASS_Y,
    /*1942*/
    T_STOREY_MASS_Z,

    /*1943*/
    T_MEMB_OFFSPH,
    /*1944*/
    T_ELEM_OFFSPH,

    /*1945*/
    T_STOREY_ALPHA_CR_X,
    /*1946*/
    T_STOREY_ALPHA_CR_Y,
    /*1947*/
    T_BEST_STIRRUPS_DENS,

    /*    */
    T_TYPES_MAXDATA
};





