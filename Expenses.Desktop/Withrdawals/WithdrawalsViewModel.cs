using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpf.Printing;
using Expenses.Core;
using Expenses.Logic;
using Expenses.Reports;
using Expenses.UI.Common;

namespace Expenses.UI.Withrdawals
{
    public class WithdrawalsViewModel : EntitiesViewModel<Withdrawal>
    {
        //private readonly WithdrawalService _withdrawals;

        public WithdrawalsViewModel() : base(WithdrawalService.Instance)
        {
            //_withdrawals = WithdrawalService.Instance;
            //Messenger.Default.Register<EntityMessage<Withdrawal>>(this, OnMessage);
        }

        public override string Title => "Alimentation de caisse";
        protected override string EntityViewName => "WithdrawalView";

        // For Reports
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime FinishDate { get; set; }
        protected override object GetEntityViewModel(Withdrawal entity) => WithdrawalViewModel.Instance(entity);

        public override void OnNavigatedTo()
        {
            StartDate = Session.Exercise.StartDate;
            FinishDate = Session.Exercise.GetLastDay();
            LoadEntities();
        }

        protected override void LoadEntities()
        {
            Entities = Service.Get(e => e.ExerciseId == Session.Exercise.Id);
        }

        //public void New()
        //{
        //    ShowDocument();
        //}

        //public void Edit(Withdrawal entity)
        //{
        //    ShowDocument(entity);
        //}

        //private void ShowDocument(Withdrawal entity = null)
        //{
        //    var vm = WithdrawalViewModel.Instance(entity);
        //    var doc = DocumentManagerService.CreateDocument("WithdrawalView", vm);
        //    doc.DestroyOnClose = true;
        //    doc.Show();
        //}

        //public void Delete(Withdrawal entity)
        //{
        //    var result = MessageBoxService.Show(
        //        "êtes-vous sûr de vouloir supprimer cet enregistrement?",
        //        "Attention",
        //        MessageButton.YesNo,
        //        MessageIcon.Warning,
        //        MessageResult.No
        //        );

        //    if (result != MessageResult.Yes) return;

        //    try
        //    {
        //        _withdrawals.Delete(entity);
        //        Entities.Remove(entity);
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBoxService.Show(e.Message, "Erreur",
        //            MessageButton.OK, MessageIcon.Error, MessageResult.OK);
        //    }
        //}

        public override bool CanNew() => !Session.Exercise.IsClosed;

        public override bool CanEdit(Withdrawal entity)
        {
            return base.CanEdit(entity) && !Session.Exercise.IsClosed;
        }

        public override bool CanDelete(Withdrawal entity)
        {
            return base.CanDelete(entity) && !Session.Exercise.IsClosed;
        }

        //protected bool AllowEdit(ITrackable entity)
        //{
        //    return entity != null 
        //        && !Session.Exercise.IsClosed 
        //        && entity.CreatedBy == Session.Identity.Id;
        //}

        //private void OnMessage(EntityMessage<Withdrawal> message)
        //{
        //    switch (message.Type)
        //    {
        //        case MessageType.Added:
        //            var added = _withdrawals.Find(message.Entity.Id);
        //            Entities.Add(added);
        //            break;
        //        case MessageType.Modified:
        //            _withdrawals.Reload(SelectedEntity);
        //            break;
        //        case MessageType.Deleted:
        //            break;
        //    }
        //}

        public void ShowMonthlyReport()
        {
            ShowReport(Entities);
        }

        private static void ShowReport(IEnumerable<Withdrawal> source)
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