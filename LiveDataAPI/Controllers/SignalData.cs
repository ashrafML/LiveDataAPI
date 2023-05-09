using LiveDataAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NAudio.Wave;

namespace LiveDataAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignalData : ControllerBase
    {
        private readonly IHubContext<BroadcastHub> _hubContext;
        private static readonly string[] Summaries = new[]
       {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        public SignalData(IHubContext<BroadcastHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpGet("broadcast")]
        public async Task<IActionResult> SendMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMyStreamData", message);
            return Ok();
        }
        [HttpGet("StreamAudio")]
        public async Task StreamAudio()
        {
            var waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(44100, 1);

            waveIn.DataAvailable += async (sender, e) =>
            {
                await _hubContext.Clients.All.SendAsync("ReceiveAudioStreamData", e.Buffer);
            };

            waveIn.StartRecording();

            await Task.Delay(-1);

            waveIn.StopRecording();
            waveIn.Dispose();
        }
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}