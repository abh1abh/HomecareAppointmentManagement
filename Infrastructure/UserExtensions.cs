using System.Security.Claims;

namespace HomecareAppointmentManagment.Infrastructure;

public static class UserExtensions
{
    public static int? TryGetHealthcareWorkerId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirstValue("HealthcareWorkerId"), out var id) ? id : (int?)null;

    public static int? TryGetClientId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirstValue("ClientId"), out var id) ? id : (int?)null;

    public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole("Admin");
    public static bool IsHealthcareWorker(this ClaimsPrincipal user) => user.IsInRole("HealthcareWorker");
    public static bool IsClient(this ClaimsPrincipal user) => user.IsInRole("Client");

    public static int? TryGetUserId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (int?)null;
}
