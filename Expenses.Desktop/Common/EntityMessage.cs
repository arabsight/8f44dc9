namespace Expenses.UI.Common
{
    public enum MessageType
    {
        Added,
        Deleted,
        Modified
    }

    public class EntityMessage<T> where T : class
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