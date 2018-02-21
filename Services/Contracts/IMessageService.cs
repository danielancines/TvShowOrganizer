using System.Windows;

namespace Labs.WPF.TvShowOrganizer.Services.Contracts
{
    public interface IMessageService
    {
        MessageBoxResult Show(string message, string caption, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage);
    }
}
