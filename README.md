This is a simple example of using gRPC with .net. Here we will have a CRUD operation about products and we are going to use sqlLite for persistance.

## Installation 

Download the .NET 7.0 SDK from the official Microsoft website:

```shell
https://dotnet.microsoft.com/en-us/download/dotnet/7.0
```

## Getting Started
1. Clone or Download the Repository: Clone this repository to your local machine or download it as a ZIP archive.
2. After downloading and installing the .NET 7.0 SDK, open a terminal or command prompt.
3. Navigate to your work directory and go to the Server folder using the cd command:

```shell
cd path/to/your/work/directory/grpc-example 
```
4. Run this command to execute the migrations and update the database
```
dotnet ef database update
```
5. Run the command to start the app
```shell
dotnet run
```