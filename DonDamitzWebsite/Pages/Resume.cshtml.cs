using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DonDamitzWebsite.Pages
{
    public class ResumeModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ResumeModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnGet()
        {
            // Display the resume page
        }

        /// <summary>
        /// Handles the download resume request
        /// </summary>
        public IActionResult OnGetDownloadResume()
        {
            // Path to your resume file in wwwroot/files folder
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "files", "DonDamitz_Resume.docx");

            // Check if file exists
            if (!System.IO.File.Exists(filePath))
            {
                TempData["ErrorMessage"] = "Resume file not found. Please contact the administrator.";
                return RedirectToPage();
            }

            try
            {
                // Read the file into a byte array
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Return the file with the correct MIME type for Word documents
                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "DonDamitz_Resume.docx"
                );
            }
            catch (Exception ex)
            {
                // Log the error (you would use ILogger in a real application)
                TempData["ErrorMessage"] = "An error occurred while downloading the resume. Please try again later.";
                return RedirectToPage();
            }
        }
    }
}

