using Miniblink.LoadResourceImpl;
using MiniBlink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ooo.LoadResourceHandlerList.Add(new EmbedLoader(typeof(MainWindow).Assembly, "wwwroot", "myurl.locl"));
            
            //JS方法绑定对象
            ooo.GlobalObjectJs = this;
            
            //加载
            ooo.LoadUri("http://myurl.locl/index.html");

            //禁用跨域
            ooo.SetCspCheckEnable(false);
        }

        [JSFunctin]
        public void Console_WriteLine(string msg)
        {
            MessageBox.Show("Console_WriteLine被调用了，参数 ：" + msg);
        }
    }
}
