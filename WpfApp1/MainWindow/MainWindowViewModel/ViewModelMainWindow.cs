using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViewEmpl;
using ViewEmpl.ViewModel;


namespace WpfApp1
{
    class ViewModelMainWindow 
    {
        
        public void ShowEmplView()
        {
            ViewModelEmpl vm = new ViewModelEmpl() { };
            EmplView view = new EmplView() { DataContext = vm };
            ViewShower.Show(view, true);
        }
        private RelayCommand viewEmplButton;
        public ICommand ViewEmplButton
        {
            get
            {
                return viewEmplButton ??
                (viewEmplButton = new RelayCommand(obj =>
                {
                    ShowEmplView();
                }));

            }
        }       
    }
}
