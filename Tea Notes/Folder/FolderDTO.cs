using Dapper.Contrib.Extensions;

namespace Tea_Notes
{
    [Table("Folders")]
    class FolderDTO
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public int ParentId { get; set; }

        public int UserId { get; set; }
    }
}
