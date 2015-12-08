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
        EndMin,
        CreatedById,
        Status
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

    public enum NewsFeedIndices
    {
        UserId,
        FirstName,
        LastName,
        Comment,
        Month,
        Day,
        Year,
        Hour,
        Minute
    }

    public enum PledgeIndices
    {
        UserId,
        EventId,
        Amount
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

    public enum EventStatus
    {
        Current,
        Cancelled,
        Removed
    }
}