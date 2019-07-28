using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FinancialTracker.Model;
using FinancialTracker.UI.Data.Lookups;
using FinancialTracker.UI.Data.Repositories;
using FinancialTracker.UI.Event;
using FinancialTracker.UI.View.Services;
using FinancialTracker.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FinancialTracker.UI.ViewModel
{
    public class PaymentDetailViewModel : DetailViewModelBase, IPaymentDetailViewModel
    {
        private IPaymentRepository _paymentRepository;
        private IRecipientLookupDataService _recipientLookupDataService;
        private IPaymentPurposeLookupDataService _paymentPurposeLookupDataService;
        private PaymentWrapper _payment;

        public PaymentWrapper Payment
        {
            get { return _payment; }
            private set
            {
                _payment = value; 
                OnPropertyChanged();
            }
        }

        public string CurrentPaymentPurpose
        {
            get { return PaymentPurposes.SingleOrDefault(p => p.ID == Payment.PaymentPurposeID).DisplayMember; }
        }

        public string CurrentRecipient
        {
            get { return Recipients.SingleOrDefault(r => r.ID == Payment.RecipientID).DisplayMember; }
        }

        public ObservableCollection<LookupItem> Recipients { get; }

        public ObservableCollection<LookupItem> PaymentPurposes { get; }

        public PaymentDetailViewModel(IPaymentRepository paymentRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IRecipientLookupDataService recipientLookupDataService,
            IPaymentPurposeLookupDataService paymentPurposeLookupDataService)
            :base(eventAggregator, messageDialogService)
        {
            _paymentRepository = paymentRepository;
            _recipientLookupDataService = recipientLookupDataService;
            _paymentPurposeLookupDataService = paymentPurposeLookupDataService;
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            Recipients = new ObservableCollection<LookupItem>();
            PaymentPurposes = new ObservableCollection<LookupItem>();
        }

        public override async Task LoadAsync(int paymentID)
        {
            Payment payment = paymentID > 0
                ? await _paymentRepository.GetByIDAsync(paymentID)
                : CreateNewPayment();

            ID = paymentID;

            InitializePayment(payment);

            await LoadRecipientsLookupAsync();
            await LoadPaymentPurposesLookupAsync();
        }

        private void InitializePayment(Payment payment)
        {
            Payment = new PaymentWrapper(payment);
            Payment.PropertyChanged += (s, e) =>
            {
                if (HasChanges == false)
                {
                    HasChanges = _paymentRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Payment.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof(Payment.Date))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Payment.ID == 0) //if a new payment was created
            {
                Payment.PaymentPurposeID = null; //little trick to set off the validation
                Payment.RecipientID = null; //little trick to set off the validation
            }

            SetTitle();
        }

        private async Task LoadRecipientsLookupAsync()
        {
            int? selectedRecipientId = Payment.RecipientID;
            Recipients.Clear();
            Recipients.Add(new NullLookupItem{DisplayMember = " - "});
            var lookup = await _recipientLookupDataService.GetRecipientLookupAsync();
            foreach (var item in lookup)
            {
                Recipients.Add(item);
            }

            Payment.RecipientID = selectedRecipientId;
        }

        private async Task LoadPaymentPurposesLookupAsync()
        {
            int? selectedPaymentPurposetId = Payment.PaymentPurposeID;
            PaymentPurposes.Clear();
            PaymentPurposes.Add(new NullLookupItem{DisplayMember = " - "});
            var lookup = await _paymentPurposeLookupDataService.GetPaymentPurposeLookupAsync();
            foreach (var item in lookup)
            {
                PaymentPurposes.Add(item);
            }

            Payment.PaymentPurposeID = selectedPaymentPurposetId;
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_paymentRepository.SaveAsync, () =>
            {
                ID = Payment.ID;
                HasChanges = _paymentRepository.HasChanges();
                RaiseDetailSavedEvent(Payment.ID, Payment.Date.ToShortDateString() + " " + CurrentPaymentPurpose);
            });
        }

        protected override bool OnSaveCanExecute()
        {
            bool canExecute = Payment != null && Payment.HasErrors == false && HasChanges;
            return canExecute;
        }

        private Payment CreateNewPayment()
        {
            var payment = new Payment
            {
                Date = DateTime.Today
            };
            _paymentRepository.Add(payment);
            return payment;
        }

        protected override async void OnDeleteExecute()
        {
            var result = await MessageDialogService.ShowOkCancelDialogAsync(
                $"Do you really want to delete the payment of {Payment.AmountInDollars} dollars to {CurrentRecipient}",
                "Question");
            if (result == MessageDialogResult.Ok)
            {
                _paymentRepository.Remove(Payment.Model);
                await _paymentRepository.SaveAsync();
                RaiseDetailDeletedEvent(Payment.ID);
            }
        }

        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(PaymentPurposeDetailViewModel))
            {
                await LoadPaymentPurposesLookupAsync();
            }

            if (args.ViewModelName == nameof(RecipientDetailViewModel))
            {
                await LoadRecipientsLookupAsync();
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(PaymentPurposeDetailViewModel))
            {
                PaymentPurposes.Remove(PaymentPurposes.Where(p => p.ID == args.ID).FirstOrDefault());
            }

            if (args.ViewModelName == nameof(RecipientDetailViewModel))
            {
                Recipients.Remove(Recipients.Where(r => r.ID == args.ID).FirstOrDefault());
            }
        }

        private void SetTitle()
        {
            Title = Payment.Date.ToShortDateString();
        }
    }
}
