using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Entities.Authorization.Role;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;


namespace AuthSystem.Domain.Factories;

/// <summary>
/// Factory برای ایجاد کاربر
/// </summary>
public static class UserFactory
{
  
    public static User CreateUserWithEmail(
        string email,
        string password,
        string firstName,
        string lastName,
        string? phoneNumber = null,
        DateTime? dateOfBirth = null,
        string? nationalCode = null,
        bool isEmailVerified = false)
    {
      
        var emailVo = Email.Create(email);

     
        var passwordHash = SecurityFactory.CreateSecurePassword(password);

       
        ValidateName(firstName, lastName);

      
        PhoneNumber? phoneVo = null;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            phoneVo = PhoneNumber.Create(phoneNumber);
        }

  
        NationalCode? nationalCodeVo = null;
        if (!string.IsNullOrWhiteSpace(nationalCode))
        {
            nationalCodeVo = NationalCode.Create(nationalCode);
        }

     
        return new User(
            Guid.NewGuid(),
            emailVo,
            passwordHash,
            firstName,
            lastName,
            phoneVo,
            dateOfBirth,
            nationalCodeVo,
         isEmailVerified);
    }

 
    public static User CreateUserWithPhone(
        string phoneNumber,
        string password,
        string firstName,
        string lastName,
        string? email = null,
        DateTime? dateOfBirth = null,
        string? nationalCode = null,
        bool isPhoneVerified = false)
    {
       
        var phoneVo = PhoneNumber.Create(phoneNumber);

       
        var passwordHash = SecurityFactory.CreateSecurePassword(password);

       
        ValidateName(firstName, lastName);

        Email? emailVo = null;
        if (!string.IsNullOrWhiteSpace(email))
        {
            emailVo = Email.Create(email);
        }

      
        NationalCode? nationalCodeVo = null;
        if (!string.IsNullOrWhiteSpace(nationalCode))
        {
            nationalCodeVo = NationalCode.Create(nationalCode);
        }

   
        return new User(
            Guid.NewGuid(),
            emailVo,
            passwordHash,
            firstName,
            lastName,
            phoneVo,
            dateOfBirth,
            nationalCodeVo,
      isPhoneVerified: isPhoneVerified);
    }

 
    public static User CreateUserForSocialLogin(
        string provider,
        string providerUserId,
        string email,
        string firstName,
        string lastName,
        string? phoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(provider))
            throw new ArgumentException("نام ارائه‌دهنده نمی‌تواند خالی باشد", nameof(provider));

        if (string.IsNullOrWhiteSpace(providerUserId))
            throw new ArgumentException("شناسه کاربر در شبکه اجتماعی نمی‌تواند خالی باشد", nameof(providerUserId));

       
        var emailVo = Email.Create(email);

      
        var passwordHash = SecurityFactory.CreateTemporaryPassword();

    
        ValidateName(firstName, lastName);

     
        PhoneNumber? phoneVo = null;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            phoneVo = PhoneNumber.Create(phoneNumber);
        }

        var user = new User(
            Guid.NewGuid(),
            emailVo,
            passwordHash,
            firstName,
            lastName,
            phoneVo,
            isEmailVerified: true,
            isSocialLogin: true);

    
        user.AddSocialLogin(provider, providerUserId);

        return user;
    }

  
    private static void ValidateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("نام نمی‌تواند خالی باشد", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("نام خانوادگی نمی‌تواند خالی باشد", nameof(lastName));

     
        if (firstName.Length > 50)
            throw new ArgumentException("نام نمی‌تواند بیشتر از 50 کاراکتر باشد", nameof(firstName));

        if (lastName.Length > 100)
            throw new ArgumentException("نام خانوادگی نمی‌تواند بیشتر از 100 کاراکتر باشد", nameof(lastName));

      
        if (!System.Text.RegularExpressions.Regex.IsMatch(firstName, @"^[\p{L}\s'-]+$"))
            throw new ArgumentException("نام فقط می‌تواند شامل حروف، فاصله، خط تیره و آپاستروف باشد", nameof(firstName));

        if (!System.Text.RegularExpressions.Regex.IsMatch(lastName, @"^[\p{L}\s'-]+$"))
            throw new ArgumentException("نام خانوادگی فقط می‌تواند شامل حروف، فاصله، خط تیره و آپاستروف باشد", nameof(lastName));
    }

   
    public static void AddRoleToUser(User user, Role role)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        if (user.HasRole(role.Id))
        {
            throw new InvalidUserRoleException(user.Id, role.Name, $"کاربر قبلاً نقش '{role.Name}' را دارد");
        }

        user.AddRole(role.Id, role.Name);
    }

 
    public static void AddRolesToUser(User user, IEnumerable<Role> roles)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (roles == null)
            throw new ArgumentNullException(nameof(roles));

        var newRoles = roles.Where(r => !user.HasRole(r.Id)).ToList();
        foreach (var role in newRoles)
        {
            user.AddRole(role.Id, role.Name);
        }
    }


    public static void RemoveRoleFromUser(User user, Role role)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        if (!user.HasRole(role.Id))
            throw new InvalidUserRoleException(user.Id, role.Name, $"کاربر نقش '{role.Name}' را ندارد");

  
        user.RemoveRole(role.Id);
    }

    
    public static User CreateTemporaryUserForEmailVerification(
        string email,
        string firstName,
        string lastName,
        string password)
    {
       
        var user = CreateUserWithEmail(
            email,
            password,
            firstName,
            lastName,
            isEmailVerified: false);

       
        user.SetStatus(UserStatus.Pending);

        return user;
    }

   
    public static User CreateTemporaryUserForPhoneVerification(
        string phoneNumber,
        string firstName,
        string lastName,
        string password)
    {
      
        var user = CreateUserWithPhone(
            phoneNumber,
            password,
            firstName,
            lastName,
            isPhoneVerified: false);

      
        user.SetStatus(UserStatus.Pending);

        return user;
    }

  
    public static User CreateAdminUser(
        string email,
        string password,
        string firstName,
        string lastName,
        string? phoneNumber = null)
    {
      
        var admin = CreateUserWithEmail(
            email,
            password,
            firstName,
            lastName,
            phoneNumber);

       
        admin.SetStatus(UserStatus.Active);

        admin.AddRole(Guid.Parse("A0000000-0000-0000-0000-000000000001"), "Admin");

        return admin;
    }

    public static User CreateGuestUser()
    {
      
        var email = $"guest-{Guid.NewGuid()}@example.com";
        var password = Guid.NewGuid().ToString();

      
        var guest = CreateUserWithEmail(
            email,
            password,
            "Guest",
            "User");

        
        guest.SetStatus(UserStatus.Active);

       
        guest.AddRole(Guid.Parse("G0000000-0000-0000-0000-000000000001"), "Guest");

        return guest;
    }

 
    public static User CreateUserWithTwoFactor(
        string email,
        string password,
        string firstName,
        string lastName,
        string? phoneNumber = null,
        bool isTwoFactorEnabled = false)
    {
       
        var user = CreateUserWithEmail(
            email,
            password,
            firstName,
            lastName,
            phoneNumber);

     
        if (isTwoFactorEnabled)
        {
            var secretKey = SecurityFactory.CreateTwoFactorSecretKey();
            user.EnableTwoFactorAuthentication(secretKey);
        }

        return user;
    }

}