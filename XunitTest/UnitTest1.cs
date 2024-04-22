using PatientServices; // Ensure you're using the correct namespace

namespace XunitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var patientService = new PatientService(); // Instantiate the class
            Assert.Equal("John Doe", patientService.GetPatientName());
        }
    }
}
