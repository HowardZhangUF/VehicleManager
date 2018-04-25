using GLCore;
using GLStyle;
using System.Windows.Forms;

namespace GLUITest
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();

            StyleManager.LoadStyle("D:\\Style.ini");
            var names = StyleManager.GetStyleNames();

            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},SingleTowardPair,Power,-2000,0,0");
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},SingleArea,ForbiddenArea2,0,0,3000,3000");
            GLCMD.Do($"Add,{GLCMD.SerialNumber.Next()},SingleArea,ForbiddenArea,4000,4000,7000,7000");
        }
    }
}