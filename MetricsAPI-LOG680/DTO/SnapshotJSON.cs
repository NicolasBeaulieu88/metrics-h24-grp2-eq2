using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;

[Table("SnapshotJSON")]
public class SnapshotJSON
{
    [System.ComponentModel.DataAnnotations.Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("project_id")] public string Project_id { get; set; }

    [Column("repository_name")] public string? Repository_name { get; set; }

    [Column("owner")] public string? Owner { get; set; }

    [Column("title")] public string Title { get; set; }

    [Column("ColumnsData", TypeName = "json")]
    public string Columns_data { get; set; }

    [Column("total_items")]
    public int Total_items { get; set; }
    
    [Column("timestamp")]
    public DateTime Timestamps { get; set; }
    
    [NotMapped]
    public Dictionary<string, int> Columns_dict_data { get; set; }
}