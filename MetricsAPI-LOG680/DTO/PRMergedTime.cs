using System.ComponentModel.DataAnnotations.Schema;

[Table("PRMergedTime")]
public class GetPRMergedTime
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

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("merged_date")]
    public DateTime MergedDate { get; set; }
    
    [Column("merged_time")]
    public DateTime MergedTime { get; set; }
}