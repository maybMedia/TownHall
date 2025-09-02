using TownHall.Core;

namespace TownHall;

public partial class Signup : ContentPage
{
	IUserService _userService;

	public Signup(IUserService userService)
	{
		InitializeComponent();
		_userService = userService;
	}

	private async void OnSignUpButtonClicked(object? sender, EventArgs e)
	{
		try
		{
			// Clear any previous error states
			ClearValidationErrors();

			// Validate all fields
			var validationResult = ValidateForm();

			if (!validationResult.IsValid)
			{
				await DisplayAlert("Validation Error", validationResult.ErrorMessage, "OK");
				return;
			}

			// Disable the button to prevent multiple submissions
			SignUpButton.IsEnabled = false;
			SignUpButton.Text = "Creating Account...";

			string FirstName = FirstNameEntry.Text.Trim();
			string LastName = LastNameEntry.Text.Trim();
			string Email = EmailEntry.Text.Trim().ToLower();
			string Password = PasswordEntry.Text;
			string Phone = PhoneEntry.Text.Trim();
			string Address = AddressEditor.Text.Trim();

			// Call your authentication service
			// Replace this with your actual registration logic
			var registrationResult = _userService.CreateUser(Email, Password, FirstName, LastName, Phone, Address);

			if (registrationResult != null)
			{
				await DisplayAlert("Success", "Account created successfully!", "OK");

				// Navigate to login page or main app
				Application.Current.MainPage = new Login(_userService);
			}
			else
			{
				await DisplayAlert("Registration Failed", "The service was unable to complete the request.", "OK");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
			// Log the exception in production
			System.Diagnostics.Debug.WriteLine($"Sign up error: {ex.Message}");
		}
		finally
		{
			// Re-enable the button
			SignUpButton.IsEnabled = true;
			SignUpButton.Text = "Create Account";
		}
	}

	private ValidationResult ValidateForm()
	{
		// Check if all required fields are filled
		if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
			return new ValidationResult(false, "First name is required.");

		if (string.IsNullOrWhiteSpace(LastNameEntry.Text))
			return new ValidationResult(false, "Last name is required.");

		if (string.IsNullOrWhiteSpace(EmailEntry.Text))
			return new ValidationResult(false, "Email address is required.");

		if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
			return new ValidationResult(false, "Password is required.");

		if (string.IsNullOrWhiteSpace(PhoneEntry.Text))
			return new ValidationResult(false, "Phone number is required.");

		if (string.IsNullOrWhiteSpace(AddressEditor.Text))
			return new ValidationResult(false, "Address is required.");

		// Validate email format
		if (!IsValidEmail(EmailEntry.Text.Trim()))
			return new ValidationResult(false, "Please enter a valid email address.");

		// Validate password strength
		var passwordValidation = ValidatePassword(PasswordEntry.Text);
		if (!passwordValidation.IsValid)
			return passwordValidation;

		// Validate phone number format
		if (!IsValidPhoneNumber(PhoneEntry.Text.Trim()))
			return new ValidationResult(false, "Please enter a valid phone number.");

		// Validate name lengths
		if (FirstNameEntry.Text.Trim().Length < 2)
			return new ValidationResult(false, "First name must be at least 2 characters long.");

		if (LastNameEntry.Text.Trim().Length < 2)
			return new ValidationResult(false, "Last name must be at least 2 characters long.");

		return new ValidationResult(true, string.Empty);
	}

	private bool IsValidEmail(string email)
	{
		try
		{
			var addr = new System.Net.Mail.MailAddress(email);
			return addr.Address == email && email.Contains("@") && email.Contains(".");
		}
		catch
		{
			return false;
		}
	}

	private ValidationResult ValidatePassword(string password)
	{
		if (password.Length < 8)
			return new ValidationResult(false, "Password must be at least 8 characters long.");

		if (!password.Any(char.IsUpper))
			return new ValidationResult(false, "Password must contain at least one uppercase letter.");

		if (!password.Any(char.IsLower))
			return new ValidationResult(false, "Password must contain at least one lowercase letter.");

		if (!password.Any(char.IsDigit))
			return new ValidationResult(false, "Password must contain at least one number.");

		if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
			return new ValidationResult(false, "Password must contain at least one special character.");

		return new ValidationResult(true, string.Empty);
	}

	private bool IsValidPhoneNumber(string phoneNumber)
	{
		// Remove common formatting characters
		var cleanedNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

		// Check if it's a reasonable length (10-15 digits is typical)
		return cleanedNumber.Length >= 10 && cleanedNumber.Length <= 15;
	}

	private void ClearValidationErrors()
	{

	}

	// Helper class for validation results
	public class ValidationResult
	{
		public bool IsValid { get; }
		public string ErrorMessage { get; }

		public ValidationResult(bool isValid, string errorMessage)
		{
			IsValid = isValid;
			ErrorMessage = errorMessage;
		}
	}

	private void OnLogInLabelTapped(object? sender, TappedEventArgs e)
	{
		Application.Current.MainPage = new Login(_userService);
	}
}