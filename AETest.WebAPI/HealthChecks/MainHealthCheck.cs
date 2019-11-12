using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace AETest.WebAPI.HealthChecks
{
    public class MainHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
           CancellationToken cancellationToken = default)
        {
            // TODO: perofrm actual health check
            var healthCheckResultHealthy = true;

            if (healthCheckResultHealthy)
            {
                return await Task.FromResult(
                    HealthCheckResult.Healthy("The check indicates a healthy result."));
            }

            return await Task.FromResult(
                HealthCheckResult.Unhealthy("The check indicates an unhealthy result."));
        }
    }
}
