using Xunit;
using Moq;
using MeasurementService;
using DefaultNamespace;
using System.Threading.Tasks;

namespace XunitTest
{
    public class MeasurementServiceTests
    {
        [Fact]
        public async Task GetMeasurementById_ValidId_ReturnsMeasurement()
        {
            // Arrange
            var measurementRepoMock = new Mock<IMeasurementRepo>();
            var measurementService = new MeasurementService.MeasurementService(measurementRepoMock.Object);
            var measurement = new Measurement
            {
                Id = 1,
                Date = DateTime.Now,
                Systolic = 120,
                Diastolic = 80,
                PatientSsn = "123456789" // Example SSN
            };

            measurementRepoMock
                .Setup(repo => repo.GetMeasurementById(It.IsAny<int>()))
                .ReturnsAsync(measurement);

            // Act
            var foundMeasurement = await measurementService.GetMeasurementById(1, "dummySSN");

            // Assert
            Assert.NotNull(foundMeasurement);
            Assert.Equal(measurement.Id, foundMeasurement.Id);
            Assert.Equal(measurement.Date, foundMeasurement.Date);
            Assert.Equal(measurement.Systolic, foundMeasurement.Systolic);
            Assert.Equal(measurement.Diastolic, foundMeasurement.Diastolic);
            Assert.Equal(measurement.PatientSsn, foundMeasurement.PatientSsn);
        }
        
        [Fact]
        public async Task CreateMeasurement_ValidMeasurement_ReturnsCreatedMeasurement()
        {
            // Arrange
            var measurementRepoMock = new Mock<IMeasurementRepo>();
            var measurementService = new MeasurementService.MeasurementService(measurementRepoMock.Object);
            var measurement = new Measurement
            {
                Id = 1,
                Date = DateTime.Now,
                Systolic = 120,
                Diastolic = 80,
                PatientSsn = "123456789" // Example SSN
            };

            measurementRepoMock
                .Setup(repo => repo.CreateMeasurement(It.IsAny<Measurement>()))
                .ReturnsAsync(measurement);

            // Act
            var createdMeasurement = await measurementService.CreateMeasurement(measurement);

            // Assert
            Assert.NotNull(createdMeasurement);
            Assert.Equal(measurement.Id, createdMeasurement.Id);
            Assert.Equal(measurement.Date, createdMeasurement.Date);
            Assert.Equal(measurement.Systolic, createdMeasurement.Systolic);
            Assert.Equal(measurement.Diastolic, createdMeasurement.Diastolic);
            Assert.Equal(measurement.PatientSsn, createdMeasurement.PatientSsn);
        }
        
        
        
        
    }
}