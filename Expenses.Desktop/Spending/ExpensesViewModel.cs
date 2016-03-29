using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Xpf.Printing;
using Expenses.Core;
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
                    Entities.Add(added);
                    break;
                case MessageType.Modified:
                    _expenses.Reload(SelectedEntity);
                    if (SelectedEntity.Category == null)
                        _expenses.LoadReference(SelectedEntity, e => e.Category);
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

        public bool CanEdit(Expense entity)
        {
            return entity != null && entity.CreatedBy == Session.Identity.Id;
        }

        private void ShowDocument(Expense entity = null)
        {
            var vm = ExpenseViewModel.Instance(entity);
            var doc = DocumentManagerService.FindDocument(vm);
            if (doc == null)
            {
                doc = DocumentManagerService.CreateDocument("ExpenseView", vm);
                doc.Id = DocumentManagerService.Documents.Count();
            }
            doc.Show();
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

        public bool CanDelete(Expense entity)
        {
            return entity != null && entity.CreatedBy == Session.Identity.Id;
        }

        public void Refresh()
        {
            LoadEntities();
        }

        // Reports
        public void ShowMonthlyReport()
        {
            ShowReport(Entities);
        }

        private static void ShowReport(IEnumerable<Expense> entities)
        {
            using (var report = new MonthlyExpensesReport {DataSource = entities})
            {
                using (var model = new XtraReportPreviewModel(report))
                {
                    var window = new DocumentPreviewWindow {Model = model};
                    report.CreateDocument(true);
                    window.ShowDialog();
                }
            }
        }
    }
}