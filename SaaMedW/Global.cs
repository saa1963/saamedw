﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Management;
using Atol.Drivers10.Fptr;

namespace SaaMedW
{
    public sealed class Global
    {
        private Global() { }

        private static readonly Lazy<Global> lazy =
            new Lazy<Global>(() => new Global());

        public static Global Source { get { return lazy.Value; } }

        public Users RUser { get; set; }

        internal void SaveColumnsWidth(UserControl uc)
        {
            var fName = uc.Name;
            DataGrid g;
            g = (DataGrid)FindInVisualTreeDown(uc, "DataGrid");
            if (g == null)
            {
                g = (DataGrid)FindInVisualTreeDown(uc, "ScrollingDataGrid");
            }
            foreach (DataGridColumn col in g.Columns)
            {
                if (col.Width.IsAbsolute)
                    Global.Source.SaveParam<double>(fName + "_" + col.DisplayIndex.ToString(), col.Width.Value);
            }
        }

        internal void SetColumnsWidth(UserControl uc)
        {
            var fName = uc.Name;
            DataGrid g;
            g = (DataGrid)FindInVisualTreeDown(uc, "DataGrid");
            if (g == null)
            {
                g = (DataGrid)FindInVisualTreeDown(uc, "ScrollingDataGrid");
            }
            foreach (DataGridColumn col in g.Columns)
            {
                if (col.Width.IsAbsolute)
                {
                    var rt = Global.Source.GetParam<double>(fName + "_" + col.DisplayIndex.ToString());
                    if (rt != 0) col.Width = rt;
                }
            }
        }

        public void SaveParam<T>(string name, T value)
        {
            var pth = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SaaMedW");
            var fname = Path.Combine(pth, typeof(T).Name + "Params.bin");
            Dictionary<string, T> d = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (!Directory.Exists(pth))
            {
                Directory.CreateDirectory(pth);
            }
            if (!File.Exists(fname))
            {
                d = new Dictionary<string, T>();
            }
            else
            {
                try
                {
                    using (FileStream fs = new FileStream(fname, FileMode.Open))
                    {
                        d = (Dictionary<string, T>)formatter.Deserialize(fs);
                    }
                }
                catch
                {
                    File.Delete(fname);
                    d = new Dictionary<string, T>();
                }
            }
            d[name] = value;
            using (FileStream fs = new FileStream(fname, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, d);
            }
        }

        public T GetParam<T>(string name) where T : new()
        {
            var pth = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SaaMedW");
            var fname = Path.Combine(pth, typeof(T).Name + "Params.bin");
            Dictionary<string, T> d = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (!Directory.Exists(pth))
            {
                Directory.CreateDirectory(pth);
            }
            if (!File.Exists(fname))
            {
                return default(T);
            }
            else
            {
                try
                {
                    using (FileStream fs = new FileStream(fname, FileMode.Open))
                    {
                        d = (Dictionary<string, T>)formatter.Deserialize(fs);
                    }
                    return d[name];
                }
                catch
                {
                    File.Delete(fname);
                    return default(T);
                }
            }
        }

        public string GetTempFilename(string ext)
        {
            var tempName0 = Path.GetTempFileName();
            var tempName = Path.Combine(Path.GetDirectoryName(tempName0), Path.GetFileNameWithoutExtension(tempName0) + ext);
            File.Delete(tempName0);
            return tempName;
        }

        public string GetNameOfMonth(int month)
        {
            string[] mths =
                { "января", "февраля", "марта", "апреля", "мая", "июня", "июля",
                    "августа", "сентября", "октября", "ноября", "декабря" };
            return mths[month - 1];
        }

        public string GetNameOfMonth0(int month)
        {
            string[] mths =
                { "январь", "февраль", "март", "апрель", "май", "июнь", "июль",
                    "август", "сентябрь", "октябрь", "ноябрь", "декабрь" };
            return mths[month - 1];
        }

        public DependencyObject FindInVisualTreeDown(DependencyObject obj, string type)
        {
            if (obj != null)
            {
                if (obj.GetType().Name == type)
                {
                    return obj;
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject childReturn = FindInVisualTreeDown(VisualTreeHelper.GetChild(obj, i), type);
                    if (childReturn != null)
                    {
                        return childReturn;
                    }
                }
            }
            return null;
        }

        public List<IdName> ListRole { get; set; } =
            new List<IdName>()
            {
                new IdName { Id = 0, Name="Администратор"},
                new IdName { Id = 1, Name="Пользователь"}
            };

        public string GetMotherboardId()
        {
            string mbInfo = String.Empty;

            //Get motherboard's serial number 
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            foreach (ManagementObject mo in mbs.Get())
            {
                mbInfo += mo["SerialNumber"].ToString();
            }
            return mbInfo;
        }
    }
}
