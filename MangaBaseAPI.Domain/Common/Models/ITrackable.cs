namespace MangaBaseAPI.Domain.Common.Models
{
    public interface ITrackable
    {
        byte[] RowVersion { get; set; }
        DateTimeOffset CreatedDateTime { get; set; }
        DateTimeOffset? ModifiedDateTime { get; set; }
    }
}
