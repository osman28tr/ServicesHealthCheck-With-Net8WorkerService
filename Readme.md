<h1>Windows Services Health Check Project</h1>

<h3>1. Entrance</h3>

<p align="justify">Windows services health check project is a windows service health check project that checks the status of windows services installed in our system by taking the names of these services from the appsettings.json file. If at least one of the services whose status is checked is not working, a notification is sent to the admin e-mail address written in the appsettings.json file. Logs the status of the services to the mongodb database. It controls the resource consumption of the services (e.g. CPU, memory usage) and instantly monitors all this data on a frontend.</p>

<h3>2. Used Technologies</h3>
.Net Worker Background Services, WindowsServices, Asp.Net Core Mvc, Asp.Net Core API, Repository Design Pattern, CQRS Design Pattern, Dtos, AutoMapper, SignalR, MailKit, MongoDb

<h3>3. General Outlines of the Architecture Used in the Project</h3>

<img src="HealthCheck.Presentation/HealthCheck.Admin/ServicesHealthCheck.Monitoring/wwwroot/images/arhitecture3.PNG" height="400px" width="650px">

<h3>4. Language and Development Environment Used: C# - .Net 8.0</h3>

<b>Note: </b> After installing the project, go to the appsettings.json file. Edit the "ConnectionStrings", "Services", "Notifications" fields according to your own system.
