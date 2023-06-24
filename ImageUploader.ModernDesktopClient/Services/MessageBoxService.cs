using ImageUploader.ModernDesktopClient.Contracts;
using System.Windows;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace ImageUploader.ModernDesktopClient.Services;

public class MessageBoxService : IMessageBoxService
{
    private readonly MessageBox _messageBox;

    public MessageBoxService(MessageBox messageBox)
    {
        _messageBox = messageBox;
    }

    public MessageBox InitializeMessageBox()
    {
        _messageBox.ButtonLeftName = "Ok";
        _messageBox.ButtonRightName = "Cancel";
        _messageBox.ButtonRightClick += OnMessageBoxButtonRightClick;
        _messageBox.ButtonLeftClick += OnMessageBoxButtonLeftClick;
        
        return _messageBox;
    }


    private void OnMessageBoxButtonRightClick(object sender, RoutedEventArgs e)
    {
        _messageBox.Visibility = Visibility.Hidden;
    }

    private void OnMessageBoxButtonLeftClick(object sender, RoutedEventArgs e)
    {
        _messageBox.Visibility = Visibility.Hidden;
    }
}