1. Install dotnet tool:

    dotnet tool install --global dotnet-ef
2. Install package Microsoft.EntityFrameworkCore.Design & Npgsql.EntityFrameworkCore.PostgreSQL

3. Run command to gen entities:

    dotnet ef dbcontext scaffold "Host=database-1.cvisbvujuezh.ap-southeast-1.rds.amazonaws.com;Port=5432;Database=langgeneral;Username=postgres;Password=t0ps3cr3tt0ps3cr3t" Npgsql.EntityFrameworkCore.PostgreSQL -o Entities

