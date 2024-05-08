using Setup.Services;

Console.WriteLine("Kurulum başladı");

SetupService setupService = new SetupService();

setupService.HealthCheckProjectInstallation();

Console.Read();
