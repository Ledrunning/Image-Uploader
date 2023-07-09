using System.Windows;
using ImageUploader.DesktopCommon.Events;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Enums;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace ImageUploader.ModernDesktopClient.Services;

public class MessageBoxService : IMessageBoxService
{
    public MessageBoxService(MessageBox messageBox)
    {
        ModernMessageBox = messageBox;
        InitializeMessageBox();
    }

    public MessageBox ModernMessageBox { get; }

    public event TemplateEventHandler<ButtonName> ButtonEvent = delegate { };

    private void InitializeMessageBox()
    {
        ModernMessageBox.ButtonLeftName = "Ok";
        ModernMessageBox.ButtonRightName = "Cancel";
        ModernMessageBox.ButtonRightClick += OnMessageBoxButtonRightClick;
        ModernMessageBox.ButtonLeftClick += OnMessageBoxButtonLeftClick;
    }

    private void OnMessageBoxButtonRightClick(object sender, RoutedEventArgs e)
    {
        ModernMessageBox.Visibility = Visibility.Hidden;
        ButtonEvent.Invoke(new TemplateEventArgs<ButtonName>(ButtonName.Cancel));
    }

    private void OnMessageBoxButtonLeftClick(object sender, RoutedEventArgs e)
    {
        ModernMessageBox.Visibility = Visibility.Hidden;
        ButtonEvent.Invoke(new TemplateEventArgs<ButtonName>(ButtonName.Cancel));
    }
}