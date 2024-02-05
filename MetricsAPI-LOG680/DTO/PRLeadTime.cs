using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;


[Table("PRLeadTime")]
public class PRLeadTime
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

    [Column("closed_date")]
    public DateTime ClosedDate { get; set; }
    
    [Column("lead_time")]
    public TimeSpan LeadTime { get; set; }
}