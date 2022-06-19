using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRFZ_Auto.script
{
    
    public class RectColor
    {
        public Rectangle rect;
        /// <summary>
        /// 相对位置
        /// </summary>
        public String FileName;
        /// <summary>
        /// name
        /// </summary>
        public String Name = "NoName";
        public RectColor(Rectangle rect, String FileName, String Name)
        {
            this.rect = rect; this.FileName = FileName;
            this.Name = Name;
        }
        public RectColor() { }
        [System.Web.Script.Serialization.ScriptIgnore]
        protected ImageColor[,] _srcIC;

        [System.Web.Script.Serialization.ScriptIgnore]
        public ImageColor[,] srcIC
        {
            get
            {
                if (_srcIC == null)
                { 
                    _srcIC = ImageColor.FromFile(Environment.CurrentDirectory + @"\imgs\rcimg" + FileName); 
                }
                return _srcIC;
            }
        }
        [System.Web.Script.Serialization.ScriptIgnore]
        public String Display { get { return ToString(); } }
        public override string ToString()
        {
            return $"[point:{rect.X},{rect.Y},size:{rect.Width},{rect.Height},File:{FileName}]";
        }
        public ImageColor[,] GetRegionIC()
        {
            return ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\region_imgs" + FileName);

        }
        public Boolean IsRegionRight(ImageColor[,] src_ic, ImageColor[,] dst_ic, out double AvgDelta)
        {

            AvgDelta = -1;

            try
            {
                var ic = src_ic;
                var rc = this;
                var rcbmp_ic = dst_ic;
                var sum_delt = 0d;
                for (int x = 0; x < rc.rect.Width; x++)
                    for (int y = 0; y < rc.rect.Height; y++)
                    {
                        var ori = ic[x + rc.rect.X, y + rc.rect.Y];
                        var dst = rcbmp_ic[x, y];
                        var dr = dst.R - ori.R;
                        var dg = dst.G - ori.G;
                        var db = dst.B - ori.B;
                        var delt = Math.Sqrt((3 * dr * dr + 4 * dg * dg + 2 * db * db)) / 3d / 255d * 100;
                        sum_delt += delt;
                    }
                AvgDelta = sum_delt / (rc.rect.Width * rc.rect.Height);

                if (AvgDelta > mrfz_ScriptConfig.scriptConfig.dlt_region)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
            }

        }

    }

    /// <summary>
    /// PUTCHAR 
    /// </summary>
    public class CharPoint
    {
        public String name;
        public Point Point;
        public override string ToString()
        {
            return $"{name}:[point:{Point.X},{Point.Y},]";
        }
        /// <summary>
        /// 放置的角色类型
        /// </summary>
       // public RoleType role_type = RoleType.治疗;
        /// <summary>
        /// 放置时的方向
        /// </summary>
        public Dir DIR_PUT = Dir.左;

        public enum Dir
        {
            上,
            下,
            左,
            右
        }


        public enum RoleType
        {
            /// <summary>
            /// 未指定
            /// </summary>
            NON,
            /// <summary>
            /// H1-N
            /// </summary>
            治疗,
            /// <summary>
            /// J1-N
            /// </summary>
            近卫,
            /// <summary>
            /// S1-N
            /// </summary>
            辅助,
            /// <summary>
            /// M1-N
            /// </summary>
            召唤师,
            /// <summary>
            /// SUL1-N
            /// </summary>
            地面召唤物,
            /// <summary>
            /// SUH1-N
            /// </summary>
            高台召唤物
        }
    }
}
