using System.Drawing;

namespace AnonManagementSystem
{
    public interface IAddModify
    {
        bool Add { get; set; }
        bool Enableedit { set; }
        string Id { set; }
        int Index { get; set; }
    }

    public interface IMdiFunction
    {
        void DataAdd();

        void DataDelete();

        void DataRefresh();

        void ExportAll2Excel();

        void ExportOne2Excel();
    }

    public interface IShowImageList
    {
        void AddImages(string key, Image img);

        void DeleteImages();

        void ShowImages();
    }
}