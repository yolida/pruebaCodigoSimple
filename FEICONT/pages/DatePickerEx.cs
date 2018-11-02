using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FEICONT.pages
{
    public class DatePickerEx : DatePicker
    {
        #region Data
        private DatePickerTextBox textBox;
        private Popup popup;
        private Button goToTodayButton;
        private ICommand gotoTodayCommand;
        #endregion
        #region Overrides
        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    textBox = this.GetTemplateChild("PART_TextBox") as DatePickerTextBox;
        //    popup = this.GetTemplateChild("PART_Popup") as Popup;

        //    if (AlternativeCalendarStyle != null)
        //    {
        //        System.Windows.Controls.Calendar calendar = popup.Child as System.Windows.Controls.Calendar;

        //        calendar.Style = AlternativeCalendarStyle;
        //        calendar.ApplyTemplate();

        //        goToTodayButton = calendar.Template.FindName("PART_GoToTodayButton", calendar) as Button;
        //        if (goToTodayButton != null)
        //        {
        //            gotoTodayCommand = new SimpleCommand(CanExecuteGoToTodayCommand, ExecuteGoToTodayCommand);
        //            goToTodayButton.Command = gotoTodayCommand;
        //        }
        //    }
        //    textBox.PreviewKeyDown -= new KeyEventHandler(DatePickerTextBox_PreviewKeyDown); //unhook
        //    textBox.PreviewKeyDown += new KeyEventHandler(DatePickerTextBox_PreviewKeyDown); //hook
        //}
        #endregion

        #region DPs
        #region AlternativeCalendarStyle

        /// <summary>
        /// AlternativeCalendarStyle Dependency Property
        /// </summary>
        //public static readonly DependencyProperty AlternativeCalendarStyleProperty =
        //    DependencyProperty.Register("AlternativeCalendarStyle", typeof(Style), typeof(DatePickerEx),
        //        new FrameworkPropertyMetadata((Style)null,
        //            new PropertyChangedCallback(OnAlternativeCalendarStyleChanged)));

        //public Style AlternativeCalendarStyle
        //{
        //    get { return (Style)GetValue(AlternativeCalendarStyleProperty); }
        //    set { SetValue(AlternativeCalendarStyleProperty, value); }
        //}

        //private static void OnAlternativeCalendarStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    DatePickerEx target = (DatePickerEx)d;
        //    target.ApplyTemplate();
        //}
        #endregion

        #endregion


        #region Command Hanlders

        //private bool CanExecuteGoToTodayCommand(object param)
        //{
        //    return goToTodayButton != null && !this.BlackoutDates.Contains(DateTime.Today);
        //}

        //private void ExecuteGoToTodayCommand(object param)
        //{
        //    this.SelectedDate = DateTime.Today;
        //}

        #endregion


        #region Private Methods




        private void DatePickerTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                int direction = e.Key == Key.Up ? 1 : -1;
                string currentDateText = Text;

                DateTime result;
                if (!DateTime.TryParse(Text, out result))
                    return;

                char delimeter = ' ';

                switch (this.SelectedDateFormat)
                {
                    case DatePickerFormat.Short: // dd/mm/yyyy
                        delimeter = '/';
                        break;
                    case DatePickerFormat.Long:  // day month  year
                        delimeter = ' ';
                        break;
                }

                int index = 3;
                bool foundIt = false;
                for (int i = Text.Length - 1; i >= 0; i--)
                {
                    if (Text[i] == delimeter)
                    {
                        --index;
                        if (textBox.CaretIndex > i)
                        {
                            foundIt = true;
                            break;
                        }
                    }
                }

                if (!foundIt)
                    index = 0;


                switch (index)
                {
                    case 0: // Day
                        result = result.AddDays(direction);
                        break;
                    case 1: // Month
                        result = result.AddMonths(direction);
                        break;
                    case 2: // Year
                        result = result.AddYears(direction);
                        break;
                }

                while (this.BlackoutDates.Contains(result))
                {
                    result = result.AddDays(direction);
                }


                DateTimeFormatInfo dtfi = DateTimeHelper.GetCurrentDateFormat();
                switch (this.SelectedDateFormat)
                {
                    case DatePickerFormat.Short:
                        this.Text = string.Format(CultureInfo.CurrentCulture, result.ToString(dtfi.ShortDatePattern, dtfi));
                        break;
                    case DatePickerFormat.Long:
                        this.Text = string.Format(CultureInfo.CurrentCulture, result.ToString(dtfi.LongDatePattern, dtfi));
                        break;
                }

                switch (index)
                {
                    case 1:
                        textBox.CaretIndex = Text.IndexOf(delimeter) + 1;
                        break;
                    case 2:
                        textBox.CaretIndex = Text.LastIndexOf(delimeter) + 1;
                        break;
                }


            }
        }
        #endregion
    }
}
