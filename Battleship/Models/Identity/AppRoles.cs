using JC.Identity.Authentication;

namespace Battleship.Models.Identity;

public class AppRoles : SystemRoles
{
    public const string RestrictedUser = nameof(RestrictedUser);
    public const string RestrictedUserDesc = "A user with restricted access. Unable to view leaderboard, send messages, or start new games.";
}