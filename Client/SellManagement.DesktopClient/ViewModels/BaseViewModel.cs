using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SellManagement.DesktopClient.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private Visibility isShowBackdrop;
        public Visibility IsShowBackdrop
        {
            get { return isShowBackdrop; }
            set
            {
                isShowBackdrop = value;
                OnPropertyChanged();
            }
        }

        public void ShowBackdrop(bool isShowWaiting)
        {
            if (isShowWaiting)
                IsShowBackdrop = Visibility.Visible;
            else
                IsShowBackdrop = Visibility.Collapsed;
        }
    }
    class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute == null ? true : _canExecute((T)parameter);
            }
            catch
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class AsyncCommand<T> : ICommand
    {
        private bool _isExecuting;
        private readonly Func<T, Task> _execute;
        private readonly Predicate<T> _canExecute;
        private readonly IErrorHandler _errorHandler;
        private readonly Action<bool> _showWaiting;
        public AsyncCommand(Predicate<T> canExecute, Func<T, Task> execute, Action<bool> showWaiting = null, IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
            _showWaiting = showWaiting;
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke((T)parameter) ?? true);
        }

        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    if (_showWaiting != null) _showWaiting(true);
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                    if (_showWaiting != null) _showWaiting(false);
                }
            }
        }


        #region Explicit implementations

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync((T)parameter).FireAndForgetSafeAsync(_errorHandler);
        }
        #endregion

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
