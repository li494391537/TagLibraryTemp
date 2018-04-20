using SqlSugar;

namespace TagLibrary.Models {
    public class FileTagMapper {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int? Id { get; set; }

        [SugarColumn(IsNullable = false)]
        public int FileId { get; set; }

        [SugarColumn(IsNullable = false)]
        public int TagId { get; set; }
    }
}
