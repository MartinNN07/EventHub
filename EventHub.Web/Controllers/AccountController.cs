using EventHub.Data.Models;
using EventHub.Services.Interfaces;
using EventHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IBookingService _bookingService;
		private readonly ILogger<AccountController> _logger;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IBookingService bookingService,
			ILogger<AccountController> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_bookingService = bookingService;
			_logger = logger;
		}

		// GET: Account/Register
		[HttpGet]
		public IActionResult Register(string? returnUrl = null)
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		// POST: Account/Register
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var user = new ApplicationUser
				{
					UserName = model.Email,
					Email = model.Email,
					FirstName = model.FirstName,
					LastName = model.LastName
				};

				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password");

					// Sign in the user after successful registration
					await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);

					TempData["Success"] = "Registration successful! Welcome to EventHub.";

					// Redirect to return URL or home
					if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
					return RedirectToAction("Index", "Home");
				}

				// Add errors to ModelState
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during user registration");
				ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
			}

			return View(model);
		}

		// GET: Account/Login
		[HttpGet]
		public IActionResult Login(string? returnUrl = null)
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			ViewData["ReturnUrl"] = returnUrl;
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		// POST: Account/Login
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				// Attempt to sign in
				var result = await _signInManager.PasswordSignInAsync(
					model.Email,
					model.Password,
					model.RememberMe,
					lockoutOnFailure: true);

				if (result.Succeeded)
				{
					_logger.LogInformation("User logged in");
					TempData["Success"] = "Welcome back!";

					// Redirect to return URL or home
					if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
					{
						return Redirect(model.ReturnUrl);
					}
					return RedirectToAction("Index", "Home");
				}

				if (result.IsLockedOut)
				{
					_logger.LogWarning("User account locked out");
					ModelState.AddModelError(string.Empty, "Account locked out. Please try again later.");
					return View(model);
				}

				ModelState.AddModelError(string.Empty, "Invalid email or password");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during login");
				ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
			}

			return View(model);
		}

		// POST: Account/Logout
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			try
			{
				await _signInManager.SignOutAsync();
				_logger.LogInformation("User logged out");
				TempData["Success"] = "You have been logged out successfully.";
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during logout");
			}

			return RedirectToAction("Index", "Home");
		}

		// GET: Account/Profile
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Profile()
		{
			try
			{
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return RedirectToAction("Login");
				}

				var bookings = await _bookingService.GetUserBookingsAsync(user.Id);
				var now = DateTime.Now;

				var viewModel = new UserProfileViewModel
				{
					UserId = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email!,
					PhoneNumber = user.PhoneNumber,
					TotalBookings = bookings.Count(),
					UpcomingEvents = bookings.Count(b => b.Event.Date >= now)
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading user profile");
				TempData["Error"] = "Unable to load profile. Please try again.";
				return RedirectToAction("Index", "Home");
			}
		}

		// POST: Account/Profile
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Profile(UserProfileViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return RedirectToAction("Login");
				}

				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.PhoneNumber = model.PhoneNumber;

				// Update email if changed
				if (user.Email != model.Email)
				{
					var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
					if (!setEmailResult.Succeeded)
					{
						foreach (var error in setEmailResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
						return View(model);
					}

					var setUserNameResult = await _userManager.SetUserNameAsync(user, model.Email);
					if (!setUserNameResult.Succeeded)
					{
						foreach (var error in setUserNameResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
						return View(model);
					}
				}

				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					TempData["Success"] = "Profile updated successfully!";
					return RedirectToAction(nameof(Profile));
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating user profile");
				ModelState.AddModelError(string.Empty, "An error occurred while updating your profile.");
			}

			return View(model);
		}

		// GET: Account/ChangePassword
		[Authorize]
		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

		// POST: Account/ChangePassword
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return RedirectToAction("Login");
				}

				var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

				if (result.Succeeded)
				{
					await _signInManager.RefreshSignInAsync(user);
					_logger.LogInformation("User changed their password successfully");
					TempData["Success"] = "Password changed successfully!";
					return RedirectToAction(nameof(Profile));
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error changing password");
				ModelState.AddModelError(string.Empty, "An error occurred while changing your password.");
			}

			return View(model);
		}

		// GET: Account/ForgotPassword
		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		// POST: Account/ForgotPassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				
				// Don't reveal that the user does not exist
				if (user == null)
				{
					TempData["Success"] = "If an account exists with that email, password reset instructions have been sent.";
					return RedirectToAction(nameof(Login));
				}

				// Generate password reset token
				var code = await _userManager.GeneratePasswordResetTokenAsync(user);

				// In a real application, you would send an email here
				// For now, we'll just show a success message
				_logger.LogInformation("Password reset token generated for user {Email}", model.Email);

				TempData["Success"] = "If an account exists with that email, password reset instructions have been sent.";
				return RedirectToAction(nameof(Login));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during password reset");
				ModelState.AddModelError(string.Empty, "An error occurred. Please try again.");
			}

			return View(model);
		}

		// GET: Account/AccessDenied
		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
