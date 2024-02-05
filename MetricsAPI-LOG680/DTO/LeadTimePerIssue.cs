
using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;

[Table("lead_time_per_issue")] 

public class LeadTimePerIssue
{
    [System.ComponentModel.DataAnnotations.Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("issue_number")]
    public string IssueNumber { get; set; }

    [Column("lead_time")]
    public int LeadTime { get; set; }
}