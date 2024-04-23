using Moq;
using PatientRepositories;
using PatientServices;

namespace XunitTest
{
    public class UnitTest1
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock = new Mock<IPatientRepository>();
        private readonly PatientService _patientService;

        public UnitTest1()
        {
            _patientService = new PatientService(_patientRepositoryMock.Object);
        }

        [Fact]
        public void Test1()
        {
            // Use _patientService instead of patientService
            Assert.Equal("John Doe", _patientService.TestMethod());
        }
    }
}
