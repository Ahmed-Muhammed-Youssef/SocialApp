# Social App
A Single Page Application (SPA) with an API built using ASP.NET Core. The frontend is built with Angular, Bootstrap v5, and Angular Material Design. Users can upload pictures, send friend requests, and chat with each other. Admins can assign or remove roles from users. I'm still working on additional features for admins.

## Live Demo
https://www.youtube.com/watch?v=omsDC4MSMSA
## Features
* User registration and authentication using JWT tokens
* Real-time messaging via SignalR
* Friend requests and connections
* User profiles with photo uploads using Cloudinary
* Admins can assign roles to users
* Admins can manage images (still in development)
## Tech Stack
### Frontend
* Angular 16 - TypeScript, component framework
* Bootstrap 5 - CSS framework for styles
* Angular Material - UI components
### Backend
* ASP.NET Core 8 - API 
* Entity Framework Core - ORM
* Dapper - Micro ORM for performance demanding database operations
* AutoMapper - Mapping profiles
* SignalR - Real-time communications
* Docker - Containerization
### Database
* SQL Server 
## Development
The app is continuously being improved and upgraded:

* The backend is upgraded from .NET 5 to .NET 8
* Angular upgrades required refactoring code and migrating to new versions, following guides like [Angular Update](https://update.angular.io/)
* Initially, a user can send a like to another user but users can't know if someone liked them or not. When two users like each other they become a match and only then they can chat with each other. Now, users can send friend requests to any user and they can also unsend the friend request. The other user gets notified about the friend request and can choose to accept (add as a friend) or decline.
* Bogus is used to seed the database with fake test data
* Using design patterns like unit of work and repository pattern
* JWT is used for authentication
* Migrated to Clean Architecture
## The Future of this App
This app is still under development and I have many plans to improve it. Some of the features that I intend to add are:
* Developing UI components using various frameworks such as Blazor, Razor, and MVC.
* Upgrading the Angular app to the latest version to benefit from the latest features and enhancements.
* Adding health checks to monitor the status and performance of the app and its components.
* Enabling users to post, comment, react, and share content like Facebook and X, to foster social interaction and engagement.
* Performing security audits and fixing any vulnerabilities that may arise.
* Building a strong authentication and authorization system to protect user data and privacy.
* Refactoring the system design from a monolithic architecture to a microservices-based one.
* Creating a mobile app version using MAUI to reach more users and platforms.
## Getting Started
* Clone the repository
* Create a [Cloudnary](https://cloudinary.com/) account
* Add your account information either in the project secrets file (recommended) or in appsettings in the following form:<br>
  `"Cloudinary": 
  "CloudName": "Add you cloudnary name",
  "APIKey": "Add your API key",
  "APISecret": "Add your API secret"
}` 
* Run `dotnet run` on the API folder to start the API
* Run `ng serve` on the WebUI folder to start the frontend!
