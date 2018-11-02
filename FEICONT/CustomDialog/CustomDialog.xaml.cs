using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FEICONT.CustomDialog
{
    /// <summary>
    /// Lógica de interacción para CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window
    {      
        private bool _bolAeroGlassEnabled = false;
        private CustomDialogResults _enumCustomDialogResult = CustomDialogResults.None;
        private int _intButtonsDisabledDelay;
        private System.Windows.Forms.Timer _objButtonDelayTimer;
        public CustomDialogResults CustomDialogResult
        {
            get { return _enumCustomDialogResult; }
        }
        public CustomDialog(int intButtonsDisabledDelay)
        {
            InitializeComponent();
            if(Environment.OSVersion.Version.Major < 6)
            {
                _bolAeroGlassEnabled = false;
                this.AllowsTransparency = true;
            }
            else
            {
                _bolAeroGlassEnabled = true;
            }
          
            _intButtonsDisabledDelay = intButtonsDisabledDelay;
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            if (_bolAeroGlassEnabled == false)
            {
                //no aero glass
                this.borderCustomDialog.Background = System.Windows.SystemColors.ActiveCaptionBrush;
                this.tbCaption.Foreground = System.Windows.SystemColors.HighlightTextBrush;
                // this.tbCaption.Foreground = System.Windows.SystemColors.ActiveCaptionTextBrush;
                this.borderCustomDialog.CornerRadius = new CornerRadius(0, 0, 0, 0);
                this.borderCustomDialog.Padding = new Thickness(0, 0, 0, 0);
                this.borderCustomDialog.BorderThickness = new Thickness(0, 0, 1, 1);
                this.borderCustomDialog.BorderBrush = System.Windows.Media.Brushes.Black;
            }
            else
            {
                this.borderCustomDialog.Background = System.Windows.SystemColors.ActiveCaptionBrush;
                this.tbCaption.Foreground = System.Windows.SystemColors.ActiveCaptionTextBrush;
                this.borderCustomDialog.Padding = new Thickness(0, 0, 0, 0);
                this.borderCustomDialog.BorderBrush = System.Windows.SystemColors.ActiveBorderBrush;
            }
          
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbAdditionalDetailsText.Visibility = Visibility.Collapsed;

            if (this.ResizeMode != ResizeMode.NoResize)
            {
                //this work around is necessary when glass is enabled and the window style is None which removes the chrome because the resize mode MUST be set to CanResize or else glass won't display
                this.MinHeight = this.ActualHeight;
                this.MaxHeight = this.ActualHeight;

                this.MinWidth = this.ActualWidth;
                this.MaxWidth = this.ActualWidth;
            }

            if (_intButtonsDisabledDelay > 0)
            {
                this.pbDisabledButtonsProgressBar.Maximum = _intButtonsDisabledDelay;
                this.pbDisabledButtonsProgressBar.IsIndeterminate = false;

                Duration objDuration = new Duration(TimeSpan.FromSeconds(_intButtonsDisabledDelay));
                System.Windows.Media.Animation.DoubleAnimation objDoubleAnimation = new System.Windows.Media.Animation.DoubleAnimation(_intButtonsDisabledDelay, objDuration);
                this.pbDisabledButtonsProgressBar.BeginAnimation(ProgressBar.ValueProperty, objDoubleAnimation);
                btnCancel.IsEnabled = false;
                btnNo.IsEnabled = false;
                btnOK.IsEnabled = false;
                btnYes.IsEnabled = false;
                _objButtonDelayTimer = new System.Windows.Forms.Timer();
                _objButtonDelayTimer.Tick += OnTimedEvent;
                _objButtonDelayTimer.Interval = _intButtonsDisabledDelay * 1000;
                _objButtonDelayTimer.Start();

            }
            else
            {
                this.pbDisabledButtonsProgressBar.Visibility = Visibility.Collapsed;
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _enumCustomDialogResult = CustomDialogResults.Cancel;
            this.DialogResult = true;
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            _enumCustomDialogResult = CustomDialogResults.No;
            this.DialogResult = true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            _enumCustomDialogResult = CustomDialogResults.OK;
            this.DialogResult = true;
        }

        private void btnYes_Click(object sender,RoutedEventArgs e)
        {
            _enumCustomDialogResult = CustomDialogResults.Yes;
            this.DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this prevents ALT-F4 from closing the dialog box
            if (this.DialogResult.HasValue && this.DialogResult.Value == true)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void expAdditionalDetails_Collapsed(object sender, RoutedEventArgs e)
        {
            this.expAdditionalDetails.Header = "Mas detalles";
            this.tbAdditionalDetailsText.Visibility = Visibility.Collapsed;
           
            this.UpdateLayout();

            if (this.ResizeMode != ResizeMode.NoResize)
            {
                this.MaxHeight = this.ActualHeight;
            }

        }
        private void expAdditionalDetails_Expanded(object sender, RoutedEventArgs e)
        {
            if (this.ResizeMode != ResizeMode.NoResize)
            {
                this.MaxHeight = double.PositiveInfinity;
            }

            this.expAdditionalDetails.Header = "Menos detalles";
            this.tbAdditionalDetailsText.Visibility = Visibility.Visible;

            this.UpdateLayout();

            if (this.ResizeMode != ResizeMode.NoResize)
            {
                this.MaxHeight = this.ActualHeight;
            }

        }
        private void OnTimedEvent(object source, EventArgs e)
        {
            _objButtonDelayTimer.Stop();
            _objButtonDelayTimer.Dispose();
            _objButtonDelayTimer = null;
            btnCancel.IsEnabled = true;
            btnNo.IsEnabled = true;
            btnOK.IsEnabled = true;
            btnYes.IsEnabled = true;
            this.pbDisabledButtonsProgressBar.Visibility = Visibility.Collapsed;
        }

        private void tbCaption_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    }
    public enum CustomDialogButtons
    {
        OK,
        OKCancel,
        YesNo,
        YesNoCancel
    }

    public enum CustomDialogIcons
    {
        None,
        Information,
        Question,
        Shield,
        Stop,
        Warning
    }

    public enum CustomDialogResults
    {
        None,
        OK,
        Cancel,
        Yes,
        No
    }
}
