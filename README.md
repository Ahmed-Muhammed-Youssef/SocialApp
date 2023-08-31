# Social App
A Single Page Application (SPA) with an API built using ASP.NET Core. The frontend is built with Angular, Bootstrap v5, and Angular Material Design. Users can upload pictures, send friend requests, and chat with each other. Admins can assign or remove roles from users. I'm still working on additional features for admins.

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
* ASP.NET Core 7 - API 
* Entity Framework Core - ORM
* AutoMapper - Mapping profiles
* SignalR - Real-time communications
* Docker - Containerization
### Database
* SQLite - Used for development
## Development
The app is continuously being improved and upgraded:

* The backend is upgraded from .NET 5 to .NET 7
* Angular upgrades required refactoring code and migrating to new versions, following guides like [Angular Update](https://update.angular.io/)
* Initially, a user can send a like to another user but users can't know if someone liked them or not. When two users like each other they become a match and only then they can chat with each other. Now, users can send friend requests to any user and they can also unsend the friend request. The other user gets notified about the friend request and can choose to accept (add as a friend) or decline.
* Bogus is used to seed the database with fake test data
* Using design patterns like unit of work and repository pattern
* JWT is used for authentication
## Getting Started
Clone the repo, run `dotnet run` on the API folder to start the API, Run `ng serve` on the WebUI folder to start the frontend!
