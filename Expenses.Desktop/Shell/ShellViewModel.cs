using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;
using Expenses.UI.Dashboard;
using Expenses.UI.Exercises;
using Expenses.UI.Users;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.UI.Shell
{
    [POCOViewModel]
    public class ShellViewModel
    {
        private readonly ExerciseService _exerciseService;
        private readonly UserService _userService;
        private ExerciseViewModel _exerciseVm;
        private LoginViewModel _loginVm;

        public ShellViewModel()
        {
            Session = Session.Default;
            _userService = UserService.Instance;
            _exerciseService = ExerciseService.Instance;
        }

        public virtual string Title { get; set; }
        public virtual string CurrentView { get; set; }
        public virtual bool IsBusy { get; protected set; }
        public virtual Session Session { get; protected set; }

        [ServiceProperty(Key = "LoginDialogService")]
        protected virtual IDialogService LoginDialogService => null;

        [ServiceProperty(Key = "ExerciseDialogService")]
        protected virtual IDialogService ExerciseDialogService => null;

        protected virtual ICurrentWindowService WindowService => null;
        protected virtual INavigationService NavigationService => null;
        protected virtual ISplashScreenService SplashScreenService => null;
        protected virtual IMessageBoxService MessageBoxService => null;
        
        public void Initialize()
        {
            ShowSplashScreen();
            ShowLoginDialog();
            if (Session.Exercise == null)
                ShowExerciseDialog();
        }

        private void ShowSplashScreen()
        {
            if (!SplashScreenService.IsSplashScreenActive)
                SplashScreenService.ShowSplashScreen();
            Session.Exercise = _exerciseService.CurrentExercise;
            SplashScreenService.HideSplashScreen();
        }

        private void ShowLoginDialog()
        {
            if (_loginVm == null)
                _loginVm = LoginViewModel.Instance;

            var loginCommand = new UICommand
            {
                Caption = "Connecter",
                IsDefault = true,
                Command = new DelegateCommand<CancelEventArgs>(OnLoginAttempt, CanLogin)
            };

            var cancelCommand = new UICommand
            {
                Caption = "Annuler",
                IsCancel = true,
                Command = new DelegateCommand(Exit)
            };

            var dialogCommands = new List<UICommand> {loginCommand, cancelCommand};

            var result = LoginDialogService.ShowDialog(dialogCommands, "Connexion", _loginVm);

            if (result == null) Exit();
        }

        private void ShowExerciseDialog()
        {
            if (_exerciseVm == null)
                _exerciseVm = ExerciseViewModel.Instance;

            _exerciseVm.Exercise = new Exercise
            {
                Date = DateTime.Today,
                CreatedBy = Session.Identity.Id
            };

            var loginCommand = new UICommand
            {
                Caption = "Enregistrer",
                IsDefault = true,
                Command = new DelegateCommand<CancelEventArgs>(OnSaveExercise)
            };

            var cancelCommand = new UICommand
            {
                Caption = "Annuler",
                IsCancel = true,
                Command = new DelegateCommand(Exit)
            };

            var dialogCommands = new List<UICommand> {loginCommand, cancelCommand};

            var result = ExerciseDialogService.ShowDialog(dialogCommands, "Exercise", _exerciseVm);

            if (result == null) Exit();
        }

        private void OnSaveExercise(CancelEventArgs e)
        {
            try
            {
                var exercise = _exerciseVm.Exercise;
                _exerciseService.SetAdded(exercise);
                _exerciseService.Save(exercise);
                Session.Exercise = exercise;
            }
            catch (Exception ex)
            {
                MessageBoxService.Show(ex.Message, "Validation",
                    MessageButton.OK, MessageIcon.Error, MessageResult.OK);
            }
        }

        private void Exit()
        {
            WindowService.Close();
        }

        private bool CanLogin(CancelEventArgs e)
        {
            return !(string.IsNullOrWhiteSpace(_loginVm.Username) ||
                     string.IsNullOrWhiteSpace(_loginVm.Password));
        }

        private void OnLoginAttempt(CancelEventArgs e)
        {
            if (_userService.Attempt(_loginVm.Username, _loginVm.Password))
            {
                Session.Identity = _userService.Identity;
                _loginVm.Reset();
            }
            else
            {
                _loginVm.Message = "Données incorrectes!";
                e.Cancel = true;
            }
        }

        // Navigation Commands
        public void Loaded()
        {
            Navigate(typeof (DashboardView));
        }

        public void Navigate(Type viewType)
        {
            var vmType = GetViewModelType(viewType);
            var vm = GetViewModelInstance(vmType);

            NavigationService.Navigate(viewType.Name, vm, null, null, false);
            CurrentView = viewType.Name;
            Title = ((IViewModel) vm).Title;
        }

        public bool CanNavigate(Type viewType)
        {
            return CurrentView != viewType.Name;
        }

        private static object GetViewModelInstance(Type type)
        {
            return typeof (ViewModelSource)
                .GetMethod("Create", Type.EmptyTypes)
                .MakeGenericMethod(type)
                .Invoke(null, null);
        }

        private static Type GetViewModelType(Type viewType)
        {
            var viewName = viewType.FullName;
            var assembly = viewType.Assembly.FullName;
            var sufix = viewName.EndsWith("View") ? "Model" : "ViewModel";
            var vmTypeName = $"{viewName + sufix}, {assembly}";
            return Type.GetType(vmTypeName);
        }
    }
}