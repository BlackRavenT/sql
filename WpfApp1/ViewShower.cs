using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    class ViewShower
    {
        public static void Show(Control p_view, bool p_isModal)//, Action<bool?> p_closeAction)
        {
            if (p_view != null)
            {
                Window wnd = new Window();
                wnd.SizeToContent = SizeToContent.WidthAndHeight;
                wnd = (Window)p_view;
                /*StackPanel sp = new StackPanel();
                sp.Children.Add(p_view);
                /*Button applyButton = new Button();
                applyButton.Content = "Принять";
                applyButton.Click += (s, e) => { if (p_isModal) wnd.DialogResult = true; else wnd.Close(); };
                /*StackPanel buttonPanel = new StackPanel();
                buttonPanel.Orientation = Orientation.Horizontal;
                buttonPanel.Children.Add(applyButton);
                sp.Children.Add(buttonPanel);
                wnd.Content = sp;
                wnd.Closed += (s, e) => p_closeAction(wnd.DialogResult);*/
                if (p_isModal)
                {
                    Button cancelButton = new Button();
                    cancelButton.Content = "Отмена";
                    cancelButton.Click += (s, e) => wnd.DialogResult = false;
                    //buttonPanel.Children.Add(cancelButton);
                    wnd.ShowDialog();
                }
                else
                {
                    wnd.Show();
                }
            }
        }
    }
}
