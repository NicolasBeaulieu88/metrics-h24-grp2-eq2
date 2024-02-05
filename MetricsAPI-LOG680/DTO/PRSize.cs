using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;

[Table("PRSize")]
public class PRSize
{
    [System.ComponentModel.DataAnnotations.Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("pr_id")]
    public string PrId { get; set; }

    [Column("username")]
    public string Username { get; set; }

    [Column("repository")]
    public string Repository { get; set; }

    [Column("saved_date")]
    public DateTime SavedDate { get; set; }

    [Column("additions")]
    public int Additions { get; set; }

    [Column("deletions")]
    public int Deletions { get; set; }

    [Column("totalChanges")]
    public int TotalChanges { get; set; }
}