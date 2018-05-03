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
        }

        private void cmbSelectType_SelectedValueChanged(object sender, EventArgs e)
        {
            // 綁資料
            switch ((sender as ComboBox).Text)
            {
                case "Pair":
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.SinglePairInfo, null);
                        dgvSingleTowerPairInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case "TowardPair":
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.SingleTowerPairInfo, null);
                        dgvSingleTowerPairInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case "Line":
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.SingleLineInfo, null);
                        dgvSingleTowerPairInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                case "Area":
                    {
                        BindingSource singleTowerPairInfoSource = new BindingSource(GLCMD.SingleAreaInfo, null);
                        dgvSingleTowerPairInfo.DataSource = singleTowerPairInfoSource;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 命令範例
        /// </summary>
        private void CMDDemo()
        {
            // 使用者用指令加入圖形(可復原)
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},ChargingDocking,-2000,0,0");
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},ForbiddenArea2,0,0,3000,3000");
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},ForbiddenArea,4000,4000,7000,7000");

            // 使用者用函式加入圖形(可復原)
            GLCMD.DoAddSingleTowardPair("Goal", 0, 2000, 45);

            // 使用者用函式加入複合圖形(不可復原)
            Random random = new Random();
            List<IPair> list = new List<IPair>();
            for (int ii = 0; ii < 100000; ++ii)
            {
                list.Add(new Pair(random.Next(-10000, 10000), random.Next(-10000, 10000)));
            }
            GLUI.ObstaclePointsID = GLCMD.AddMultiPair("@ObstaclePoints", list);
        }
    }
}