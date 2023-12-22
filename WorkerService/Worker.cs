using Business.Abstract;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class Worker : IHostedService, IDisposable
    {
        private  Timer _timer;
        IPasswordResetService _passwordResetService;

        public Worker(IPasswordResetService passwordResetService)
        {
            _passwordResetService=passwordResetService;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(EditPasswordReset, null, TimeSpan.Zero, TimeSpan.FromHours(2)); // Her 2 saatte çalýþacak þekilde ayarla
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            EditPasswordReset(null);
            Dispose();
            return Task.CompletedTask;
        }

        private void EditPasswordReset(object state)
        {
            var result = _passwordResetService.GetAll();
            if (result.Data != null & result.Data.Count > 0)
            {
                result.Data.ForEach(x =>
                {
                    x.Code = "";
                });
                _passwordResetService.UpdateList(result.Data);
            }
        }
    }
}
