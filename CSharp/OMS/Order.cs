namespace ETS.OMS
{
    public struct Order : IOrder
    {
        public ulong Id { get; set; }

        public decimal Price { get; set; }

        public Side Side { get; set; }

        public ulong Volume { get; set; }
    }
}
