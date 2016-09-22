using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ICSharpCode.SharpZipLib.Zip;

namespace DataProcess
{
    public class DataHandle
    {
        public string dirpath { get; set; }
        public DataHandle()
        {
            TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs excArgs) =>
            {
                excArgs.SetObserved();
                throw excArgs.Exception;
            };
        }

        public void ImportData()
        {
            if (File.Exists(dirpath+ @"\EquipmentManagement.db"))
            {
                
            }
            if (File.Exists(dirpath + @"\SparePartManagement.db"))
            {

            }
            if (File.Exists(dirpath + @"\EquipmentImages.db"))
            {

            }
            if (File.Exists(dirpath + @"\VehiclesImages.db"))
            {

            }
            if (File.Exists(dirpath + @"\OilEngineImages.db"))
            {

            }
            if (File.Exists(dirpath + @"\EventsImages.db"))
            {

            }
            if (File.Exists(dirpath + @"\SparePartImages.db"))
            {

            }
        }
        //public bool BackupData()
        //{
        //    bool result = false;
        //    string backupZip = dirpath.TrimEnd('\\') + @"\ZBDataBase" + DateTime.Now.ToString("yyyyMMdd") + ".zip";
        //    if (!Directory.Exists(tempbackupdir))
        //    {
        //        Directory.CreateDirectory(tempbackupdir);
        //        File.SetAttributes(tempbackupdir, FileAttributes.Hidden | FileAttributes.System);
        //    }
        //    Task ziptask = new Task(() =>
        //    {
        //        DirectoryFileZip dirfileZip = new DirectoryFileZip(subdirs);
        //        result = dirfileZip.ZipFileDirectory(tempbackupdir, backupZip, string.Empty);
        //    }, TaskCreationOptions.AttachedToParent);
        //    ziptask.Start();
        //    ziptask.Wait();
        //    return result;
        //}
    }

    public class DirectoryFileZip
    {
        /// <summary>
        /// The subdirs
        /// </summary>
        private readonly string[] _subdirs;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DirectoryFileZip()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="subdirs">The _subdirs.</param>
        public DirectoryFileZip(string[] subdirs)
        {
            _subdirs = subdirs;
        }

        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="parentPath">The parent path.</param>
        /// <returns></returns>
        public bool ZipFileDirectory(string strDirectory, string zipedFile, string parentPath)
        {
            using (FileStream zipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream s = new ZipOutputStream(zipFile))
                {
                    ZipCompress(strDirectory, s, parentPath);
                }
            }
            return true;
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="s">The ZipOutputStream Object.</param>
        /// <param name="parentPath">The parent path.</param>
        private void ZipCompress(string strDirectory, ZipOutputStream s, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            foreach (string file in filenames) //遍历所有的文件和目录
            {
                if (Directory.Exists(file)) //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    DirectoryInfo subdirinfo = new DirectoryInfo(file);
                    if (_subdirs.Length == 0)
                    {
                        pPath += subdirinfo.Name;
                        pPath += "\\";
                        ZipCompress(file, s, pPath);
                    }
                    else
                    {
                        bool zipfilemark = false;
                        foreach (string sub in _subdirs)
                        {
                            if (subdirinfo.FullName.Contains(sub))
                            {
                                zipfilemark = true;
                                pPath += subdirinfo.Name;
                                pPath += "\\";
                            }
                        }
                        if (zipfilemark)
                        {
                            ZipCompress(file, s, pPath);
                        }
                    }
                }
                else //否则直接压缩文件
                {
                    FileInfo fileinfo = new FileInfo(file);
                    bool zipfilemark = false;
                    if (_subdirs.Length > 0)
                    {
                        foreach (string sub in _subdirs)
                        {
                            if (fileinfo.FullName.Contains(sub))
                            {
                                zipfilemark = true;
                            }
                        }
                    }
                    if (zipfilemark || _subdirs.Length == 0)
                    {
                        using (FileStream streamToZip = new FileStream(file, FileMode.Open, FileAccess.Read))
                        {
                            string fileName = parentPath + fileinfo.Name;
                            ZipEntry entry = new ZipEntry(fileName) { DateTime = fileinfo.LastWriteTime };
                            byte[] buffer = new byte[4096];
                            //Crc32 crc = new Crc32();
                            //crc.Reset();
                            //crc.Update(buffer);
                            //entry.Crc = crc.Value;
                            s.SetLevel(5);
                            s.PutNextEntry(entry);
                            int sizeRead;
                            do
                            {
                                sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                            streamToZip.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="strDirectory">The STR directory.</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        /// <returns></returns>
        public bool UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
            { strDirectory = Directory.GetCurrentDirectory(); }
            if (!strDirectory.EndsWith("\\"))
            { strDirectory = strDirectory + "\\"; }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string pathToZip = theEntry.Name;

                    if (pathToZip != "")
                    {
                        var directoryName = Path.GetDirectoryName(pathToZip);
                        Directory.CreateDirectory(strDirectory + directoryName);

                        bool existfile = (File.Exists(strDirectory + pathToZip) && overWrite);
                        if (existfile || (!File.Exists(strDirectory + pathToZip)))
                        {
                            if (existfile)
                            {
                                File.SetAttributes(strDirectory + pathToZip, FileAttributes.Normal);
                            }
                            using (FileStream streamWriter = File.Create(strDirectory + pathToZip))
                            {
                                byte[] data = new byte[4096];
                                while (true)
                                {
                                    int size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    { streamWriter.Write(data, 0, size); }
                                    else
                                    { break; }
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                s.Close();
            }
            return true;
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="unzipDir">The unzip dir.</param>
        /// <param name="strDirectory">The STR directory.</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        /// <returns></returns>
        public bool UnZip(string zipedFile, string unzipDir, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
            { strDirectory = Directory.GetCurrentDirectory(); }
            if (!strDirectory.EndsWith("\\"))
            { strDirectory = strDirectory + "\\"; }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string pathToZip = theEntry.Name;

                    if (pathToZip != "" && pathToZip.Contains(unzipDir))
                    {
                        var directoryName = Path.GetDirectoryName(pathToZip);
                        Directory.CreateDirectory(strDirectory + directoryName);

                        bool existfile = (File.Exists(strDirectory + pathToZip) && overWrite);
                        if (existfile || (!File.Exists(strDirectory + pathToZip)))
                        {
                            if (existfile)
                            {
                                File.SetAttributes(strDirectory + pathToZip, FileAttributes.Normal);
                            }
                            using (FileStream streamWriter = File.Create(strDirectory + pathToZip))
                            {
                                byte[] data = new byte[4096];
                                while (true)
                                {
                                    int size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    { streamWriter.Write(data, 0, size); }
                                    else
                                    { break; }
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                s.Close();
            }
            return true;
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="unzipDirs">The unzip dirs.</param>
        /// <param name="strDirectory">The STR directory.</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        /// <returns></returns>
        public bool UnZip(string zipedFile, string[] unzipDirs, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
            { strDirectory = Directory.GetCurrentDirectory(); }
            if (!strDirectory.EndsWith("\\"))
            { strDirectory = strDirectory + "\\"; }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string pathToZip = theEntry.Name;

                    foreach (string unzipdir in unzipDirs)
                    {
                        if (pathToZip != "" && pathToZip.Contains(unzipdir))
                        {
                            var directoryName = Path.GetDirectoryName(pathToZip);
                            Directory.CreateDirectory(strDirectory + directoryName);

                            bool existfile = (File.Exists(strDirectory + pathToZip) && overWrite);
                            if (existfile || (!File.Exists(strDirectory + pathToZip)))
                            {
                                if (existfile)
                                {
                                    File.SetAttributes(strDirectory + pathToZip, FileAttributes.Normal);
                                }
                                using (FileStream streamWriter = File.Create(strDirectory + pathToZip))
                                {
                                    byte[] data = new byte[4096];
                                    while (true)
                                    {
                                        int size = s.Read(data, 0, data.Length);
                                        if (size > 0)
                                        { streamWriter.Write(data, 0, size); }
                                        else
                                        { break; }
                                    }
                                    streamWriter.Close();
                                }
                            }
                        }
                    }
                }
                s.Close();
            }
            return true;
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        /// <exception cref="System.IO.FileNotFoundException">指定要压缩的文件:  + fileToZip +  不存在!</exception>
        public void ZipFiles(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream zipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream zipStream = new ZipOutputStream(zipFile))
                {
                    using (FileStream streamToZip = new FileStream(fileToZip, FileMode.Open, FileAccess.Read))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\", StringComparison.Ordinal) + 1);

                        ZipEntry zipEntry = new ZipEntry(fileName);

                        zipStream.PutNextEntry(zipEntry);

                        zipStream.SetLevel(compressionLevel);

                        byte[] buffer = new byte[blockSize];

                        int sizeRead;
                        do
                        {
                            sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, sizeRead);
                        }
                        while (sizeRead > 0);

                        streamToZip.Close();
                    }

                    zipStream.Finish();
                    zipStream.Close();
                }
                zipFile.Close();
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名</param>
        /// <exception cref="System.IO.FileNotFoundException">指定要压缩的文件:  + fileToZip +  不存在!</exception>
        public void ZipFiles(string fileToZip, string zipedFile)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream fs = File.OpenRead(fileToZip))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                using (FileStream zipFile = File.Create(zipedFile))
                {
                    using (ZipOutputStream zipStream = new ZipOutputStream(zipFile))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                        ZipEntry zipEntry = new ZipEntry(fileName);
                        zipStream.PutNextEntry(zipEntry);
                        zipStream.SetLevel(5);

                        zipStream.Write(buffer, 0, buffer.Length);
                        zipStream.Finish();
                        zipStream.Close();
                    }
                }
            }
        }
    }
}
