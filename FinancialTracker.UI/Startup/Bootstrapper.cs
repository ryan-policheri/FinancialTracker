using Autofac;
using FinancialTracker.DataAccess;
using FinancialTracker.UI.Data.Lookups;
using FinancialTracker.UI.Data.Repositories;
using FinancialTracker.UI.View.Services;
using FinancialTracker.UI.ViewModel;
using Prism.Events;

namespace FinancialTracker.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<FinancialTrackerDBContext>().AsSelf();

            builder.RegisterType<PaymentRepository>().As<IPaymentRepository>();
            builder.RegisterType<RecipientRepository>().As<IRecipientRepository>();
            builder.RegisterType<PaymentPurposeRepository>().As<IPaymentPurposeRepository>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<PaymentNavigationViewModel>().As<IPaymentNavigationViewModel>();
            builder.RegisterType<RecipientNavigationViewModel>().As<IRecipientNavigationViewModel>();
            builder.RegisterType<PaymentPurposeNavigationViewModel>().As<IPaymentPurposeNavigationViewModel>();
            builder.RegisterType<PaymentDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(PaymentDetailViewModel));
            builder.RegisterType<RecipientDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(RecipientDetailViewModel));
            builder.RegisterType<PaymentPurposeDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(PaymentPurposeDetailViewModel));

            return builder.Build();
        }
    }
}
