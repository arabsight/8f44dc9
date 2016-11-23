using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Exercises
{
    public class ExercisesViewModel : EntitiesViewModel<Exercise>
    {
        //private ExerciseViewModel _exerciseVm;

        public ExercisesViewModel() : base(ExerciseService.Instance) {}

        public override string Title => "Exercises";

        protected override string EntityViewName => "ExerciseSummaryView";

        //protected override void OnMessage(EntityMessage<Exercise> message)
        //{
        //    if (message.Type != MessageType.Modified) return;
        //    Service.Reload(SelectedEntity);
        //    Session.Exercise = SelectedEntity;
        //}

        protected override object GetEntityViewModel(Exercise entity) => 
            ExerciseSummaryViewModel.Instance(entity);

        public void CloseExercise(Exercise entity) => ShowDocument(entity);

        public bool CanCloseExercise(Exercise entity)
        {
            return entity != null
                   && entity.IsClosed == false
                   && entity.IsCurrent;
        }

        //public override void New()
        //{
        //    if (_exerciseVm == null)
        //        _exerciseVm = ExerciseViewModel.Instance;

        //    var last = Service.Get().OrderByDescending(e => e.Date).FirstOrDefault();

        //    _exerciseVm.Exercise = new Exercise
        //    {
        //        Date = last?.Date.AddMonths(1) ?? DateTime.Today,
        //        CreatedBy = Session.Identity.Id
        //    };

        //    var loginCommand = new UICommand
        //    {
        //        Caption = "Enregistrer",
        //        IsDefault = true,
        //        Command = new DelegateCommand<CancelEventArgs>(OnSaveExercise)
        //    };

        //    var cancelCommand = new UICommand
        //    {
        //        Caption = "Annuler",
        //        IsCancel = true
        //    };

        //    var dialogCommands = new List<UICommand> { loginCommand, cancelCommand };

        //    ExerciseDialogService.ShowDialog(dialogCommands, "Exercise", _exerciseVm);
        //}

        //private void OnSaveExercise(CancelEventArgs e)
        //{
        //    try
        //    {
        //        var exercise = _exerciseVm.Exercise;
        //        exercise.Date = new DateTime(exercise.Date.Year, exercise.Date.Month, 1);
        //        Service.SetAdded(exercise);
        //        Service.Save(exercise);
        //        Session.Exercise = exercise;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBoxService.Show(ex.Message, "Erreur",
        //            MessageButton.OK, MessageIcon.Error, MessageResult.OK);
        //    }
        //}

        //[ServiceProperty(Key = "ExerciseDialogService")]
        //protected virtual IDialogService ExerciseDialogService => null;
    }
}