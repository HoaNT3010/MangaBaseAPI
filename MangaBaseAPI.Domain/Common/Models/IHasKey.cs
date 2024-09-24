namespace MangaBaseAPI.Domain.Common.Models
{
    public interface IHasKey<T>
    {
        T Id { get; set; }
    }
}
