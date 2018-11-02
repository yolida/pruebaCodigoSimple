namespace FEICONT
{
	public class ProgressDialogSettings
	{
		public static ProgressDialogSettings WithLabelOnly = new ProgressDialogSettings(false, false, true);
		public static ProgressDialogSettings WithSubLabel = new ProgressDialogSettings(true, false, true);
		public static ProgressDialogSettings WithSubLabelAndCancel = new ProgressDialogSettings(true, true, true);
        //Cristhian|08/02/2018|FEICONT2-587
        /*Se agrega una configuración para mostrar el boton de cancelar proceso*/
        /*NUEVO INICIO*/
        public static ProgressDialogSettings WithCancel = new ProgressDialogSettings(false, true, true);
        /*NUEVO FIN*/

        public bool ShowSubLabel { get; set; }
		public bool ShowCancelButton { get; set; }
		public bool ShowProgressBarIndeterminate { get; set; }

		public ProgressDialogSettings()
		{
			ShowSubLabel = false;
			ShowCancelButton = false;
			ShowProgressBarIndeterminate = true;
		}

		public ProgressDialogSettings(bool showSubLabel, bool showCancelButton, bool showProgressBarIndeterminate)
		{
			ShowSubLabel = showSubLabel;
			ShowCancelButton = showCancelButton;
			ShowProgressBarIndeterminate = showProgressBarIndeterminate;
		}
	}
}
