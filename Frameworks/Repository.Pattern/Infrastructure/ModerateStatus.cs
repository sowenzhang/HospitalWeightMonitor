namespace Repository.Pattern.Infrastructure
{
    // when a new record is added, it should first go to IsPeding
    // if the record is approved manually,  it will go to IsApproved status
    // finally if the record is deleted, the value will go to IsDeleted
    public enum ModerateStatus
    {
        IsPending, 
        IsApproved,
        IsDeleted
    }
}