using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xapp.Models
{


    public class DirModel
    {
        public string DirName { get; set; }
        public string DirPath { get; set; }
        public DateTime DirAccessed { get; set; }
    }
    public class FileModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public MvcHtmlString FileComment { get; set; }
        public string FileSizeText { get; set; }
        public DateTime FileAccessed { get; set; }
    }
    public class ExplorerModel
    {
        public List<DirModel> dirModelList;
        public List<FileModel> fileModelList;
        public string Currentdirectory { get; set; }
        public string Parentpath { get; set; }
        public ExplorerModel(List<DirModel> _dirModelList, List<FileModel> _fileModelList)
        {
            dirModelList = _dirModelList;
            fileModelList = _fileModelList;
        }
    }


}