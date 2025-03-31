using DeLong.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace DeLong.Service.Services;

public abstract class AuditableService
{
    protected readonly IHttpContextAccessor _httpContextAccessor;

    public AuditableService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected long GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return _httpContextAccessor.HttpContext == null ? 0 : throw new Exception("User not authenticated");
        }
        return long.Parse(userIdClaim);
    }

    protected long GetCurrentBranchId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("BranchId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return _httpContextAccessor.HttpContext == null ? 0 : throw new Exception("BranchId not authenticated");
        }
        return long.Parse(userIdClaim);
    }
    protected string GetCurrentRole()
    {
        var roleClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Role")?.Value;
        if (string.IsNullOrEmpty(roleClaim))
            throw new Exception("Role not found in token");
        return roleClaim;
    }


    protected void SetCreatedFields(Auditable entity)
    {
        entity.CreatedBy = GetCurrentUserId();
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.IsDeleted = false;
    }

    protected void SetCreatedBranch(Auditable entity)
    {
        entity.CreatedBy = GetCurrentUserId();
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.IsDeleted = false;
    }


    protected void SetUpdatedFields(Auditable entity)
    {
        entity.UpdatedBy = GetCurrentUserId();
        entity.UpdatedAt = DateTimeOffset.UtcNow;
    }
}