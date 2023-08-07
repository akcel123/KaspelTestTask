namespace KaspelTestTask.Persistence;

public static class DbInitializer
{
    public static void Initialize(KaspelTestTaskDbContext context)
    {
        //context.Database.EnsureDeleted(); //использовалось в ходе разработки
        context.Database.EnsureCreated();
    }
}

