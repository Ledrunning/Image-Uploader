using System.ComponentModel;
using ImageUploader.DesktopCommon.Events;
using ImageUploader.ModernDesktopClient.Enums;
using Wpf.Ui.Controls;

namespace ImageUploader.ModernDesktopClient.Contracts;

public interface IMessageBoxService
{
    MessageBox ModernMessageBox { get; }

    event TemplateEventHandler<ButtonName> ButtonEvent;
}