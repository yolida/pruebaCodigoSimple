using FEICONT.ayuda;
using FEICONT.Base;
using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Alertas de resumen diario.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para ResumenDiario_Alerta.xaml
    /// </summary>
    public partial class ResumenDiario_Alerta : Page
    {
        List<ComboBoxPares> veces_al_dia = new List<ComboBoxPares>();
        List<ComboBoxPares> veces_por_hora = new List<ComboBoxPares>();
        private clsEntityAlarms entidad_alarma;
        private string declarante_id;
        private bool exist;
        //Metodo constructor
        public ResumenDiario_Alerta(clsEntityAlarms alarm, string IdDeclarante)
        {
            try
            {
                InitializeComponent();
                declarante_id = IdDeclarante;
                //Asiganar valores al combobox de veces al dia.
                veces_al_dia.Add(new ComboBoxPares("2", "2"));
                veces_al_dia.Add(new ComboBoxPares("3", "3"));
                veces_al_dia.Add(new ComboBoxPares("4", "4"));
                veces_al_dia.Add(new ComboBoxPares("6", "6"));
                veces_al_dia.Add(new ComboBoxPares("24", "24"));
                cboVecesDia.DisplayMemberPath = "_Value";
                cboVecesDia.SelectedValuePath = "_key";
                cboVecesDia.SelectedIndex = 0;
                cboVecesDia.ItemsSource = veces_al_dia;
                //Asignar valores al combobox de veces por hora.
                veces_por_hora.Add(new ComboBoxPares("2", "2"));
                veces_por_hora.Add(new ComboBoxPares("3", "3"));
                veces_por_hora.Add(new ComboBoxPares("4", "4"));
                veces_por_hora.Add(new ComboBoxPares("6", "6"));
                veces_por_hora.Add(new ComboBoxPares("60", "60"));
                cboVecesHora.DisplayMemberPath = "_Value";
                cboVecesHora.SelectedValuePath = "_key";
                cboVecesHora.SelectedIndex = 0;
                cboVecesHora.ItemsSource = veces_por_hora;
                //cboVecesDia.SelectedIndex = 0;
                //cboVecesHora.SelectedIndex = 0;
                //Si alarma no esta definida.
                if (alarm == null)
                {
                    exist = false;
                    chkEnvioManual.IsChecked = true;
                    //rbtEnviomanual_nomostrarglobo.IsChecked = true;
                }
                else
                {
                    //Si alarma esta definida.
                    exist = true;
                    entidad_alarma = alarm;

                    this.chkEnvioAutomatico.IsChecked = clsBaseUtil.cs_fxStringToBoolean(entidad_alarma.Cs_pr_Envioautomatico);
                    // this.rbtEnvioautomatico_minutos.IsChecked = clsBaseUtil.cs_fxStringToBoolean(entidad_alarma.Cs_pr_Envioautomatico_minutos);
                    //Buscar de acuerdo a la alarma definida el valor a  seleccionar. 
                    switch (entidad_alarma.Cs_pr_Envioautomatico_minutosvalor)
                    {
                        case "2":
                            this.cboVecesDia.SelectedIndex = 0;
                            break;
                        case "3":
                            this.cboVecesDia.SelectedIndex = 1;
                            break;
                        case "4":
                            this.cboVecesDia.SelectedIndex = 2;
                            break;
                        case "6":
                            this.cboVecesDia.SelectedIndex = 3;
                            break;
                        case "24":
                            this.cboVecesDia.SelectedIndex = 4;
                            break;
                        default:
                            this.cboVecesDia.SelectedIndex = 0;
                            break;
                    }

                    // this.rbtEnvioautomatico_hora.IsChecked = clsBaseUtil.cs_fxStringToBoolean(entidad_alarma.Cs_pr_Envioautomatico_hora);
                    if (entidad_alarma.Cs_pr_Envioautomatico_horavalor != "")
                    {
                        try
                        {
                            this.dtpEnvioautomatico_horavalor.Value = Convert.ToDateTime(entidad_alarma.Cs_pr_Envioautomatico_horavalor);
                        }
                        catch
                        {

                        }

                    }
                    //this.dtpEnvioautomatico_horavalor.Value = Convert.ToDateTime(entidad_alarma.Cs_pr_Envioautomatico_horavalor);
                    this.chkEnvioManual.IsChecked = clsBaseUtil.cs_fxStringToBoolean(entidad_alarma.Cs_pr_Enviomanual);
                    //this.rbtEnviomanual_mostrarglobo.IsChecked = clsBaseUtil.cs_fxStringToBoolean(entidad_alarma.Cs_pr_Enviomanual_mostrarglobo);
                    if (entidad_alarma.Cs_pr_Enviomanual_mostrarglobo == "T" || entidad_alarma.Cs_pr_Enviomanual_mostrarglobo == "F" || entidad_alarma.Cs_pr_Enviomanual_mostrarglobo == "")
                    {
                        entidad_alarma.Cs_pr_Enviomanual_mostrarglobo = DateTime.Now.Date.ToString();
                    }
                    this.dtpEnviomanual_horavalor.Value = Convert.ToDateTime(entidad_alarma.Cs_pr_Enviomanual_mostrarglobo);//poner la hora aqui de globos 
                                                                                                                            //Buscar de acuerdo a la alarma definida el valor a  seleccionar . 
                    switch (entidad_alarma.Cs_pr_Enviomanual_mostrarglobo_minutosvalor)
                    {
                        case "2":
                            this.cboVecesHora.SelectedIndex = 0;
                            break;
                        case "3":
                            this.cboVecesHora.SelectedIndex = 1;
                            break;
                        case "4":
                            this.cboVecesHora.SelectedIndex = 2;
                            break;
                        case "6":
                            this.cboVecesHora.SelectedIndex = 3;
                            break;
                        case "60":
                            this.cboVecesHora.SelectedIndex = 4;
                            break;
                        default:
                            this.cboVecesHora.SelectedIndex = 0;
                            break;
                    }
                    //  rbtEnviomanual_nomostrarglobo.IsChecked = clsBaseUtil.cs_fxStringToBoolean(entidad_alarma.Cs_pr_Enviomanual_nomostrarglobo);             
                }
            }
            catch(Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Error al Iniciar Ventana Alerta Resumen Diario: "+ ex.ToString());
            }
        }
        //Evento click para enviar los cambios de configuracion de alertas para resumen diario.
        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxPares cbpVecesDia = (ComboBoxPares)cboVecesDia.SelectedItem;
                ComboBoxPares cbpVecesHora = (ComboBoxPares)cboVecesHora.SelectedItem;
                //Si la alarma esta definida.Salvar los cambios ingresados. 
                if (exist == true)
                {
                    entidad_alarma.Cs_pr_Envioautomatico = clsBaseUtil.cs_fxBooleanToString((bool)chkEnvioAutomatico.IsChecked);
                    //entidad_alarma.Cs_pr_Envioautomatico_minutos = clsBaseUtil.cs_fxBooleanToString((bool)rbtEnvioautomatico_minutos.IsChecked);
                    entidad_alarma.Cs_pr_Envioautomatico_minutosvalor = cbpVecesDia._Id;
                    //entidad_alarma.Cs_pr_Envioautomatico_hora = clsBaseUtil.cs_fxBooleanToString((bool)rbtEnvioautomatico_hora.IsChecked);
                    entidad_alarma.Cs_pr_Envioautomatico_horavalor = dtpEnvioautomatico_horavalor.Text;
                    entidad_alarma.Cs_pr_Enviomanual = clsBaseUtil.cs_fxBooleanToString((bool)chkEnvioManual.IsChecked);
                    //entidad_alarma.Cs_pr_Enviomanual_mostrarglobo = clsBaseUtil.cs_fxBooleanToString((bool)rbtEnviomanual_mostrarglobo.IsChecked);
                    entidad_alarma.Cs_pr_Enviomanual_mostrarglobo_minutosvalor = cbpVecesHora._Id;
                    entidad_alarma.Cs_pr_Enviomanual_mostrarglobo = dtpEnviomanual_horavalor.Text;
                    //entidad_alarma.Cs_pr_Enviomanual_nomostrarglobo = clsBaseUtil.cs_fxBooleanToString((bool)rbtEnviomanual_nomostrarglobo.IsChecked);
                    dtpEnvioautomatico_horavalor.Value = Convert.ToDateTime(entidad_alarma.Cs_pr_Envioautomatico_horavalor);
                    dtpEnviomanual_horavalor.Value = Convert.ToDateTime(entidad_alarma.Cs_pr_Enviomanual_mostrarglobo);
                    entidad_alarma.cs_pxActualizar(true);
                }
                //Caso no este definido las alarmas. Crear la alarma con los valores definidos por el usuario.
                else
                {

                    clsEntityAlarms entidad_new = new clsEntityAlarms();
                    entidad_new.Cs_pr_Declarant_Id = declarante_id;
                    entidad_new.Cs_pr_Alarms_Id = Guid.NewGuid().ToString();
                    entidad_new.Cs_pr_Envioautomatico = clsBaseUtil.cs_fxBooleanToString((bool)chkEnvioAutomatico.IsChecked);
                    //entidad_new.Cs_pr_Envioautomatico_minutos = clsBaseUtil.cs_fxBooleanToString((bool)rbtEnvioautomatico_minutos.IsChecked);
                    entidad_new.Cs_pr_Envioautomatico_minutosvalor = cbpVecesDia._Id;
                    //entidad_new.Cs_pr_Envioautomatico_hora = clsBaseUtil.cs_fxBooleanToString((bool)rbtEnvioautomatico_hora.IsChecked);
                    entidad_new.Cs_pr_Envioautomatico_horavalor = dtpEnvioautomatico_horavalor.Text;
                    entidad_new.Cs_pr_Enviomanual = clsBaseUtil.cs_fxBooleanToString((bool)chkEnvioManual.IsChecked);
                    //entidad_new.Cs_pr_Enviomanual_mostrarglobo = clsBaseUtil.cs_fxBooleanToString((bool)rbtEnviomanual_mostrarglobo.IsChecked);
                    entidad_new.Cs_pr_Enviomanual_mostrarglobo_minutosvalor = cbpVecesHora._Id;
                    entidad_new.Cs_pr_Enviomanual_mostrarglobo = dtpEnviomanual_horavalor.Text;
                    //entidad_new.Cs_pr_Enviomanual_nomostrarglobo = clsBaseUtil.cs_fxBooleanToString((bool)rbtEnviomanual_nomostrarglobo.IsChecked);
                    entidad_new.Cs_pr_Iniciarconwindows = "F";
                    entidad_new.Cs_pr_Tipo = "1";
                    this.dtpEnvioautomatico_horavalor.Value = Convert.ToDateTime(entidad_new.Cs_pr_Envioautomatico_horavalor);
                    dtpEnviomanual_horavalor.Value = Convert.ToDateTime(entidad_new.Cs_pr_Enviomanual_mostrarglobo);
                    entidad_new.cs_pxInsertar(true);
                    exist = true;
                    entidad_alarma = entidad_new;
                }

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Error al registrar nueva configuracion de alarma Resumen diario ->" + ex.ToString());
            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("14");
                ayuda.ShowDialog();
            }
        }
    }
}
