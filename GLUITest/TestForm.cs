using Geometry;
using GLCore;
using GLStyle;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GLUITest
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();

            StyleManager.LoadStyle("D:\\Style.ini");

            // 使用者用指令加入圖形(可復原)
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},SingleTowardPair,Power,-2000,0,0");
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},SingleArea,ForbiddenArea2,0,0,3000,3000");
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},SingleArea,ForbiddenArea,4000,4000,7000,7000");

            // 使用者用函式加入圖形(可復原)
            GLCMD.DoAddSingleTowardPair("Goal", 0, 2000, 45);

            // 使用者用函式加入複合圖形(不可復原)
            Random random = new Random();
            List<IPair> list = new List<IPair>();
            for (int ii = 0; ii < 100000; ++ii)
            {
                list.Add(new Pair(random.Next(-10000,10000), random.Next(-10000, 10000)));
            }
            GLCMD.AddMultiPair("ObstaclePoints", list);
        }
    }
}