namespace BH.oM.Adapters.Robot
{
    /***************************************************/
    /****               Public Enums                ****/
    /***************************************************/

    public enum MaterialDB
    {
        American,
        British,
        Eurocode        
    }

    /***************************************************/

    public enum SectionDB
    {
        UKST,
        AISC
    }

    /***************************************************/

    public enum ObjectProperties
    {
        Name, 
        Number
    }

    /***************************************************/

    public enum BarProperties
    {
        FramingElementDesignProperties,
        StructureObjectType                
    }

    /***************************************************/

    public enum NodeProperties
    {
        CoordinateSystem
    }

    /***************************************************/

    public enum Commands
    {

    }

    /***************************************************/

    public enum DesignCode_Steel
    {
        Default = 0,
        BS5950,
        BS5950_2000,
        BS_EN_1993_1_2005_NA_2008_A1_2014,
        ANSI_AISC_360_10   
    }

    /***************************************************/

}
