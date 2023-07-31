using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Mgm.VI.IO.IoUtility;

namespace Mgm.VI.IO
{
    public class DirectoryNode : IDirectoryNode
    {
        #region Private Variables
        private Guid _originalGuid;
        private Guid _nodeGuid = new Guid();
        private bool _isLeaf = true;
        private bool _isEmpty = true;
        private bool _isEmptyOfFiles = true;
        private string _name;
        private string _physicalPath;
        private DirectoryNode[] _subdirectories;
        private FileNode[] _files;

        private static int _countOfFilesNotCopied = 0;
        private static Dictionary<string, IError> _uncopiedFiles = new Dictionary<string, IError>(); //TKey is filepath, TValue is error message
        private static string _errorLogFullPath = string.Empty;
        #endregion Private Variables

        #region Properties
        public Guid OriginalGuid { get { return _originalGuid; } }
        public Guid NodeGuid { get { return _nodeGuid; } }
        public bool IsLeaf { get { return _isLeaf; } }
        public bool IsEmpty { get { return _isEmpty; } }
        public bool IsEmptyOfFiles { get { return _isEmptyOfFiles; } }
        public string Name { get { return _name; } }
        public string PhysicalPath { get { return _physicalPath; } }
        public bool Exists { get { return Directory.Exists(_physicalPath); } }
        public IDirectoryNode[] Subdirectories
        {
            get
            {
                return _subdirectories;
            }
            set
            {
                _subdirectories = (DirectoryNode[])value;
                _isLeaf = false;
                _isEmpty = false;
            }
        }
        public IFileNode[] Files
        {
            get
            {
                return _files;
            }
            set
            {
                _files = (FileNode[])value;
                _isEmptyOfFiles = false;
                _isEmpty = false;
            }
        }
        public string LastErrorMessage { get; private set; }
        public Error LastError { get; set; }
        public string errorLogFullPath
        {
            get
            {
                return _errorLogFullPath;
            }
            private set
            {
                FileNode errorFile = new FileNode("Error Log", value);
                if (errorFile.Exists || errorFile.Create())
                {
                    _errorLogFullPath = value;
                }
            }
        }
        #endregion Properties

        #region Constructors
        public DirectoryNode(string name, string physicalPath)
        {
            _name = name;
            _physicalPath = physicalPath;
            _subdirectories = GetSubDirectories();
            _files = GetFiles();
        }
        public DirectoryNode(DirectoryNode directoryNode)
             : this(directoryNode.Name, directoryNode.PhysicalPath)
        {
            _isLeaf = directoryNode.IsLeaf;
            _isEmpty = directoryNode.IsEmpty;
            _isEmptyOfFiles = directoryNode.IsEmptyOfFiles;

            if (directoryNode.Subdirectories != null)
            {
                Subdirectories = directoryNode.Subdirectories;
            }
            if (directoryNode.Files != null)
            {
                Files = directoryNode.Files;
            }
        }
        #endregion Constructors

        #region Private Methods
        private void IncrementCountOfUncopiedFiles()
        { _countOfFilesNotCopied++; }
        private void LogFailedFileCopy(string filePath, string errorMessage, Exception exception = null)
        {
            Error error = null;

            if (exception == null)
            {
                error = new Error(errorMessage, exception);
            }
            else
            {
                error = new Error(errorMessage);
            }

            _uncopiedFiles.Add(filePath, error);
            //TODO: Add FileLogger
            IncrementCountOfUncopiedFiles();
        }
        private void LogFailedFileCopy(string filePath, Exception exception)
        {
            LogFailedFileCopy(filePath, exception.Message, exception);
        }
        private bool DeletePhysicalDirectory()
        {
            bool deleteResult;
            try
            {
                if (Exists)
                {
                    Directory.Delete(_physicalPath);
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

        #endregion Private Methods 

        #region Public Methods
        public bool CopyTo(
                            string targetDir,
                            string destinationRootPath,
                            bool createTargetDirectoryIfNotExisting,
                            CopyMode copyMode,
                            string errorLogFullPath,
                            MergeMode mergeMode,
                            bool deleteContentInTarget,
                            bool    archiveTarget,
                            List<string>    extensionExclusionList                  = null,
                            List<string>    directoryNameExclusionListBeginningWith = null,
                            List<string>    directoryNameExclusionListContaining    = null,
                            List<string>    directoryNameExclusionListEndingWith    = null
                            )
        {
            bool proceedWithCopy = true;
            bool result = true;

            string sourcePath = _physicalPath.EndsWith(@"\") ? _physicalPath : _physicalPath + @"\";
            destinationRootPath = targetDir;
            targetDir = targetDir.EndsWith(@"\") ? targetDir : targetDir + @"\";

           

            ///////////////////////////////////////////////////////////
            /////////////
            //////////////      TODO:  Fix the exception Extensions bug. Why aren't they picked up?
            /////////////
            ////////////////////////////////////////////////////////////

            try
            {
                if (this.Exists)
                {
                    DirectoryNode deployToDir = new DirectoryNode("Deployment Directory", targetDir);
                    //proceedWithCopy = (deployToDir.Exists ? true : (createTargetDirectoryIfNotExisting ? deployToDir.Create() : false));
                    #region if (this.Exists)

                    if (!deployToDir.Exists)
                    {
                        if (createTargetDirectoryIfNotExisting)
                        {
                            deployToDir.Create();
                        }
                        else // If the destination directory doesn't exist and we're not allow to create it, then exit.
                        {
                            proceedWithCopy = false;
                        }
                    }

                    if (proceedWithCopy)
                    {
                        this.CopyFiles(sourcePath,
                                            targetDir,
                                            createTargetDirectoryIfNotExisting,
                                            copyMode,
                                            errorLogFullPath,
                                            mergeMode,
                                            deleteContentInTarget,
                                            extensionExclusionList,
                                            directoryNameExclusionListBeginningWith,
                                            directoryNameExclusionListContaining,
                                            directoryNameExclusionListEndingWith
                                            );

                        this.CopyDirectories(sourcePath,
                                            targetDir,
                                            destinationRootPath,
                                            createTargetDirectoryIfNotExisting,
                                            copyMode,
                                            errorLogFullPath,
                                            mergeMode,
                                            deleteContentInTarget,
                                            archiveTarget,
                                            extensionExclusionList,
                                            directoryNameExclusionListBeginningWith,
                                            directoryNameExclusionListContaining,
                                            directoryNameExclusionListEndingWith

                                            );
                    }
                    else
                    {
                        result = false;
                    }
                    #endregion if (this.Exists)
                }
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
                result = false;
            }
            return result;
        }
        private void CopyDirectories(string sourcePath, 
                                        string destinationPath, 
                                        string destinationRootPath,
                                        bool createDestinationDirectoryIfNotExisting, 
                                        CopyMode copyMode, 
                                        string errorLogFullPath, 
                                        MergeMode mergeMode, 
                                        bool deleteContentInTarget,
                                        bool    archiveTarget, 
                                        List<string> extensionExclusionList                     = null,
                                        List<string> directoryNameExclusionListBeginningWith    = null,
                                        List<string> directoryNameExclusionListContaining       = null,
                                        List<string> directoryNameExclusionListEndingWith       = null
                                    )
        {
            bool result = true;
            foreach (string drs in Directory.GetDirectories(sourcePath))
            {
                if (copyMode == CopyMode.Recursive)
                {
                    createDestinationDirectoryIfNotExisting = true;
                }
                else
                {
                    createDestinationDirectoryIfNotExisting = false;
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(drs);
                if (CopyFolderContents_old(drs,
                                            Path.Combine(destinationPath, directoryInfo.Name).ToString(),
                                            destinationRootPath,
                                            createDestinationDirectoryIfNotExisting,
                                            copyMode,
                                            errorLogFullPath,
                                            mergeMode,
                                            deleteContentInTarget,
                                            false/*isRoot*/,
                                            archiveTarget) == false)
                {
                    result = false;
                }
            }

        }
        private void CopyFiles(string sourceDirectoryPath, 
                                string destinationPath, 
                                bool createDestinationDirectoryIfNotExisting, 
                                CopyMode copyMode, 
                                string errorLogFullPath, 
                                MergeMode mergeMode, 
                                bool deleteContentInTarget, 
                                List<string> extensionExclusionList = null,
                                List<string> directoryNameExclusionListBeginningWith = null,
                                List<string> directoryNameExclusionListContaining = null,
                                List<string> directoryNameExclusionListEndingWith = null

                            )
        {
            bool proceedWithCopy = true;
            bool result = true;

            sourceDirectoryPath = sourceDirectoryPath.EndsWith(@"\") ? sourceDirectoryPath : sourceDirectoryPath + @"\";
            destinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";


            if (Directory.Exists(sourceDirectoryPath))
            {
                #region try catch
                try
                {
                    #region if ( Directory.Exists(destinationPath) )
                    if (Directory.Exists(destinationPath)) // If destination directory already exists, which suggests that it may contain files
                    {
                        #region if (mergeMode != MergeMode.NoOverwriteNoMerge)
                        if (mergeMode != MergeMode.NoOverwriteNoMerge)
                        {
                            if (deleteContentInTarget)
                            {
                                #region if ( deleteContentInTarget )
                                try
                                {
                                    bool purgeResult = PurgeDirectoryContent(destinationPath);
                                }
                                catch (Exception ex)
                                {
                                    LastErrorMessage = ex.Message;
                                    result = false;
                                    LogFailedFileCopy(sourceDirectoryPath,
                                                        string.Format("DirDELETE EXCEPTION: Destination directory could not be deleted at path {0}",
                                                                        destinationPath,
                                                                        ex
                                                                      )
                                                      );
                                }
                                #endregion if (deleteContentInTarget)
                            }
                            else
                            {
                                proceedWithCopy = false;
                                LogFailedFileCopy(sourceDirectoryPath, string.Format("DirDELETE PROBLEM:Destination directory could not be purged at path {0}", destinationPath));
                            }
                        }
                        #endregion if (mergeMode != MergeMode.NoOverwriteNoMerge)
                    }
                    else // if destinationDirectory does NOT exist
                    {
                        #region if (createDestinationDirectoryIfNotExisting)
                        if (createDestinationDirectoryIfNotExisting)
                        {
                            try
                            {
                                Directory.CreateDirectory(destinationPath);
                            }
                            catch (Exception ex)
                            {
                                LastErrorMessage = ex.Message;
                                result = false;
                                LogFailedFileCopy(sourceDirectoryPath,
                                                    string.Format("DirCREATE EXCEPTION: Destination directory could not be created at path {0}",
                                                                    destinationPath,
                                                                    ex
                                                                  )
                                                  );
                            }
                        }
                        else
                        {
                            proceedWithCopy = false;
                            LogFailedFileCopy(sourceDirectoryPath, string.Format("DirCREATE PROBLEM: Destination directory could not be created at path {0}", destinationPath));
                        }
                        #endregion if (createDestinationDirectoryIfNotExisting)
                    }
                    #endregion if ( Directory.Exists(destinationPath) )

                    #region if (proceedWithCopy)
                    if (proceedWithCopy)
                    {
                        #region foreach (string file in Directory.GetFiles(sourceDirectoryPath))
                        foreach (string file in Directory.GetFiles(sourceDirectoryPath))
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            if (mergeMode == MergeMode.OverwriteAndMerge)
                            {
                                if (IsThisFileExtensionIncluded(file, extensionExclusionList))
                                {
                                    try
                                    {
                                        fileInfo.CopyTo(string.Format(@"{0}\{1}", destinationPath, fileInfo.Name), true);
                                    }
                                    catch (Exception ex)
                                    {
                                        result = false;
                                        LastErrorMessage = ex.Message;
                                        LogFailedFileCopy(sourceDirectoryPath,
                                                            string.Format("FileCOPY EXCEPTION: Destination directory could not be deleted at path {0}",
                                                                            destinationPath,
                                                                            ex
                                                                          )
                                                          );
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                                LogFailedFileCopy(fileInfo.FullName, "FileCOPY PROBLEM: File Already Exists at Destination. <Overwrite> was NOT set to true.");
                            }
                        }
                        #endregion foreach (string file in Directory.GetFiles(sourceDirectoryPath))
                    }
                    else
                    {
                        result = false;
                        // LogFailedFileCopy(sourceDirectoryPath, "FileCOPY PROBLEM: File Already Exists at Destination. <Overwrite> was NOT set to true.");
                    }
                    #endregion if (proceedWithCopy)
                }
                catch (Exception ex)
                {
                    LastErrorMessage = ex.Message;
                    result = false;
                    LogFailedFileCopy(sourceDirectoryPath,
                                        string.Format("DirFORCE-CREATE EXCEPTION: Destination directory could not be created at path {0}",
                                                        destinationPath,
                                                        ex
                                                      )
                                      );
                }
                #endregion try catch
            }
        }
        public static bool CopyErrorsToLogFile(string filePath, bool append = false)
        {
            bool copyResult = true;

            //errorLogFullPath = filePath;

            //if (!string.IsNullOrEmpty(errorLogFullPath))
            //{
            //    using (StreamWriter sw = new StreamWriter(errorLogFullPath, append))
            //    {
            //        foreach (KeyValuePair<string, IError> item in UncopiedFiles)
            //        {
            //            sw.WriteLine(string.Format("{0} | {1} An Exception | {2}",
            //                                        item.Key,
            //                                        item.Value.IsAnException ? "Is" : "Not",
            //                                        item.Value.Description)
            //                        );
            //        }
            //    }
            //}

            return copyResult;
        }
        private static bool IsThisFileExtensionIncluded(string file, List<string> extensionExclusionList = null)
        {
            return (extensionExclusionList == null) ? true
                : (!extensionExclusionList.Exists(extension => extension == Path.GetExtension(file)));
        }
        private static bool PurgeDirectoryContent(string destinationPath)
        {
            bool purgeResult = true;
            DirectoryInfo dirInfo = new DirectoryInfo(destinationPath);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                    purgeResult = false;
                }
            }

            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch
                {
                    purgeResult = false;
                }
            }


            return purgeResult;
        }
        public bool Create()
        {
            bool directoryCreated = false;
            if (!Directory.Exists(_physicalPath))
            {
                try
                {
                    Directory.CreateDirectory(_physicalPath);
                    directoryCreated = true;
                }
                catch (Exception ex)
                {
                    directoryCreated = false; //Redundant. TODO: Decide on an action
                    LastErrorMessage = ex.Message;
                    LastError = new Error(ex);
                }
            }
            return directoryCreated;
        }
        public bool Delete(bool resursively = true)
        {
            bool deleteResult = false;
            _isLeaf = DeleteSubdirectories();
            _isEmptyOfFiles = DeleteFiles();

            if ( Exists && _isLeaf && _isEmptyOfFiles)
            {
                deleteResult = DeletePhysicalDirectory();
            }

            return deleteResult;
        }
        public bool DeleteSubdirectories()
        {
            bool deleteResult = true;

            List<DirectoryNode> undeletedDirectories = new List<DirectoryNode>();
            for (int i = _subdirectories.Length - 1; i >= 0; i++)
            foreach (DirectoryNode directoryNode in _subdirectories)
            {
                if ( ! _subdirectories[i].Delete())
                {
                        undeletedDirectories.Add(_subdirectories[i]);
                        deleteResult = false;
                }
            }

            _subdirectories = (undeletedDirectories.Count() > 0 ? undeletedDirectories.ToArray() : null);

            if (_isEmptyOfFiles && _subdirectories == null)
            {
                _isEmpty = true;
            }
            return deleteResult;
        }
        public bool DeleteFiles()
        {
            List<FileNode> undeletedFileNodes = new List<FileNode>();
            for (int i = _files.Length - 1; i >= 0; i++)
            {
                if ( !_files[i].Delete())
                {
                    undeletedFileNodes.Add(_files[i]);
                    _isEmptyOfFiles = false;
                }
            }

            _files = (undeletedFileNodes.Count() > 0 ? undeletedFileNodes.ToArray() : null);

            if (_isEmptyOfFiles && _subdirectories == null)
            {
                _isEmpty = true;
            }

            return _isEmptyOfFiles;
        }
        public FileNode[] GetFiles()
        {
            return GetFiles(Directory.GetFiles(_physicalPath));
        }
        public FileNode[] GetFiles(string[] files)
        {
            List<FileNode> nodes = new List<FileNode>();

            foreach (string file in files)
            {
                FileNode node = new FileNode(Path.GetFileName(file), file);
                nodes.Add(node);
            }
            return nodes.ToArray();
        }
        public DirectoryNode[] GetSubDirectories()
        {
            return GetSubDirectories(Directory.GetDirectories(_physicalPath));
        }
        public DirectoryNode[] GetSubDirectories(string[] directories)
        {
            List<DirectoryNode> nodes = new List<DirectoryNode>();

            foreach (string directory in directories)
            {
                DirectoryNode node = new DirectoryNode(Path.GetDirectoryName(directory), directory);
                nodes.Add(node);
            }
            return nodes.ToArray();
        }

        #endregion Public Methods

        #region Static Methods
        #endregion Static Methods
    }
}
