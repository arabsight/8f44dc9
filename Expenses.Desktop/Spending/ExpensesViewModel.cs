using System;
using DevExpress.Xpf.Printing;
using Expenses.Core;
using Expenses.Logic;
using Expenses.Reports;
using Expenses.UI.Common;
using DevExpress.Mvvm.Native;

namespace Expenses.UI.Spending
{
    public class ExpensesViewModel : EntitiesViewModel<Expense>
    {
        public ExpensesViewModel() : base(ExpenseService.Instance) {}

        public override string Title => "Dépenses";

        protected override string EntityViewName => "ExpenseView";

        public override void OnNavigatedTo()
        {
            LoadEntities();
        }

        protected override object GetEntityViewModel(Expense entity) => ExpenseViewModel.Instance(entity);

        protected override void LoadEntities()
        {
            Entities = Service.Get(e => e.ExerciseId == Session.Exercise.Id);
        }

        protected override void OnMessage(EntityMessage<Expense> message)
        {
            switch (message.Type)
            {
                case MessageType.Added:
                    var added = Service.Find(message.Entity.Id);

                    if (added.Category == null)
                        Service.LoadReference(added, e => e.Category);

                    if (added.ReceiptType == null)
                        Service.LoadReference(added, e => e.ReceiptType);

                    Entities.Add(added);
                    break;
                case MessageType.Modified:
                    Service.Reload(SelectedEntity);

                    if (SelectedEntity.Category == null)
                        Service.LoadReference(SelectedEntity, e => e.Category);

                    if (SelectedEntity.ReceiptType == null)
                        Service.LoadReference(SelectedEntity, e => e.ReceiptType);
                    break;
                case MessageType.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override bool CanNew() => !Session.Exercise.IsClosed;

        public override bool CanEdit(Expense entity)
        {
            return base.CanEdit(entity) && !Session.Exercise.IsClosed;
        }

        public override bool CanDelete(Expense entity)
        {
            return base.CanDelete(entity) && !Session.Exercise.IsClosed;
        }

        // Reports
        public void ShowMonthlyReport()
        {
            var entities = ((ExpenseService)Service).GetMonthlyTotalsByCategory(Session.Exercise);
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
            using (var report = new MonthlyExpensesReport {DataSource = Entities})
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