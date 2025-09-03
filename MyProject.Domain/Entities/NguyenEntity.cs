using MyProject.Domain.Entities.Implements;

namespace MyProject.Domain.Entities
{
    public class NguyenEntity : Entity<Guid>
    {
        public string Name { get; set; }
    }

    public class Testtiep : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }
    }
}
