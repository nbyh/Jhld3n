using System.Collections.Generic;
using System.Drawing;

namespace AnonManagementSystem
{
    public interface IMdiFunction
    {
        void DataAdd();

        void DataDelete();

        void DataRefresh();

        void ExportOne2Excel();

        void ExportAll2Excel();
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
        void ShowImages();

        void AddImages(string key, Image img);

        void DeleteImages();
    }
}