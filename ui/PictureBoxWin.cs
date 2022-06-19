using lib.image;
using script;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GamePageScript.ui.GamePageCreator;

namespace MRFZ_Auto.ui
{
    public partial class PictureBoxWin : Form
    {
      
        Bitmap CurBitmap; 
        public PictureBoxWin(Bitmap bitmap, state State)
        {
            InitializeComponent();
            this.cvs.BackgroundImage = bitmap;
            this.State = State;
            this.CurBitmap = bitmap; 
            this.SetStyle(ControlStyles.DoubleBuffer |

         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint,
         true);
            this.UpdateStyles();

            cvs.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).
                SetValue(cvs, true, null);
            cvs.MouseDown += PictureBox1_MouseDown;
            cvs.MouseMove += PictureBox1_MouseMove;
            cvs.MouseUp += PictureBox1_MouseUp;
        }
        public class Result
        {
            public PP pp;
            public Point p;
            public Rectangle rect;

        }
        public Result result;
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        { 
            if (State == state.RecPoint || State == state.ClickToNext)
            {

                var P = e.Location;
                switch (State)
                {
                    case state.RecPoint:
                        var col = CurBitmap.GetPixel(P.X, P.Y); 
                        PP pp = new PP() { color = new ImageColor(col), loc = P };
                        result = new Result() { pp = pp };
                        DialogResult = DialogResult.OK;
                        this.Close(); 

                        break;
                    case state.ClickToNext:

                        result = new Result() { p= P };
                        DialogResult = DialogResult.OK;
                        this.Close(); 
                        break;
                } 
            }
            else
            {
                if (State == state.RecRect||
                    State== state.RegionBlock)
                {
                    // CurBitmap
                    ed = new Point(e.Location.X, e.Location.Y);
                    Rect = GetRect(st, ed);
                    result = new Result() { rect=Rect };
                    DialogResult = DialogResult.OK;
                    this.Close(); 
                    this.State = state.None;  
                }
            }

        }
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        { 
            if (State == state.RecPoint || State == state.ClickToNext)
            { 
            }
            else
            {
                if (State == state.RecRect||
                    State== state.RegionBlock)
                {
                    DownKey = true;
                    st = new Point(e.Location.X, e.Location.Y);
                    ed = new Point(e.Location.X, e.Location.Y);
                    Rect = GetRect(st, ed);
                    DrawCVS();
                    // CurBitmap
                    //Bitmap bmp = pictureBox1.BackgroundImage as Bitmap;
                    //var P = e.Location;
                }
            }
        }
        private Rectangle GetRect(Point p1, Point p2)
        {
            int x = 0, y = 0, w = 0, h = 0;
            x = Math.Min(p1.X, p2.X);
            y = Math.Min(p1.Y, p2.Y);
            w = Math.Abs(p1.X - p2.X);
            h = Math.Abs(p1.Y - p2.Y);
            return new Rectangle(x, y, w, h);
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        { 
            if (State == state.RecPoint || State == state.ClickToNext)
            {

            }
            else
            {
                if ((State == state.RecRect 
                    
                    ||State== state.RegionBlock)&& DownKey)
                {
                    ed = new Point(e.Location.X, e.Location.Y);
                    Rect = GetRect(st, ed);
                    DrawCVS();
                }
            }
        }
       
        state State = state.None;
        private Rectangle Rect;

        public bool DownKey { get; private set; } = false;

        public Point st;
        public Point ed;

        public void DrawCVS()
        {


            Bitmap bmp = new Bitmap(1280, 720);
            var g = Graphics.FromImage(bmp);
            g.DrawImage(CurBitmap, 0, 0);
            if (State == state.RecRect
                ||State== state.RegionBlock)
            {
                g.DrawRectangle(Pens.White, Rect);
            }
            cvs.BackgroundImageLayout = ImageLayout.Stretch;
            cvs.BackgroundImage = bmp;
            // g.Dispose();
        }
    }
}
