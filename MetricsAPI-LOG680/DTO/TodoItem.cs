using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;

[Table("todo_items")]
public class TodoItem
{
    [System.ComponentModel.DataAnnotations.Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")] 
    public int Id { get; set; }
    
    [Column("title")]
    public string Title { get; set; }
    
    [Column("is_completed")]
    public bool IsCompleted { get; set; }
}