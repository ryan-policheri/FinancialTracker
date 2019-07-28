using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FinancialTracker.Model;
using FinancialTracker.UI.Data.Repositories;
using FinancialTracker.UI.Event;
using FinancialTracker.UI.View;
using FinancialTracker.UI.View.Services;
using FinancialTracker.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace FinancialTracker.UI.ViewModel
{
    public class RecipientDetailViewModel : DetailViewModelBase, IRecipientDetailViewModel
    {
        private IRecipientRepository _recipientRepository;
        private RecipientWrapper _recipient;
        private LookupItem _selectedPayment;

        public RecipientDetailViewModel(IEventAggregator eventAggregator, 
            IMessageDialogService messageDialogService,
            IRecipientRepository recipientRepository) : base(eventAggregator, messageDialogService)
        {
            _recipientRepository = recipientRepository;
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            AddPaymentCommand = new DelegateCommand(OnAddPaymentExecute);
            ViewPaymentCommand = new DelegateCommand(OnViewPaymentExecute, OnViewPaymentCanExecute);

            PaymentLookup = new ObservableCollection<LookupItem>();
        }

        public RecipientWrapper Recipient
        {
            get { return _recipient; }
            private set
            {
                _recipient = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LookupItem> PaymentLookup { get; set; }

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

        public ICommand AddPaymentCommand { get; set; }

        public ICommand ViewPaymentCommand { get; set; }      

        public override async Task LoadAsync(int recipientID)
        {
            Recipient recipient;
            if (recipientID > 0)
            {
                recipient = await _recipientRepository.GetByIDAsync(recipientID);
                LoadPaymentLookup(recipientID);
            }
            else
            {
                recipient = CreateNewRecipient();
            }

            ID = recipientID;

            InitializeRecipient(recipient);
        }

        private Recipient CreateNewRecipient()
        {
            var recipient = new Recipient();
            _recipientRepository.Add(recipient);
            return recipient;
        }

        private void InitializeRecipient(Recipient recipient)
        {
            Recipient = new RecipientWrapper(recipient);
            Recipient.PropertyChanged += (sender, args) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _recipientRepository.HasChanges();
                }

                if (args.PropertyName == nameof(Recipient.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if (args.PropertyName == nameof(Recipient.Name))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Recipient.ID == 0) //ID will be for a new recipient
            {
                Recipient.Name = ""; //trick to trigger validation for a new recipeint
            }

            SetTitle();
        }

        private async void LoadPaymentLookup(int recipientID)
        {
            IEnumerable<LookupItem> payments = await _recipientRepository.GetPaymentLookupByRecipientIDAsync(recipientID);
            PaymentLookup.Clear();
            foreach (LookupItem payment in payments)
            {
                PaymentLookup.Add(payment);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Recipient != null && !Recipient.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_recipientRepository.SaveAsync, () =>
            {
                ID = Recipient.ID;
                HasChanges = _recipientRepository.HasChanges();
                RaiseDetailSavedEvent(Recipient.ID, Recipient.Name);
            });
        }

        protected override async void OnDeleteExecute()
        {
            if (await _recipientRepository.HasPaymentsAsync(Recipient.ID))
            {
                await MessageDialogService.ShowInfoDialogAsync($"{Recipient.Name} cannot be deleted because it has corresponding payments.");
                return;
            }

            MessageDialogResult result =
                await MessageDialogService.ShowOkCancelDialogAsync(
                    $"Do you really want to delete the recipient {Recipient.Name}?", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _recipientRepository.Remove(Recipient.Model);
                await _recipientRepository.SaveAsync();
                RaiseDetailDeletedEvent(Recipient.ID);
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
            if (args.ViewModelName == nameof(PaymentPurposeDetailViewModel))
            {
                LoadPaymentLookup(this.ID);
            }

            if (args.ViewModelName == nameof(PaymentDetailViewModel))
            {
                await _recipientRepository.ReloadPaymentAsync(args.ID);
                await LoadAsync(this.ID);
            }
        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(PaymentPurposeDetailViewModel))
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
            Title = Recipient.Name;
        }
    }
}
