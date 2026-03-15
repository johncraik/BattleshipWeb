using Battleship.Models.Identity;
using JC.Communication.Extensions;
using JC.Communication.Logging.Data;
using JC.Communication.Logging.Models.Email;
using JC.Communication.Logging.Models.Messaging;
using JC.Communication.Logging.Models.Notifications;
using JC.Communication.Messaging.Data;
using JC.Communication.Messaging.Models.DomainModels;
using JC.Communication.Notifications.Data;
using JC.Communication.Notifications.Models;
using JC.Core.Models;
using JC.Github.Data;
using JC.Github.Extensions;
using JC.Github.Models;
using JC.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Battleship.Data;

public class ApplicationDbContext : IdentityDataDbContext<ApplicationUser, ApplicationRole>,
    IGithubDbContext, IEmailDbContext, INotificationDbContext, IMessagingDbContext
{
    public ApplicationDbContext(DbContextOptions options, IUserInfo userInfo) 
        : base(options, userInfo)
    {
    }

    //Github Tables:
    public DbSet<ReportedIssue> ReportedIssues { get; set; }
    public DbSet<IssueComment> IssueComments { get; set; }
    
    //Email Tables:
    public DbSet<EmailLog> EmailLogs { get; set; }
    public DbSet<EmailRecipientLog> EmailRecipientLogs { get; set; }
    public DbSet<EmailContentLog> EmailContentLogs { get; set; }
    public DbSet<EmailSentLog> EmailSentLogs { get; set; }
    
    //Notification Tables:
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationStyle> NotificationStyles { get; set; }
    public DbSet<NotificationLog> NotificationLogs { get; set; }
    
    //Messaging Tables:
    public DbSet<ChatThread> ChatThreads { get; set; }
    public DbSet<ThreadDeleted> DeletedThreads { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    public DbSet<ChatMetadata> ChatMetadata { get; set; }
    public DbSet<ThreadActivityLog> ThreadActivityLogs { get; set; }
    public DbSet<MessageReadLog> MessageReadLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyGithubMappings();
        modelBuilder.ApplyEmailMappings();
        modelBuilder.ApplyNotificationMappings();
        modelBuilder.ApplyMessagingMappings();
    }
}