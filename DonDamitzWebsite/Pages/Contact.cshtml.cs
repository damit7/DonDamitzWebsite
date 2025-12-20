using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DonDamitzWebsite.Models;
using DonDamitzWebsite.Services;

namespace DonDamitzWebsite.Pages
{
    /// <summary>
    /// Page Model for the Contact page
    /// Handles the presentation layer logic for the contact form
    /// </summary>
    public class ContactModel : PageModel
    {
        private readonly ContactService _contactService;
        private readonly ILogger<ContactModel> _logger;

        [BindProperty]
        public ContactMessage ContactMessage { get; set; } = new ContactMessage();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public ContactModel(ContactService contactService, ILogger<ContactModel> logger)
        {
            _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handles GET requests to display the contact form
        /// </summary>
        public void OnGet()
        {
            // Display the contact form
        }

        /// <summary>
        /// Handles POST requests when the contact form is submitted
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            // Clear any previous messages
            SuccessMessage = null;
            ErrorMessage = null;

            // Validate the model state
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please correct the errors below and try again.";
                return Page();
            }

            // Get the user's IP address for rate limiting
            ContactMessage.IPAddress = GetClientIPAddress();

            // Submit the message through the business logic layer
            var result = await _contactService.SubmitMessageAsync(ContactMessage);

            if (result.Success)
            {
                SuccessMessage = "Thank you for your message! I'll get back to you as soon as possible.";

                // Clear the form by creating a new model instance
                ContactMessage = new ContactMessage();

                // Clear ModelState to remove any validation messages
                ModelState.Clear();

                return Page();
            }
            else
            {
                ErrorMessage = result.ErrorMessage ?? "An error occurred while submitting your message. Please try again.";
                return Page();
            }
        }

        /// <summary>
        /// Gets the client's IP address from the request
        /// </summary>
        private string GetClientIPAddress()
        {
            try
            {
                // Check for X-Forwarded-For header (in case behind a proxy/load balancer)
                var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    // Take the first IP if there are multiple
                    var ips = forwardedFor.Split(',');
                    return ips[0].Trim();
                }

                // Fall back to RemoteIpAddress
                return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting client IP address");
                return "Unknown";
            }
        }
    }
}