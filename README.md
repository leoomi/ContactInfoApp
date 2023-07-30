# ContactInfoApp
Repo created for an interview coding challenge. This is a simple Contacts app that allows to store multiple people and multiple contact information for each person.
This uses .Net Core 7.0 and Angular 15. For simplicity's sake, this uses SQLite with Entity Framework Core.

## Running project
The project can be run using the dotnet command from the root directory:
```
dotnet restore
dotnet run --project src/ContactInfo.App
```

This was built using the .Net Core Angular template and is supposed to run the Angular project but doesn't seem to work, at least for me.
So the Angular project has to be run independently for development from the src/ContactInfo.App/ClientApp directory: 
```
npm install
npm start
```

## Tests
Tests can be run with:
```
dotnet test
```
GitHub actions also run unit tests on Pull Requests to main.

## Deploying
The project can be built for production by running:
```
dotnet public -c Release -o out
```
Alternatively, the project can be built and run through Docker by using the Dockerfile.
GitHub actions were implemented in .github/workflows for CI/CD. On main branch merges, builds a docker image, pushes to Docker Hub, and updates the container running in the EC2.
