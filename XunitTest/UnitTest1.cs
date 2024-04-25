using DefaultNamespace;
using Moq;
using PatientRepositories;
using PatientServices;

namespace XunitTest
{
    public class PatientTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock =
            new Mock<IPatientRepository>();
        private readonly PatientService _patientService;

        public PatientTests()
        {
            _patientService = new PatientService(_patientRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_ValidPatient_ReturnsCreatedPatient()
        {
            // Arrange
            var patientRepositoryMock = new Mock<IPatientRepository>();
            var patientService = new PatientService(patientRepositoryMock.Object);
            var patient = new Patient
            {
                Ssn = "123456789", // Example SSN
                Mail = "example@example.com", // Example email
                Name = "John Doe", // Example name
                Measurements = new List<Measurement>() // Assuming you have a Measurement class
            };

            patientRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<Patient>()))
                .ReturnsAsync(patient);

            // Act
            var createdPatient = await patientService.Create(patient);

            // Assert
            Assert.NotNull(createdPatient);
            Assert.Equal(patient.Ssn, createdPatient.Ssn);
            Assert.Equal(patient.Mail, createdPatient.Mail);
            Assert.Equal(patient.Name, createdPatient.Name);
        }

        [Fact]
        public async Task GetPatientById_ValidId_ReturnsPatient()
        {
            // Arrange
            var patientRepositoryMock = new Mock<IPatientRepository>();
            var patientService = new PatientService(patientRepositoryMock.Object);
            var patient = new Patient
            {
                Ssn = "123456789", // Example SSN
                Mail = "lmao.dk", // Example email
                Name = "John Doe", // Example name
                Measurements = new List<Measurement>() // Assuming you have a Measurement class
            };

            patientRepositoryMock
                .Setup(repo => repo.GetPatientById(It.IsAny<string>()))
                .ReturnsAsync(patient);

            // Act
            var foundPatient = await patientService.GetPatientById(patient.Ssn);

            // Assert
            Assert.NotNull(foundPatient);
            Assert.Equal(patient.Ssn, foundPatient.Ssn);
            Assert.Equal(patient.Mail, foundPatient.Mail);
            Assert.Equal(patient.Name, foundPatient.Name);
        }

        [Fact]
        public async Task GetAllPatients_ReturnsListOfPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient
                {
                    Ssn = "123456789",
                    Mail = "example1@example.com",
                    Name = "John Doe",
                    Measurements = new List<Measurement>()
                },
                new Patient
                {
                    Ssn = "987654321",
                    Mail = "example2@example.com",
                    Name = "Jane Smith",
                    Measurements = new List<Measurement>()
                },
                // Add more patients as needed
            };

            var patientRepositoryMock = new Mock<IPatientRepository>();
            patientRepositoryMock.Setup(repo => repo.GetAllPatients()).ReturnsAsync(patients);

            var patientService = new PatientService(patientRepositoryMock.Object);

            // Act
            var returnedPatients = await patientService.GetAllPatients();

            // Assert
            Assert.NotNull(returnedPatients);
            Assert.Equal(patients.Count, returnedPatients.Count);

            // Check each patient individually
            for (int i = 0; i < patients.Count; i++)
            {
                Assert.Equal(patients[i].Ssn, returnedPatients[i].Ssn);
                Assert.Equal(patients[i].Mail, returnedPatients[i].Mail);
                Assert.Equal(patients[i].Name, returnedPatients[i].Name);
                // Add additional checks for other properties if necessary
            }
        }

        [Fact]
        public async Task Delete_ValidId_DeletesPatient()
        {
            // Arrange
            var patientRepositoryMock = new Mock<IPatientRepository>();
            var patientService = new PatientService(patientRepositoryMock.Object);
            var idToDelete = "123456789"; // Example ID to delete

            // Act
            await patientService.Delete(idToDelete);

            // Assert
            patientRepositoryMock.Verify(repo => repo.Delete(idToDelete), Times.Once);
        }
    }
}
