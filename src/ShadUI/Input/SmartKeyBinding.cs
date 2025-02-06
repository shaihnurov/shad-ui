using System;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using ShadUI.Extensions;

namespace ShadUI.Input;

/// <summary>
///     A smart key binding that provides special handling for TextBox controls while maintaining regular key binding
///     functionality.
///     When a TextBox is focused, the key event is first passed to the TextBox before executing the command.
/// </summary>
public class SmartKeyBinding : KeyBinding, ICommand
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SmartKeyBinding" /> class.
    ///     Sets itself as the command handler.
    /// </summary>
    public SmartKeyBinding()
    {
        Command = this;
    }

    /// <summary>
    ///     Determines whether the command can be executed in its current state.
    ///     Returns true if a TextBox is focused or if the underlying command can execute.
    /// </summary>
    /// <param name="parameter">
    ///     Data used by the command. If the command does not require data, this parameter can be set to
    ///     null.
    /// </param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public bool CanExecute(object? parameter)
    {
        var focusManager = Avalonia.Application.Current.GetTopLevel()?.FocusManager;
        return focusManager?.GetFocusedElement() is TextBox || Command.CanExecute(parameter);
    }

    /// <summary>
    ///     Executes the command with the specified parameter.
    ///     If a TextBox is focused, the key event is first passed to the TextBox.
    ///     If the TextBox doesn't handle the event, the command is then executed.
    /// </summary>
    /// <param name="parameter">
    ///     Data used by the command. If the command does not require data, this parameter can be set to
    ///     null.
    /// </param>
    public void Execute(object? parameter)
    {
        var focusManager = Avalonia.Application.Current.GetTopLevel()?.FocusManager;
        if (focusManager?.GetFocusedElement() is TextBox textBox)
        {
            var ev = new KeyEventArgs
            {
                Key = Gesture.Key,
                KeyModifiers = Gesture.KeyModifiers,
                RoutedEvent = InputElement.KeyDownEvent
            };
            textBox.RaiseEvent(ev);
            if (!ev.Handled && CanExecute(parameter))
                Command.Execute(parameter);
        }
        else
        {
            Command.Execute(parameter);
        }
    }

    /// <summary>
    ///     Occurs when changes occur that affect whether the command should execute.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => Command.CanExecuteChanged += value;
        remove => Command.CanExecuteChanged -= value;
    }
}