using System.Windows;

namespace hyperJLO
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetProg();
        }

        JoyconManager joyconManager = new JoyconManager();
        bool progState = false;
        int num = 1;

        private void SetProg()
        {
            joyconManager.Scan();
            if (joyconManager.j.Count > 0)
            {
                joyconManager.Start();
                progState = true;
                MesBlock.Text = "ジョイコンが見つかり処理が正常に行われました。";
                done.Content = "終了する";
            }
            else
            {
                progState = false;
                MesBlock.Text = "ジョイコンが見つかりませんでした。(" + num + ")";
                done.Content = "接続を再確認";
            }
        }

        private void done_Click(object sender, RoutedEventArgs e)
        {
            if (progState)
            {
                joyconManager.OnApplicationQuit();
                Close();
            }
            else
            {
                this.IsEnabled = false;
                num++;
                SetProg();
                this.IsEnabled = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            joyconManager.OnApplicationQuit();
        }
    }
}
