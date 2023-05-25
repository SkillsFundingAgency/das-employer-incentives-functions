namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types
{
    public enum EmploymentCheckResult
    {
        Employed = 0,
        NotEmployed = 1,
        NinoNotFound = 2,
        NinoFailure = 3,
        NinoInvalid = 4,
        PAYENotFound = 5,
        PAYEFailure = 6,
        NinoAndPAYENotFound = 7,
        HmrcFailure = 8
    }
}
