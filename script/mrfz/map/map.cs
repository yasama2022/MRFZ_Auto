using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRFZ_Auto.script.mrfz.map
{
    public class map
    {
        public List<String> ImgFiles = new List<string>();
        public String Name = "";
        public static Size ImgSize = new Size(1280,720);
        public map(List<String> ImgFiles)
        {
            this.ImgFiles = ImgFiles;
        }
        public map() { }
    }
}
