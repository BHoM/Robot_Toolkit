using System;
using System.Windows.Forms;
using RobotOM;
using Marshal = System.Runtime.InteropServices.Marshal;

namespace RobotToolkit
{
    /// <summary>
    /// Input/output class to control how Robot application is accessed
    /// </summary>
    public class FileIO
    {
        /// <summary>Robotlink object</summary>
        public RobotLink robotLink { get; set; }
        /// <summary>True if a license is available</summary>
        public bool HasLicense { get; set; }
        /// <summary>Robot application object</summary>
        public RobotApplication robapp = new RobotApplication();

        /// <summary>
        /// Constructs an empty FileIO object
        /// </summary>
        public FileIO()
        {
            robotLink = new RobotLink();
            HasLicense = robotLink.Link.LicenseCheckEntitlement(IRobotLicenseEntitlement.I_LE_LOCAL_SOLVE) == IRobotLicenseEntitlementStatus.I_LES_ENTITLED;

            if (!HasLicense)
                MessageBox.Show("Could not obtain robot license");
        }

        /// <summary>
        /// Constructs a FileIO object using a path/filename as a string
        /// </summary>
        /// <param name="path"></param>
        public FileIO(string path) : this()
        {
            Open(path);
        }

        /// <summary>
        /// Opens a Robot model using a path/filename as a string
        /// </summary>
        /// <param name="path"></param>
        public void Open(string path)
        {
            robotLink.Link.Project.Open(path);
        }

        /// <summary>
        /// Opens a blank Robot project with 3D frame type UI
        /// </summary>
        public void New()
        {
            robotLink.Link.Project.New(IRobotProjectType.I_PT_FRAME_3D);
        }

        /// <summary>
        /// Saves a Robot model
        /// </summary>
        public void Save()
        {
            robotLink.Link.Project.Save();
        }

        /// <summary>
        /// Saves a Robot model to a given file/folder path name
        /// </summary>
        /// <param name="path"></param>
        public void SaveAs(string path)
        {
            robotLink.Link.Project.SaveAs(path);
        }

       
}

    //Lots of useful information on how this class is supposed to function can be found here:
    // http://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface
    
        /// <summary>
        /// Robot Link object
        /// </summary>
    public class RobotLink: IDisposable
    {
        /// <summary>Robot COM link</summary>
        public RobotApplication Link { get; private set; }


        /// <summary>Contructs and empty RobotLink</summary>
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


       /// <summary>
       /// Dispose the link object
       /// </summary>
        public void Dispose()
        {
            //Maybe this should be in the Dispose(bool) method but I think that doing so will keep it alive indefinitely
            GC.KeepAlive(Link);

            Dispose(true);
            GC.SuppressFinalize(this);
        }



        /// <summary>
        /// Dispose the link
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

        /// <summary>
        /// RobotLink
        /// </summary>
        ~RobotLink()
        {
            Dispose(false);
        }
    }
}
