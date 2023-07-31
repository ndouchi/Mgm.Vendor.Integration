using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgm.VI.IO
{
    public class FileNode : IFileNode
    {
        #region Private Variables
        private Guid _originalGuid;
        private Guid _nodeGuid = new Guid();
        private string _name;
        private string _physicalPath;
        #endregion Private Variables

        #region Properties
        public Guid OriginalGuid { get { return _originalGuid; } }
        public Guid NodeGuid { get { return _nodeGuid; } }

        public string Name { get { return _name; } }
        public string PhysicalPath { get { return _physicalPath; } }
        public bool Exists { get { return File.Exists(_physicalPath); } }
        public string LastErrorMessage { get; private set; }
        public Error LastError { get; set; }
        #endregion Properties

        #region Constructors
        public FileNode(string name, string physicalPath)
        {
            _name = name;
            _physicalPath = physicalPath;
        }
        #endregion

        #region Public Methods
        public bool Create(bool createDirectoryIfNotExisting = false)
        {
            bool fileCreated = false;
            if (!File.Exists(_physicalPath))
            {
                try
                {
                    string directoryPath = Path.GetDirectoryName(_physicalPath);
                    if (!Directory.Exists(directoryPath) && createDirectoryIfNotExisting)
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    using (File.Create(_physicalPath)) { } // Relinquishes file automatically after creation.
                    fileCreated = true;
                }
                catch (Exception ex) //TODO: Instead of swallowing the exceptin, do what?
                {
                    fileCreated = false; //Redundant. TODO: Decide on an action
                    LastErrorMessage = ex.Message;
                    LastError = new Error(ex);
                }
            }
            return fileCreated;
        }
        public bool Delete()
        {
            return DeletePhysicalFile();
        }
        #endregion Public Methods
        private bool DeletePhysicalFile()
        {
            bool deleteResult;
            try
            {
                if (Exists)
                {
                    File.Delete(_physicalPath);
                }
                deleteResult = true;
            }
            catch (Exception ex) //TODO: Instead of swallowing the exceptin, do what?
            {
                deleteResult = false; //Redundant. TODO: Decide on an action
                LastErrorMessage = ex.Message;
                LastError = new Error(ex);
            }

            return deleteResult;
        }

    }
}
