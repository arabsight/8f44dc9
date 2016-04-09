using System;
using DevExpress.Mvvm;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Exercises
{
    public class ExercisesViewModel : EntitiesViewModel<Exercise>
    {
        private readonly ExerciseService _exercises;
        //private readonly ExpenseService _expenses;
        //private readonly WithdrawalService _withdrawals;

        public ExercisesViewModel()
        {
            _exercises = ExerciseService.Instance;
            //_withdrawals = WithdrawalService.Instance;
            //_expenses = ExpenseService.Instance;
            Messenger.Default.Register<EntityMessage<Exercise>>(this, OnMessage);
        }

        private void OnMessage(EntityMessage<Exercise> message)
        {
            if (message.Type != MessageType.Modified) return;
            _exercises.Reload(SelectedEntity);
            Session.Exercise = SelectedEntity;
        }

        public override string Title => "Exercises";

        public override void OnNavigatedTo()
        {
            LoadEntities();
        }

        public void LoadEntities()
        {
            Entities = _exercises.Get();
        }

        public void New()
        {
            //ShowDocument();
        }

        public void Edit(Exercise entity)
        {
            //ShowDocument(entity);
        }

        public void CloseExercise(Exercise entity)
        {
            //var eTotal = _expenses.GetTotalByExercise(entity);
            //var wTotal = _withdrawals.GetTotalByExercise(entity);
            //var balance = wTotal + entity.Balance - eTotal;
            //_exercises.Close(entity);
            ShowDocument(entity);
        }

        private void ShowDocument(Exercise entity = null)
        {
            var vm = ExerciseSummaryViewModel.Instance(entity);
            var document = DocumentManagerService.CreateDocument("ExerciseSummaryView", vm);
            document.DestroyOnClose = true;
            document.Show();
        }

        public void Delete(Exercise entity)
        {
            var result = MessageBoxService.Show(
                "êtes-vous sûr de vouloir supprimer cet utilisateur?", "Attention",
                MessageButton.YesNo, MessageIcon.Warning, MessageResult.No);

            if (result != MessageResult.Yes) return;

            try
            {
                _exercises.Delete(entity);
                Entities.Remove(entity);
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message, "Erreur",
                    MessageButton.OK, MessageIcon.Error, MessageResult.OK);
            }
        }
        

        public bool CanNew()
        {
            return Session.Identity.IsAdmin;
        }

        public bool CanEdit(Exercise entity)
        {
            return entity != null && Session.Identity.IsAdmin;
        }

        public bool CanDelete(Exercise entity)
        {
            return entity != null && Session.Identity.IsAdmin;
        }

        public bool CanCloseExercise(Exercise entity)
        {
            return entity != null 
                && Session.Identity.IsAdmin 
                && entity.IsClosed == false
                && entity.IsCurrent;
        }
    }
}
