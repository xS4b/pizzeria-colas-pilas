﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace laboratorioPizzeriaExpress
{
    public partial class MainForm : Form
    {
       
        private Queue<string> colaPedidos = new Queue<string>();     
        private List<string> listaPremium = new List<string>();     
        private Stack<string> pilaBitacora = new Stack<string>();   

        public MainForm()
        {
            InitializeComponent();
            ActualizarUI();
        }

     
        // PASO 1: Nuevo pedido (Normal o Premium usando el CheckBox)
        private void BtnNuevoPedido_Click(object sender, EventArgs e)
        {
            string cliente = txtCliente.Text.Trim();

          
            // Validar entrada
            if (string.IsNullOrEmpty(cliente))
            {
                lblEstado.Text = "Por favor, ingrese el nombre del cliente.";
                return;
            }

          
            // revisar si es premium
            if (chkPremium.Checked)
            {
              
            	// se agrega a la nueva lista 
                listaPremium.Add(cliente);
                
                // Registrar
                pilaBitacora.Push(string.Format("PREMIUM:{0}", cliente));
                lblEstado.Text = string.Format("¡Pedido PREMIUM registrado para {0}! ⭐", cliente);
            }
            else
            {
                // ce agrega a la cola normal
                colaPedidos.Enqueue(cliente);
                
                // Registrar en la bitácora
                pilaBitacora.Push(string.Format("PEDIDO:{0}", cliente));
                lblEstado.Text = string.Format("Pedido normal registrado para {0}", cliente);
            }

            // Limpiar campos y actualizar interfaz
            txtCliente.Clear();
            chkPremium.Checked = false; // Desmarcar el checkbox para el siguiente cliente
            ActualizarUI();
        }

        // PASO 2: Entregar pedido (Dando PRIORIDAD absoluta a la lista Premium)
        private void BtnEntregar_Click(object sender, EventArgs e)
        {
            // revisar la lista primium
            if (listaPremium.Count > 0)
            {
                
                string clientePremium = listaPremium[0];
                listaPremium.RemoveAt(0); // Lo removemos de la lista de espera
                
                pilaBitacora.Push(string.Format("ENTREGADO_PREMIUM:{0}", clientePremium));
                lblEstado.Text = string.Format("¡Pedido PREMIUM entregado a {0}! ⭐", clientePremium);
            }
            // si no haypremium atender normal
            else if (colaPedidos.Count > 0)
            {
                string clienteNormal = colaPedidos.Dequeue();
                
                pilaBitacora.Push(string.Format("ENTREGADO:{0}", clienteNormal));
                lblEstado.Text = string.Format("Pedido entregado a {0}", clienteNormal);
            }
            // si ambas estan vacias mostrar esto
            else
            {
                lblEstado.Text = "No hay pedidos pendientes.";
                return;
            }

            ActualizarUI();
        }

        private void BtnDeshacer_Click(object sender, EventArgs e)
        {
            if (pilaBitacora.Count == 0)
            {
                lblEstado.Text = "No hay acciones para deshacer.";
                return;
            }

            string ultimaAccion = pilaBitacora.Pop();
            string nombre = "";

         
            if (ultimaAccion.StartsWith("PEDIDO:"))
            {
                nombre = ultimaAccion.Replace("PEDIDO:", "");
                
              
                Queue<string> temporal = new Queue<string>(colaPedidos);
                colaPedidos.Clear();
                foreach (string p in temporal)
                {
                    if (p != nombre) colaPedidos.Enqueue(p);
                }
                lblEstado.Text = string.Format("Se deshizo el pedido normal de {0}", nombre);
            }
           
            else if (ultimaAccion.StartsWith("PREMIUM:"))
            {
                nombre = ultimaAccion.Replace("PREMIUM:", "");
                
                
                listaPremium.Remove(nombre);
                lblEstado.Text = string.Format("Se deshizo el registro Premium de {0}", nombre);
            }
          
            else if (ultimaAccion.StartsWith("ENTREGADO:"))
            {
                nombre = ultimaAccion.Replace("ENTREGADO:", "");
                colaPedidos.Enqueue(nombre); // Regresa a la fila normal
                lblEstado.Text = string.Format("Se deshizo la entrega a {0}", nombre);
            }
         
            else if (ultimaAccion.StartsWith("ENTREGADO_PREMIUM:"))
            {
                nombre = ultimaAccion.Replace("ENTREGADO_PREMIUM:", "");
                listaPremium.Insert(0, nombre); // Regresa al inicio de la lista de prioridad
                lblEstado.Text = string.Format("Se deshizo la entrega Premium a {0}", nombre);
            }
            else
            {
                lblEstado.Text = "Acción desconocida en bitácora.";
            }

            ActualizarUI();
        }

        // PASO 4: Limpiar todo (reiniciar sistema)
        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            colaPedidos.Clear();
            listaPremium.Clear(); // Limpiar también la lista premium
            pilaBitacora.Clear();
            lblEstado.Text = "Sistema reiniciado.";
            ActualizarUI();
        }

        // Sincronizar la interfaz gráfica con las colecciones de datos
        private void ActualizarUI()
        {
           
            lstPedidos.Items.Clear();
            lstPremium.Items.Clear(); // nueva lista
            lstBitacora.Items.Clear();

            // 1. Mostrar la lista de clientes Premium
            foreach (string p in listaPremium)
                lstPremium.Items.Add(string.Format("⭐ {0}", p));
            
            if (listaPremium.Count == 0)
                lstPremium.Items.Add("(Sin clientes Premium pendientes)");

            // 2. Mostrar la cola de pedidos normales
            foreach (string p in colaPedidos)
                lstPedidos.Items.Add(p);
            
            if (colaPedidos.Count == 0)
                lstPedidos.Items.Add("(Sin pedidos pendientes)");

            // 3. Mostrar la bitácora (pila)
            foreach (string accion in pilaBitacora)
                lstBitacora.Items.Add(accion);
            
            if (pilaBitacora.Count == 0)
                lstBitacora.Items.Add("(Sin acciones registradas)");

            // Actualizar contadores globales en la interfaz
            lblContador.Text = string.Format("Normales: {0} | Premium: {1} | Historial: {2}",
                colaPedidos.Count, listaPremium.Count, pilaBitacora.Count);
        }
    }
}