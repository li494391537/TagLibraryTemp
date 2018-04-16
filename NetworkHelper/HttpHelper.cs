using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace Lirui.NetworkHelper {
    internal class HttpHelper {

        #region 字段、属性
        private readonly HttpListener listener = new HttpListener();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        #endregion

        #region 事件
        public event Action<HttpListenerContext> ReceivedData;
        #endregion

        public HttpHelper(string[] prefixes) {
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            foreach (string s in prefixes) {
                listener.Prefixes.Add(s);
            }

        }

        private async Task<HttpListenerContext> Receive() {
            return await listener.GetContextAsync();
        }

        public void Start() {
            listener.Start();
            Task.Factory.StartNew(() => {
                while (true) {
                    var task = Receive();
                    task.Wait();
                    var context = task.Result;
                    ReceivedData?.Invoke(context);
                }
            }, cancellationTokenSource.Token);
        }

        public void Stop() {
            cancellationTokenSource.Cancel();
            listener.Stop();
        }
        
        public void Send() {
            HttpClient httpClient = new HttpClient();
        }
    }
}
