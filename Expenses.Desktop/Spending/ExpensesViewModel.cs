using System;
using System.Collections.Generic;
using DevExpress.Mvvm;
using DevExpress.Xpf.Printing;
using Expenses.Core;
using Expenses.Core.Shared;
using Expenses.Logic;
using Expenses.Reports;
using Expenses.UI.Common;

namespace Expenses.UI.Spending
{
    public class ExpensesViewModel : EntitiesViewModel<Expense>
    {
        private readonly ExpenseService _expenses;

        public ExpensesViewModel()
        {
            _expenses = ExpenseService.Instance;
            Messenger.Default.Register<EntityMessage<Expense>>(this, OnMessage);
        }

        public override string Title => "Dépenses";

        public override void OnNavigatedTo()
        {
            LoadEntities();
        }

        private void LoadEntities()
        {
            Entities = _expenses.GetByExercise(Session.Exercise.Id);
        }

        private void OnMessage(EntityMessage<Expense> message)
        {
            switch (message.Type)
            {
                case MessageType.Added:
                    var added = _expenses.Find(message.Entity.Id);

                    if (added.Category == null)
                        _expenses.LoadReference(added, e => e.Category);

                    if (added.ReceiptType == null)
                        _expenses.LoadReference(added, e => e.ReceiptType);

                    Entities.Add(added);
                    break;
                case MessageType.Modified:
                    _expenses.Reload(SelectedEntity);

                    if (SelectedEntity.Category == null)
                        _expenses.LoadReference(SelectedEntity, e => e.Category);

                    if (SelectedEntity.ReceiptType == null)
                        _expenses.LoadReference(SelectedEntity, e => e.ReceiptType);
                    break;
                case MessageType.Deleted:
                    break;
            }
        }

        public void New()
        {
            ShowDocument();
        }

        public void Edit(Expense entity)
        {
            ShowDocument(entity);
        }

        public bool CanNew()
        {
            return !Session.Exercise.IsClosed;
        }

        public bool CanEdit(Expense entity)
        {
            return AllowEdit(entity);
        }

        public bool CanDelete(Expense entity)
        {
            return AllowEdit(entity);
        }

        private bool AllowEdit(ITrackable entity)
        {
            return entity != null
                && !Session.Exercise.IsClosed
                && entity.CreatedBy == Session.Identity.Id;
        }

        public void Delete(Expense entity)
        {
            var result = MessageBoxService.Show(
                "êtes-vous sûr de vouloir supprimer cette dépense?",
                "Attention",
                MessageButton.YesNo,
                MessageIcon.Warning,
                MessageResult.No);

            if (result != MessageResult.Yes) return;

            try
            {
                _expenses.Delete(entity);
                Entities.Remove(entity);
            }
            catch (Exception e)
            {
                MessageBoxService.Show(
                    e.Message,
                    "Erreur",
                    MessageButton.OK,
                    MessageIcon.Error,
                    MessageResult.OK);
            }
        }

        public void Refresh()
        {
            LoadEntities();
        }

        private void ShowDocument(Expense entity = null)
        {
            var vm = ExpenseViewModel.Instance(entity);
            var doc = DocumentManagerService.CreateDocument("ExpenseView", vm);
            doc.DestroyOnClose = true;
            doc.Show();
        }

        // Reports
        public void ShowMonthlyReport()
        {
            var entities = _expenses.GetMonthlyTotalsByCategory(Session.Exercise);
            var withrawalsTotal = WithdrawalService.Instance.GetTotalByExercise(Session.Exercise);
            entities.ForEach(e => e.Withdrawals = withrawalsTotal);

            using (var report = new MonthlyStateReport { DataSource = entities })
            {
                using (var model = new XtraReportPreviewModel(report))
                {
                    var window = new DocumentPreviewWindow { Model = model };
                    report.CreateDocument(true);
                    window.ShowDialog();
                }
            }
        }

        public void ShowDetailedReport()
        {
            using (var report = new MonthlyExpensesReport { DataSource = Entities })
            {
                using (var model = new XtraReportPreviewModel(report))
                {
                    var window = new DocumentPreviewWindow { Model = model };
                    report.CreateDocument(true);
                    window.ShowDialog();
                }
            }
        }
    }
}