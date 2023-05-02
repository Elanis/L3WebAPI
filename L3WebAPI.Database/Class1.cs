namespace L3WebAPI.Database {
    public class Class1 {

    }
}


// 1/ Installer postgres
// 2/ Créer user (sous pgadmin: login/role, clic droit, create, login/role.
//      Puis remplir username/password et cocher "Can login")
// 3/ Créer db
// 4/ Lancer solution .NET
// 5/ Créer projet "Class Library" / "Bibliothèque de classes", .NET 7, "L3WebAPI.Database"
// 6/ Lancer une CLI et taper la commande "dotnet tool install --global dotnet-ef"
// 7/ Installer le NuGet "Npgsql.EntityFrameworkCore.PostgreSQL" 7.0.4 sur L3WebAPI.Database
// 8/ Installer le NuGet "Microsoft.EntityFrameworkCore.Design" 7.0.5 sur L3WebAPI

public class WagonId {
    public Guid Value { get; set; }

}
