using Microsoft.AspNetCore.Mvc;
using ManagingAgriculture.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ManagingAgriculture.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Support sign-in by email or username.
            // If input looks like an email try finding the user by email first.
            ApplicationUser? user = null;
            if (!string.IsNullOrWhiteSpace(model.Email) && model.Email.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }
            if (user == null)
            {
                // fallback to username lookup
                user = await _userManager.FindByNameAsync(model.Email);
            }

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // Use PasswordSignInAsync with the user's username to ensure proper sign-in behavior
            if (string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (signInResult.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                
                if (roles.Contains("SystemAdmin")) return RedirectToAction("Index", "Admin");
                if (roles.Contains("ITSupport")) return RedirectToAction("Index", "ITSupport");
                if (roles.Contains("Boss")) return RedirectToAction("Index", "Boss");
                if (roles.Contains("Manager")) return RedirectToAction("Index", "Manager");
                if (roles.Contains("Employee")) return RedirectToAction("Index", "Employee");

                // Default Role (User/Freelancer)
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, [FromServices] ManagingAgriculture.Data.ApplicationDbContext _context)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.RegisterAsCompany && string.IsNullOrWhiteSpace(model.CompanyName))
            {
                ModelState.AddModelError("CompanyName", "Company Name is required when registering as a Company Boss.");
                return View(model);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.RegisterAsCompany)
                {
                    // Create Company
                    var company = new Company
                    {
                        Name = model.CompanyName!,
                        CreatedByUserId = user.Id
                    };
                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();

                    // Link User to Company and Assign Boss Role
                    user.CompanyId = company.Id;
                    await _userManager.UpdateAsync(user);
                    await _userManager.AddToRoleAsync(user, "Boss");
                }
                else
                {
                    // Assign User Role (Freelancer)
                    // Even if they have an invitation, we don't auto-join them yet.
                    // They must accept it in the Dashboard.
                    await _userManager.AddToRoleAsync(user, "User");
                }

                // Registration succeeded — redirect to Login so user can sign in.
                TempData["SuccessMessage"] = "Registration successful. You can sign in now.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
