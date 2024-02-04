using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;

[Table("lead_time_timeframe")] 

public class LeadTimeTimeframe
{
    [System.ComponentModel.DataAnnotations.Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("ticket_id")]
    public string TicketId { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("lead_time")]
    public int LeadTime { get; set; }
}