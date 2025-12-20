using System.ComponentModel.DataAnnotations;

namespace DonDamitzWebsite.Models
{
    /// <summary>
    /// Represents a contact form message submitted by a visitor
    /// </summary>
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Your Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters")]
        [Display(Name = "Subject")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        [StringLength(2000, ErrorMessage = "Message cannot exceed 2000 characters")]
        [Display(Name = "Message")]
        public string Message { get; set; } = string.Empty;

        public DateTime SubmittedOn { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? IPAddress { get; set; }
    }
}
