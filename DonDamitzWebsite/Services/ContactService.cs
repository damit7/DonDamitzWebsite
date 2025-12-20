using DonDamitzWebsite.Data;
using DonDamitzWebsite.Models;

namespace DonDamitzWebsite.Services
{
    /// <summary>
    /// Business Logic Layer for Contact functionality
    /// Handles validation, rate limiting, and business rules
    /// </summary>
    public class ContactService
    {
        private readonly ContactRepository _repository;
        private readonly ILogger<ContactService> _logger;

        // Rate limiting settings
        private const int MaxMessagesPerPeriod = 5;
        private const int RateLimitPeriodMinutes = 15;

        public ContactService(ContactRepository repository, ILogger<ContactService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Submits a contact message with validation and rate limiting
        /// </summary>
        /// <param name="message">The contact message to submit</param>
        /// <returns>Success status and any error message</returns>
        public async Task<(bool Success, string? ErrorMessage)> SubmitMessageAsync(ContactMessage message)
        {
            try
            {
                // Validate the message
                var validationResult = ValidateMessage(message);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Contact message validation failed: {Error}", validationResult.ErrorMessage);
                    return (false, validationResult.ErrorMessage);
                }

                // Check rate limiting
                if (!string.IsNullOrEmpty(message.IPAddress))
                {
                    var rateLimitCheck = await CheckRateLimitAsync(message.IPAddress);
                    if (!rateLimitCheck.Allowed)
                    {
                        _logger.LogWarning("Rate limit exceeded for IP: {IP}", message.IPAddress);
                        return (false, rateLimitCheck.ErrorMessage);
                    }
                }

                // Sanitize input
                SanitizeMessage(message);

                // Set submission time
                message.SubmittedOn = DateTime.Now;

                // Save to database
                var messageId = await _repository.CreateAsync(message);

                _logger.LogInformation("Contact message submitted successfully. ID: {MessageId}, From: {Email}",
                    messageId, message.Email);

                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting contact message from {Email}", message.Email);
                return (false, "An error occurred while submitting your message. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves a contact message by ID
        /// </summary>
        public async Task<ContactMessage?> GetMessageByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact message ID: {MessageId}", id);
                return null;
            }
        }

        /// <summary>
        /// Gets all contact messages
        /// </summary>
        public async Task<List<ContactMessage>> GetAllMessagesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all contact messages");
                return new List<ContactMessage>();
            }
        }

        /// <summary>
        /// Validates a contact message
        /// </summary>
        private (bool IsValid, string? ErrorMessage) ValidateMessage(ContactMessage message)
        {
            if (message == null)
            {
                return (false, "Message cannot be null");
            }

            if (string.IsNullOrWhiteSpace(message.Name))
            {
                return (false, "Name is required");
            }

            if (string.IsNullOrWhiteSpace(message.Email))
            {
                return (false, "Email is required");
            }

            if (!IsValidEmail(message.Email))
            {
                return (false, "Please enter a valid email address");
            }

            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                return (false, "Subject is required");
            }

            if (string.IsNullOrWhiteSpace(message.Message))
            {
                return (false, "Message is required");
            }

            //if (message.Message.Length < 10)
            //{
            //    return (false, "Message must be at least 10 characters long");
            //}

            if (message.Message.Length > 2000)
            {
                return (false, "Message cannot exceed 2000 characters");
            }

            // Check for spam patterns
            if (ContainsSpamPatterns(message.Message))
            {
                return (false, "Your message appears to contain spam content");
            }

            return (true, null);
        }

        /// <summary>
        /// Sanitizes the message content
        /// </summary>
        private void SanitizeMessage(ContactMessage message)
        {
            message.Name = message.Name.Trim();
            message.Email = message.Email.Trim().ToLowerInvariant();
            message.Subject = message.Subject.Trim();
            message.Message = message.Message.Trim();
        }

        /// <summary>
        /// Checks if the IP address has exceeded the rate limit
        /// </summary>
        private async Task<(bool Allowed, string? ErrorMessage)> CheckRateLimitAsync(string ipAddress)
        {
            try
            {
                var messageCount = await _repository.GetMessageCountByIPAsync(ipAddress, RateLimitPeriodMinutes);

                if (messageCount >= MaxMessagesPerPeriod)
                {
                    return (false, $"You have exceeded the maximum number of messages ({MaxMessagesPerPeriod}) within {RateLimitPeriodMinutes} minutes. Please try again later.");
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking rate limit for IP: {IP}", ipAddress);
                // Allow the message if rate limit check fails (fail open for better user experience)
                return (true, null);
            }
        }

        /// <summary>
        /// Validates email format using basic regex
        /// </summary>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Simple email validation
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks for common spam patterns in the message
        /// </summary>
        private bool ContainsSpamPatterns(string message)
        {
            var spamKeywords = new[]
            {
                "viagra", "cialis", "pharmacy", "lottery", "winner",
                "click here", "buy now", "limited time", "act now",
                "congratulations you won", "claim your prize"
            };

            var lowerMessage = message.ToLowerInvariant();
            return spamKeywords.Any(keyword => lowerMessage.Contains(keyword));
        }

        /// <summary>
        /// Tests the database connection
        /// </summary>
        public async Task<bool> TestDatabaseConnectionAsync()
        {
            try
            {
                return await _repository.TestConnectionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection test failed");
                return false;
            }
        }
    }
}