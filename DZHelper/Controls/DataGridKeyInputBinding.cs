using System.Windows.Controls;
using System.Windows.Input;

namespace DZHelper.Controls
{
    public static class DataGridKeyInputBinding
    {
        /// <summary>
        /// Thêm một KeyBinding vào DataGrid.
        /// </summary>
        /// <param name="dataGrid">DataGrid cần thêm KeyBinding.</param>
        /// <param name="keyGesture">Phím tắt (KeyGesture) để kích hoạt hành động.</param>
        /// <param name="command">Lệnh (ICommand) được thực thi khi kích hoạt phím tắt.</param>
        public static void AddKeyBinding(this DataGrid dataGrid, KeyGesture keyGesture, ICommand command)
        {
            if (dataGrid == null) throw new ArgumentNullException(nameof(dataGrid));
            if (keyGesture == null) throw new ArgumentNullException(nameof(keyGesture));
            if (command == null) throw new ArgumentNullException(nameof(command));

            var keyBinding = new KeyBinding(command, keyGesture);
            dataGrid.InputBindings.Add(keyBinding);
        }

        /// <summary>
        /// Xóa tất cả KeyBinding khớp với KeyGesture khỏi DataGrid.
        /// </summary>
        /// <param name="dataGrid">DataGrid cần xóa KeyBinding.</param>
        /// <param name="keyGesture">Phím tắt (KeyGesture) cần xóa.</param>
        public static void RemoveKeyBinding(this DataGrid dataGrid, KeyGesture keyGesture)
        {
            if (dataGrid == null) throw new ArgumentNullException(nameof(dataGrid));
            if (keyGesture == null) throw new ArgumentNullException(nameof(keyGesture));

            var bindingsToRemove = new List<InputBinding>();

            foreach (InputBinding binding in dataGrid.InputBindings)
            {
                if (binding is KeyBinding keyBinding && keyBinding.Gesture.Equals(keyGesture))
                {
                    bindingsToRemove.Add(binding);
                }
            }

            foreach (var binding in bindingsToRemove)
            {
                dataGrid.InputBindings.Remove(binding);
            }
        }

        /// <summary>
        /// Gắn một KeyBinding với hành động cụ thể trên DataGrid.
        /// </summary>
        /// <param name="dataGrid">DataGrid cần gắn hành động.</param>
        /// <param name="keyGesture">Phím tắt (KeyGesture) để kích hoạt hành động.</param>
        /// <param name="action">Hành động được thực thi khi kích hoạt phím tắt.</param>
        public static void AddKeyBinding(this DataGrid dataGrid, KeyGesture keyGesture, Action action)
        {
            if (dataGrid == null) throw new ArgumentNullException(nameof(dataGrid));
            if (keyGesture == null) throw new ArgumentNullException(nameof(keyGesture));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var command = new RelayCommand(_ => action());
            AddKeyBinding(dataGrid, keyGesture, command);
        }

        /// <summary>
        /// Lớp RelayCommand để hỗ trợ ICommand.
        /// </summary>
        private class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute?.Invoke(parameter) ?? true;
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
    }
}
