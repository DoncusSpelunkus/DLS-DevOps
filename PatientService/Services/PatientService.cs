﻿﻿using DefaultNamespace;
using PatientRepositories;

namespace PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public Task<Patient?> Create(Patient patient)
        {
            return _patientRepository.Create(patient);
        }

        public Task<Patient?> GetPatientById(string id)
        {
            return _patientRepository.GetPatientById(id);
        }

        public Task Delete(string id)
        {
            return _patientRepository.Delete(id);
        }

        public Task<Patient?> Update(Patient patient)
        {
            return _patientRepository.Update(patient);
        }

        public Task<List<Patient>> GetAllPatients()
        {
            return _patientRepository.GetAllPatients();
        }

        public string TestMethod()
        {
            return "John Doe";
        }

        public void RebuildDb()
        {
            _patientRepository.RebuildDb();
        }



    }
}