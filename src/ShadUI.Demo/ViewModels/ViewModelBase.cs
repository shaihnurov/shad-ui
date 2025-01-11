using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ShadUI.Demo.ViewModels;

public abstract class ViewModelBase: ObservableValidator
{
    private readonly Dictionary<string, string> _customErrors = new();

    /// <summary>
    ///     Add a custom error to the view model.
    /// </summary>
    /// <param name="propertyName">Name of related property</param>
    /// <param name="errorMessage">Custom error message</param>
    public void AddCustomError(string propertyName, string errorMessage)
    {
        _customErrors.TryAdd(propertyName, errorMessage);

        var args = new DataErrorsChangedEventArgs(propertyName);
        CustomErrorsChanged?.Invoke(this, args);
    }

    /// <summary>
    ///     Get custom error message for a property.
    /// </summary>
    /// <param name="propertyName">Name of related property</param>
    /// <returns>Returns error message if found. Otherwise, returns empty string</returns>
    public string GetCustomErrors(string? propertyName) =>
        _customErrors.TryGetValue(propertyName ?? "", out var error) ? error : string.Empty;

    /// <summary>
    ///     Clear all custom errors.
    /// </summary>
    public void ClearCustomErrors()
    {
        _customErrors.Clear();
    }

    /// <summary>
    ///     Invoked when a custom error is added.
    /// </summary>
    public EventHandler<DataErrorsChangedEventArgs>? CustomErrorsChanged;
}