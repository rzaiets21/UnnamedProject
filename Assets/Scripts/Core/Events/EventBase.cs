namespace Core.Events
{
    public class EventBase
    {
        private static int Id;

        private int _id;

        public EventBase()
        {
            _id = Id++;
        }
    }
}
