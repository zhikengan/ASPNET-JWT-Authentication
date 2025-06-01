using JwtAuth.Dtos.Request;
using JwtAuth.Dtos.Response;
using JwtAuth.Helpers;
using JwtAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace JwtAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public AuthController(
            ILogger<AuthController> logger,
            AuthService authService,
            TokenService tokenService)
        {
            _logger = logger;
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpGet("TestAnonymous")]
        [AllowAnonymous]
        public IActionResult TestAnonymous()
        {
            return Ok(new
            {
                Message = "This is an anonymous endpoint. No authentication required."
            });
        }

        [HttpGet("TestAuth")]
        [Authorize]
        public IActionResult Test()
        {
            var userId = IdentityUtils.GetUserId(HttpContext);
            var username = IdentityUtils.GetUsername(HttpContext);

            return Ok(new
            {

                UserId = userId,
                Username = username,
                Message = "This is a protected endpoint. You are authenticated."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            // Simulate a login action
            _logger.LogInformation("User logged in successfully.");

            var user = await _authService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            if (user is null)
            {
                _logger.LogWarning("Invalid login attempt for user {Username}", loginRequest.Username);
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            // Generate JWT token
            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.UserName);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id, user.UserName);

            //TODO : store refresh token in database or cache for later use
            // Create a user session (optional, depending on your application logic)
            var userSession = await _authService.CreateSessionAsync(user, refreshToken);

            return Ok(new LoginResponse
            {
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token,
                UserId = user.Id,
                UserName = user.UserName
            });
        }

        [HttpPost("refreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var refreshToken = refreshTokenRequest.RefreshToken;

            // get user from HttpContext
            var userId = IdentityUtils.GetUserId(HttpContext);
            var username = IdentityUtils.GetUsername(HttpContext);

            // Validate the refresh token
            var userSession = await _authService.GetUserSessionAsync(userId, refreshToken);
            if (userSession is null || userSession.IsRevoked)
            {
                // revoke all the sessions for the user
                // user will require to login again when access token is expired
                await _authService.RevokeAllSessionsAsync(userId);

                _logger.LogWarning("Refresh token is revoked for user {UserId}", userId);
                return Unauthorized(new { Message = "Refresh token is revoked" });
            }

            // Revoke the old refresh token
            await _authService.RevokeSessionAsync(userId, refreshToken);

            // Generate new access and refresh tokens
            var newAccessToken = _tokenService.GenerateAccessToken(userId, username);
            var newRefreshToken = _tokenService.GenerateRefreshToken(userId, username);

            await _authService.CreateSessionAsync(userSession.User, newRefreshToken);

            return Ok(new
            {
                AccessToken = newAccessToken.Token,
                RefreshToken = newRefreshToken.Token,
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(LogoutRequest logoutRequest)
        {
            // get user from HttpContext
            var userId = IdentityUtils.GetUserId(HttpContext);

            // revoke the refresh token
            var isRevoked = await _authService.RevokeSessionAsync(userId, logoutRequest.RefreshToken);

            return Ok(new LogoutResponse
            {
                Message = isRevoked ? "Logout successful" : "Logout failed",
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            // Simulate a registration action
            var appUser = new Models.AppUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email
            };
            var createdUser = await _authService.RegisterAsync(appUser, registerRequest.Password);
            if (createdUser is null)
            {
                _logger.LogWarning("Registration failed for user {Username}", registerRequest.Username);
                return BadRequest(new { Message = "Registration failed" });
            }

            // Generate JWT token
            var accessToken = _tokenService.GenerateAccessToken(createdUser.Id, createdUser.UserName);
            var refreshToken = _tokenService.GenerateRefreshToken(createdUser.Id, createdUser.UserName);

            return Ok(new RegisterResponse
            {
                Message = "Registration successful",
            });
        }
    }
}
