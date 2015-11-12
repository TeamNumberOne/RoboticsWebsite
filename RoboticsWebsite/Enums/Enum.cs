namespace RoboticsWebsite.Enums
{
    public enum EventIndices
    {
        EventId = 0,
        Type,
        Title,
        Description,
        Month,
        Day,
        Year,
        StartHour,
        StartMin,
        EndHour,
        EndMin
    }

    public enum UserIndices
    {
        UserId = 0,
        Type,
        Email,
        Password,
        Status,
        FirstName,
        LastName
    }

    public enum EventType
    {
        Class,
        Competition,
        Meeting,
        Initial
    }

    public enum UserType
    {
        Admin,
        Student,
        Volunteer,
        Sponsor,
        Guest
    }

    public enum UserStatus
    {
        Pending,
        Approved,
        Rejected,
        Unknown
    }
}