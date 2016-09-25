using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EquipmentInformationData
{
    public class DbPublicFunction
    {
        public static string ReturnDbConnectionString(string path, string dbfile)
        {
            string dbpath = Path.Combine(path, dbfile);
            return $"data source={dbpath}";
        }
    }
}
