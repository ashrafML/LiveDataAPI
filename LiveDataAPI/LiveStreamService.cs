using Microsoft.AspNetCore.SignalR.Client;
using System.Reactive.Linq;

namespace LiveDataAPI
{
    [Obsolete("Not Use Noe",true)]
    public class LiveStreamService
    {
        public IObservable<string> ConnectToSignalRHub(string hubUrl, string targetMethod)
        {
            HubConnection hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            return Observable.Create<string>(async observer =>
            {
                // Subscribe to the target method and forward received messages to the observer
                hubConnection.On<string>(targetMethod, message => observer.OnNext(message));

                // Start the connection to the SignalR Hub
                await hubConnection.StartAsync();

                // Dispose the hub connection when the observable is disposed
                return async () => await hubConnection.DisposeAsync();
            });
        }
    }
}
