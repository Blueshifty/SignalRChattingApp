namespace SignalRApp.Business.Utilities.Mapper
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}