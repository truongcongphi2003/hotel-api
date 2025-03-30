namespace hotel_booking_models
{
    public class Room : BaseEntity
    {
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string Name { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TaxAndFee { get; set; }
        public string? Gift { get; set; }
        public string? GiftDescription { get; set; }
        public bool? IsCancelable { get; set; }
        public bool? IsPayAtHotel { get; set; }
        public RoomType RoomType { get; set; }
        public ICollection<CancellationPolicy> CancellationPolicies { get; set; } = new List<CancellationPolicy>();
    }
}
