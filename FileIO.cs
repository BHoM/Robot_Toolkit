using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RobotOM;
using Marshal = System.Runtime.InteropServices.Marshal;

namespace RobotToolkit
{
    public class FileIO
    {
        public RobotLink robotLink { get; set; }
        public bool HasLicense { get; set; }
        public RobotApplication robapp = new RobotApplication();

        public FileIO()
        {
            robotLink = new RobotLink();
            HasLicense = robotLink.Link.LicenseCheckEntitlement(IRobotLicenseEntitlement.I_LE_LOCAL_SOLVE) == IRobotLicenseEntitlementStatus.I_LES_ENTITLED;

            if (!HasLicense)
                MessageBox.Show("Could not obtain robot license");
        }

        public FileIO(string path) : this()
        {
            Open(path);
        }

        public void Open(string path)
        {
            robotLink.Link.Project.Open(path);
        }

        public void New()
        {
            robotLink.Link.Project.New(IRobotProjectType.I_PT_FRAME_3D);
        }

        public void Save()
        {
            robotLink.Link.Project.Save();
        }

        public void SaveAs(string path)
        {
            robotLink.Link.Project.SaveAs(path);
        }

       
}

    //Lots of useful information on how this class is supposed to function can be found here:
    // http://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface
    
    public class RobotLink: IDisposable
    {
        //private static GsaLink uniqueInstance { get; set; } //Singleton

        public RobotApplication Link { get; private set; } //The Com link


        //Constructor
        public RobotLink()
        {
            try
            {
                Link = new RobotApplication();
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't initialise Robot" + Environment.NewLine + e.Message, "Error");
                Link = null;
            }
        }


        ////Singleton
        ///// <summary>
        ///// Use to get instance
        ///// </summary>
        ///// <returns></returns>
        //public static GsaLink GetInstance()
        //{
        //    if (uniqueInstance == null)
        //        uniqueInstance = new GsaLink();

        //    return uniqueInstance;
        //}


        public void Dispose()
        {
            //Maybe this should be in the Dispose(bool) method but I think that doing so will keep it alive indefinitely
            GC.KeepAlive(Link);

            Dispose(true);
            GC.SuppressFinalize(this);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing">True if it is safe to free managed objects.
        /// True when called from Dispose(), false when called from Finalize()</param>
        void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Link != null)
                {
                    Marshal.FinalReleaseComObject(Link);
                }
            }
            //uniqueInstance = null;
            GC.Collect();
        }


        ~RobotLink()
        {
            Dispose(false);
        }
    }
}
