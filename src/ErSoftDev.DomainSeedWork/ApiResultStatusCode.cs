namespace ErSoftDev.DomainSeedWork
{
    public enum ApiResultStatusCode
    {
        Success = 0,
        Failed = 1,
        Unknown = 2
    }

    public enum ApiResultErrorCode
    {
        NotFound = 1,
        BadRequest = 2,
        LogicError = 3,
        DbError = 4,
        AlreadyExists = 5,
        Unauthorized = 6,
        TokenIsExpired = 7,
        TokenHasNotClaim = 8,
        TokenIsNotSafeWithSecurityStamp = 9,
        ParametersAreNotValid = 10,
        PasswordsAreNotEqual = 11,
        AllFieldsOfAddressMustBeFillOrNonOfFields = 12,
        UsernameOrPasswordIsNotCorrect = 13,
        OneOfTheBrowserOrDeviceNameMustBeFill = 14
    }
}