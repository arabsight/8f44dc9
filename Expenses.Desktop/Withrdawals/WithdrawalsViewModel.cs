using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Xpf.Printing;
using Expenses.Core;
using Expenses.Logic;
using Expenses.Reports;
using Expenses.UI.Common;

namespace Expenses.UI.Withrdawals
{
    public class WithdrawalsViewModel : EntitiesViewModel<Withdrawal>
    {
        private readonly WithdrawalService _withdrawals;

        public WithdrawalsViewModel()
        {
            _withdrawals = WithdrawalService.Instance;
            Messenger.Default.Register<EntityMessage<Withdrawal>>(this, OnMessage);
        }

        public override string Title => "Alimentation de caisse";

        // For Reports
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime FinishDate { get; set; }

        public override void OnNavigatedTo()
        {
            StartDate = Session.Exercise.StartDate;
            FinishDate = DateTime.Today;
            LoadEntities();
        }

        private void LoadEntities()
        {
            Entities = _withdrawals.GetByExercise(Session.Exercise.Id);
        }

        public void New()
        {
            ShowDocument();
        }

        public void Edit(Withdrawal entity)
        {
            ShowDocument(entity);
        }

        private void ShowDocument(Withdrawal entity = null)
        {
            var vm = WithdrawalViewModel.Instance(entity);
            var doc = DocumentManagerService.FindDocument(vm);
            if (doc == null)
            {
                doc = DocumentManagerService.CreateDocument("WithdrawalView", vm);
                doc.Id = DocumentManagerService.Documents.Count();
            }
            doc.Show();
        }

        public bool CanEdit(Withdrawal entity)
        {
            return entity != null && entity.CreatedBy == Session.Identity.Id;
        }

        public void Delete(Withdrawal entity)
        {
            var result = MessageBoxService.Show(
                "êtes-vous sûr de vouloir supprimer cet enregistrement?",
                "Attention",
                MessageButton.YesNo,
                MessageIcon.Warning,
                MessageResult.No
                );

            if (result != MessageResult.Yes) return;

            try
            {
                _withdrawals.Delete(entity);
                Entities.Remove(entity);
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message, "Erreur",
                    MessageButton.OK, MessageIcon.Error, MessageResult.OK);
            }
        }

        public bool CanDelete(Withdrawal entity)
        {
            return entity != null && entity.CreatedBy == Session.Identity.Id;
        }

        public void Refresh()
        {
            LoadEntities();
        }

        private void OnMessage(EntityMessage<Withdrawal> message)
        {
            switch (message.Type)
            {
                case MessageType.Added:
                    var added = _withdrawals.Find(message.Entity.Id);
                    Entities.Add(added);
                    break;
                case MessageType.Modified:
                    _withdrawals.Reload(SelectedEntity);
                    break;
                case MessageType.Deleted:
                    break;
            }
        }

        public void ShowMonthlyReport()
        {
            ShowReport(Entities);
        }

        private void ShowReport(IEnumerable<Withdrawal> source)
        {
            using (var report = new MonthlyWithdrawalsReport {DataSource = source})
            {
                using (var model = new XtraReportPreviewModel(report))
                {
                    var window = new DocumentPreviewWindow {Model = model};
                    report.CreateDocument(true);
                    window.ShowDialog();
                }
            }
        }

        public void ShowCustomReport()
        {
            var source = Entities.Where(e => e.Date >= StartDate && e.Date <= FinishDate).ToList();
            ShowReport(source);
        }

        public bool CanShowCustomReport()
        {
            return StartDate <= FinishDate;
        }
    }
}