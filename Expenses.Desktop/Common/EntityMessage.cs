using Expenses.Core.Shared;

namespace Expenses.UI.Common
{
    public enum MessageType
    {
        Added,
        Deleted,
        Modified
    }

    public class EntityMessage<T> where T : ITrackable
    {
        public EntityMessage(T entity, MessageType type)
        {
            Entity = entity;
            Type = type;
        }

        public MessageType Type { get; private set; }

        public T Entity { get; private set; }
    }
}