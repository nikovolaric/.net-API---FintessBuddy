namespace fitnessBudyApi.Models;

public class Exercise
{
    public long id { get; set; }
    public required string name { get; set; }
    public required string description { get; set; }
    public required string body_part { get; set; }
}
