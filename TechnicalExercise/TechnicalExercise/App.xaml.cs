using System.Configuration;
using System.Data;
using System.Windows;
using TechnicalExercise.Services;
using TechnicalExercise.ViewModels;
using TechnicalExercise.Views;
using Unity;

namespace TechnicalExercise
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _container = new UnityContainer();
            _container.RegisterType<IAirportDatabase, AirportDatabase>();
            _container.RegisterType<MainViewModel>();

            var mainWindow = new MainWindow
            {
                DataContext = _container.Resolve<MainViewModel>()
            };

            mainWindow.Show();

            //_container.RegisterType<IAirportDatabase, AirportDatabase>();

            //var mainWindow = _container.Resolve<MainWindow>();
            //mainWindow.DataContext = _container.Resolve<MainViewModel>();
            //mainWindow.Show();
        }
    }
}
