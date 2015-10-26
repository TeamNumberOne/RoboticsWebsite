namespace RoboticsWebsite.Enums
{
    public enum EventIndices
    {
        EventId = 0,
        Type,
        Title,
        Description,
        StartTime,
        EndTime,
        Day
    }

    public enum UserIndices
    {
        UserId = 0,
        Type,
        Email,
        Password,
        Status
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
        Pending, // Used if user exists but hasn't been approved yet
        Rejected,
        Unknown  // Used to determine if the user is in the db or not
    }

    public enum UserStatus
    {
        Pending,
        Approved,
        Rejected
    }
}