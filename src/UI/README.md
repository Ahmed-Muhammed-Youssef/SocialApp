# SocialApp UI

This project is the frontend for the **SocialApp** application, built using **Angular**. It provides a rich, interactive user interface for a social networking platform, featuring user authentication, personalized newsfeeds, user profiles, and real-time direct messaging.

## 🚀 Tech Stack

- **Framework:** [Angular](https://angular.dev/) (v21)
- **UI Components:** [Angular Material](https://material.angular.io/)
- **Real-time Communication:** [@microsoft/signalr](https://www.npmjs.com/package/@microsoft/signalr)
- **Styling:** SCSS / CSS
- **Reactivity:** [RxJS](https://rxjs.dev/)

## 📂 Project Structure

The source code is thoughtfully organized within `src/app/` to ensure scalability and maintainability:

- **`auth/`**: Contains the `Login` and `Signup` components (including **Google Sign-In** support), route guards (`authenticatedGuard`, `notAuthenticatedGuard`) to manage access control, and the `auth-interceptor` HTTP interceptor that attaches the JWT bearer token to outgoing API requests.
- **`direct-chat/`**: Houses the `Chat` component, utilizing Microsoft SignalR for real-time, bi-directional messaging capabilities between users.
- **`home/`**: The landing page for unauthenticated users, providing an entry point to the application.
- **`newsfeed/`**: Features the `NewsfeedPage` component with its `create-post`, `post-list`, and `post-item` subcomponents, displaying a stream of posts and updates for the authenticated user.
- **`user/`**: Contains user-specific components such as the `Profile` page for viewing user details and `user-pictures` for managing uploaded pictures and setting the profile picture.
- **`shared/`**: Contains common, reusable elements used across the application:
  - `global-loader/`: A global loading indicator component.
  - `nav-bar/`: The main navigation bar.
  - `models/`: TypeScript interfaces and classes defining the data structures.
  - `services/`: Shared services (e.g., `chat-hub.service` for the SignalR connection, `data-reference.service` for city lookups, `loader` for the global loading state).

### Environment Configuration

API endpoints are configured per environment in `src/environments/`:

| File | `apiUrl` / `hubUrl` |
| :--- | :--- |
| `environment.development.ts` | `https://localhost:5001/api` / `https://localhost:5001/hubs` (local backend) |
| `environment.ts` (production) | The Azure-hosted API |

Run the [backend](../API/README.md) locally on `https://localhost:5001` for the development configuration to work out of the box.

## 🗺️ Routing Configuration

The application's routing (`app.routes.ts`) is structured as follows:

| Path | Component | Route Guard | Description |
| :--- | :--- | :--- | :--- |
| `/` | `Home` | `notAuthenticatedGuard` | Landing page for guests. |
| `/login` | `Login` | `notAuthenticatedGuard` | User login page. |
| `/signup` | `Signup` | `notAuthenticatedGuard` | User registration page. |
| `/newsfeed` | `NewsfeedPage` | `authenticatedGuard` | Main content feed for logged-in users. |
| `/profile/:id` | `Profile` | `authenticatedGuard` | View a specific user's profile. |
| `/chat/:id` | `Chat` | `authenticatedGuard` | Real-time direct messaging interface. |

## 🛠️ Getting Started

### Prerequisites

Ensure you have the following installed on your local machine:
- [Node.js](https://nodejs.org/) (LTS recommended)
- [Angular CLI](https://github.com/angular/angular-cli) (`npm install -g @angular/cli`)

### Installation

1. Navigate to the UI project directory if you aren't already there:
   ```bash
   cd src/UI
   ```
2. Install the necessary dependencies:
   ```bash
   npm install
   ```

### Development Server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

### Building for Production

To build the project for production, run:

```bash
ng build
```

This will compile your project and store the optimized build artifacts in the `dist/` directory.

### Running Tests

Unit tests are written with **Jasmine** and run via **Karma**:

```bash
ng test
```

Code style is enforced with **Prettier** (configuration embedded in `package.json`).

## 📋 Roadmap / Next Steps

Based on the capabilities already supported by the backend API, the following features are planned for implementation in the UI to ensure full synchronization:

### 1. Friend Requests & Friends Management
The backend fully supports friend requests (`FriendRequestsController`) and user relationship tracking (`UsersController`), which currently lack UI counterparts.
- **Friend Requests View:** Create a dropdown in the navigation bar or a dedicated page to view and manage incoming friend requests.
- **Profile Interactions:** Update the user profile component to include action buttons:
  - Add Friend
  - Accept Request
  - Cancel/Reject Request
- **User Discovery:** Implement a "Find Friends" or user search page utilizing the backend's filtering and pagination capabilities (`MinAge`, `MaxAge`, `RelationFilter`).

### 2. Edit Profile Functionality
While viewing profiles and uploading profile pictures are supported, the UI currently lacks a mechanism to edit text-based user profile information.
- **Edit Profile Form:** Create a dialog or settings page to allow users to update their `FirstName`, `LastName`, `Bio`, and `CityId` via the backend `PUT /api/users` endpoint.
- **City Selection Dropdown:** Integrate with the `GET /api/referencedata/cities` endpoint to populate a dropdown for selecting the `CityId` in the edit form.

### 3. Admin Dashboard & Role Management
The backend implements a complete Role-Based Access Control (RBAC) system protected by a `RequireAdminRole` policy (`RolesController` and `UserRolesController`). The UI currently lacks an interface for this.
- **Role Management:** Create an admin panel to view, create, and delete system roles using the `/api/roles` endpoints.
- **User Roles Management:** Add functionality to the admin panel or user profiles allowing administrators to assign (`POST`) and revoke (`DELETE`) roles for specific users via the `/api/users/{userId}/roles` endpoints.
