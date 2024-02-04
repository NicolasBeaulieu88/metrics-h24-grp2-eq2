using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;

[Table("column_activity_count")]

public class ColumnActivityCount
{
    [System.ComponentModel.DataAnnotations.Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")] 
    public int Id { get; set; }

    [Column("column_name")]
    public string ColumnName { get; set; }
    
    [Column("active_ticket_count")]
    public int ActiveTicketCount { get; set; }
}

