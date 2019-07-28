using FinancialTracker.Model;
using FinancialTracker.UI.Data.Repositories;
using FinancialTracker.UI.Event;
using FinancialTracker.UI.View.Services;
using FinancialTracker.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinancialTracker.UI.ViewModel
{
    public class PaymentPurposeDetailViewModel : DetailViewModelBase, IPaymentPurposeDetailViewModel
    {
        private IPaymentPurposeRepository _paymentPurposeRepository;
        private PaymentPurposeWrapper _paymentPurpose;
        private LookupItem _selectedPayment;

        public PaymentPurposeDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IPaymentPurposeRepository paymentPurposeRepository) : base(eventAggregator, messageDialogService)
        {
            _paymentPurposeRepository = paymentPurposeRepository;
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            AddPaymentCommand = new DelegateCommand(OnAddPaymentExecute);
            ViewPaymentCommand = new DelegateCommand(OnViewPaymentExecute, OnViewPaymentCanExecute);

            PaymentLookup = new ObservableCollection<LookupItem>();
        }

        public PaymentPurposeWrapper PaymentPurpose
        {
            get { return _paymentPurpose; }
            private set
            {
                _paymentPurpose = value;
                OnPropertyChanged();
            }
        }

        public LookupItem SelectedPayment
        {
            get { return _selectedPayment; }
            set
            {
                if (value != _selectedPayment)
                {
                    _selectedPayment = value;
                    OnPropertyChanged();
                    ((DelegateCommand)ViewPaymentCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<LookupItem> PaymentLookup { get; }

        public ICommand AddPaymentCommand { get; set; }

        public ICommand ViewPaymentCommand { get; set; }

        public override async Task LoadAsync(int paymentPurposeID)
        {
            PaymentPurpose paymentPurpose;
            if (paymentPurposeID > 0)
            {
                paymentPurpose = await _paymentPurposeRepository.GetByIDAsync(paymentPurposeID);
                LoadPaymentLookup(paymentPurposeID);
            }
            else
            {
                paymentPurpose = CreateNewPaymentPurpose();
            }

            ID = paymentPurposeID;

            InitializePaymentPurpose(paymentPurpose);
        }

        private PaymentPurpose CreateNewPaymentPurpose()
        {
            var paymentPurpose = new PaymentPurpose();
            _paymentPurposeRepository.Add(paymentPurpose);
            return paymentPurpose;
        }

        private void InitializePaymentPurpose(PaymentPurpose paymentPurpose)
        {
            PaymentPurpose = new PaymentPurposeWrapper(paymentPurpose);
            PaymentPurpose.PropertyChanged += (sender, args) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _paymentPurposeRepository.HasChanges();
                }

                if (args.PropertyName == nameof(PaymentPurpose.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if (args.PropertyName == nameof(PaymentPurpose.Purpose))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (PaymentPurpose.ID == 0) //ID will be for a new payment purpose
            {
                PaymentPurpose.Purpose = ""; //trick to trigger validation for a new payment purpose
            }

            SetTitle();
        }

        protected override bool OnSaveCanExecute()
        {
            return PaymentPurpose != null && !PaymentPurpose.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_paymentPurposeRepository.SaveAsync, () =>
            {
                ID = PaymentPurpose.ID;
                HasChanges = _paymentPurposeRepository.HasChanges();
                RaiseDetailSavedEvent(PaymentPurpose.ID, PaymentPurpose.Purpose);
            });
        }

        protected override async void OnDeleteExecute()
        {
            if (await _paymentPurposeRepository.HasPaymentsAsync(PaymentPurpose.ID))
            {
                await MessageDialogService.ShowInfoDialogAsync($"{PaymentPurpose.Purpose} cannot be deleted because it has corresponding payments.");
                return;
            }

            MessageDialogResult result =
                await MessageDialogService.ShowOkCancelDialogAsync(
                    $"Do you really want to delete the payment purpose {PaymentPurpose.Purpose}?", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _paymentPurposeRepository.Remove(PaymentPurpose.Model);
                await _paymentPurposeRepository.SaveAsync();
                RaiseDetailDeletedEvent(PaymentPurpose.ID);
            }
        }

        private async void LoadPaymentLookup(int paymentPurposeID)
        {
            IEnumerable<LookupItem> payments = await _paymentPurposeRepository.GetPaymentLookupByPaymentPurposeIDAsync(paymentPurposeID);
            PaymentLookup.Clear();
            foreach (LookupItem payment in payments)
            {
                PaymentLookup.Add(payment);
            }
        }

        private void OnAddPaymentExecute()
        {
            EventAggregator.GetEvent<OpenDetailViewEvent>()
                .Publish(new OpenDetailViewEventArgs
                {
                    ViewModelName = nameof(PaymentDetailViewModel)
                });
        }

        private void OnViewPaymentExecute()
        {
            if (SelectedPayment != null)
            {
                EventAggregator.GetEvent<OpenDetailViewEvent>()
                    .Publish(new OpenDetailViewEventArgs
                    {
                        ID = SelectedPayment.ID,
                        ViewModelName = nameof(PaymentDetailViewModel)
                    });
            }
        }

        private bool OnViewPaymentCanExecute()
        {
            return SelectedPayment != null;
        }

        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(RecipientDetailViewModel))
            {
                LoadPaymentLookup(this.ID);
            }

            if (args.ViewModelName == nameof(PaymentDetailViewModel))
            {
                await _paymentPurposeRepository.ReloadPaymentAsync(args.ID);
                await LoadAsync(this.ID);
            }
        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(RecipientDetailViewModel))
            {
                LoadPaymentLookup(this.ID);
            }

            if (args.ViewModelName == nameof(PaymentDetailViewModel))
            {
                await LoadAsync(this.ID);
            }
        }

        private void SetTitle()
        {
            Title = PaymentPurpose.Purpose;
        }
    }
}
