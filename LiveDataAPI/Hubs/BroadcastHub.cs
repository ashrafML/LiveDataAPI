using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic;

namespace LiveDataAPI.Hubs
{
    public class BroadcastHub : Hub
    {
       
        public async IAsyncEnumerable<int> StreamData(CancellationToken cancellationToken)
        {
            int counter = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                yield return counter++;
                await Task.Delay(1000, cancellationToken);
            }
        }
        public async Task SendStreamData(int data)
        {
            await Clients.All.SendAsync("ReceiveMyStreamData", data);
        }
        public async Task SendAudioStreamData(short[] data, int sampleRate)
        {
            await Clients.All.SendAsync("ReceiveAudioStreamData", data, sampleRate);
        }
    }
}
