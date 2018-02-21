using Labs.WPF.TvShowOrganizer.Services.Contracts;
using System.Windows;

namespace Labs.WPF.TvShowOrganizer.Services
{
    public class MessageBoxService : IMessageService
    {
        public MessageBoxResult Show(string message, string caption, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            return MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
        }
    }
}
