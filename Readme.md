<h1>Windows Services Health Check Project</h1>

<h3>1. Entrance</h3>

<p align="justify">Windows services health check project is a windows service health check project that checks the status of windows services installed in our system by taking the names of these services from the appsettings.json file. If at least one of the services whose status is checked is not working, a notification is sent to the admin e-mail address written in the appsettings.json file. Logs the status of the services to the mongodb database. It controls the resource consumption of the services (e.g. CPU, memory usage) and instantly monitors all this data on a frontend.</p>

<p align="justify">The project being developed is discussed in 3 parts. These are as follows:<br/><br>
<b>HealthCheck.Infrastructure:</b> This is the part where various business and database processes of our application are carried out and the infrastructure of our application is hosted.<br><br>
<b>HealthCheck.Presentation:</b> This is the part where data from the worker service communicates with SignalR and is instantly displayed on the frontend.<br><br>
<b>HealthCheck.Servis:</b> Background services (worker services) required for our application are the part that includes some 3rd party services.
</p>

<h3>2. Technical Information</h3>
<h4>HealthCheck.Infrastructure - Layers</h4>

<p align="justify">
    <b>ServicesHealthCheck.Datas: </b> 
It is the layer where classes corresponding to collections in the database are defined.
</p>

<p align="justify">
    <b>ServicesHealthCheck.DataAccessLayer: </b> It is the layer where database-related CRUD operations are performed. There is a context class that corresponds to the database and where configuration settings related to the database are made. It references the entities layer to correspond to collections in the database.
</p>

<p align="justify">
    <b>ServicesHealthCheck.Shared: </b> It is the layer where classes and interfaces commonly used by more than one layer are defined. It contains IEntity and Settings elements. IEntity defines classes as collections of databases. The Settings class is the class that corresponds to the database configuration settings.
</p>

<p align="justify">
    <b>ServicesHealthCheck.Dtos: </b> It is the layer where dto classes are hosted instead of direct entities in the execution of database-related CRUD operations. Dto classes are used to send and receive data with the outside world. It increases the security of our application. Sending and receiving data to the outside world through entities creates security vulnerabilities and causes unwanted information flow. For this reason, dto classes are used to interact with the outside world.
</p>

<p align="justify">
    <b>ServicesHealthCheck.BusinessLayer: </b> It is the layer where application-related business processes are carried out and database manager classes are located. CQRS design pattern is used to separate data reading and writing operations in CRUD operations. In this way, flexibility and sustainability are increased. The mediator library is used to use the CQRS design pattern effectively. The automapper library is used to easily perform mapping between dto classes and entity classes. Mailkit library is used to send notifications. The signalr.client library is used to send instant data to the SignalR layer.
</p>

<h3>3. Used Technologies</h3>
<p align="justify">
.Net Worker Background Services, WindowsServices, Asp.Net Core Mvc, Asp.Net Core API, Repository Design Pattern, CQRS Design Pattern, Mediator, Dtos, AutoMapper, SignalR, MailKit, MongoDb</p>

<h3>4. General Outlines of the Architecture Used in the Project</h3>

<img src="HealthCheck.Presentation/HealthCheck.Admin/ServicesHealthCheck.Monitoring/wwwroot/images/arhitecture3.PNG" height="400px" width="650px">

<h3>5. Language and Development Environment Used: C# - .Net 8.0</h3>


<h3>6. Video Impression</h3>
<p align="justify">
For the video demonstration of the project, change the place that says "github.com" in your URL to "github.dev". Then you can follow this path and access the video.<br>
HealthCheck.Presentation -> HealthCheck.Admin -> ServicesHealthCheck.Monitoring -> wwwroot -> videos -> projectvideoimpression.mp4
</p>

<br/>

<p align="justify"> <b>Note: </b> After installing the project, go to the appsettings.json file. Edit the "ConnectionStrings", "Services", "Notifications" fields according to your own system.</p>

<p align="justify"> <b>Note: </b> In order for the notification service to work properly in the application, please update the e-mail addresses under the "Notifications" heading in the appsettings.json file accordingly. Then, turn on the two-step authentication feature of the sender's e-mail address in the application. Finally, go to the security section of the e-mail address and create a password for the application from the "application passwords" section. Go to the mailservice class in the business layer and type your password in the 2nd parameter shown in the </p>

<img src="HealthCheck.Presentation/HealthCheck.Admin/ServicesHealthCheck.Monitoring/wwwroot/images/mailservice.PNG">
<br><br>
<footer><b>Supported by CTS Yazılım as an intern project</b></footer>
