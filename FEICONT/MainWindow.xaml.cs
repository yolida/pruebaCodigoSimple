using DataLayer.CRUD;
using FEICONT.ayuda;
using Extension.Base;
using Extension.Datos;
using FEICONT.pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables antiguas
        private clsEntityAlarms alarm;
        private clsEntityAlarms alarmRD;
        private clsEntityAlarms alarmRE;
        private clsBaseConexion conexion;
        private clsEntityDeclarant Empresa;
        private clsEntityDatabaseLocal DatabaseLocal;
        private clsEntityUsers Usuario;
        private clsEntityAccount Perfil;
        private int fracciones;
        private int horas;
        private DateTime hora_base;
        private string hora_procesada;
        private string hora_ahora;
        private int minutos;
        private List<string> Pendientes_envío;
        private List<List<string>> Pendientes_envío_ResumenDiario;
        private List<List<string>> Pendientes_envío_Retencion;
        private List<string> Pendientes_envío_enviar;
        private List<List<string>> Pendientes_envío_ResumenDiario_enviar;
        private List<List<string>> Pendientes_envío_Retencion_enviar;
        private System.Windows.Forms.NotifyIcon ntiEnvio = new System.Windows.Forms.NotifyIcon();
        private System.Windows.Forms.NotifyIcon ntiEnvioRD = new System.Windows.Forms.NotifyIcon();
        private System.Windows.Forms.NotifyIcon ntiEnvioRE = new System.Windows.Forms.NotifyIcon();
        private System.Windows.Forms.NotifyIcon ntiCertificadoDigital = new System.Windows.Forms.NotifyIcon();
        private bool CerrarAplicacion = true;
        private string currentDirectory = Directory.GetCurrentDirectory();
        private string fileExecutable;
        private int seleccionado = 0;
        private string seleccionadoMenu = "0";
        private System.Timers.Timer aTimerFactura = new System.Timers.Timer();
        private System.Timers.Timer aTimerEnvio = new System.Timers.Timer();
        private BackgroundWorker worker = new BackgroundWorker();
        private BackgroundWorker workerWeb = new BackgroundWorker();
        public string versionCompilado = "02.01.01";//Ahora lo maneja el modulo ModVers
        //public string buildCompilado = "15112017";//Ahora lo maneja el modulo ModVers
        #endregion Variables antiguas

        private bool disponible         =   false;
        private string mensaje          =   string.Empty;
        private string mensajeCabecera  =   string.Empty;
        Data_Usuario data_Usuario       =   new Data_Usuario();
        Data_DatosFox data_DatosFox;
        public MainWindow(int idUserEmpresa, string idUsuario)
        {
            InitializeComponent();
            data_Usuario.Read_Id_User_Empresa(idUserEmpresa);

            if (!string.IsNullOrEmpty(data_Usuario.IdUser_Empresa.ToString()) && data_Usuario.IdUser_Empresa != 0)
            {
                data_DatosFox   =   new Data_DatosFox(data_Usuario.IdDatosFox);
                data_DatosFox.Read_DatosFox();
                disponible      =   true;
                Data_Contribuyente data_Contribuyente   =   new Data_Contribuyente(data_DatosFox.IdEmisor);
                data_Contribuyente.Read_Contribuyente();
                lblEmpresa.Content  =   data_Contribuyente.NombreLegal;
                lblUsuario.Content  =   data_Usuario.IdUsuario;
            }
            else
                data_Usuario.IdUsuario  = idUsuario;

            if (disponible == false)    //  Validación inicial para la apertura de todos los reportes
            {
                mensaje         =   "Antes de proceder debe registrar los datos de acceso de emisor en 'Configuración de sistema -> Información del declarante' ";
                mensajeCabecera =   "Registro necesario";
            }

            string directory = Environment.CurrentDirectory;
            //clsBaseLog.cs_pxRegistarAdd(currentDirectory);
            //if (!File.Exists(currentDirectory + "\\lcl.txt"))
            //{
            //    fileExecutable = currentDirectory + "\\Srlc.exe";
            //    Process process = new Process();
            //    process.StartInfo.UseShellExecute = true;
            //    process.StartInfo.FileName = fileExecutable;
            //    process.StartInfo.CreateNoWindow = true;
            //    process.Start();
            //    process.Close();
            //}

            //worker = new BackgroundWorker()
            //{
            //    WorkerSupportsCancellation = true,
            //    WorkerReportsProgress = true
            //};
            //// worker.DoWork += worker_DoWork; // Se hará el desarrollo de las alarmas despues de terminar las prioridades
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            //workerWeb = new BackgroundWorker()
            //{
            //    WorkerSupportsCancellation = true,
            //    WorkerReportsProgress = true
            //};
            //workerWeb.DoWork += workerWeb_DoWork;
            //workerWeb.RunWorkerCompleted += workerWeb_RunWorkerCompleted;
            //string fecha = clsBaseLicencia.getFechaVencimiento();
            // lblAlerta.Content = fecha;
        }
        /// <summary>
        /// Metodo para determinar cambio de estado en los menus del lado izquierda.
        /// </summary>
        /// <param name="data"></param>
        public void EstadoMenu(bool estado, string origen)
        {
            if (origen == "1")
            {
                DatabaseLocal = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(Empresa.Cs_pr_Declarant_Id);
            }
            //Si estado es false ->bloquear el menu lateral.
            if (estado == false)
            {
                for (int i = 1; i <= 15; i++)
                {
                    string lblNameItem = "lblItem" + i.ToString();
                    Label objecto = (Label)FindName(lblNameItem);
                    objecto.IsEnabled = false;
                }
            }
            else
            {
                for (int i = 1; i <= 15; i++)
                {
                    string lblNameItem = "lblItem" + i.ToString();
                    Label objecto = (Label)FindName(lblNameItem);
                    objecto.IsEnabled = true;
                }
            }
        }
        //Evento de carga en la pagina principal
        private void mainForm_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void runWorkerSendWeb(object sender, EventArgs e)
        {
            if (!workerWeb.IsBusy)
            {
                workerWeb.RunWorkerAsync();
            }
        }
        private void runWorkerAlertas(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }

            //Timer facturas y notas asociadas
            /* DispatcherTimer dispatch = new DispatcherTimer();
             dispatch.Interval = new TimeSpan(0, 1, 0);
             dispatch.Tick += new System.EventHandler(this.tmrAlarmaFactura_Tick);
             dispatch.Start();
              //Timer resumen diario
            DispatcherTimer dispatchRD = new DispatcherTimer();
            dispatchRD.Interval = new TimeSpan(0, 1, 0);
            dispatchRD.Tick += new System.EventHandler(this.tmrAlarmaRD_Tick);
            dispatchRD.Start();

            //Timer retencion
            DispatcherTimer dispatchRE = new DispatcherTimer();
            dispatchRE.Interval = new TimeSpan(0, 1, 0);
            dispatchRE.Tick += new System.EventHandler(this.tmrAlarmaRE_Tick);
            dispatchRE.Start();

            //Timer envio de comprobantes a web
            DispatcherTimer dispatchWEB = new DispatcherTimer();
            dispatchWEB.Interval = new TimeSpan(0, 5, 0);
            dispatchWEB.Tick += new System.EventHandler(this.tmrInsertarAWeb_Tick);
            dispatchWEB.Start()
            
            */
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            tmrAlarmaFactura_Tick(sender, e);
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //do something
            }
            else
            {
                //do something else
            }
        }
        void workerWeb_DoWork(object sender, DoWorkEventArgs e)
        {
            tmrInsertarAWeb_Tick(sender, e);
        }
        void workerWeb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //do something
            }
            else
            {
                //do something else
            }
        }
        //Metodo que invoca el timer de insertar documentos en web.
        private void tmrInsertarAWeb_Tick(object sender, EventArgs e)
        {
            if (Empresa != null)
            {
                new clsEntityDatabaseWeb().cs_pxEnviarAWeb(Empresa);
            }
        }
        //Metodo que invoca el timer de verificar las alarmas de facturas.
        private void tmrAlarmaFactura_Tick(object sender, EventArgs e)
        {
            try
            {
                if (alarm != null && Empresa != null)
                {//Si la alarma esta definida
                    Pendientes_envío = new clsEntityDocument(DatabaseLocal).cs_pxObtenerPendientesEnvio();
                    //Enviar un número de veces al día automatico
                    if (alarm.Cs_pr_Envioautomatico == "T" && Pendientes_envío.Count > 0)
                    {
                        //hora_ahora = DateTime.Now.ToShortTimeString();
                        DateTime hora_actual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        DateTime hora_inicio = Convert.ToDateTime(alarm.Cs_pr_Envioautomatico_horavalor.Trim());
                        int comparados = DateTime.Compare(hora_actual, hora_inicio);
                        //si ambas horas son iguales realizar el envio
                        if (comparados >= 0)
                        {
                            if (comparados == 0)
                            {
                                ntiEnvio.Dispose();
                                ntiEnvio = new System.Windows.Forms.NotifyIcon();
                                ntiEnvio.Visible = true;
                                ntiEnvio.Icon = SystemIcons.Information;
                                ntiEnvio.BalloonTipText = "Se están enviando a SUNAT los comprobantes electrónicos pendientes.";
                                ntiEnvio.BalloonTipTitle = "Envío automático de comprobantes.";
                                ntiEnvio.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                ntiEnvio.ShowBalloonTip(1000);
                                cs_pxEntregarComprobantesAutomático();
                            }
                            else
                            {
                                fracciones = Convert.ToInt32(alarm.Cs_pr_Envioautomatico_minutosvalor);
                                horas = 24 / Convert.ToInt32(alarm.Cs_pr_Envioautomatico_minutosvalor);
                                hora_base = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                                //Recorrer las fracciones de tiempo para evaluar la alarma
                                for (int i = 0; i < fracciones; i++)
                                {
                                    hora_base = hora_base.AddHours(horas * i);
                                    hora_procesada = hora_base.ToString("yyyy-MM-dd HH:00:00").Trim();
                                    hora_ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00").Trim();
                                    //Si la hora a revisar es igual a la hora actual mostrar notificacion
                                    if (hora_ahora == hora_procesada)
                                    {
                                        ntiEnvio.Dispose();
                                        ntiEnvio = new System.Windows.Forms.NotifyIcon();
                                        ntiEnvio.Visible = true;
                                        ntiEnvio.Icon = SystemIcons.Information;
                                        ntiEnvio.BalloonTipText = "Se están enviando a SUNAT los comprobantes electrónicos pendientes.";
                                        ntiEnvio.BalloonTipTitle = "Envío automático de comprobantes.";
                                        ntiEnvio.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                        ntiEnvio.ShowBalloonTip(1000);
                                        cs_pxEntregarComprobantesAutomático();
                                        break;
                                    }
                                }
                            }

                        }
                    }
                    if (alarm.Cs_pr_Enviomanual == "T" && alarm.Cs_pr_Enviomanual_mostrarglobo_minutosvalor != "" && Pendientes_envío.Count > 0)
                    {


                        //Si es envio manual  y mostrar recordatorio cada cierto tiempo.
                        //Dividir los periodos de envio por hora.
                        DateTime hora_inicio = Convert.ToDateTime(alarm.Cs_pr_Enviomanual_mostrarglobo);
                        DateTime hora_actual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        int comparados = DateTime.Compare(hora_actual, hora_inicio);
                        //si ambas horas son iguales realizar el envio
                        if (comparados >= 0)
                        {
                            if (comparados == 0)
                            {
                                ntiEnvio.Dispose();
                                ntiEnvio = new System.Windows.Forms.NotifyIcon();
                                ntiEnvio.Visible = true;
                                ntiEnvio.Icon = SystemIcons.Information;
                                ntiEnvio.BalloonTipText = "Existen " + Pendientes_envío.Count.ToString() + " comprobantes electrónicos pendientes de envío a SUNAT.";
                                ntiEnvio.BalloonTipTitle = "Envío de comprobantes electrónicos.";
                                ntiEnvio.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                ntiEnvio.ShowBalloonTip(1000);

                            }
                            else
                            {
                                hora_base = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                                fracciones = Convert.ToInt32(alarm.Cs_pr_Enviomanual_mostrarglobo_minutosvalor);
                                minutos = 60 / Convert.ToInt32(alarm.Cs_pr_Enviomanual_mostrarglobo_minutosvalor);
                                //Por cada periodo de hora detectado se revisa si coincida la hora actual con el programado.
                                for (int i = 0; i < fracciones; i++)
                                {
                                    hora_procesada = hora_base.AddMinutes(minutos * i).ToString("yyyy-MM-dd HH:mm:00").Trim();
                                    hora_ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");
                                    //si las horas coinciden mostrar notificacion recordatoria.
                                    if (hora_ahora == hora_procesada)
                                    {
                                        ntiEnvio.Dispose();
                                        ntiEnvio = new System.Windows.Forms.NotifyIcon();
                                        ntiEnvio.Visible = true;
                                        ntiEnvio.Icon = SystemIcons.Information;
                                        ntiEnvio.BalloonTipText = "Existen " + Pendientes_envío.Count.ToString() + " comprobantes electrónicos pendientes de envío a SUNAT.";
                                        ntiEnvio.BalloonTipTitle = "Envío de comprobantes electrónicos.";
                                        ntiEnvio.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                        ntiEnvio.ShowBalloonTip(1000);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("proc fac ->" + ex.ToString());
            }

            try
            {
                //Alarma resumen diario
                if (alarmRD != null)
                {

                    Pendientes_envío_ResumenDiario = new clsEntitySummaryDocuments(DatabaseLocal).cs_pxObtenerPendientesEnvioRD();

                    //Caso de envio automatico con numero de veces por dia y que haya pendientes de envio.
                    if (alarmRD.Cs_pr_Envioautomatico == "T" && Pendientes_envío_ResumenDiario.Count > 0)
                    {
                        DateTime hora_actual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        DateTime hora_inicio = Convert.ToDateTime(alarmRD.Cs_pr_Envioautomatico_horavalor.Trim());
                        int comparados = DateTime.Compare(hora_actual, hora_inicio);
                        //si ambas horas son iguales realizar el envio
                        if (comparados >= 0)
                        {
                            if (comparados == 0)
                            {
                                ntiEnvioRD = new System.Windows.Forms.NotifyIcon();
                                ntiEnvioRD.Visible = true;
                                ntiEnvioRD.Icon = SystemIcons.Information;
                                ntiEnvioRD.BalloonTipText = "Se están agregando a resumen diarios los documentos pendientes.";
                                ntiEnvioRD.BalloonTipTitle = "Envío automático a resumen diario.";
                                ntiEnvioRD.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                ntiEnvioRD.ShowBalloonTip(1000);
                                cs_pxEntregarComprobantesAutomáticoRD();
                            }
                            else
                            {
                                //Se divide las horas del dia en fracciones de tiempo dadas.                
                                fracciones = Convert.ToInt32(alarmRD.Cs_pr_Envioautomatico_minutosvalor);
                                horas = 24 / Convert.ToInt32(alarmRD.Cs_pr_Envioautomatico_minutosvalor);
                                hora_base = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                                //Por cada fraccion se verifica si es igual al periodo actual.
                                for (int i = 0; i < fracciones; i++)
                                {
                                    hora_base = hora_base.AddHours(horas * i);
                                    hora_procesada = hora_base.ToString("yyyy-MM-dd HH:00:00").Trim();
                                    hora_ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00").Trim();
                                    //Si la hora a revisar es igual a la hora actual mostrar notificacion
                                    if (hora_ahora == hora_procesada)
                                    {
                                        ntiEnvioRD = new System.Windows.Forms.NotifyIcon();
                                        ntiEnvioRD.Visible = true;
                                        ntiEnvioRD.Icon = SystemIcons.Information;
                                        ntiEnvioRD.BalloonTipText = "Se están generando y enviando a SUNAT los resumenes diarios pendientes.";
                                        ntiEnvioRD.BalloonTipTitle = "Envío automático de resumen diario.";
                                        ntiEnvioRD.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                        ntiEnvioRD.ShowBalloonTip(1000);
                                        cs_pxEntregarComprobantesAutomáticoRD();
                                        break;
                                    }
                                }
                            }

                        }
                    }

                    //Caso de envio manual con notificaciones y pendientes de envio.
                    if (alarmRD.Cs_pr_Enviomanual == "T" && alarmRD.Cs_pr_Enviomanual_mostrarglobo_minutosvalor != "" && Pendientes_envío_ResumenDiario.Count > 0)
                    {
                        DateTime hora_inicio = Convert.ToDateTime(alarmRD.Cs_pr_Enviomanual_mostrarglobo);
                        DateTime hora_actual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        int comparados = DateTime.Compare(hora_actual, hora_inicio);
                        //si ambas horas son iguales realizar el envio
                        if (comparados >= 0)
                        {
                            if (comparados == 0)
                            {
                                ntiEnvioRD = new System.Windows.Forms.NotifyIcon();
                                ntiEnvioRD.Visible = true;
                                ntiEnvioRD.Icon = SystemIcons.Information;
                                ntiEnvioRD.BalloonTipText = "Existen " + Pendientes_envío_ResumenDiario.Count.ToString() + " resumenes diarios pendientes de envío a SUNAT.";
                                ntiEnvioRD.BalloonTipTitle = "Envío de resumen diario.";
                                ntiEnvioRD.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                ntiEnvioRD.ShowBalloonTip(1000);
                            }
                            else
                            {
                                //MOSTRAR GLOBO RECORDATORIO CADA CIERTO TIEMPO
                                //Dividir los periodos de envio por hora.
                                hora_base = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                                fracciones = Convert.ToInt32(alarmRD.Cs_pr_Enviomanual_mostrarglobo_minutosvalor);
                                minutos = 60 / Convert.ToInt32(alarmRD.Cs_pr_Enviomanual_mostrarglobo_minutosvalor);
                                //Por cada periodo de hora detectado se revisa si coincida la hora actual con el programado.
                                for (int i = 0; i < fracciones; i++)
                                {
                                    hora_procesada = hora_base.AddMinutes(minutos * i).ToString("yyyy-MM-dd HH:mm:00").Trim();
                                    hora_ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");
                                    //si las horas coinciden mostrar notificacion recordatoria.
                                    if (hora_ahora == hora_procesada)
                                    {
                                        ntiEnvioRD = new System.Windows.Forms.NotifyIcon();
                                        ntiEnvioRD.Visible = true;
                                        ntiEnvioRD.Icon = SystemIcons.Information;
                                        ntiEnvioRD.BalloonTipText = "Existen " + Pendientes_envío_ResumenDiario.Count.ToString() + " resumenes diarios pendientes de envío a SUNAT.";
                                        ntiEnvioRD.BalloonTipTitle = "Envío de resumen diario.";
                                        ntiEnvioRD.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                        ntiEnvioRD.ShowBalloonTip(1000);
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch { }

            try
            {
                //Alarma Retencion
                if (alarmRE != null)
                {
                    Pendientes_envío_Retencion = new clsEntityRetention(DatabaseLocal).cs_pxObtenerPendientesEnvio();
                    //Caso de envio automatico con numero de veces por dia y que haya pendientes de envio.
                    if (alarmRE.Cs_pr_Envioautomatico == "T" && Pendientes_envío_Retencion.Count > 0)
                    {

                        DateTime hora_actual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        DateTime hora_inicio = Convert.ToDateTime(alarmRE.Cs_pr_Envioautomatico_horavalor.Trim());
                        int comparados = DateTime.Compare(hora_actual, hora_inicio);
                        //si ambas horas son iguales realizar el envio
                        if (comparados >= 0)
                        {
                            if (comparados == 0)
                            {
                                ntiEnvioRE = new System.Windows.Forms.NotifyIcon();
                                ntiEnvioRE.Visible = true;
                                ntiEnvioRE.Icon = SystemIcons.Information;
                                ntiEnvioRE.BalloonTipText = "Se están enviando a SUNAT los comprobantes de retencion pendientes.";
                                ntiEnvioRE.BalloonTipTitle = "Envío automático de comprobantes de retencion.";
                                ntiEnvioRE.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                ntiEnvioRE.ShowBalloonTip(1000);
                                cs_pxEntregarComprobantesAutomáticoRE();
                            }
                            else
                            {
                                //Se divide las horas del dia en fracciones de tiempo dadas.                
                                fracciones = Convert.ToInt32(alarmRE.Cs_pr_Envioautomatico_minutosvalor);
                                horas = 24 / Convert.ToInt32(alarmRE.Cs_pr_Envioautomatico_minutosvalor);
                                //Por cada fraccion se verifica si es igual al periodo actual.
                                for (int i = 0; i < fracciones; i++)
                                {
                                    hora_base = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                                    hora_base = hora_base.AddHours(horas * i);
                                    hora_procesada = hora_base.ToString("yyyy-MM-dd HH:00:00").Trim();
                                    hora_ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00").Trim();
                                    //Si la hora a revisar es igual a la hora actual mostrar notificacion
                                    if (hora_ahora == hora_procesada)
                                    {
                                        ntiEnvioRE = new System.Windows.Forms.NotifyIcon();
                                        ntiEnvioRE.Visible = true;
                                        ntiEnvioRE.Icon = SystemIcons.Information;
                                        ntiEnvioRE.BalloonTipText = "Se están enviando a SUNAT los comprobantes de retencion pendientes.";
                                        ntiEnvioRE.BalloonTipTitle = "Envío automático de comprobantes de retencion.";
                                        ntiEnvioRE.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                        ntiEnvioRE.ShowBalloonTip(1000);
                                        cs_pxEntregarComprobantesAutomáticoRE();
                                        break;
                                    }
                                }
                            }

                        }
                    }
                    //Caso de envio manual con notificaciones y pendientes de envio.
                    if (alarmRE.Cs_pr_Enviomanual == "T" && alarmRE.Cs_pr_Enviomanual_mostrarglobo_minutosvalor != "" && Pendientes_envío_Retencion.Count > 0)
                    {
                        DateTime hora_inicio = Convert.ToDateTime(alarmRE.Cs_pr_Enviomanual_mostrarglobo);
                        DateTime hora_actual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        int comparados = DateTime.Compare(hora_actual, hora_inicio);
                        //si ambas horas son iguales realizar el envio
                        if (comparados >= 0)
                        {
                            if (comparados == 0)
                            {
                                ntiEnvioRE = new System.Windows.Forms.NotifyIcon();
                                ntiEnvioRE.Visible = true;
                                ntiEnvioRE.Icon = SystemIcons.Information;
                                ntiEnvioRE.BalloonTipText = "Existen " + Pendientes_envío_Retencion.Count.ToString() + "comprobantes de retencion pendientes de envío a SUNAT.";
                                ntiEnvioRE.BalloonTipTitle = "Envío comprobantes de retención .";
                                ntiEnvioRE.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                ntiEnvioRE.ShowBalloonTip(1000);
                            }
                            else
                            {
                                //MOSTRAR GLOBO RECORDATORIO CADA CIERTO TIEMPO
                                //Dividir los periodos de envio por hora.
                                hora_base = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                                fracciones = Convert.ToInt32(alarmRE.Cs_pr_Enviomanual_mostrarglobo_minutosvalor);
                                minutos = 60 / Convert.ToInt32(alarmRE.Cs_pr_Enviomanual_mostrarglobo_minutosvalor);
                                //Por cada periodo de hora detectado se revisa si coincida la hora actual con el programado.
                                for (int i = 0; i < fracciones; i++)
                                {
                                    hora_procesada = hora_base.AddMinutes(minutos * i).ToString("yyyy-MM-dd HH:mm:00").Trim();
                                    hora_ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");
                                    //si las horas coinciden mostrar notificacion recordatoria.
                                    if (hora_ahora == hora_procesada)
                                    {
                                        ntiEnvioRE = new System.Windows.Forms.NotifyIcon();
                                        ntiEnvioRE.Visible = true;
                                        ntiEnvioRE.Icon = SystemIcons.Information;
                                        ntiEnvioRE.BalloonTipText = "Existen " + Pendientes_envío_Retencion.Count.ToString() + "comprobantes de retención pendientes de envío a SUNAT.";
                                        ntiEnvioRE.BalloonTipTitle = "Envío comprobantes de retención.";
                                        ntiEnvioRE.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                        ntiEnvioRE.ShowBalloonTip(1000);
                                        break;
                                    }
                                }
                            }

                        }
                    }

                }
            }
            catch (Exception) { }
        }

        //Metodo para enviar comprobantes de facturas de manera automatica.
        private void cs_pxEntregarComprobantesAutomático()
        {
            try
            {
                if (Empresa != null)
                {
                    Pendientes_envío_enviar = new clsEntityDocument(DatabaseLocal).cs_pxObtenerPendientesEnvio();
                    foreach (var item in Pendientes_envío_enviar)
                    {
                        /* if (item[3].ToString() != "03" && item[9] != "03")
                         {*/
                        clsBaseSunat sunat = new clsBaseSunat(DatabaseLocal);
                        sunat.cs_pxEnviarCE(item, false);
                        // }
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR15", ex.Message);
                clsBaseLog.cs_pxRegistarAdd("Envio automatico facturas y notas" + ex.Message);
            }
        }
        //Metodo para enviar comprobantes resumenes diarios de manera automatica.
        private void cs_pxEntregarComprobantesAutomáticoRD()
        {
            try
            {
                if (Empresa != null)
                {
                    //agregar los comprobantes pendientes a sus resumenes
                    //--buscar los pendientes
                    //Obtener los comprobante a enviar en resumen diario.
                    Pendientes_envío_ResumenDiario_enviar = new clsEntitySummaryDocuments(DatabaseLocal).cs_pxObtenerPendientesEnvioRD();
                    foreach (var item in Pendientes_envío_ResumenDiario_enviar)
                    {
                        clsBaseSunat sunat = new clsBaseSunat(DatabaseLocal);
                        sunat.cs_pxEnviarRC(item[0], false);
                    }
                }

            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR15", ex.Message);
                clsBaseLog.cs_pxRegistarAdd("Envio de resumenes diarios automatico" + ex.Message);
            }
        }
        private void cs_pxEntregarComprobantesAutomáticoRE()
        {
            try
            {
                if (Empresa != null)
                {
                    //Obtener los comprobante a enviar en resumen diario.
                    Pendientes_envío_Retencion_enviar = new clsEntityRetention(DatabaseLocal).cs_pxObtenerPendientesEnvio();
                    foreach (var item in Pendientes_envío_Retencion_enviar)
                    {
                        clsBaseSunat sunat = new clsBaseSunat(DatabaseLocal);
                        sunat.cs_pxEnviarCERetention(item[0], false);
                    }
                }

            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR15", ex.Message);
                clsBaseLog.cs_pxRegistarAdd("Envio de retencion automatico" + ex.Message);
            }
        }
        //Metodo para reiniciar parametros de conexion.
        private void cs_pxReiniciarConexión()
        {
            if (Empresa != null)
            {
                conexion = new clsBaseConexion(DatabaseLocal);
            }
        }
        //Metodo para reiniciar permisos en la aplicacion
        private void cs_pxReiniciar()
        {
            //Verificar la base de datos de esta empresa.
            //Leer la configuración de la base de datos de esta empresa
            //Solo se puede crear una base de datos si existe la empresa.
            bool aux_conexionestado = conexion.cs_fxConexionEstado();
            //Si la conexion no esta establecida
            if (aux_conexionestado.Equals(false))
            {
                EstadoMenu(false, "0");
            }

            //Si la conexion esta establecida.
            if (aux_conexionestado.Equals(true))
            {

                clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(DatabaseLocal.Cs_pr_Declarant_Id);
                bool aux_existerutacertificadodigital = File.Exists(declarante.Cs_pr_Rutacertificadodigital);

                //Cristhian|17/08/2017|FEI2-323
                /*Codigo para mostrar la alerta de vencimiento del Certificado Digital*/
                /*MODIFICACIóN INICIO*/
                /*Se verifica la existencia del certificado digital de la empresa*/
                if (aux_existerutacertificadodigital == true)
                {
                    try
                    {
                        ntiCertificadoDigital.Dispose();
                        ntiCertificadoDigital = new System.Windows.Forms.NotifyIcon();
                        ntiCertificadoDigital.Visible = true;
                        ntiCertificadoDigital.Icon = SystemIcons.Information;

                        X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(declarante.Cs_pr_Rutacertificadodigital), declarante.Cs_pr_Parafrasiscertificadodigital);
                        //verificar fechas de vencimiento si ya vencio mostrar mensaje 
                        DateTime fechaVencimientoCertificado = cert.NotAfter;
                        DateTime fechaHoy = DateTime.Now;
                        int result = DateTime.Compare(fechaHoy, fechaVencimientoCertificado);
                        if (result >= 0)
                        {
                            /*Si el certificado ya se vencio entonces se muestra la notificacion con el siguiente mensaje*/
                            MessageBox.Show("Su certificado digital ha expirado.Para renovar su certificado digital contáctese con su proveedor.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                        else
                        {
                            /*Se verifica si la alerta del Certificado Digital esta activado*/
                            if (declarante.Cs_pr_Alerta_Dias != "")
                            {
                                /*Se obtiene el numero de dias que se establece en la alerta del Certificado Digital*/
                                int DiasDeAlerta = Convert.ToInt32(declarante.Cs_pr_Alerta_Dias);

                                /*Se obtiene la diferencia de Dias, Horas y Minutos entre dos fechas. Se pone la fecha del certificado primero
                                ya uqe se supone es de una fecha superior a la actual*/
                                TimeSpan ts = fechaVencimientoCertificado - fechaHoy;

                                //Solo necesitamos la diferencia de dias.
                                int DiferenciaDeDias = ts.Days;

                                /*Si la diferencia de dias es igual o es menor a los dias de alerta establecido en el sistema,
                                entonces se mostrara el numero de dias que tiene para renovar su certificado digital*/
                                if (DiferenciaDeDias < DiasDeAlerta + 1 && DiferenciaDeDias > 0)
                                {
                                    /*Este mensaje se mostrara cuando llege la fecha de la alerta en adelante, hasta un dia antes de 
                                      la expiracion del certificado*/
                                    ntiCertificadoDigital.BalloonTipText = "Su certificado digital esta pronto a expirar. Le quedan " + DiferenciaDeDias + " dias para poder darlo de alta.Para renovar su certificado digital contáctese con su proveedor.";
                                    ntiCertificadoDigital.BalloonTipTitle = "Aviso de Certificado Digital";
                                    ntiCertificadoDigital.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
                                    ntiCertificadoDigital.ShowBalloonTip(1000);
                                }
                                /*Si la diferencia de dias es igual a 0 entonces se mostrara un mensaje donde se indica que su certificado digital vence Hoy*/
                                else if (DiferenciaDeDias == 0)
                                {
                                    /*Este mensaje se mostrara cuando llege el ultimo dia de la expiración del certificado digital*/
                                    ntiCertificadoDigital.BalloonTipText = "Su certificado digital ¡Vence HOY!. Para renovar su certificado digital contáctese con su proveedor.";
                                    ntiCertificadoDigital.BalloonTipTitle = "Aviso de Certificado Digital";
                                    ntiCertificadoDigital.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
                                    ntiCertificadoDigital.ShowBalloonTip(1000);
                                }
                            }
                            else
                            {
                                /*Este mensaje se mostrara cuando no se halla establecido la alerta para el certificado digital*/
                                ntiCertificadoDigital.BalloonTipText = "No tiene establecido su alerta de Certificado Digital.";
                                ntiCertificadoDigital.BalloonTipTitle = "Aviso de Certificado Digital";
                                ntiCertificadoDigital.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                                ntiCertificadoDigital.ShowBalloonTip(1000);
                            }
                        }
                        /*MODIFICACIóN FIN*/

                        bool servidorBeta = new clsBaseSunat(DatabaseLocal).isServidorBeta(declarante);

                        /*Llamando al modulo para obtener los Titulos del Formulario Principal*/
                        Extension.ModVers Titulo = new Extension.ModVers();
                        List<string> Titulo_Formulario = new List<string>();
                        Titulo_Formulario = Titulo.Cabecera_Ventana();

                        if (servidorBeta == true)
                        {
                            Title = Titulo_Formulario[0];
                        }
                        else
                        {
                            Title = Titulo_Formulario[1];
                        }
                    }
                    catch (Exception)
                    {
                        EstadoMenu(false, "0");
                        clsBaseMensaje.cs_pxMsgEr("ERR17", "La ubicación o la contraseña del certificado digital (*.pfx) no es válido.");
                    }
                }
                else
                {
                    EstadoMenu(false, "0");
                    clsBaseMensaje.cs_pxMsgEr("ERR17", "La ubicación o la contraseña del certificado digital (*.pfx) no es válido.");
                }
            }
        }
        //Evento de cerrar sesion de usuario.
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            aTimerEnvio.Close();
            aTimerFactura.Close();
            //aTimerResumen.Close();
            //aTimerRetencion.Close();
            CerrarAplicacion = false;
            Close();
        }
        //Evento de carga en la pagina incrustada dentro la ventana principal.
        private void pageContainer_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                (e.Content as Configuracion_bdlocal).Tag = this;
            }
            catch
            { }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (CerrarAplicacion)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
        private void seleccionarItem(int item, string idMenu)
        {
            try
            {
                seleccionadoMenu = idMenu;
                if (seleccionado != 0)
                {
                    string lblNameItemAnterior = "lblItem" + seleccionado.ToString();
                    Label objectoAnterior = (Label)FindName(lblNameItemAnterior);
                    objectoAnterior.Background = System.Windows.Media.Brushes.Transparent;
                }

                var converter = new System.Windows.Media.BrushConverter();
                var brush = (System.Windows.Media.Brush)converter.ConvertFromString("#0d3f7c");
                string lblNameItem = "lblItem" + item.ToString();
                Label objecto = (Label)FindName(lblNameItem);
                objecto.Background = brush;
                seleccionado = item;
            }
            catch
            {

            }
        }
        private void selectItem(int item, int total)
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (System.Windows.Media.Brush)converter.ConvertFromString("#0d3f7c");
            for (int i = 1; i <= total; i++)
            {
                string lblNameItem = "lblItem" + i.ToString();

                Label objecto = (Label)FindName(lblNameItem);
                if (i == item)
                {
                    objecto.Background = brush;
                }
                else
                {
                    objecto.Background = System.Windows.Media.Brushes.Transparent;
                }
            }
        }
        private void clkReporteFactura(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("01", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    //this.pageContainer.Source = new Uri("pages/Reporte_Factura.xaml", UriKind.RelativeOrAbsolute);
                //    Reporte_Factura repSunat = new Reporte_Factura(DatabaseLocal);
                //    this.pageContainer.Navigate(repSunat);
                //    seleccionarItem(1, "4");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(1, "4");
                //}
                ////selectItem(1, 16);
            }
            else
            {
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void clkReporteResumen(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("02", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    //this.pageContainer.Source = new Uri("pages/Reporte_Resumen.xaml", UriKind.RelativeOrAbsolute);
                //    Reporte_Resumen repResumen = new Reporte_Resumen(DatabaseLocal);
                //    this.pageContainer.Navigate(repResumen);
                //    seleccionarItem(2, "5");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(2, "5");
                //}
                //selectItem(2, 16);
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkReporteBoleta(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("03", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    // this.pageContainer.Source = new Uri("pages/Reporte_Boleta.xaml", UriKind.RelativeOrAbsolute);
                //    Reporte_Boleta repBoleta = new Reporte_Boleta(DatabaseLocal);
                //    this.pageContainer.Navigate(repBoleta);
                //    seleccionarItem(3, "6");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(3, "6");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkReporteBaja(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("04", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    Reporte_ComunicacionBaja repComunicacionBaja = new Reporte_ComunicacionBaja(DatabaseLocal);
                //    this.pageContainer.Navigate(repComunicacionBaja);
                //    //this.pageContainer.Source = new Uri("pages/Reporte_ComunicacionBaja.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(4, "7");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(4, "7");
                //}
                // selectItem(4, 16);
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkReporteRetencion(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("05", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    Reporte_Retencion repRetencion = new Reporte_Retencion(DatabaseLocal);
                //    this.pageContainer.Navigate(repRetencion);
                //    //this.pageContainer.Source = new Uri("pages/Reporte_Retencion.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(5, "8");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(5, "8");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkReportGeneral(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("06", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    Reporte_General repGeneral = new Reporte_General(DatabaseLocal);
                //    this.pageContainer.Navigate(repGeneral);
                //    // this.pageContainer.Source = new Uri("pages/Reporte_General.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(6, "9");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(6, "9");
                //}
                // selectItem(5, 16);
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkFacturaSunat(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                Factura_Sunat factura_Sunat = new Factura_Sunat(this, data_Usuario);
                pageContainer.Navigate(factura_Sunat);
                seleccionarItem(7, "10");
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkFacturaAlerta(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("08", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    Factura_Alerta facAlerta = new Factura_Alerta(alarm);
                //    this.pageContainer.Navigate(facAlerta);
                //    seleccionarItem(8, "11");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(8, "11");
                //}
                //  selectItem(7, 16);
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkResumenDiarioGenerar(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                ResumenDiario_Generar resDiarioGen = new ResumenDiario_Generar(this, DatabaseLocal);
                this.pageContainer.Navigate(resDiarioGen);
                seleccionarItem(9, "12");

                //bool permitido = clsEntityPermisos.accesoPermitido("09", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    // this.pageContainer.Source = new Uri("pages/ResumenDiario_Generar.xaml", UriKind.RelativeOrAbsolute);
                //    ResumenDiario_Generar resDiarioGen = new ResumenDiario_Generar(this, DatabaseLocal);
                //    this.pageContainer.Navigate(resDiarioGen);
                //    seleccionarItem(9, "12");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(9, "12");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkResumenDiarioSunat(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("10", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    ResumenDiario_Sunat resDiarioSun = new ResumenDiario_Sunat(this, DatabaseLocal);
                //    this.pageContainer.Navigate(resDiarioSun);
                //    // this.pageContainer.Source = new Uri("pages/ResumenDiario_Sunat.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(10, "13");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(10, "13");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkResumenDiarioAlerta(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("11", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    if (Empresa != null)
                //    {
                //        ResumenDiario_Alerta rdalerta = new ResumenDiario_Alerta(alarmRD, Empresa.Cs_pr_Declarant_Id);
                //        this.pageContainer.Navigate(rdalerta);
                //        seleccionarItem(11, "14");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Debe iniciar sesión con una empresa para poder configurar esta opción.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    }
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(11, "14");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkComunicacionGenerar(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                GenerarComunicacionBaja generarComunicacionBaja =   new GenerarComunicacionBaja(this, data_Usuario);
                this.pageContainer.Navigate(generarComunicacionBaja);
                seleccionarItem(12, "15");
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkComunicacionSunat(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("13", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    ComunicacionBaja_Sunat comBajaSunat = new ComunicacionBaja_Sunat(this, DatabaseLocal);
                //    this.pageContainer.Navigate(comBajaSunat);
                //    // this.pageContainer.Source = new Uri("pages/ComunicacionBaja_Sunat.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(13, "16");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(13, "16");
                //}
                //  selectItem(12, 16);
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkRetencionEnvio(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("14", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    Retencion_Sunat retSunat = new Retencion_Sunat(DatabaseLocal, this);
                //    this.pageContainer.Navigate(retSunat);
                //    //this.pageContainer.Source = new Uri("pages/Retencion_Sunat.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(14, "17");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(14, "17");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkRetencionAlertas(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("15", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    if (Empresa != null)
                //    {
                //        Retencion_Alerta rdalerta = new Retencion_Alerta(alarmRE, Empresa.Cs_pr_Declarant_Id);
                //        this.pageContainer.Navigate(rdalerta);
                //        seleccionarItem(15, "18");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Debe iniciar sesión con una empresa para poder configurar esta opción.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    }
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(15, "18");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkConfiguracionEmpresa(object sender, RoutedEventArgs e)
        {
            Configuracion_declarante configuracion_Declarante = new Configuracion_declarante(data_Usuario);
            this.pageContainer.Navigate(configuracion_Declarante);
            seleccionarItem(18, "23");
        }
        private void clkConfiguracionUsuario(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("19", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    this.pageContainer.Source = new Uri("pages/Configuracion_usuarios.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(19, "24");
                //}
                //else
                //{
                //    // MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(19, "24");
                //}
                //   selectItem(16, 16);
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkConfiguracionPermisos(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("20", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    if (Empresa != null)
                //    {
                //        Configuracion_permisos confPermisosUsuario = new Configuracion_permisos(Empresa.Cs_pr_Declarant_Id, this);
                //        this.pageContainer.Navigate(confPermisosUsuario);
                //        seleccionarItem(20, "25");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Debe iniciar sesión con una empresa para poder configurar esta opción.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    }
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(20, "25");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkAcercaDe(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                seleccionarItem(27, "0");

                /*Llamando al modulo para obtener la version y el build del Compilado*/
                Extension.ModVers VersionFEI = new Extension.ModVers();

                AcercaDe acercaDe = new AcercaDe(VersionFEI.Vers_Compilado(), VersionFEI.Build_Compilado());
                acercaDe.ShowDialog();
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkConfiguracionGenbackup(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("21", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    if (Empresa != null)
                //    {
                //        Utilitario_GeneracionBackup utilGenBackup = new Utilitario_GeneracionBackup(Empresa, this);
                //        this.pageContainer.Navigate(utilGenBackup);
                //        seleccionarItem(21, "26");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Debe iniciar sesión con una empresa para poder configurar esta opción.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    }
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(21, "26");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkConfiguracionRestbackup(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("22", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    if (Empresa != null)
                //    {
                //        Utilitario_RestauracionBackup utilRestBackup = new Utilitario_RestauracionBackup(Empresa, this);
                //        this.pageContainer.Navigate(utilRestBackup);
                //        seleccionarItem(22, "27");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Debe iniciar sesión con una empresa para poder configurar esta opción.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    }
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(22, "27");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkConfiguracionRuta(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("23", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    seleccionarItem(23, "28");
                //    RutaArchivo frmRuta = new RutaArchivo();
                //    frmRuta.ShowDialog();

                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(23, "28");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkConfiguracionEstructura(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("24", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    seleccionarItem(24, "29");
                //    if (Empresa != null)
                //    {
                //        if (System.Windows.Forms.MessageBox.Show("Se va proceder a verificar la estructura de la base de datos configurada para la sesión actual y actualizarla. ¿Está seguro que desea proceder con la operación?", "¿Está seguro?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                //        {
                //            new Loading(Empresa).ShowDialog();
                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show("Debe iniciar sesión con una empresa para poder realizar esta acción.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    }
                //}
                //else
                //{
                //    // MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(24, "29");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkConfiguracionLicencia(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("25", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    seleccionarItem(25, "30");
                //    // string directory = Environment.CurrentDirectory;
                //    StreamReader sr = new StreamReader(currentDirectory + "\\lcl.txt", System.Text.Encoding.Default);
                //    string textoPeticion = sr.ReadToEnd();
                //    sr.Close();
                //    bool existe = new clsBaseLicencia().licenceExists();
                //    // bool activo = clsBaseLicencia.licenceActive(DateTime.Now.ToString("yyyy-MM-dd"));
                //    if (existe)
                //    {
                //        string[] datosLicencia = new clsBaseLicencia().getDatosLicencia();
                //        if (datosLicencia != null)
                //        {
                //            string fecha = new clsBaseLicencia().getFechaVencimiento();
                //            string tipoLicencia = datosLicencia[6];
                //            string versionLicencia = datosLicencia[3];
                //            string codigoPeticion = textoPeticion;
                //            string codigoActivacion = datosLicencia[0];
                //            Licencia lic = new Licencia(codigoActivacion, fecha, tipoLicencia, codigoPeticion, versionLicencia);
                //            lic.ShowDialog();
                //        }
                //        else
                //        {
                //            MessageBox.Show("Ha ocurrido un error al cargar los datos de la licencia. Contáctese con su proveedor");
                //        }
                //    }
                //    else
                //    {
                //        FEI.Extension.ModVers Version_Compilado = new Extension.ModVers();
                //        versionCompilado = Version_Compilado.Vers_Compilado();
                //        Activacion frmActivacion = new Activacion(textoPeticion, versionCompilado);
                //        frmActivacion.ShowDialog();
                //        if (frmActivacion.DialogResult.HasValue && frmActivacion.DialogResult.Value)
                //        {
                //            // MessageBox.Show("Se ha realizado la activacion");
                //        }
                //    }
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(25, "30");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkAyuda(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //seleccionarItem(26,"0");
                AyudaPrincipal ayuda = new AyudaPrincipal("0");
                ayuda.ShowDialog();
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkReversionGenerar(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("28", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    if (Empresa != null)
                //    {
                //        ReversionGenerar_Retencion reversionGenerarRetencion = new ReversionGenerar_Retencion(DatabaseLocal, this);
                //        this.pageContainer.Navigate(reversionGenerarRetencion);
                //        seleccionarItem(28, "19");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Debe iniciar sesión con una empresa para poder configurar esta opción.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    }
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(28, "19");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkReversionSunat(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("29", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    ReversionSunat_Retencion creversionCRESunat = new ReversionSunat_Retencion(this, DatabaseLocal);
                //    this.pageContainer.Navigate(creversionCRESunat);
                //    // this.pageContainer.Source = new Uri("pages/ComunicacionBaja_Sunat.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(29, "20");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(29, "20");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void main_Form_KeyDown(object sender, KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                if (seleccionadoMenu == "0")
                {

                    AyudaPrincipal ayuda = new AyudaPrincipal("0");
                    ayuda.ShowDialog();
                }
                else
                {

                    if (pageContainer.IsKeyboardFocusWithin == false)
                    {
                        AyudaPrincipal ayuda = new AyudaPrincipal(seleccionadoMenu);
                        ayuda.ShowDialog();
                    }
                    else
                    {

                    }

                }
            }
        }

        private void clkReceptorValidar(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("30", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    Receptor_Validar receptorValidar = new Receptor_Validar(DatabaseLocal, this, Empresa.Cs_pr_Ruc);
                //    this.pageContainer.Navigate(receptorValidar);
                //    // this.pageContainer.Source = new Uri("pages/ComunicacionBaja_Sunat.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(30, "20");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(30, "20");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkReceptorCompras(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("31", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    Receptor_Compras receptorCompras = new Receptor_Compras(DatabaseLocal, this, Empresa.Cs_pr_Ruc);
                //    this.pageContainer.Navigate(receptorCompras);
                //    // this.pageContainer.Source = new Uri("pages/ComunicacionBaja_Sunat.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(31, "20");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(31, "20");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkTransferenciaDatos(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("32", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{

                //    if (Empresa != null)
                //    {
                //        Process myProcess = new Process();
                //        string RutaInstalacion = Directory.GetCurrentDirectory();

                //        string ArchivoEjecutable = RutaInstalacion + "\\TransData.exe";
                //        myProcess.StartInfo.UseShellExecute = true;
                //        myProcess.StartInfo.FileName = ArchivoEjecutable;
                //        myProcess.StartInfo.Arguments = Empresa.Cs_pr_Declarant_Id;
                //        myProcess.StartInfo.CreateNoWindow = true;
                //        myProcess.Start();

                //    }
                //    else
                //    {
                //        MessageBox.Show("Debe iniciar sesión con una empresa para poder configurar esta opción.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    }
                //    /* Receptor_Compras receptorCompras = new Receptor_Compras(DatabaseLocal, this, Empresa.Cs_pr_Ruc);
                //     this.pageContainer.Navigate(receptorCompras);
                //    */
                //    seleccionarItem(32, "20");
                //}
                //else
                //{
                //    seleccionarItem(32, "20");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkCertificadoDigital(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("33", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    CertificadoDigital CertificadoDigital = new CertificadoDigital();
                //    this.pageContainer.Navigate(CertificadoDigital);

                //    seleccionarItem(33, "20");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(33, "20");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void clkAyudaSunat(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                try
                {
                    string carpeta_de_PDF = Directory.GetCurrentDirectory();
                    System.Diagnostics.Process.Start(carpeta_de_PDF + "\\PDF\\Listado-Errores-SUNAT.pdf");

                    seleccionarItem(34, "20");
                }
                catch (Exception ex)
                {
                    clsBaseLog.cs_pxRegistarAdd("Ver Lista Errores SUNAT: " + ex.ToString());
                }
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        /*Creado para la Opcion de Validar XML*/
        private void clkReceptorVerificar(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                //bool permitido = clsEntityPermisos.accesoPermitido("35", Perfil.Cs_pr_Account_Id, Perfil.Cs_pr_Users_Id);
                //if (permitido)
                //{
                //    Receptor_Validar receptorValidar = new Receptor_Validar(DatabaseLocal, this, Empresa.Cs_pr_Ruc);
                //    this.pageContainer.Navigate(receptorValidar);
                //    // this.pageContainer.Source = new Uri("pages/ComunicacionBaja_Sunat.xaml", UriKind.RelativeOrAbsolute);
                //    seleccionarItem(35, "20");
                //}
                //else
                //{
                //    //MessageBox.Show("No tiene permiso para acceder a este módulo.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    seleccionarItem(35, "20");
                //}
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void btnConfigurarDB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void linkRevisionLogs_Click(object sender, RoutedEventArgs e)
        {
            if (disponible)
            {
                RevisionLogs revisionLogs   =   new RevisionLogs(data_Usuario);
                    pageContainer.Navigate(revisionLogs);
            }
            else
                MessageBox.Show(mensaje, mensajeCabecera, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
