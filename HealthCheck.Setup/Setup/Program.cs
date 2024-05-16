using Setup.Services;

SetupService setupService = new SetupService();

setupService.HealthCheckProjectInstallation();

Console.Read();
