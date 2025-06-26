#pragma warning disable SA1200 // Using directives should be placed correctly
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TodoListApp.DataAccess;
using TodoListApp.WebApi.Helpers.Security;
using TodoListApp.WebApi.Interfaces;
using TodoListApp.WebApi.Models.Dtos.AuthDtos;
using TodoListApp.WebApi.Models.Models;
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
#pragma warning restore SA1200 // Using directives should be placed correctly

namespace TodoListApp.WebApi.AuthService;

public class EmailSender : IEmailSender
{
    private readonly UserDbContext userDbContext;
    private readonly IUserRepositroy userRepositroy;

    public EmailSender(IUserRepositroy userRepositroy, UserDbContext userDbContext)
    {
        this.userRepositroy = userRepositroy;
        this.userDbContext = userDbContext;
    }

    public async Task<ServiceResult> RequestPasswordReset(string email)
    {
        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
        {
            return new ServiceResult(false, "Gmail only.");
        }

        var user = await this.userRepositroy.SelectUserByEmailAsync(email);
        if (user == null)
        {
            return new ServiceResult(false, "User not found.");
        }

        var code = new Random().Next(100000, 999999).ToString();

        // Remove any existing unconfirmed reset code for this email
        var existing = this.userDbContext.Confirmations
            .Where(x => x.Email == email && x.Type == ConfirmationType.PasswordReset && !x.IsConfirmed);

        this.userDbContext.Confirmations.RemoveRange(existing);

        // Save new code
        var confirmation = new EmailConfirmation
        {
            Email = email,
            Code = code,
            Type = ConfirmationType.PasswordReset,
            ExpirationTime = DateTime.UtcNow.AddMinutes(15),
            IsConfirmed = false,
        };

        _ = await this.userDbContext.Confirmations.AddAsync(confirmation);
        _ = await this.userDbContext.SaveChangesAsync();

        // Send email
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("App", "no.reply.todolist.code@gmail.com"));
        message.To.Add(new MailboxAddress(string.Empty, email));
        message.Subject = "Reset Code";
        message.Body = new TextPart("plain") { Text = $"Code: {code}" };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync("no.reply.todolist.code@gmail.com", "npsm qycf ozqa rmos");
        _ = await client.SendAsync(message);
        await client.DisconnectAsync(true);

        return new ServiceResult(true, "Code sent.");
    }

    public async Task<ServiceResult> VerifyPasswordReset(VerifyResetRequest request)
    {
        if (!Regex.IsMatch(request.Email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
        {
            return new ServiceResult(false, "Gmail only.");
        }

        var confirmation = await this.userDbContext.Confirmations.FirstOrDefaultAsync(x =>
            x.Email == request.Email &&
            x.Code == request.Code &&
            x.Type == ConfirmationType.PasswordReset &&
            !x.IsConfirmed &&
            x.ExpirationTime > DateTime.UtcNow);

        if (confirmation == null)
        {
            return new ServiceResult(false, "Invalid or expired code.");
        }

        ApplicationUser? user = await this.userRepositroy.SelectUserByEmailAsync(request.Email);
        if (user == null)
        {
            return new ServiceResult(false, "User not found.");
        }

        var (hash, salt) = PasswordHasher.Hasher(request.NewPassword);
        user.Salt = salt;
        user.PasswordHash = hash;
        await this.userRepositroy.UpdateUserAsync(user);

        confirmation.IsConfirmed = true;
        _ = await this.userDbContext.SaveChangesAsync();

        return new ServiceResult(true, "Password reset. Log in again.");
    }

    public async Task<ServiceResult> SendEmailConfirmationCodeAsync(string email)
    {
        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
        {
            return new ServiceResult(false, "Gmail only.");
        }

        var code = new Random().Next(100000, 999999).ToString();

        // Remove existing unconfirmed email verification codes
        var old = this.userDbContext.Confirmations.Where(x =>
            x.Email == email &&
            x.Type == ConfirmationType.EmailVerification &&
            !x.IsConfirmed);
        this.userDbContext.Confirmations.RemoveRange(old);

        var confirmation = new EmailConfirmation
        {
            Email = email,
            Code = code,
            Type = ConfirmationType.EmailVerification,
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
        };

        _ = await this.userDbContext.Confirmations.AddAsync(confirmation);
        _ = await this.userDbContext.SaveChangesAsync();

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("TodoList App", "no.reply.todolist.code@gmail.com"));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = "Email Confirmation Code";
        message.Body = new TextPart("plain") { Text = $"Your confirmation code is: {code}" };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync("no.reply.todolist.code@gmail.com", "npsm qycf ozqa rmos");
        _ = await client.SendAsync(message);
        await client.DisconnectAsync(true);

        return new ServiceResult(true, "Confirmation code sent.");
    }

    public async Task<bool> VerifyEmailConfirmationAsync(string email, string code)
    {
        var confirmation = await this.userDbContext.Confirmations.FirstOrDefaultAsync(x =>
            x.Email == email &&
            x.Code == code &&
            x.Type == ConfirmationType.EmailVerification &&
            !x.IsConfirmed &&
            x.ExpirationTime > DateTime.UtcNow);

        if (confirmation is null)
        {
            return false;
        }

        confirmation.IsConfirmed = true;
        _ = await this.userDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsEmailConfirmedAsync(string email)
    {
        return await this.userDbContext.Confirmations.AnyAsync(x =>
            x.Email == email &&
            x.Type == ConfirmationType.EmailVerification &&
            x.IsConfirmed);
    }
}
