using MyProject.Domain.Entities.Implements;

namespace MyProject.Domain.Entities
{
    public class GetNguyenEntityPagingRes : PaginationResDTO<NguyenDto>
    {
    }
    public class NguyenEntity : Entity<Guid>
    {
        public string Name { get; set; }
    }
    public class NguyenDto
    {
        public string Name { get; set; }
    }
    public class Testtiep : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }
    }
    public abstract class PaginationResDTO<T>
      where T : class
    {
        public int TotalRecord { get; set; }

        public IEnumerable<T> Records { get; set; }
    }
}
