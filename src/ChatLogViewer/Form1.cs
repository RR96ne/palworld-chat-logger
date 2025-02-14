using Microsoft.Web.WebView2.Core;
using System;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.Threading;

namespace ChatLogViewer
{
    public partial class Form1 : Form
    {
        private bool _webViewInitialized = false;
		private CancellationTokenSource _tokenSource = null;
        private readonly string _pipeName = "palworld_chat_logger";
		private Task _pipeTask = null;

		public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            webView.CoreWebView2InitializationCompleted += this.webViewInitializationCompleted;
            try
            {
                await webView.EnsureCoreWebView2Async();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Program initialization failed.\r\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }

            //MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);
        }

        private void webViewInitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                webView.NavigateToString(
@"<html>
	<head>
		<meta charset=""utf-8"">
		<style type=""text/css"">
			body{
				background-color:black;
				color:white;
			}
            .say{
                color:yellow;
            }
            .guild{
                color:aquamarine;
            }
        </style>
        <script language=""javascript"" type=""text/javascript"">
            function addLog(str,category) {
                let ele = document.getElementById(""log"");
                let newElement = document.createElement(""div"");
                let newContent = document.createTextNode(str);
                newElement.appendChild(newContent);
                newElement.className=category;
                ele.appendChild(newElement);
            }
            function clearLog() {
                let ele = document.getElementById(""log"");
                while(ele.firstChild){
                    ele.removeChild(ele.firstChild);
                }
            }
            function scrollToEnd(){
                let end = document.documentElement.offsetHeight;
                window.scrollTo(0,end);
            }
        </script>
    </head>
	<body>
		<div id=""log"" name=""log"">
		</div>
	</body>
</html>"
					);
                _webViewInitialized = true;
                buttonClear.Enabled = true;

                _pipeTask = StartServer();

			}
            else
            {
                MessageBox.Show("Program initialization failed.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

		private Task StartServer()
		{
			if (_tokenSource is null)
			{
				_tokenSource = new CancellationTokenSource();
			}
			var token = _tokenSource.Token;
			return Task.Run(async () =>
			{
                while (!token.IsCancellationRequested)
                {
                    using (var server = new NamedPipeServerStream(_pipeName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
                    {
                        try
                        {
                            await server.WaitForConnectionAsync(token);
                        }
                        catch (OperationCanceledException ex)
                        {
                            break;
                        }

                        using (StreamReader reader = new StreamReader(server, Encoding.Unicode))
                        {
                            while (server.IsConnected)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    break;
                                }

                                string line = null;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    if (token.IsCancellationRequested)
                                    {
                                        break;
                                    }

									line = line.Replace("\0", "");

									string category = "none";
                                    if (line.StartsWith("1"))
                                    {
                                        category = "global";
                                    }
                                    else if (line.StartsWith("2"))
                                    {
                                        category = "guild";
                                    }
                                    else if (line.StartsWith("3"))
                                    {
                                        category = "say";
                                    }
                                    if (line.Length > 1)
                                    {
                                        line = line.Substring(1);
                                    }
                                    
                                    //時刻を付加
                                    line = DateTime.Now.ToString("HH:mm:ss") + line;

									if (!token.IsCancellationRequested)
                                    {
                                        if (InvokeRequired)
                                        {
                                            Invoke((Action)(async () =>
                                            {
                                                
                                                await webView.ExecuteScriptAsync($"addLog('{line}','{category}');");
												if (checkBoxScroll.Checked)
												{
													await webView.ExecuteScriptAsync(@"scrollToEnd();");
												}

											}));
                                        }
                                        else
                                        {
                                            await webView.ExecuteScriptAsync($"addLog('{line}','{category}');");
                                            if (checkBoxScroll.Checked)
											{
												await webView.ExecuteScriptAsync(@"scrollToEnd();");
											}
										}

                                        if (checkBoxPopSound.Checked)
                                        {


                                            using (Stream popStream = Properties.Resources.pop)
                                            using (System.Media.SoundPlayer player = new System.Media.SoundPlayer(popStream))
                                            {
                                                player.PlaySync();
                                            }

										}
									}
                                }
                            }
                        }
                    }
                }
			},token).ContinueWith(t =>
			{
				_tokenSource.Dispose();
				_tokenSource = null;
			});
		}

        private async void buttonClear_Click(object sender, EventArgs e)
        {
            if (_webViewInitialized)
            {
                await webView.ExecuteScriptAsync(@"clearLog();");
            }
        }

        private async void checkBoxScroll_CheckedChanged(object sender, EventArgs e)
        {
            if (_webViewInitialized)
            {
                if (checkBoxScroll.Checked)
                {
                    await webView.ExecuteScriptAsync(@"scrollToEnd();");
                }
            }
        }

        private void checkBoxTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBoxTopMost.Checked;
        }

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!(_tokenSource is null))
			{
				_tokenSource.Cancel();
			}
            if (_pipeTask != null)
            {
                _pipeTask.Wait();
            }
		}
	}
}
