namespace hotel_booking_utilities.Pagination
{
    public class PageResult<T>
    {
        public T PageItems { get; set; }
        public int Items { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public int NumberOfPages { get; set; }
        public int PreviousPage { get; set; }
    }
}
