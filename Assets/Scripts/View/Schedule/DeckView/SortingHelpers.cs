namespace View.ScheduleView.Deck
{
    public enum Order { Ascending, Descending }
    public enum SortingType { ObtainDate, Name, ActionCost }
    
    public struct SortingState
    {
        public SortingType Type { get; }
        public Order Order { get; }

        public SortingState(SortingType type, Order order)
        {
            Type = type;
            Order = order;
        }
    }
}