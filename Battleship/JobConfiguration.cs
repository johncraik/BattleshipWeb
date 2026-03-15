using JC.BackgroundJobs.Extensions;
using JC.Communication.Email.Services;
using JC.Communication.Extensions;
using JC.Communication.Messaging.Services;
using JC.Communication.Notifications.Services;
using JC.Core.Extensions;
using JC.Core.Services;
using JC.SqlServer.Hangfire;

namespace Battleship;

public static class JobConfiguration
{
    public static WebApplicationBuilder ConfigureJobs(this WebApplicationBuilder builder)
    {
        // ── Background Jobs (Hangfire) ──────────────────────────────
        builder.Services.AddHangfireSqlServer(builder.Configuration);

// Core cleanup jobs
        builder.Services.ConfigureCoreBackgroundJobs(options =>
        {
            options.EnableAuditCleanupJob = true;
            options.EnableSoftDeleteCleanupJob = true;
        });
        builder.Services.AddHangfireJob<AuditCleanupJob>(options =>
        {
            options.Cron = "0 3 * * *"; // Daily at 03:00 UTC
        });
        builder.Services.AddHangfireJob<SoftDeleteCleanupJob>(options =>
        {
            options.Cron = "0 4 * * *"; // Daily at 04:00 UTC
        });

// Email cleanup jobs
        builder.Services.ConfigureEmailBackgroundJobs(options =>
        {
            options.EnableEmailLogCleanupJob = true;
        });
        builder.Services.AddHangfireJob<EmailLogCleanupJob>(options =>
        {
            options.Cron = "0 5 * * *"; // Daily at 05:00 UTC
        });

// Notification cleanup jobs
        builder.Services.AddHangfireJob<NotificationLogCleanupJob>(options =>
        {
            options.Cron = "0 5 * * 0"; // Weekly Sunday at 05:00 UTC
        });

// Messaging cleanup jobs
        builder.Services.ConfigureMessagingBackgroundJobs(options =>
        {
            options.EnableActivityLogCleanupJob = true;
            options.EnableReadLogCleanupJob = true;
        });
        builder.Services.AddHangfireJob<ActivityLogCleanupJob>(options =>
        {
            options.Cron = "0 3 * * 0"; // Weekly Sunday at 03:00 UTC
        });
        builder.Services.AddHangfireJob<ReadLogCleanupJob>(options =>
        {
            options.Cron = "0 3 * * 0"; // Weekly Sunday at 03:00 UTC
        });
        
        return builder;
    }
}