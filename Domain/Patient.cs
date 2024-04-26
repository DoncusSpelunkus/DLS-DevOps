using System.ComponentModel.DataAnnotations;

namespace DefaultNamespace;

public class Patient
{
    [Key] public string Ssn { get; set; }
    [EmailAddress] public string Mail { get; set; }
    public string Name { get; set; }
    public List<Measurement>? measurements { get; set; }
}
