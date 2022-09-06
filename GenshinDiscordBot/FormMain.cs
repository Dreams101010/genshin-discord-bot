using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace GenshinDiscordBotUI
{
    internal partial class FormMain : Form
    {
        private Application application;
        private ILogger Logger { get; set; }
        private CancellationTokenSource? cancellationTokenSource;
        private Task? BotTask { get; set; }
        public FormMain(Application application, ILogger logger)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InitializeComponent();
            notifyIcon1.Visible = false;
            Minimize();
            StartBot();
        }

        private async void button_Start_Click(object sender, EventArgs e)
        {
            StartBot();
        }

        private async void button_Stop_Click(object sender, EventArgs e)
        {
            await StopBotAsync();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Minimize();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Maximize();
        }

        private async void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you want to close the app?", "App closing",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                await StopBotAsync();
            }
            else if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void StartBot()
        {
            button_Start.Enabled = false;
            Logger?.Information("Bot starting...");
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            BotTask = application.StartApplication(token);
            Logger?.Information("Bot will be online shortly. Press ENTER to shut the application down.");
            button_Stop.Enabled = true;
        }

        private async Task StopBotAsync()
        {
            try
            {
                button_Stop.Enabled = false;
                Logger?.Information("Bot shutting down...");
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                }
                if (BotTask != null)
                {
                    await BotTask; // await bot to finish shutting down properly
                }
            }
            catch (OperationCanceledException)
            {
                Logger?.Information("Bot has shut down.");
            }
            button_Start.Enabled = true;
        }

        private void Minimize()
        {
            Hide();
            notifyIcon1.Visible = true;
        }

        private void Maximize()
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
    }
}
