using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly ExerciseService _exercises;
        private readonly UserService _users;
        private ExerciseViewModel _exerciseVm;
        private LoginViewModel _loginVm;

        public ShellViewModel()
        {
            Session = Session.Default;
            _users = UserService.Instance;
            _exercises = ExerciseService.Instance;
        }

        public virtual string Title { get; protected set; }
        public virtual string CurrentView { get; protected set; }
        public virtual bool IsBusy { get; protected set; }
        public virtual Session Session { get; protected set; }
        public virtual ObservableCollection<Exercise> Exercises { get; protected set; }

        [ServiceProperty(Key = "LoginDialogService")]
        protected virtual IDialogService LoginDialogService => null;

        [ServiceProperty(Key = "ExerciseDialogService")]
        protected virtual IDialogService ExerciseDialogService => null;

        protected virtual ICurrentWindowService WindowService => null;
        protected virtual INavigationService NavigationService => null;
        protected virtual ISplashScreenService SplashScreenService => null;
        protected virtual IMessageBoxService MessageBoxService => null;

        public virtual bool IsAuthenticated { get; protected set; }

        public void Initialize()
        {
            LoadExercises();
            ShowLoginDialog();
            if (Session.Exercise == null)
                ShowExerciseDialog();
        }

        private void LoadExercises()
        {
            SplashScreenService.ShowSplashScreen();
            Exercises = _exercises.Get();
            Session.Exercise = _exercises.One(e => e.IsCurrent);
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
                Session.Exercise = _exercises.Create(_exerciseVm.Exercise);
            }
            catch (Exception ex)
            {
                MessageBoxService.Show(ex.Message, "Erreur",
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
            if (_users.Attempt(_loginVm.Username, _loginVm.Password))
            {
                Session.Identity = _users.Identity;
                _loginVm.Reset();
                IsAuthenticated = true;
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

        public void Logout()
        {
            IsAuthenticated = false;
            PageChanged();
            ShowLoginDialog();
        }

        public void PageChanged()
        {
            if (NavigationService?.Current == null) return;
            NavigationService.Navigate(Names.HomeViewName);
            CurrentView = Names.HomeViewName;
            Title = Names.AppTitle;
        }
    }
}