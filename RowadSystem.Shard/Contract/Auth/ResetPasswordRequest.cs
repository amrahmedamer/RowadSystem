﻿namespace RowadSystem.Shard.Contract.Auth;

public record ResetPasswordRequest
(
    string Email,
    string Code,
    string NewPassword
 );
