namespace ETS.OMS
{
    public struct Order : IOrder
    {
        public ulong Id { get; set; }

        public decimal Price { get;}

        public Side Side { get; }

        public ulong Volume { get; set; }
    }
}
