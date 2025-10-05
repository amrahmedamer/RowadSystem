namespace  RowadSystem.Shard.Abstractions;

public static class UserErrors
{
    public static Error UserNotFound = new Error("UserNotFound", "The user was not found.", StatusCodes.Status404NotFound);
    public static Error InvalidCredentials = new Error("InvalidCredentials", "The provided credentials are invalid.", StatusCodes.Status401Unauthorized);
    public static Error UserAlreadyExists = new Error("UserAlreadyExists", "A user with the provided details already exists.", StatusCodes.Status409Conflict);
    public static Error UnauthorizedAccess = new Error("UnauthorizedAccess", "You do not have permission to access this resource.", StatusCodes.Status403Forbidden);
    public static Error AccountLocked = new Error("AccountLocked", "Your account has been locked due to multiple failed login attempts. Please try again later.", StatusCodes.Status423Locked);
    public static Error OtpFailed = new Error("OtpFailed", "OTP verification failed. Please try again.", StatusCodes.Status400BadRequest);
    public static Error EmailOtpFailed = new Error("EmailOtpFailed", "OTP verification for email confirmation failed. Please try again.", StatusCodes.Status400BadRequest);
    public static Error EmailNotConfirmed = new Error("EmailNotConfirmed", "Your email address is not confirmed. Please check your inbox and confirm your email to continue.", StatusCodes.Status403Forbidden);
    public static Error InvalidToken = new Error("InvalidToken", "The token provided is invalid or expired. Please log in again to obtain a new token.", StatusCodes.Status401Unauthorized);
    public static Error InvalidRefreshToken = new Error("InvalidRefreshToken", "The refresh token is invalid, expired, or has already been used. Please log in again.", StatusCodes.Status401Unauthorized);
    public static Error AddressRequired = new("User.AddressRequired", "An address is required to proceed.", StatusCodes.Status400BadRequest);

}
