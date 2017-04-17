using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenVPNTray
{
    class OpenVPNTrayApp : ApplicationContext
    {
        private NotifyIcon vpnTray;
        private ServiceController vpnController;

        public OpenVPNTrayApp()
        {
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            vpnController = new ServiceController("OpenVPNService");
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            vpnTray = new NotifyIcon();
            vpnTray.BalloonTipText = "OpenVPNTray";
            GetVPNStatus();

            vpnTray.ContextMenu= new ContextMenu();

            // Start MenuItem
            MenuItem vpnStartCmd = new MenuItem();
            vpnStartCmd.Text = "Start VPN";
            vpnStartCmd.Click += VPNStartCmd_Click;
            vpnTray.ContextMenu.MenuItems.Add(vpnStartCmd);

            // Restart MenuItem
            MenuItem vpnRestartCmd = new MenuItem();
            vpnRestartCmd.Text = "Restart VPN";
            vpnRestartCmd.Click += VPNRestartCmd_Click;
            vpnTray.ContextMenu.MenuItems.Add(vpnRestartCmd);

            // Stop MenuItem
            MenuItem vpnStopCmd = new MenuItem();
            vpnStopCmd.Text = "Stop VPN";
            vpnStopCmd.Click += VPNStopCmd_Click;
            vpnTray.ContextMenu.MenuItems.Add(vpnStopCmd);

            // Add separator
            vpnTray.ContextMenu.MenuItems.Add("-");

            // Exit MenuItem
            MenuItem vpnExitCmd = new MenuItem();
            vpnExitCmd.Text = "Exit";
            vpnExitCmd.Click += VpnExitCmd_Click; ;
            vpnTray.ContextMenu.MenuItems.Add(vpnExitCmd);

            vpnTray.Visible = true;
        }



        private void VPNStartCmd_Click(object sender, EventArgs e)
        {
            if (vpnController.Status != ServiceControllerStatus.Stopped) return;
            vpnController.Start();
            vpnController.WaitForStatus(ServiceControllerStatus.Running);
            GetVPNStatus();
        }

        private void VPNRestartCmd_Click(object sender, EventArgs e)
        {
            if (vpnController.Status != ServiceControllerStatus.Running) return;
            DialogResult q = MessageBox.Show("Are you sure you want to restart the VPN service?", "Restart VPN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (q == DialogResult.No) return;

            vpnController.Stop();
            vpnController.WaitForStatus(ServiceControllerStatus.Stopped);
            GetVPNStatus();

            if (vpnController.Status != ServiceControllerStatus.Stopped) return;
            vpnController.Start();
            vpnController.WaitForStatus(ServiceControllerStatus.Running);
            GetVPNStatus();
        }

        private void VPNStopCmd_Click(object sender, EventArgs e)
        {
            if (vpnController.Status != ServiceControllerStatus.Running) return;
            DialogResult q = MessageBox.Show( "Are you sure you want to stop the VPN service?", "Stop VPN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (q == DialogResult.No) return;

            vpnController.Stop();
            vpnController.WaitForStatus(ServiceControllerStatus.Stopped);
            GetVPNStatus();
        }

        private void GetVPNStatus()
        {
            switch (vpnController.Status)
            {
                case ServiceControllerStatus.Running:
                    vpnTray.Icon = Properties.Resources.on;
                    break;
                default:
                    vpnTray.Icon = Properties.Resources.off;
                    break;
            }
        }

        private void VpnExitCmd_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            //Cleanup so that the icon will be removed when the application is closed
            vpnTray.Visible = false;
        }

    }
}
