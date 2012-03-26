using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KBProject.Common;

namespace W3Validity
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    delegate void callback();
    ValidityParser[] validators;
    public MainWindow()
    {
      InitializeComponent();
      textBox1.Text = Options.Get("Url");
    }

    private void button1_Click(object sender, RoutedEventArgs e)
    {
      button1.IsEnabled = false;
      Log.Info("MainProgram", "Started collecting data");
      Options.Set("Url", textBox1.Text);
      (new ValidityCollector(textBox1.Text)).Collect(Finished, UpdateProgress);
    }


    void Finished(ValidityParser[] validators)
    {
      this.validators = validators;
      this.Dispatcher.Invoke((callback)ShowValidators, null);
    }

    void ShowValidators()
    {
     button1.IsEnabled = true;
     stackPanel1.Children.Clear();
     foreach (ValidityParser val in validators)
       stackPanel1.Children.Add(val.Exp);
    }

    void UpdateProgress(int max, int current)
    {
      this.Dispatcher.Invoke((callback)delegate
      {
        progressBar1.Maximum = max;
        progressBar1.Value = current;
      }, null);
    }

    protected override void OnClosed(EventArgs e)
    {
      //Options.Save();
      base.OnClosed(e);
    }
  }
}
