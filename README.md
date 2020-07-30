# Digital Apprenticeships Service

## Employer Incentives Azure Functions

Licensed under the [MIT license](https://github.com/SkillsFundingAgency/das-employer-incentives-functions/blob/master/LICENSE)

#### Requirements

- Install [.NET Core 2.2 SDK](https://www.microsoft.com/net/download)
- Install [Visual Studio 2019](https://www.visualstudio.com/downloads/) with these workloads:
    - ASP.NET and web development
    - Azure development
- Install [SQL Server 2017 Developer Edition](https://go.microsoft.com/fwlink/?linkid=853016)
- Install [SQL Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Install [Azure Storage Emulator](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) (Make sure you are on atleast v5.3)
- Install [Azure Storage Explorer](http://storageexplorer.com/) 
- Install [Specflow](http://specflow.org/documentation/Installation/)
- Administrator Access

#### Setup

- Clone this repository
- Open Visual Studio as an administrator


##### Config
TBC


#### Run the solution
Start the project SFA.DAS.EmployerIncentives.Functions in Visual Studio

#### HttpTriggerRefreshLegalEntities

Access to the HTTP endpoint for this function is restricted in some environments, this restriction applies to the Azure Portal as well as tools running on your local machine (eg Postman, etc).  To gain access to this endpoint login to the [Azure Portal](https://portal.azure.com), search for the function app then navigate to Networking > Configure Access Restrictions > Add Rule and add an Allow rule for your IP Address.  Be sure to remove the Allow rule when you're done.


