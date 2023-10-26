using LIT.Core.Mvvm;
using LIT.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Windows;

namespace LIT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Login>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<NotificationService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        // Add a method to initialize NotificationService
        private void InitializeNotificationService(Guid userId)
        {
            var notificationService = Container.Resolve<NotificationService>();
            notificationService.InitializeBackgroundTask(userId);
        }

        // Event or method to trigger the initialization after successful login
        public void UserLoggedIn(Guid userId)
        {
            // Assuming you have an event or method that is called after login
            // Call InitializeNotificationService here
            InitializeNotificationService(userId);
        }
    }
}
