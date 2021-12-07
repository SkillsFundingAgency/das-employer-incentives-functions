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
Create a row in the Azure table storage Configuration table with the key SFA.DAS.EmployerIncentives.Functions_1.0 using the config JSON from the das-employer-config repository


#### Run the solution
Start the project SFA.DAS.EmployerIncentives.Functions in Visual Studio


