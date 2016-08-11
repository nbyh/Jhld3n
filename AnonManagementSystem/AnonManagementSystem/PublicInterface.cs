using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AnonManagementSystem
{
    public interface IMdiFunction
    {
        void DataAdd();
        void DataDelete();
        void DataRefresh();
    }

    public interface IAddModify
    {
        bool Enableedit { set; }
        int Index { get; set; }
        string Id { set; }
        bool Add { get; set; }
    }

    public interface IShowImageList
    {
        Dictionary<string, Image> ImgDictionary { get; set; }
        void ShowImages();
    }
}
