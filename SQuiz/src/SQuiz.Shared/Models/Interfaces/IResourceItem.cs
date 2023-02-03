namespace SQuiz.Shared.Models.Interfaces
{
    public interface IResourceItem
    {
        public string Id { get; set; }
        public int ShortId { get; set; }

        public DateTimeOffset? DateCreated { get; set; }

        public DateTimeOffset? DateUpdated { get; set; }
    }
}
