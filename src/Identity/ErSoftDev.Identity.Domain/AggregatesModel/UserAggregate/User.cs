﻿using System.Text;
using ErSoftDev.DomainSeedWork;
using ErSoftDev.Framework.IdGenerate;

namespace ErSoftDev.Identity.Domain.AggregatesModel.UserAggregate
{
    public class User : BaseEntity<long>, IAggregateRoot
    {
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string? CellPhone { get; private set; }
        public string? Email { get; private set; }
        public string? Image { get; private set; }
        public string SecurityStampToken { get; private set; }
        public Address? Address { get; private set; }


        private readonly List<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles;

        private readonly List<UserLogin> _userLogins = new();
        public IReadOnlyCollection<UserLogin> UserLogins => _userLogins;

        private readonly List<UserRefreshToken> _userRefreshTokens = new();
        public IReadOnlyCollection<UserRefreshToken> UserRefreshTokens => _userRefreshTokens;

        private User()
        {

        }

        public User(long id, string firstname, string lastname, string username, string password, string checkPassword)
        {
            var parameterValidation = new StringBuilder();
            if (id == 0)
                parameterValidation.Append(nameof(id) + " ");
            if (string.IsNullOrWhiteSpace(firstname))
                parameterValidation.Append(nameof(firstname) + " ");
            if (string.IsNullOrWhiteSpace(lastname))
                parameterValidation.Append(nameof(lastname) + " ");
            if (string.IsNullOrWhiteSpace(username))
                parameterValidation.Append(nameof(username) + " ");
            if (string.IsNullOrWhiteSpace(password))
                parameterValidation.Append(nameof(password) + " ");
            if (string.IsNullOrWhiteSpace(checkPassword))
                parameterValidation.Append(nameof(checkPassword) + " ");
            if (parameterValidation.Length > 0)
                throw new AppException(ApiResultStatusCode.Failed, ApiResultErrorCode.ParametersAreNotValid,
                    parameterValidation.ToString());

            if (password != checkPassword)
                throw new AppException(ApiResultStatusCode.Failed, ApiResultErrorCode.PasswordsAreNotEqual);

            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Username = username;
            Password = password;
            CreatorUserId = id;
        }

        public void UpdateUser(string firstname, string lastname, string cellPhone, string email, string image, Address address)
        {
            var parameterValidation = new StringBuilder();
            if (string.IsNullOrWhiteSpace(firstname))
                parameterValidation.Append(nameof(firstname) + " ");
            if (string.IsNullOrWhiteSpace(lastname))
                parameterValidation.Append(nameof(lastname) + " ");
            if (parameterValidation.Length > 0)
                throw new AppException(ApiResultStatusCode.Failed, ApiResultErrorCode.ParametersAreNotValid,
                    parameterValidation.ToString());

            var addressProperty = address.GetType().GetProperties();
            var addressPropertiesAtLeastHasOneValue = false;
            var addressPropertiesAtLeastHasOneNotValue = false;
            foreach (var property in addressProperty)
            {
                var value = property.GetValue(this, null);
                if (value != null)
                    addressPropertiesAtLeastHasOneValue = true;
                else
                    addressPropertiesAtLeastHasOneNotValue = true;
            }
            if (addressPropertiesAtLeastHasOneValue && addressPropertiesAtLeastHasOneNotValue)
                throw new AppException(ApiResultStatusCode.Failed,
                    ApiResultErrorCode.AllFieldsOfAddressMustBeFillOrNonOfFields);

            Firstname = firstname;
            Lastname = lastname;
            CellPhone = cellPhone;
            Email = email;
            Image = image;
            Address = address;
        }

        public string UpdateSecurityStampTokenAndGetRefreshToken(string securityStampToken,
            DateTime refreshTokenExpiry, string? deviceName, string? deviceUniqueId, string? fcmToken, string? browser,
            IIdGenerator idGenerator)
        {
            SecurityStampToken = securityStampToken;
            UpdaterUserId = Id;

            var refreshToken = string.Concat(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var previousRefreshToken = _userRefreshTokens.FirstOrDefault(token =>
                token.IsActive && token.IsRevoke == false && token.IsUse == false);
            previousRefreshToken?.DeletePreviousRefreshToken();

            _userRefreshTokens.Add(new UserRefreshToken(idGenerator.CreateId(), Id, refreshToken, true, false, false,
                refreshTokenExpiry));

            _userLogins.Add(new UserLogin(idGenerator.CreateId(), Id, deviceName, deviceUniqueId, fcmToken, browser));

            return refreshToken;
        }
    }
}
