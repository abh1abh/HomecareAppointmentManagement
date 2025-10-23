using System.Security.Claims;

namespace HomecareAppointmentManagement.Infrastructure;

// With help from AI
public static class UserExtensions // Extension methods for ClaimsPrincipal and Authorized user
{
    // Try to get HealthcareWorkerId from claims
    public static int? TryGetHealthcareWorkerId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirstValue("HealthcareWorkerId"), out var id) ? id : (int?)null;

    // Try to get ClientId from claims  
    public static int? TryGetClientId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirstValue("ClientId"), out var id) ? id : (int?)null;

    // Try to get Admin status from claims
    public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole("Admin");

    // Try to get HealthcareWorker status from claims
    public static bool IsHealthcareWorker(this ClaimsPrincipal user) => user.IsInRole("HealthcareWorker");

    // Try to get Client status from claims
    public static bool IsClient(this ClaimsPrincipal user) => user.IsInRole("Client");

    // Try to get UserId from claims
    public static string? TryGetUserId(this ClaimsPrincipal user)
    => user.FindFirstValue(ClaimTypes.NameIdentifier);
}