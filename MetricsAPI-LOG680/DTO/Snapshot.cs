using System.ComponentModel.DataAnnotations.Schema;

namespace MetricsAPI_LOG680.DTO;

[Table("snapshots")]
public class Snapshot
{
    [System.ComponentModel.DataAnnotations.Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")] 
    public int Id { get; set; }
    
    [Column("project_id")]
    public string Project_id { get; set; }
    
    [Column("repository_name")]
    public string? Repository_name { get; set; }
    
    [Column("owner")]
    public string? Owner { get; set; }
    
    [Column("title")]
    public string Title { get; set; }
    
    [Column("backlog_items")]
    public int Backlog_items { get; set; }
    
    [Column("a_faire_items")]
    public int A_faire_items { get; set; }
    
    [Column("en_cours_items")]
    public int En_cours_items { get; set; }
    
    [Column("revue_items")]
    public int Revue_items { get; set; }
    
    [Column("terminee_items")]
    public int Terminee_items { get; set; }
    
    [Column("total_items")]
    public int Total_items { get; set; }
    
    [Column("timestamp")]
    public DateTime Timestamps { get; set; }
}