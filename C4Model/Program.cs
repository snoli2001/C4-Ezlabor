using Structurizr;
using Structurizr.Api;
using System.Linq;

namespace C4Model
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 65772;
            const string apiKey = "60814d99-843f-4209-aaf8-5543b56f7d22";
            const string apiSecret = "3ee20c69-c531-4ae7-80ec-fdf71168069c";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Ezlabor", "Ezlabor - C4 Model");
            Model model = workspace.Model;

            SoftwareSystem EzLaborystem = model.AddSoftwareSystem("Ezlabor System", "Ofrece el contenido estático y la aplicación desde una página de red de freelancers y empleadores");
            SoftwareSystem gmailSystem = model.AddSoftwareSystem("Gmail System", "Sistema de correos electrónicos interno de google");
            SoftwareSystem stripeSystem = model.AddSoftwareSystem("Stripe System", "Sistema de pagos por internet ");
            SoftwareSystem TwilioSystem = model.AddSoftwareSystem("Twilio System", "Sistema de verificiación de cuenta mediante SMS ");
            //SoftwareSystem googleMapsApi = model.AddSoftwareSystem("Google Maps API", "Permite a los clientes consultar información de sus cuentas y realizar operaciones.");

            Person Empresa = model.AddPerson("Empresa", "Compañia que busca los servicios de un freelancer");
            Person Freelancer = model.AddPerson("Freelancer", "Personas especializadas en ciertos rubros que buscan trabajar de manera independiente");
            Person Empleador = model.AddPerson("Empleador", "Persona con una microempresa o emprendimiento que necesita del servico de un freelancer");

            EzLaborystem.AddTags("Main System");
            gmailSystem.AddTags("Gmail API");
            //googleMapsApi.AddTags("Google Maps API");

            Empresa.Uses(EzLaborystem, "Visita el sitio usando", "[HTTPS]");
            Freelancer.Uses(EzLaborystem, "Visita el sitio usando","[HTTPS]");
            Empleador.Uses(EzLaborystem, "Visita el sitio usando", "[HTTPS]");

            EzLaborystem.Uses(gmailSystem, "Enviar mensajes de correos electrónicos interno de google");
            EzLaborystem.Uses(stripeSystem, "Realiza peticiones a la API");
            EzLaborystem.Uses(TwilioSystem, "Realiza peticiones a la API");
            gmailSystem.Delivers(Empresa, "Envia mensajes de correo electrónico");
            gmailSystem.Delivers(Freelancer, "Envia mensajes de correo electrónico");
            gmailSystem.Delivers(Empleador, "Envia mensajes de correo electrónico");


            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SystemContextView contextView = viewSet.CreateSystemContextView(EzLaborystem, "Contexto", "Diagrama de contexto - Ezlabor");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Person) { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Gmail API") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Google Maps API") { Background = "#a5cdff", Color = "#ffffff", Shape = Shape.RoundedBox });

            // 2. Diagrama de Contenedores
            Container webApplication = EzLaborystem.AddContainer("Aplicación Web Responsive", "Permite alos usuarios administrar el perfil, ver los mensajes, ver ofertas laboraler, etc", "Angular, nginx port 80");
            Container restApi = EzLaborystem.AddContainer("RESTful API", "Proporciona funcionalidad de red entre freelancers de perros y dueños", "SpringBoot, nginx port 5000");
            Container database = EzLaborystem.AddContainer("Base de Datos", "Repositorio de información de los usuarios", "Mysql");
            Container LandingPage = EzLaborystem.AddContainer("Landing Page", "Página con información de la StartUp", "Html, CSS y JS");

            webApplication.AddTags("WebApp");
            restApi.AddTags("API");
            database.AddTags("Database");
            LandingPage.AddTags("LandingPage");

            Empresa.Uses(webApplication, "Usa", "https 443");
            Empresa.Uses(LandingPage, "Usa", "https 443");
            Freelancer.Uses(webApplication, "Usa", "https 443");
            Freelancer.Uses(LandingPage, "Usa", "https 443");
            Empleador.Uses(webApplication, "Usa", "https 443");
            Empleador.Uses(LandingPage, "Usa", "https 443");
            webApplication.Uses(restApi, "Usa", "https 443");
            webApplication.Uses(stripeSystem, "https 443");
            webApplication.Uses(gmailSystem, "https 443");
            webApplication.Uses(TwilioSystem, "https 443");
            restApi.Uses(database, "Persistencia de datos");
            LandingPage.Uses(webApplication, "Redirige");

            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjM1MyIgaGVpZ2h0PSIyNTAwIiB2aWV3Qm94PSIwIDAgMjU2IDI3MiIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBwcmVzZXJ2ZUFzcGVjdFJhdGlvPSJ4TWlkWU1pZCI+PHBhdGggZD0iTS4xIDQ1LjUyMkwxMjUuOTA4LjY5N2wxMjkuMTk2IDQ0LjAyOC0yMC45MTkgMTY2LjQ1LTEwOC4yNzcgNTkuOTY2LTEwNi41ODMtNTkuMTY5TC4xIDQ1LjUyMnoiIGZpbGw9IiNFMjMyMzciLz48cGF0aCBkPSJNMjU1LjEwNCA0NC43MjVMMTI1LjkwOC42OTd2MjcwLjQ0NGwxMDguMjc3LTU5Ljg2NiAyMC45MTktMTY2LjU1eiIgZmlsbD0iI0I1MkUzMSIvPjxwYXRoIGQ9Ik0xMjYuMTA3IDMyLjI3NEw0Ny43MTQgMjA2LjY5M2wyOS4yODUtLjQ5OCAxNS43MzktMzkuMzQ3aDcwLjMyNWwxNy4yMzMgMzkuODQ1IDI3Ljk5LjQ5OC04Mi4xNzktMTc0LjkxN3ptLjIgNTUuODgybDI2LjQ5NiA1NS4zODNoLTQ5LjgwNmwyMy4zMS01NS4zODN6IiBmaWxsPSIjRkZGIi8+PC9zdmc+" });
            styles.Add(new ElementStyle("API") { Background = "#929000", Color = "#ffffff", Shape = Shape.RoundedBox, Icon = "data:image/svg+xml;base64,PHN2ZyBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB2aWV3Qm94PSIwIDAgNzY3LjggNzY4IiB3aWR0aD0iMjQ5OSIgaGVpZ2h0PSIyNTAwIj48c3R5bGU+LnN0MHtmaWxsOiM3N2JjMWZ9PC9zdHlsZT48cGF0aCBjbGFzcz0ic3QwIiBkPSJNNjk4LjMgNDBjLTEwLjggMjUuOC0yNC41IDUwLjMtNDEgNzIuOEM1ODUuMSA0MC42IDQ4Ny4xIDAgMzg1IDAgMTczLjggMCAwIDE3NCAwIDM4NS41IDAgNDkxIDQzLjIgNTkyIDExOS42IDY2NC44bDE0LjIgMTIuNmM2OS40IDU4LjUgMTU3LjMgOTAuNyAyNDggOTAuNyAyMDAuOCAwIDM2OS42LTE1Ny40IDM4My45LTM1OCAxMC41LTk4LjItMTguMy0yMjIuNC02Ny40LTM3MC4xem0tNTI0IDYyN2MtNi4yIDcuNy0xNS43IDEyLjItMjUuNiAxMi4yLTE4LjEgMC0zMi45LTE0LjktMzIuOS0zM3MxNC45LTMzIDMyLjktMzNjNy41IDAgMTQuOSAyLjYgMjAuNyA3LjQgMTQuMSAxMS40IDE2LjMgMzIuMyA0LjkgNDYuNHptNTIyLjQtMTE1LjRjLTk1IDEyNi43LTI5Ny45IDg0LTQyOCA5MC4xIDAgMC0yMy4xIDEuNC00Ni4zIDUuMiAwIDAgOC43LTMuNyAyMC04IDkxLjMtMzEuOCAxMzQuNS0zOCAxOTAtNjYuNSAxMDQuNS01My4yIDIwNy44LTE2OS42IDIyOS4zLTI5MC43QzYyMS45IDM5OC4yIDUwMS4zIDQ5OC4zIDM5MS40IDUzOWMtNzUuMyAyNy44LTIxMS4zIDU0LjgtMjExLjMgNTQuOGwtNS41LTIuOUM4MiA1NDUuOCA3OS4yIDM0NS4xIDI0Ny41IDI4MC4zYzczLjctMjguNCAxNDQuMi0xMi44IDIyMy44LTMxLjggODUtMjAuMiAxODMuMy04NCAyMjMuMy0xNjcuMiA0NC44IDEzMy4xIDk4LjcgMzQxLjUgMi4xIDQ3MC4zeiIvPjwvc3ZnPg==" });
            styles.Add(new ElementStyle("Database") { Background = "#ff0000", Color = "#ffffff", Shape = Shape.Cylinder });
            styles.Add(new ElementStyle("LandingPage") { Background = "#d1c07b", Color = "#ffffff", Shape = Shape.WebBrowser });
            ContainerView containerView = viewSet.CreateContainerView(EzLaborystem, "Contenedor", "Diagrama de contenedores - EzLabor");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            // 3. Diagrama de Componentes

            //Controllers
            Component signinController = restApi.AddComponent("Sign in Controller", "Permite a los usuarios ingresar al sistema de Ezlabor", "SpringBoot REST Controller");
            Component notificaionSystemController = restApi.AddComponent("Notification System Controller", "Permite al usuario recibir notificaciones", "SpringBoot REST Controller");
            Component subscriptionController = restApi.AddComponent("Subscription System Controller", "Permite al usuario recibir notificaciones", "SpringBoot REST Controller");
            Component MessageSystemController = restApi.AddComponent("Message System Controller", "Permite a los usuarios comunicarse mediante mensajes det texto", "SpringBoot REST Controller");
            Component LocationsController = restApi.AddComponent( "Location System Controller", "Permite a los usuarios ver los distintos tipos de localidades", "SpringBoot REST Controller");
            Component UserProfileController = restApi.AddComponent("User profile System Controller", "Permite a los usuarios editar su perfil", "SpringBoot REST Controller");
            Component HiringController = restApi.AddComponent("Hiring System Controller", "Permite usar los servicios de contratación y reunión que estan disponibles", "SpringBoot REST Controller");

            //Services
            Component signinService= restApi.AddComponent("Sign in Service", "Permite usar los servicios de sign in que se encuentra en la API");
            Component notificaionSystemService= restApi.AddComponent("Notification System Service", "Permite usar el servicio de notificación de la API");
            Component subscriptionService= restApi.AddComponent("Subscription System Service", "Permite usar los servicios de subscripción de la API");
            Component MessageSystemService= restApi.AddComponent("Message System Service", "Permite usar el servicio de mensajería de la API");
            Component LocationsService= restApi.AddComponent("Locations System Service", "Permite usar el servicio de localización de la API");
            Component UserProfileService= restApi.AddComponent("User profile System Service", "Permite usar el servicio de perfil de usuario de la API");
            Component HiringService= restApi.AddComponent("Hiring System Service", "Permite usar el servicio de contratación  de la API");

            //Repositories
            Component signinRepository= restApi.AddComponent("Sign in Repository", "Permite la comunicación entre el servicio de sign in  y la base de datos");
            Component notificaionSystemRepository = restApi.AddComponent("Notification System Repository", "Permite la comunicación entre el servicio de notificación y la base de datos");
            Component subscriptionRepository= restApi.AddComponent("Subscription System Repository", "Permite la comunicación entre el servicio de subscripción y la base de datos");
            Component MessageSystemRepository= restApi.AddComponent("Message  System Repository", "Permite la comunicación entre el servicio de mensajería  y la base de datos");
            Component LocationsRepository= restApi.AddComponent("Location System Repository", "Permite la comunicación entre el servicio localización y la base de datos");
            Component UserProfileRepository= restApi.AddComponent("User profile System Repository", "Permite la comunicación entre el servicio de perfil de usuario y la base de datos");
            Component HiringRepository= restApi.AddComponent("Hiring System Repository", "Permite la comunicación entre el servicio de contratación  y la base de datos");

            // Uses
            restApi.Components.Where(c => "SpringBoot REST Controller".Equals(c.Technology)).ToList().ForEach(c => webApplication.Uses(c, "Uses", "HTTPS"));
          
            signinController.Uses(signinService, "Uses");
            notificaionSystemController.Uses(notificaionSystemService, "Uses");
            subscriptionController.Uses(subscriptionService, "Uses");
            MessageSystemController.Uses(MessageSystemService, "Uses");
            LocationsController.Uses(LocationsService, "Uses");
            UserProfileController.Uses(UserProfileService, "Uses");
            HiringController.Uses(HiringService , "Uses");


            signinService.Uses(signinRepository, "Uses");
            notificaionSystemService.Uses(notificaionSystemRepository, "Uses");
            subscriptionService.Uses(subscriptionRepository, "Uses");
            MessageSystemService.Uses(MessageSystemRepository, "Uses");
            LocationsService.Uses(LocationsRepository, "Uses");
            UserProfileService.Uses(UserProfileRepository, "Uses");
            HiringService.Uses(HiringRepository, "Uses");


            signinRepository.Uses(database, "Lee y escribes datos");
            notificaionSystemRepository.Uses(database, "Lee y escribes datos");
            subscriptionRepository.Uses(database, "Lee y escribes datos");
            MessageSystemRepository.Uses(database, "Lee y escribes datos");
            LocationsRepository.Uses(database, "Lee y escribes datos");
            UserProfileRepository.Uses(database, "Lee y escribes datos");
            HiringRepository.Uses(database, "Lee y escribes datos");
          


            ComponentView componentViewForRestApi = viewSet.CreateComponentView(restApi, "Components", "The components diagram for the REST API");
            componentViewForRestApi.PaperSize = PaperSize.A4_Landscape;
            componentViewForRestApi.AddAllContainers();
            componentViewForRestApi.AddAllComponents();
            componentViewForRestApi.Add(Empleador);   
            componentViewForRestApi.Add(Empresa);   
            componentViewForRestApi.Add(Freelancer);   

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}