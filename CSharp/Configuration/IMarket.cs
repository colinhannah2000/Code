namespace ETS.Configuration
{
    public interface IMarket
    {
        string Symbol { get; }
        string Name { get;  }
        string Industry { get;  }
        ulong Capitalisation { get;  }
        decimal Weight { get;  }
    }
}
