using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class CronjobService : IHostedService, IDisposable
{
    private Timer _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(Cronjob, null, TimeSpan.Zero, 
            TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private void Cronjob(object state)
    {
        var currentTime = DateTime.Now;
        var midnight = currentTime.Date.AddDays(1);
        var timeToWait = midnight - currentTime;

        _timer.Change(timeToWait, TimeSpan.FromDays(1));

        using (var client = new HttpClient())
        {
            var result = client.GetAsync("http://146.190.191.28/user02eq2/metrics/SnapshotJSON?token=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&owner=NicolasBeaulieu88&projectId=PVT_kwHOAWqqs84AbBL1").Result;
            
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}