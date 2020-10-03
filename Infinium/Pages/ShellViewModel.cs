using Infinium.Pages.Events;
using Infinium.Pages;
using Stylet;
using System.Windows;

namespace Company.WpfApplication1.Pages
{
    public class ShellViewModel : Conductor<IScreen>,
        IHandle<OnClick_LoginButton>
    {
        private IEventAggregator _eventAggregator;
        private LoginViewModel _loginViewModel;
        public ShellViewModel(IEventAggregator eventAggregator, LoginViewModel loginViewModel)
        {
            _eventAggregator = eventAggregator;
            _loginViewModel = loginViewModel;
            ActiveItem = loginViewModel;
        }

        public void Handle(OnClick_LoginButton message)
        {
            //ActiveItem = HomeViewModel;
        }
    }
}
