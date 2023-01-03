using SQuiz.Shared.Models.Interfaces;

namespace SQuiz.Shared.Models
{
    public abstract class ResourceItem : IResourceItem
    {
        public string Id { get; set; }
        public int ShortId { get; set; }

        public DateTimeOffset? DateCreated { get; set; } = DateTime.Now;

        public DateTimeOffset? DateUpdated { get; set; } = DateTime.Now;
    }
}
