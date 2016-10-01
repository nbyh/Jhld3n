using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EquipmentInformationData;
using ICSharpCode.SharpZipLib.Zip;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;

namespace AnonManagementSystem
{
    public class DataHandle
    {
        public delegate void StatusSet(string info);
        public event StatusSet SetStatusInfo;
        public bool isManual { get; set; }
        public string Dirpath { get; set; }

        public DataHandle()
        {
            TaskScheduler.UnobservedTaskException += (sender, excArgs) =>
            {
                excArgs.SetObserved();
                throw excArgs.Exception;
            };
        }

        private void TasksEnded(Task<bool>[] tasks)
        {
            string[] r = tasks.Select(s => s.Result.ToString()).ToArray();
            CommonLogHelper.GetInstance("LogInfo").Info($"完成合并数据任务，结果为：{string.Join("，", r)}");
            SetStatusInfo?.Invoke("合并数据完成");
            if (isManual)
            {
                MessageBox.Show($"完成合并数据任务！结果为：{string.Join("，", r)}", @"信息", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        public void ImportData()
        {
            CommonLogHelper.GetInstance("LogInfo").Info("开始合并数据任务");
            TaskFactory taskFactory = new TaskFactory();
            Task<bool>[] tasks = {
                    taskFactory.StartNew(EquipManagemetDbCombine),
                    taskFactory.StartNew(SparePartDbCombine),
                    taskFactory.StartNew(EquipImagesDbCombine),
                    taskFactory.StartNew(VehiclesImagesDbCombine),
                    taskFactory.StartNew(OilEngineImagesDbCombine),
                    taskFactory.StartNew(EventsImageDbCombine),
                    taskFactory.StartNew(SparePartImageDbCombine)
                };
            taskFactory.ContinueWhenAll(tasks, TasksEnded, CancellationToken.None);
        }

        private bool SparePartImageDbCombine()
        {
            try
            {
                if (File.Exists(Dirpath + @"\SparePartImages.db"))
                {
                    SparePartImagesDB dbs = new SparePartImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(AppDomain.CurrentDomain.BaseDirectory, @"ZBDataBase\SparePartImages.db"));
                    SparePartImagesDB dbd = new SparePartImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(Dirpath, "SparePartImages.db"));
                    dbs.BulkCopy(dbd.SparePartImages.Select(s => s));
                }
                else
                {
                    CommonLogHelper.GetInstance("LogInfo").Info("文件" + Dirpath + @"\SparePartImages.db不存在");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"合并SparePartImages失败", e);
                return false;
            }
        }

        private bool EventsImageDbCombine()
        {
            try
            {
                if (File.Exists(Dirpath + @"\EventsImages.db"))
                {
                    EventsImagesDB dbs = new EventsImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(AppDomain.CurrentDomain.BaseDirectory, @"ZBDataBase\EventsImages.db"));
                    EventsImagesDB dbd = new EventsImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(Dirpath, "EventsImages.db"));
                    dbs.BulkCopy(dbd.EventsImages.Select(s => s));
                }
                else
                {
                    CommonLogHelper.GetInstance("LogInfo").Info("文件" + Dirpath + @"\EventsImages.db不存在");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"合并EventsImages失败", e);
                return false;
            }
        }

        private bool OilEngineImagesDbCombine()
        {
            try
            {
                if (File.Exists(Dirpath + @"\OilEngineImages.db"))
                {
                    OilEngineImagesDB dbs = new OilEngineImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(AppDomain.CurrentDomain.BaseDirectory, @"ZBDataBase\OilEngineImages.db"));
                    OilEngineImagesDB dbd = new OilEngineImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(Dirpath, "OilEngineImages.db"));
                    dbs.BulkCopy(dbd.OilEngineImages.Select(s => s));
                }
                else
                {
                    CommonLogHelper.GetInstance("LogInfo").Info("文件" + Dirpath + @"\OilEngineImages.db不存在");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"合并OilEngineImages失败", e);
                return false;
            }
        }

        private bool VehiclesImagesDbCombine()
        {
            try
            {
                if (File.Exists(Dirpath + @"\VehiclesImages.db"))
                {
                    VehiclesImagesDB dbs = new VehiclesImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(AppDomain.CurrentDomain.BaseDirectory, @"ZBDataBase\VehiclesImages.db"));
                    VehiclesImagesDB dbd = new VehiclesImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(Dirpath, "VehiclesImages.db"));
                    dbs.BulkCopy(dbd.VehiclesImages.Select(s => s));
                }
                else
                {
                    CommonLogHelper.GetInstance("LogInfo").Info("文件" + Dirpath + @"\VehiclesImages.db不存在");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"合并VehiclesImages失败", e);
                return false;
            }
        }

        private bool EquipImagesDbCombine()
        {
            try
            {
                if (File.Exists(Dirpath + @"\EquipmentImages.db"))
                {
                    EquipmentImagesDB dbs = new EquipmentImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(AppDomain.CurrentDomain.BaseDirectory, @"ZBDataBase\EquipmentImages.db"));
                    EquipmentImagesDB dbd = new EquipmentImagesDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(Dirpath, "EquipmentImages.db"));
                    dbs.BulkCopy(dbd.EquipmentImages.Select(s => s));
                }
                else
                {
                    CommonLogHelper.GetInstance("LogInfo").Info("文件" + Dirpath + @"\EquipmentImages.db不存在");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"合并EquipmentImages失败", e);
                return false;

            }
        }

        private bool SparePartDbCombine()
        {
            try
            {
                if (File.Exists(Dirpath + @"\SparePartManagement.db"))
                {
                    SparePartManagementDB dbs = new SparePartManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(AppDomain.CurrentDomain.BaseDirectory, @"ZBDataBase\SparePartManagement.db"));
                    SparePartManagementDB dbd = new SparePartManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(Dirpath, "SparePartManagement.db"));
                    dbs.BulkCopy(dbd.SpareParts.Select(s => s));
                }
                else
                {
                    CommonLogHelper.GetInstance("LogInfo").Info("文件" + Dirpath + @"\SparePartManagement.db不存在");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"合并SparePartManagement失败", e);
                return false;
            }
        }

        private bool EquipManagemetDbCombine()
        {
            try
            {
                if (File.Exists(Dirpath + @"\EquipmentManagement.db"))
                {
                    EquipmentManagementDB dbs = new EquipmentManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(AppDomain.CurrentDomain.BaseDirectory, @"ZBDataBase\EquipmentManagement.db"));
                    EquipmentManagementDB dbd = new EquipmentManagementDB(new SQLiteDataProvider(), DbPublicFunction.ReturnDbConnectionString(Dirpath, "EquipmentManagement.db"));
                    dbs.BulkCopy(dbd.CombatEquipments.Select(s => s));
                    dbs.BulkCopy(dbd.CombatVehicles.Select(s => s));
                    dbs.BulkCopy(dbd.OilEngines.Select(s => s));
                    dbs.BulkCopy(dbd.Events.Select(s => s));
                    dbs.BulkCopy(dbd.EventData.Select(s => s));
                    dbs.BulkCopy(dbd.Materials.Select(s => s));
                }
                else
                {
                    CommonLogHelper.GetInstance("LogInfo").Info("文件" + Dirpath + @"\EquipmentManagement.db不存在");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                CommonLogHelper.GetInstance("LogError").Error(@"合并EquipmentManagement失败", e);
                return false;
            }
        }

        public bool BackupData(string sourcepath)
        {
            bool result = false;
            string backupZip = Dirpath.TrimEnd('\\') + @"\ZBDataBase" + DateTime.Now.ToString("yyyyMMdd") + ".zip";
            Task ziptask = new Task(() =>
            {
                DirectoryFileZip dirfileZip = new DirectoryFileZip(new[] { "ZBDataBase" });
                result = dirfileZip.ZipFileDirectory(sourcepath, backupZip, string.Empty);
            }, TaskCreationOptions.AttachedToParent);
            ziptask.Start();
            ziptask.Wait();
            return result;
        }
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
