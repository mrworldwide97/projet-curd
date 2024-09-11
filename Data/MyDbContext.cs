using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    // Définir les DbSets pour les entités
    public DbSet<Participant> Participants { get; set; }
}

public class Participant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;  // Valeur par défaut pour éviter l'erreur nullable
}
