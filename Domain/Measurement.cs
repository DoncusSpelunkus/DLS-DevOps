

namespace DefaultNamespace;

public class Measurement
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
    public bool Seen { get; set; } = false;
    
    public string PatientSsn { get; set; }
}

public class MeasurementDto
{
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
    public string PatientSsn { get; set; }
}