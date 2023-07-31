using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgm.VI.IO
{
    public static class IoUtility
    {
        #region Private Variables
        private static int _countOfFilesNotCopied   = 0;
        private static int _countOfFilesCopied      = 0;
        private static Dictionary<string, IError> _uncopiedFiles = new Dictionary<string, IError>(); //TKey is filepath, TValue is error message
        private static string _errorLogFullPath = string.Empty;
        private delegate void WriteToFile(string segment);
        #endregion Private Variables

        #region Private Methods
        private static void IncremendCountOfCopiedFiles()
        {
            _countOfFilesCopied++;
        }
        private static void IncrementCountOfUncopiedFiles ()
        {            _countOfFilesNotCopied++;        }
        #endregion Private Methods

        #region Properties
        public static int CountOfFilesNotCopied
        {
            get
            {
                return _countOfFilesNotCopied;
            }
        }
        public static string errorLogFullPath
        {
            get
            {
                return _errorLogFullPath;
            }
            private set
            {
                if (File.Exists(value) || CreateEmptyFile(value, true))
                {
                    _errorLogFullPath = value;
                }
            }
        }
        public static List<string> FailedFiles { get; set; }
        public static string LastErrorMessage { get; private set; }
        public static Exception LastError { get; private set; }
        public static Dictionary<string, IError> UncopiedFiles
        {
            get
            {
                return _uncopiedFiles;
            }
        }
        #endregion Properties

        #region Public Methods
        public static bool AppendToFile(string filePath, List<string> segments, bool copySegmentsAsLines = false)
        {
            bool copyResult = true;
            bool append = true;

            WriteToFile write = null;

            using (StreamWriter sw = new StreamWriter(filePath, append))
            {
                if (copySegmentsAsLines)
                {
                    write = sw.WriteLine;
                }
                else
                {
                    write = sw.Write;
                }

                foreach (string segment in segments)
                {
                    write(string.Format("{0}", segment));
                }
            }

            return copyResult;
        }
        public static bool AppendToFile(string filePath, string line, bool logErrors = false, string errorLogFullPath = "")
        {
            bool copyResult = true;
            bool append     = true;

            errorLogFullPath = filePath;

            if (logErrors && !string.IsNullOrEmpty(errorLogFullPath))
            {
                using (StreamWriter sw = new StreamWriter(errorLogFullPath, append))
                {
                    foreach (KeyValuePair<string, IError> item in UncopiedFiles)
                    {
                        sw.WriteLine(string.Format("{0} | {1} An Exception | {2}",
                                                    item.Key,
                                                    item.Value.IsAnException ? "Is" : "Not",
                                                    item.Value.Description)
                                    );
                    }
                }
            }

            return copyResult;
        }
        public static bool ArchiveDirectory(string directoryToArchive, string destinationRootPath)
        {
            bool archiveResult = true;
            DirectoryInfo dirInfo = new DirectoryInfo(directoryToArchive);
            try
            {
                string timestamp = DateTime.Now.ToString().Replace(":", "-").Replace("/", "-");
                string archiveDestination = $"{directoryToArchive.Trim('\\')}-{timestamp}";

                CopyFolderContents_old(directoryToArchive,
                                    archiveDestination,
                                    destinationRootPath,
                                    true,
                                    CopyMode.Recursive,
                                    "ArchiveLog.txt",
                                    MergeMode.OverwriteAndMerge,
                                    true, /* deleteContentInTarget */
                                    false, /* isRoot */
                                    true    /* archiveTarget */
                                    );
            }
            catch (Exception ex)
            {
                archiveResult = false;
                LogFailedFileCopy(directoryToArchive,
                                    $"ArchiveDirectory EXCEPTION: Destination directory could not be moved or renamed at path {directoryToArchive}",
                                             ex
                                    );
            }
            return archiveResult;
        }
        public static void ClearLastError()
        {
            LastError = null;
            LastErrorMessage = string.Empty;

        }
        //TODO: Implement MERGE
        public static bool CopyErrorsToLogFile(string filePath, bool append = false)
        {
            bool copyResult = true;

            errorLogFullPath = filePath;

            if (!string.IsNullOrEmpty(errorLogFullPath))
            {
                using (StreamWriter sw = new StreamWriter(errorLogFullPath, append))
                {
                    foreach (KeyValuePair<string, IError> item in UncopiedFiles)
                    {
                        sw.WriteLine(string.Format("{0} | {1} An Exception | {2}",
                                                    item.Key,
                                                    item.Value.IsAnException ? "Is" : "Not",
                                                    item.Value.Description)
                                    );
                    }
                }
            }

            return copyResult;
        }
        public static bool CopyFolderContents_old(  string          sourcePath, 
                                                string          destinationPath,
                                                string          destinationRootPath,
                                                bool            createDestinationDirectoryIfNotExisting,
                                                CopyMode        copyMode,
                                                string          errorLogFullPath,
                                                MergeMode       mergeMode,
                                                bool            deleteContentInTarget,
                                                bool            isRoot,
                                                bool            archiveTarget,
                                                bool            flattenAll                              = false,
                                                bool            numerify                                = false,
                                                bool            preservePathInFilename                  = false,
                                                bool            shortenFilenames                        = false,
                                                List<string>    extensionExclusionList                  = null,
                                                List<string>    directoryNameExclusionListBeginningWith = null,
                                                List<string>    directoryNameExclusionListContaining    = null,
                                                List<string>    directoryNameExclusionListEndingWith    = null
                                             )
        {
            bool proceedWithCopy = true;
            bool result = true;

            if (isRoot)
            {
                destinationRootPath = destinationPath;
            }

            sourcePath = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\";
            destinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";

            bool noExclusionApplies = IsCopyContentAllowed( sourcePath,
                                                            destinationPath,
                                                            extensionExclusionList,
                                                            directoryNameExclusionListBeginningWith,
                                                            directoryNameExclusionListContaining,
                                                            directoryNameExclusionListEndingWith
                                                          );

            if (noExclusionApplies)
            {
                #region Process Try-Catch To Copy Content
                try
                {
                    if (Directory.Exists(sourcePath))
                    {
                        #region if (Directory.Exists(sourcePath))
                        if (archiveTarget && isRoot && Directory.Exists(destinationPath))
                        {
                            ArchiveDirectory(destinationPath, destinationRootPath);
                        }

                        if (!Directory.Exists(destinationPath))
                        {
                            if (createDestinationDirectoryIfNotExisting)
                            {
                                if (isRoot || !flattenAll)
                                {
                                    Directory.CreateDirectory(destinationPath);
                                }
                            }
                            else // If the destination directory doesn't exist and we're not allow to create it, then exit.
                            {
                                proceedWithCopy = false;
                            }
                        }

                        if (proceedWithCopy)
                        {
                            CopyFiles(  sourcePath,
                                        destinationPath,
                                        destinationRootPath,
                                        createDestinationDirectoryIfNotExisting,
                                        copyMode,
                                        errorLogFullPath,
                                        mergeMode,
                                        deleteContentInTarget,
                                        isRoot,
                                        flattenAll,
                                        numerify,
                                        preservePathInFilename,
                                        shortenFilenames,
                                        extensionExclusionList,
                                        directoryNameExclusionListBeginningWith,
                                        directoryNameExclusionListContaining,
                                        directoryNameExclusionListEndingWith
                                        );

                            CopyDirectories(   
                                        sourcePath,
                                        destinationPath,
                                        destinationRootPath,
                                        createDestinationDirectoryIfNotExisting,
                                        copyMode,
                                        errorLogFullPath,
                                        mergeMode,
                                        deleteContentInTarget,
                                        false,
                                        archiveTarget,
                                        flattenAll,
                                        numerify,
                                        preservePathInFilename,
                                        shortenFilenames,
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
                        #endregion if (Directory.Exists(sourcePath))
                    }
                }
                catch (Exception ex)
                {
                    LastErrorMessage = ex.Message;
                    result = false;
                }
                #endregion Process Try-Catch To Copy Content 
            }
            return result;
        }
        //TODO: Implement MERGE
        public static bool CopyFolderContents(  string          sourcePath,
                                                string          destinationPath,
                                                string          destinationRootPath,
                                                bool            createDestinationDirectoryIfNotExisting,
                                                CopyMode        copyMode,
                                                string          errorLogFullPath,
                                                MergeMode       mergeMode,
                                                bool            deleteContentInTarget,
                                                bool            isRoot,
                                                bool            archiveTarget,
                                                bool            flattenAll                              = false,
                                                bool            numerify                                = false,
                                                bool            preservePathInFilename                  = false,
                                                bool            pathTrailsFilename                      = false,
                                                bool            shortenFilenames                        = false,
                                                bool            testMode                                = false,
                                                List<string>    extensionExclusionList                  = null,
                                                List<string>    directoryNameExclusionListBeginningWith = null,
                                                List<string>    directoryNameExclusionListContaining    = null,
                                                List<string>    directoryNameExclusionListEndingWith    = null
                                             )
        {
            bool proceedWithCopy = true;
            bool result = true;

            if (isRoot)
            {
                destinationRootPath = destinationPath;
            }

            sourcePath = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\";
            destinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";

            bool noExclusionApplies = IsCopyContentAllowed(sourcePath,
                                                            destinationPath,
                                                            extensionExclusionList,
                                                            directoryNameExclusionListBeginningWith,
                                                            directoryNameExclusionListContaining,
                                                            directoryNameExclusionListEndingWith
                                                          );

            if (noExclusionApplies)
            {
                #region Process Try-Catch To Copy Content
                try
                {
                    if (Directory.Exists(sourcePath))
                    {
                        #region if (Directory.Exists(sourcePath))
                        if (archiveTarget && isRoot && Directory.Exists(destinationPath))
                        {
                            ArchiveDirectory(destinationPath, destinationRootPath);
                        }

                        if (!Directory.Exists(destinationPath))
                        {
                            #region if (createDestinationDirectoryIfNotExisting)
                            if (createDestinationDirectoryIfNotExisting)
                            {
                                if (isRoot || !flattenAll)
                                {
                                    if (testMode)
                                    {
                                        LogFailedFileCopy(destinationPath, "Running Test Mode : Directory Created");
                                    }
                                    else
                                    {
                                        Directory.CreateDirectory(destinationPath);
                                    }
                                }
                            }
                            else // If the destination directory doesn't exist and we're not allow to create it, then exit.
                            {
                                proceedWithCopy = false;
                            }
                            #endregion if (createDestinationDirectoryIfNotExisting)
                        }

                        if (proceedWithCopy)
                        {
                            CopyFiles(sourcePath,
                                        destinationPath,
                                        destinationRootPath,
                                        createDestinationDirectoryIfNotExisting,
                                        copyMode,
                                        errorLogFullPath,
                                        mergeMode,
                                        deleteContentInTarget,
                                        isRoot,
                                        flattenAll,
                                        numerify,
                                        preservePathInFilename,
                                        pathTrailsFilename,
                                        shortenFilenames,
                                        testMode,
                                        extensionExclusionList,
                                        directoryNameExclusionListBeginningWith,
                                        directoryNameExclusionListContaining,
                                        directoryNameExclusionListEndingWith
                                        );

                            CopyDirectories(
                                        sourcePath,
                                        destinationPath,
                                        destinationRootPath,
                                        createDestinationDirectoryIfNotExisting,
                                        copyMode,
                                        errorLogFullPath,
                                        mergeMode,
                                        deleteContentInTarget,
                                        false,
                                        archiveTarget,
                                        flattenAll,
                                        numerify,
                                        preservePathInFilename,
                                        pathTrailsFilename,
                                        shortenFilenames,
                                        testMode,
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
                        #endregion if (Directory.Exists(sourcePath))
                    }
                }
                catch (Exception ex)
                {
                    LastErrorMessage = ex.Message;
                    result = false;
                }
                #endregion Process Try-Catch To Copy Content 
            }
            return result;
        }
        public static void CopyDirectories(string sourcePath,
                                              string destinationPath,
                                              string destinationRootPath,
                                              bool createDestinationDirectoryIfNotExisting,
                                              CopyMode copyMode,
                                              string errorLogFullPath,
                                              MergeMode mergeMode,
                                              bool deleteContentInTarget,
                                              bool isRoot,
                                              bool archiveTarget,
                                              bool flattenAll = false,
                                              bool numerify = false,
                                              bool preservePathInFilename = false,
                                              bool shortenFilenames = false,
                                              List<string> extensionExclusionList = null,
                                              List<string> directoryNameExclusionListBeginningWith = null,
                                              List<string> directoryNameExclusionListContaining = null,
                                              List<string> directoryNameExclusionListEndingWith = null
                                              )
        {
            bool result = true;
            if (isRoot)
            {
                destinationRootPath = destinationPath;
            }
            foreach (string drs in Directory.GetDirectories(sourcePath))
            {
                createDestinationDirectoryIfNotExisting = (copyMode == CopyMode.Recursive ? true : false);

                DirectoryInfo directoryInfo = new DirectoryInfo(drs);
                if (CopyFolderContents_old(drs,
                                            Path.Combine(destinationPath, directoryInfo.Name).ToString(),
                                            destinationRootPath,
                                            createDestinationDirectoryIfNotExisting,
                                            copyMode,
                                            errorLogFullPath,
                                            mergeMode,
                                            deleteContentInTarget,
                                            isRoot,
                                            archiveTarget,
                                            flattenAll,
                                            numerify,
                                            preservePathInFilename,
                                            shortenFilenames,
                                            extensionExclusionList,
                                            directoryNameExclusionListBeginningWith,
                                            directoryNameExclusionListContaining,
                                            directoryNameExclusionListEndingWith
                                            ) == false)
                {
                    result = false;
                }
            }
        }
        public static void CopyDirectories(string sourcePath,
                                                string destinationPath,
                                                string destinationRootPath,
                                                bool createDestinationDirectoryIfNotExisting,
                                                CopyMode copyMode,
                                                string errorLogFullPath,
                                                MergeMode mergeMode,
                                                bool deleteContentInTarget,
                                                bool isRoot,
                                                bool archiveTarget,
                                                bool flattenAll = false,
                                                bool numerify = false,
                                                bool preservePathInFilename = false,
                                                bool pathTrailsFilename = false,
                                                bool shortenFilenames = false,
                                                bool testMode = false,
                                                List<string> extensionExclusionList = null,
                                                List<string> directoryNameExclusionListBeginningWith = null,
                                                List<string> directoryNameExclusionListContaining = null,
                                                List<string> directoryNameExclusionListEndingWith = null
                                                )
        {
            bool result = true;
            if (isRoot)
            {
                destinationRootPath = destinationPath;
            }
            foreach (string drs in Directory.GetDirectories(sourcePath))
            {
                createDestinationDirectoryIfNotExisting = (copyMode == CopyMode.Recursive ? true : false);

                DirectoryInfo directoryInfo = new DirectoryInfo(drs);
                if (CopyFolderContents(drs,
                                            Path.Combine(destinationPath, directoryInfo.Name).ToString(),
                                            destinationRootPath,
                                            createDestinationDirectoryIfNotExisting,
                                            copyMode,
                                            errorLogFullPath,
                                            mergeMode,
                                            deleteContentInTarget,
                                            isRoot,
                                            archiveTarget,
                                            flattenAll,
                                            numerify,
                                            preservePathInFilename,
                                            pathTrailsFilename,
                                            shortenFilenames,
                                            testMode,
                                            extensionExclusionList,
                                            directoryNameExclusionListBeginningWith,
                                            directoryNameExclusionListContaining,
                                            directoryNameExclusionListEndingWith
                                            ) == false)
                {
                    result = false;
                }
            }
        }
        public static bool CopyFiles(string sourceDirectoryPath,
                                                string destinationPath,
                                                string destinationRootPath,
                                                string errorLogFullPath,
                                                MergeMode mergeMode = MergeMode.MergeButNoOverwrite,
                                                bool isRoot = false,
                                                bool flattenAll = false,
                                                bool numerify = false,
                                                bool preservePathInFilename = false,
                                                bool shortenFilenames = false,
                                                List<string> extensionExclusionList = null,
                                                List<string> directoryNameExclusionListBeginningWith = null,
                                                List<string> directoryNameExclusionListContaining = null,
                                                List<string> directoryNameExclusionListEndingWith = null
                                             )
        {
            if (isRoot)
            {
                destinationRootPath = destinationPath;
            }
            return CopyFiles(sourceDirectoryPath,
                                destinationPath,
                                destinationRootPath,
                                false,
                                CopyMode.ShallowFilesOnly,
                                errorLogFullPath,
                                mergeMode,
                                false,
                                isRoot,
                                flattenAll,
                                numerify,
                                preservePathInFilename,
                                shortenFilenames,
                                extensionExclusionList,
                                directoryNameExclusionListBeginningWith,
                                directoryNameExclusionListContaining,
                                directoryNameExclusionListEndingWith
                                );
        }
        public static bool CopyFiles(string sourceDirectoryPath,
                                                string destinationPath,
                                                string destinationRootPath,
                                                bool createDestinationDirectoryIfNotExisting,
                                                CopyMode copyMode,
                                                string errorLogFullPath,
                                                MergeMode mergeMode,
                                                bool deleteContentInTarget,
                                                bool isRoot = false,
                                                bool flattenAll = false,
                                                bool numerify = false,
                                                bool preservePathInFilename = false,
                                                bool shortenFilenames = false,
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

            if (isRoot)
            {
                destinationRootPath = destinationPath;
            }

            if (Directory.Exists(sourceDirectoryPath))
            {
                #region try catch
                try
                {
                    #region if ( Directory.Exists(destinationPath) )
                    if (Directory.Exists(destinationPath)) // If destination directory already exists, which suggests that it may contain files
                    {
                        #region if (mergeMode != MergeMode.NoOverwriteNoMerge)
                        if (mergeMode != MergeMode.OverwriteAndMerge)
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
                                //    if (mergeMode == MergeMode.NoOverwriteNoMerge)
                                //  proceedWithCopy = false;
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
                            if (flattenAll && !isRoot)
                            {
                                //    proceedWithCopy = false;
                            }
                            else
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
                        }
                        else
                        {
                            proceedWithCopy = false;
                            LogFailedFileCopy(sourceDirectoryPath, string.Format("DirCREATE PROBLEM: Destination directory could not be created at path {0} because createDestinationDirectoryIfNotExisting was set to FALSE", destinationPath));
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
                                        if (flattenAll)
                                        {
                                            string destinationSubDirectoryFlattened = destinationPath.Substring(destinationRootPath.Length);
                                            destinationSubDirectoryFlattened = string.IsNullOrWhiteSpace(destinationSubDirectoryFlattened.Trim())
                                                                                    ? string.Empty
                                                                                    : destinationSubDirectoryFlattened.Replace('\\', '_');
                                            //string newFileName                      = string.IsNullOrEmpty(destinationSubDirectoryFlattened.Trim())
                                            //                                        ? fileInfo.Name
                                            //                                        : string.Format(@"{0}\{1}-{2}",
                                            //                                                        destinationRootPath,
                                            //                                                        destinationSubDirectoryFlattened,
                                            //                                                        fileInfo.Name
                                            //                                                        );
                                            string newFileName = string.Format(@"{0}\{1}{2}",
                                                                                                    destinationRootPath,
                                                                                                    string.IsNullOrWhiteSpace(destinationSubDirectoryFlattened.Trim())
                                                                                                    ? string.Empty
                                                                                                    : destinationSubDirectoryFlattened + "-",
                                                                                                    fileInfo.Name
                                                                                                    );
                                            IncremendCountOfCopiedFiles();
                                            if (numerify)
                                            {
                                                newFileName = _countOfFilesCopied.ToString() + "-" + newFileName;
                                            }
                                            fileInfo.CopyTo(newFileName, true);
                                        }
                                        else
                                        {
                                            IncremendCountOfCopiedFiles();
                                            fileInfo.CopyTo(string.Format(@"{0}\{1}", destinationPath, fileInfo.Name), true);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        result = false;
                                        LastErrorMessage = ex.Message;
                                        LogFailedFileCopy(sourceDirectoryPath,
                                                            string.Format("FileCOPY EXCEPTION: a file could not be copied from path {0}\\{1}",
                                                                            destinationPath, fileInfo.Name,
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
            return result;
        }
        public static bool CopyFiles(string sourceDirectoryPath,
                                                string destinationPath,
                                                string destinationRootPath,
                                                bool createDestinationDirectoryIfNotExisting,
                                                CopyMode copyMode,
                                                string errorLogFullPath,
                                                MergeMode mergeMode,
                                                bool deleteContentInTarget,
                                                bool isRoot = false,
                                                bool flattenAll = false,
                                                bool numerify = false,
                                                bool preservePathInFilename = false,
                                                bool pathTrailsFilename = false,
                                                bool shortenFilenames = false,
                                                bool testMode = false,
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

            if (isRoot)
            {
                destinationRootPath = destinationPath;
            }

            if (Directory.Exists(sourceDirectoryPath))
            {
                #region try catch
                try
                {
                    #region if ( Directory.Exists(destinationPath) )
                    if (Directory.Exists(destinationPath)) // If destination directory already exists, which suggests that it may contain files
                    {
                        #region if (mergeMode != MergeMode.NoOverwriteNoMerge)
                        if (mergeMode != MergeMode.OverwriteAndMerge)
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
                                //    if (mergeMode == MergeMode.NoOverwriteNoMerge)
                                //  proceedWithCopy = false;
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
                            if (flattenAll && !isRoot)
                            {
                                //    proceedWithCopy = false;
                            }
                            else
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
                        }
                        else
                        {
                            proceedWithCopy = false;
                            LogFailedFileCopy(sourceDirectoryPath, string.Format("DirCREATE PROBLEM: Destination directory could not be created at path {0} because createDestinationDirectoryIfNotExisting was set to FALSE", destinationPath));
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
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                            string fileExtension = Path.GetExtension(file);

                            if (true)//mergeMode == MergeMode.OverwriteAndMerge)
                            {
                                #region MergeMode.OverwriteAndMerge
                                if (IsThisFileExtensionIncluded(file, extensionExclusionList))
                                {
                                    #region IsThisFileExtensionIncluded
                                    try
                                    {
                                        if (flattenAll)
                                        {
                                            #region FlattenAll
                                            string destinationSubDirectoryFlattened = destinationPath.Substring(destinationRootPath.Length);
                                            destinationSubDirectoryFlattened = string.IsNullOrWhiteSpace(destinationSubDirectoryFlattened.Trim())
                                                                                    ? string.Empty
                                                                                    : destinationSubDirectoryFlattened.Replace('\\', '_');
                                            //string fileName = preservePathInFilename ?
                                            //                                          (pathTrailsFilename ? string.Format(@"{1}-{0}", destinationRootPath,
                                            //                                                                string.IsNullOrWhiteSpace(destinationSubDirectoryFlattened.Trim()), fileInfo.Name)
                                            //                                                              : string.Format(@"{1}-{1}", destinationRootPath,
                                            //                                                                string.IsNullOrWhiteSpace(destinationSubDirectoryFlattened.Trim()), fileInfo.Name)

                                            //                                           )
                                            //                                         : fileInfo.Name;
                                            string newFileName = pathTrailsFilename ?
                                                                                        string.Format(@"{0}\{1}",
                                                                                                    destinationRootPath,
                                                                                                    string.IsNullOrWhiteSpace(destinationSubDirectoryFlattened.Trim())
                                                                                                    ? fileInfo.Name
                                                                                                    : string.Format(@"{0}-{1}{2}", fileNameWithoutExtension,
                                                                                                                                    destinationSubDirectoryFlattened,
                                                                                                                                    fileExtension)
                                                                                                    )
                                                                                     :
                                                                                        string.Format(@"{0}\{1}",
                                                                                                    destinationRootPath,
                                                                                                    string.IsNullOrWhiteSpace(destinationSubDirectoryFlattened.Trim())
                                                                                                    ? fileInfo.Name
                                                                                                    : string.Format(@"{0}-{1}", destinationSubDirectoryFlattened, fileInfo.Name)
                                                                                                    );
                                            //string newFileName =
                                            //                        string.Format(@"{0}\{1}",
                                            //                                                        destinationRootPath,
                                            //                                                        string.IsNullOrWhiteSpace(destinationSubDirectoryFlattened.Trim())
                                            //                                                        ? fileInfo.Name
                                            //                                                        : string.Format(@"{0}-{1}", destinationSubDirectoryFlattened, fileInfo.Name)
                                            //                                                        );                                                
                                            IncremendCountOfCopiedFiles();
                                            if (numerify)
                                            {
                                                //newFileName = _countOfFilesCopied.ToString() + "-" + newFileName;
                                                newFileName = string.Format(@"{0}\\{1}.{2}", Path.GetDirectoryName(newFileName), _countOfFilesCopied.ToString(), Path.GetFileName(newFileName));
                                            }
                                            if (testMode)
                                            {
                                                LogFailedFileCopy(newFileName, "Running Test Mode : File Copied");
                                            }
                                            else
                                            {
                                                fileInfo.CopyTo(newFileName, true);
                                            }
                                            #endregion FlattenAll
                                        } // if (flattenAll)
                                        else // if ( ! flattenAll) - In other words, if PRESERVE directory structure
                                        {
                                            IncremendCountOfCopiedFiles();
                                            string destinationFile = Path.Combine(destinationPath, fileInfo.Name);
                                            if (!File.Exists(destinationFile)
                                                || (mergeMode == MergeMode.OverwriteAndMerge))
                                            {
                                                fileInfo.CopyTo(string.Format(@"{0}\{1}", destinationPath, fileInfo.Name), true);
                                            }
                                            else
                                            {
                                                LogFailedFileCopy(destinationFile, $"We did not copy {destinationFile}");
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        #region Exception
                                        result = false;
                                        LastErrorMessage = ex.Message;
                                        LogFailedFileCopy(sourceDirectoryPath,
                                                            string.Format("FileCOPY EXCEPTION: a file could not be copied from path {0}\\{1}",
                                                                            destinationPath, fileInfo.Name,
                                                                            ex
                                                                          )
                                                          );
                                        #endregion Exception
                                    }
                                    #endregion IsThisFileExtensionIncluded
                                }
                                #endregion MergeMode.OverwriteAndMerge
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
            return result;
        }
        public static bool CreateEmptyFile(string path, bool createDirectoryIfNotExisting = false)
        {
            bool fileCreated = false;
            if (!File.Exists(path))
            {
                try
                {
                    string directoryPath = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directoryPath) && createDirectoryIfNotExisting)
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    using (File.Create(path)) { } // Relinquishes file automatically after creation.
                    fileCreated = true;
                }
                catch (Exception ex) //TODO: Instead of swallowing the exceptin, do what?
                {
                    fileCreated = false; //Redundant. TODO: Decide on an action
                }
            }
            return fileCreated;
        }
        public static bool IsCopyContentAllowed(string sourcePath, string destinationPath, 
                                                    List<string> extensionExclusionList, 
                                                    List<string> directoryNameExclusionListBeginningWith, 
                                                    List<string> directoryNameExclusionListContaining, 
                                                    List<string> directoryNameExclusionListEndingWith)
        {
            bool isDirectoryCopyAllowed = true;
            sourcePath = sourcePath.ToLower();

            if (isDirectoryCopyAllowed && directoryNameExclusionListBeginningWith != null)
            {
                foreach (string exclusion in directoryNameExclusionListBeginningWith)
                {
                    if (sourcePath.StartsWith(exclusion))
                    {
                        isDirectoryCopyAllowed = false;
                        break;
                    }
                }

            }

            if (isDirectoryCopyAllowed && directoryNameExclusionListContaining != null)
            {
                foreach (string exclusion in directoryNameExclusionListContaining)
                {
                    if (sourcePath.Contains(exclusion))
                    {
                        isDirectoryCopyAllowed = false;
                        break;
                    }
                }

            }

            if (isDirectoryCopyAllowed && directoryNameExclusionListEndingWith != null)
            {
                foreach (string exclusion in directoryNameExclusionListEndingWith)
                {
                    if (sourcePath.EndsWith(exclusion))
                    {
                        isDirectoryCopyAllowed = false;
                        break;
                    }
                }

            }

            return isDirectoryCopyAllowed;
        }
        public static bool IsThisFileExtensionIncluded(string file, List<string> extensionExclusionList = null)
        {
            return (extensionExclusionList == null) ? true
                : (!extensionExclusionList.Exists(extension => extension == Path.GetExtension(file)));
        }
        public static void LogFailedFileCopy(string filePath, string errorMessage, Exception exception = null)
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
        public static void LogFailedFileCopy(string filePath, Exception exception)
        {
            LogFailedFileCopy(filePath, exception.Message, exception);
        }
        public static bool PurgeDirectoryContent(string destinationPath)
        {
            bool purgeResult = true;
            DirectoryInfo dirInfo = new DirectoryInfo(destinationPath);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    purgeResult = false;
                    LogFailedFileCopy(destinationPath,
                                        $"PurgeDirectoryContent::file.Delete({file.Name}) EXCEPTION: Destination directory could not be deleted at path {destinationPath}",
                                                 ex
                                        );
                }
            }

            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch (Exception ex)
                {
                    purgeResult = false;
                    LogFailedFileCopy(destinationPath,
                                        $"PurgeDirectoryContent::dir.Delete({dir.Name}) EXCEPTION: Destination directory could not be deleted at path {destinationPath}",
                                                 ex
                                        );
                }
            }


            return purgeResult;
        }
        public static List<string> ReadLinesFromFile(string filePath, int numOfLines)
        {
            List<string> lines = new List<string>();

            int counter = 0;
            string line;

            // Read the file and display it line by line.
            using (System.IO.StreamReader file = new System.IO.StreamReader(filePath))
            {
                while ((line = file.ReadLine()) != null && counter < numOfLines)
                {
                    lines.Add(line);
                    counter++;
                }
            }
            return lines;
        }
        /// <summary>
        /// TryRun runs any method safely within a try-catch block and returns the exception as a parameter
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static bool TryRun(Action action, out Exception exception)
        {
            bool ranSuccessfully = false;
            exception = null;

            try
            {
                action();
                ranSuccessfully = true;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return ranSuccessfully;
        }
    }
    #endregion Public Methods
}
