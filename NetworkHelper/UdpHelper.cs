using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lirui.NetworkHelper {
    internal class UdpHelper {

        public UdpClient UdpClient { get; }
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public event Action<byte[]> ReceivedData;

        public UdpHelper(int port) {
            UdpClient = new UdpClient(port);
        }

        public async Task<int> Send(IPEndPoint endPoint, byte[] data) {
            return await UdpClient.SendAsync(data, data.Length, endPoint);
        }

        public async Task<byte[]> Receive() {
            var result = await UdpClient.ReceiveAsync();
            return result.Buffer;
        }

        public void BeginReceive() {
            Task.Factory.StartNew(() => {
                while (true) {
                    Task<byte[]> task = Receive();
                    task.Wait();
                    ReceivedData?.Invoke(task.Result);
                }
            }, cancellationTokenSource.Token);
        }

        public void StopReceive() {
            cancellationTokenSource.Cancel();
        }
    }
}
