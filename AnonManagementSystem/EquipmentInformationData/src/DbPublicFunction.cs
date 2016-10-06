using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EquipmentInformationData
{
    public class DbPublicFunction
    {
        public static IEnumerable<CombatEquipment> CompareTimeResult(IEnumerable<CombatEquipment> q, string proName, string express, DateTime dt)
        {
            switch (express)
            {
                case ">":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) > dt);

                case "<":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) < dt);

                case "≥":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) >= dt);

                case "≤":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) <= dt);

                default:
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) == dt);
            }
        }

        public static IEnumerable<Event> CompareTimeResult(IEnumerable<Event> q, string proName, string express, DateTime dt)
        {
            switch (express)
            {
                case ">":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) > dt);

                case "<":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) < dt);

                case "≥":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) >= dt);

                case "≤":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) <= dt);

                default:
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) == dt);
            }
        }

        public static IEnumerable<SparePart> CompareTimeResult(IEnumerable<SparePart> q, string proName, string express, DateTime dt)
        {
            switch (express)
            {
                case ">":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) > dt);

                case "<":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) < dt);

                case "≥":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) >= dt);

                case "≤":
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) <= dt);

                default:
                    return q.Where(s => (DateTime)(s.GetType().GetProperty(proName).GetValue(s, null)) == dt);
            }
        }

        public static string ReturnDbConnectionString(string dbfile)
        {
            return $@"data source={AppDomain.CurrentDomain.BaseDirectory}{dbfile};";
        }

        public static string ReturnDbConnectionString(string path, string dbfile)
        {
            string dbpath = Path.Combine(path, dbfile);
            return $"data source={dbpath};";
        }
    }
}