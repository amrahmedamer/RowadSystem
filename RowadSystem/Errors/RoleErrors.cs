namespace  RowadSystem.Shard.Abstractions;

public static class RoleErrors
{
    public static Error RoleNotFound = new Error("RoleNotFound", "The role was not found.", StatusCodes.Status404NotFound);
    public static Error PermissionNotFound = new Error("PermissionNotFound", "The permission was not found.", StatusCodes.Status404NotFound);
    public static Error RoleAlreadyExists = new Error("RoleAlreadyExists", "The role already exists.", StatusCodes.Status409Conflict);

}
