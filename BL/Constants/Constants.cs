namespace BL.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string CompanyAdmin = "CompanyAdmin";
        public const string Freelancer = "Freelancer";
    }

    public static class Jwt
    {
        public const string SecureKey = "JWT:SecureKey";
    }

    public static class Messages
    {
        public const string WrongPasswordMessage = "Incorrect username or password";

        public const string DuplicateRole = "Role with the same name already exists.";
        public const string RoleNameNotFound = "Role was not found, Role name: ";
        public const string RoleIdNotFound = "Role was not found, RoleId: ";

        public const string DuplicateCountry = "Country with the same name already exists.";
        public const string CountryNotFound = "Country was not found, CountryId: ";
        public const string CountryNotFoundByName = "Country was not found, Country name: ";

        public const string DuplicateTown = "Town with the same name already exists.";
        public const string TownNotFound = "Town was not found, TownyId: ";


        public const string DuplicateLocation = "Location with the same name already exists.";
        public const string LocationNotFound = "Location was not found, LocationId: ";

        public const string DuplicateJobType = "Job type with the same name already exists.";
        public const string JobTypeNotFound = "Job type was not found, JobTypeId: ";

        public const string DuplicateUsername = "User with the same username already exists";
        public const string DuplicateEmail = "User with the same email already exists";

        public const string ContractorNotFound = "Contractor was not found, ContractorId: ";

        public const string ContractorLocationNotFound = "ContractorLocation was not found, ContractorLocationId: ";

        public const string DuplicateContractorLocation = "Same contractorLocation already exists";

        public const string JobPostNotFound = "Job post was not found, JobPostId: ";

        public const string JobApplicationNotFound = "Job application was not found, JobApplicatonId: ";

        public const string DuplicateJobApplication = "Job application with the same name already exists.";
        public const string DuplicateCompanyInContractor = "Company with the same name already exists in contractor.";

        public const string DuplicateFreelancerInContractor = "Freelancer with the same name already exists in contractor.";

        public const string PersonNotFound = "Person (AdminCmpany/Freelancer) was not found, PersonDto: ";

        public const string ContractorMustHaveCompanyOrBeFreelancer = "CompanyName must be provided if contractor is not a freelancer.";
    }
}
