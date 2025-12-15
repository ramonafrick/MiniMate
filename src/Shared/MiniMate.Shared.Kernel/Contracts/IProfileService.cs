namespace MiniMate.Shared.Kernel.Contracts
{
    public interface IProfileService
    {
        Task<IUserProfile> GetProfileAsync();
        Task SaveProfileAsync(IUserProfile profile);
    }

    public interface IUserProfile
    {
        string? UserName { get; }
        string? LocationName { get; }
        double? Latitude { get; }
        double? Longitude { get; }
    }
}
