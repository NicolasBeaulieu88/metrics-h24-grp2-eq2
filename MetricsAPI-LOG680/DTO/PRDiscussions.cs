using System.ComponentModel.DataAnnotations.Schema;

[Table("PRDiscussions")]
public class PRDiscussions
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

    [Column("comments")]
    public int Comments { get; set; }

    [Column("reviews")]
    public int Reviews { get; set; }

    [Column("reviewRequests")]
    public int ReviewRequests { get; set; }

    [Column("totalDiscussions")]
    public int TotalDiscussions { get; set; }
}