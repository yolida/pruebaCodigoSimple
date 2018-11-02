using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FEICONT.CustomDialog
{
    public class CustomDialogWindow
    {
        private CustomDialogButtons _enumButtons = CustomDialogButtons.OK;
        private CustomDialogResults _enumCustomDialogResult = CustomDialogResults.None;
        private CustomDialogResults _enumDefaultButton = CustomDialogResults.None;
        private CustomDialogIcons _enumFooterIcon = CustomDialogIcons.None;
        private CustomDialogIcons _enumInstructionIcon = CustomDialogIcons.None;
        private int _intButtonsDisabledDelay = 0;
        private string _strAdditionalDetailsText = string.Empty;
        //example:  "btnDashboard_Click"
        private string _strCallingMethodName = string.Empty;
        //example:  "CustomDialogSample.exe"
        private string _strCallingModuleName = string.Empty;
        //example:  "ApplicationMainWindow"	
        private string _strCallingReflectedTypeName = string.Empty;
        private string _strCaption = string.Empty;
        private string _strFooterText = string.Empty;
        private string _strInstructionHeading = string.Empty;
        private string _strInstructionText = string.Empty;

        public string AdditionalDetailsText
        {
            get { return _strAdditionalDetailsText; }
            set { _strAdditionalDetailsText = value; }
        }

        /// <summary>
        /// Gets or sets the buttons that will be displayed.  The default is the OK button.
        /// </summary>
        public CustomDialogButtons Buttons
        {
            get { return _enumButtons; }
            set { _enumButtons = value; }
        }

        /// <summary>
        /// Gets or sets the number of seconds that the buttons will be disabled, providing time for the user to read the dialog before dismissing it.  Assigning a value also causes a progress bar to display the elapsed time before the buttons are enabled.
        /// </summary>
        public int ButtonsDisabledDelay
        {
            get { return _intButtonsDisabledDelay; }
            set { _intButtonsDisabledDelay = value; }
        }

        /// <summary>
        /// Gets or sets the calling method name.  If not set, the value will be determined from the stack frame and the calling method name will be used.  Normally this value is not set by the calling code.
        /// </summary>
        public string CallingMethodName
        {
            get { return _strCallingMethodName; }
            set { _strCallingMethodName = value; }
        }

        /// <summary>
        /// Gets or sets the calling module name.  If not set, the value will be determined from the stack frame and the calling module name will be used.  Normally this value is not set by the calling code.
        /// </summary>
        public string CallingModuleName
        {
            get { return _strCallingModuleName; }
            set { _strCallingModuleName = value; }
        }

        /// <summary>
        /// Gets or sets the calling type name.  If not set, the value will be determined from the stack frame and the calling reflected type name will be used.  Normally this value is not set by the calling code.
        /// </summary>
        public string CallingReflectedTypeName
        {
            get { return _strCallingReflectedTypeName; }
            set { _strCallingReflectedTypeName = value; }
        }

        /// <summary>
        /// Gets or sets the dialog box window caption.  The caption is displayed in the window chrome.
        /// </summary>
        public string Caption
        {
            get { return _strCaption; }
            set { _strCaption = value; }
        }

        /// <summary>
        /// Gets or sets the default button for the dialog box.  This value defaults to none.
        /// </summary>
        public CustomDialogResults DefaultButton
        {
            get { return _enumDefaultButton; }
            set { _enumDefaultButton = value; }
        }

        /// <summary>
        /// Gets or sets the icon displayed in the dialog footer.  This values defaults to none.
        /// </summary>
        public CustomDialogIcons FooterIcon
        {
            get { return _enumFooterIcon; }
            set { _enumFooterIcon = value; }
        }

        /// <summary>
        /// Gets or sets the optional text displayed in the footer.
        /// </summary>
        public string FooterText
        {
            get { return _strFooterText; }
            set { _strFooterText = value; }
        }

        /// <summary>
        /// Gets or sets the heading text displayed in the dialog box.
        /// </summary>
        public string InstructionHeading
        {
            get { return _strInstructionHeading; }
            set { _strInstructionHeading = value; }
        }

        /// <summary>
        /// Gets or sets the icon displayed to the left of the instruction text.  This value defaults to none.
        /// </summary>
        public CustomDialogIcons InstructionIcon
        {
            get { return _enumInstructionIcon; }
            set { _enumInstructionIcon = value; }
        }

        /// <summary>
        /// Gets or sets the text message for the dialog.
        /// </summary>
        public string InstructionText
        {
            get { return _strInstructionText; }
            set { _strInstructionText = value; }
        }
        public CustomDialogWindow(int intButtonsDisabledDelay = 0)
        {
            _intButtonsDisabledDelay = intButtonsDisabledDelay;
        }


        public CustomDialogWindow(string strCaption, string strInstructionHeading, string strInstructionText, CustomDialogButtons enumButtons, CustomDialogResults enumDefaultButton, CustomDialogIcons enumInstructionIcon, int intButtonsDisabledDelay = 0)
        {
            _strCaption = strCaption;
            _strInstructionHeading = strInstructionHeading;
            _strInstructionText = strInstructionText;
            _enumButtons = enumButtons;
            _enumDefaultButton = enumDefaultButton;
            _enumInstructionIcon = enumInstructionIcon;
            _intButtonsDisabledDelay = intButtonsDisabledDelay;
        }


        public CustomDialogWindow(string strCaption, string strInstructionHeading, string strInstructionText, string strFooterText, CustomDialogButtons enumButtons, CustomDialogResults enumDefaultButton, CustomDialogIcons enumInstructionIcon, CustomDialogIcons enumFooterIcon, int intButtonsDisabledDelay = 0)
        {
            _strCaption = strCaption;
            _strInstructionHeading = strInstructionHeading;
            _strInstructionText = strInstructionText;
            _strFooterText = strFooterText;
            _enumButtons = enumButtons;
            _enumDefaultButton = enumDefaultButton;
            _enumInstructionIcon = enumInstructionIcon;
            _enumFooterIcon = enumFooterIcon;
            _intButtonsDisabledDelay = intButtonsDisabledDelay;

        }


        public CustomDialogWindow(string strCaption, string strInstructionHeading, string strInstructionText, string strAdditionalDetailsText, string strFooterText, CustomDialogButtons enumButtons, CustomDialogResults enumDefaultButton, CustomDialogIcons enumInstructionIcon, CustomDialogIcons enumFooterIcon, int intButtonsDisabledDelay = 0)
        {
            _strCaption = strCaption;
            _strInstructionHeading = strInstructionHeading;
            _strInstructionText = strInstructionText;
            _strAdditionalDetailsText = strAdditionalDetailsText;
            _strFooterText = strFooterText;
            _enumButtons = enumButtons;
            _enumDefaultButton = enumDefaultButton;
            _enumInstructionIcon = enumInstructionIcon;
            _enumFooterIcon = enumFooterIcon;
            _intButtonsDisabledDelay = intButtonsDisabledDelay;

        }

        public CustomDialogResults Show()
        {
            //get the calling code information
            System.Diagnostics.StackTrace objTrace = new System.Diagnostics.StackTrace();
            if (_strCallingReflectedTypeName.Length == 0)
            {
                _strCallingReflectedTypeName = objTrace.GetFrame(1).GetMethod().ReflectedType.Name;
            }

            if (_strCallingMethodName.Length == 0)
            {
                _strCallingMethodName = objTrace.GetFrame(1).GetMethod().Name;
            }

            if (_strCallingModuleName.Length == 0)
            {
                _strCallingModuleName = objTrace.GetFrame(1).GetMethod().Module.Name;
            }

            CustomDialog obj = new CustomDialog(this.ButtonsDisabledDelay);
            obj.tbCaption.Text = Caption;
            obj.Title = Caption;
            if (this.FooterText.Length > 0)
            {
                obj.tbFooterText.Text = this.FooterText;

                if (this.FooterIcon != CustomDialogIcons.None)
                {
                    obj.imgFooterIcon.Source = new BitmapImage(GetIcon(this.FooterIcon));

                }
                else
                {
                    obj.imgFooterIcon.Visibility = Visibility.Collapsed;
                }

            }
            else
            {
                obj.tbFooterText.Visibility = Visibility.Collapsed;
                obj.imgFooterIcon.Visibility = Visibility.Collapsed;
            }

            if (this.InstructionIcon == CustomDialogIcons.None)
            {
                obj.imgInstructionIcon.Visibility = Visibility.Collapsed;

            }
            else
            {
                obj.imgInstructionIcon.Source = new BitmapImage(GetIcon(this.InstructionIcon));
            }

            obj.tbInstructionText.Text = this.InstructionText;

            obj.tbInstructionHeading.Text = this.InstructionHeading;

            if (this.AdditionalDetailsText.Length > 0)
            {
                obj.tbAdditionalDetailsText.Text = this.AdditionalDetailsText;

            }
            else
            {
                obj.expAdditionalDetails.Visibility = Visibility.Collapsed;
            }

            SetButtons(obj);

            obj.ShowInTaskbar = true;

            obj.ShowDialog();
            _enumCustomDialogResult = obj.CustomDialogResult;

            LogDialog();

            return _enumCustomDialogResult;
        }

        private Uri GetIcon(CustomDialogIcons enumCustomDialogIcon)
        {
            switch (enumCustomDialogIcon)
            {
                case CustomDialogIcons.Information:
                    return new Uri("/FEICONT;component/images/CustomDialogInformation.png", UriKind.Relative);
                case CustomDialogIcons.None:
                    return null;
                case CustomDialogIcons.Question:
                    return new Uri("/FEICONT;component/images/CustomDialogQuestion.png", UriKind.Relative);
                case CustomDialogIcons.Shield:
                    return new Uri("/FEICONT;component/images/CustomDialogShield.png", UriKind.Relative);
                case CustomDialogIcons.Stop:
                    return new Uri("/FEICONT;component/images/CustomDialogStop.png", UriKind.Relative);
                case CustomDialogIcons.Warning:
                    return new Uri("/FEICONT;component/images/CustomDialogWarning.png", UriKind.Relative);             
            }
            return null;
        }



        private void LogDialog()
        {
            //TODO - developers, you can log the result here.  There is rich information within this class to provides great tracking of your users dialog box activities.
            //ensure that you review each property and include them in your log entry
            //don't forget to log the Windows user name also

        }
        private void SetButtons(CustomDialog obj)
        {
            switch (this.Buttons)
            {
                case CustomDialogButtons.OK:
                    obj.btnCancel.Visibility = Visibility.Collapsed;
                    obj.btnNo.Visibility = Visibility.Collapsed;
                    obj.btnYes.Visibility = Visibility.Collapsed;

                    break;
                case CustomDialogButtons.OKCancel:
                    obj.btnNo.Visibility = Visibility.Collapsed;
                    obj.btnYes.Visibility = Visibility.Collapsed;

                    break;
                case CustomDialogButtons.YesNo:
                    obj.btnOK.Visibility = Visibility.Collapsed;
                    obj.btnCancel.Visibility = Visibility.Collapsed;

                    break;
                case CustomDialogButtons.YesNoCancel:
                    obj.btnOK.Visibility = Visibility.Collapsed;

                    break;
              }

            switch (this.DefaultButton)
            {

                case CustomDialogResults.Cancel:
                    obj.btnCancel.IsDefault = true;

                    break;
                case CustomDialogResults.No:
                    obj.btnNo.IsDefault = true;
                    obj.btnCancel.IsCancel = true;

                    break;
                case CustomDialogResults.None:
                    //do nothing
                    obj.btnCancel.IsCancel = true;

                    break;
                case CustomDialogResults.OK:
                    obj.btnOK.IsDefault = true;
                    obj.btnCancel.IsCancel = true;

                    break;
                case CustomDialogResults.Yes:
                    obj.btnYes.IsDefault = true;
                    obj.btnCancel.IsCancel = true;

                    break;
              }
        }

    }
}
