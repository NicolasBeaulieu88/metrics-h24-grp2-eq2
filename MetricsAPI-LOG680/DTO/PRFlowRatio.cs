using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;


[Table("PRFlowRatio")]
public class PRFlowRatio
{
    [System.ComponentModel.DataAnnotations.Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    public string Username { get; set; }

    [Column("repository")]
    public string Repository { get; set; }

    [Column("saved_date")]
    public DateTime SavedDate { get; set; }

    [Column("openedPR")]
    public int OpenedPR { get; set; }

    [Column("closedPR")]
    public int ClosedPR { get; set; }

    [Column("flowRatio")]
    public double FlowRatio { get; set; }
}