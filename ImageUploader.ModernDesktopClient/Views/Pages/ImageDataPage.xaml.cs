using System.Windows;
using Wpf.Ui.Common.Interfaces;

namespace ImageUploader.ModernDesktopClient.Views.Pages
{
    /// <summary>
    /// Interaction logic for ImageDataPage.xaml
    /// </summary>
    public partial class ImageDataPage : INavigableView<ViewModels.ImageDataViewModel>
    {
        public ViewModels.ImageDataViewModel ViewModel
        {
            get;
        }

        public ImageDataPage(ViewModels.ImageDataViewModel viewModel)
        {
            ViewModel = viewModel;
            
            InitializeComponent();
        }
    }
}