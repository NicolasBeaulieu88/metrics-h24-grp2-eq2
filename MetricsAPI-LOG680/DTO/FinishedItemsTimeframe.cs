using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;

[Table("finished_items_timeframe")] 

public class FinishedItemsTimeframe
{
    [System.ComponentModel.DataAnnotations.Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("finished_items_count")]
    public int FinishedItemsCount { get; set; }
}