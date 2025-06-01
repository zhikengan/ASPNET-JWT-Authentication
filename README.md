# ASP.NET Jwt Authentication 

This JWT authentication flow involves issuing an access token and refresh token upon user authentication. The access token is used for accessing protected resources, while the refresh token is used to obtain new tokens when the access token expires. The diagram below outlines the process.

## Flow Overview 
![image](https://github.com/user-attachments/assets/54ee94d3-5ae0-438f-a58c-c54595674ad0)

---

## Current Project Refresh Token Implementation

**Step (8) – Refresh Token Handling:**
- **(a)** In this implementation, every time a refresh token is used, a new refresh token is issued to the client, and the old refresh token is invalidated (revoked).
- **(b)** If a request is made using an old (revoked) refresh token, all refresh tokens belonging to the user are invalidated. This action forces a logout on all devices after their access tokens expire, enhancing security by preventing session hijacking across devices.

---

## Considerations / Improvements

- **Memory Usage:** Storing refresh tokens (including revoked ones) for every user can consume significant memory over time.  
- **Recommendation:** Implement an automated cleanup service to periodically remove expired or invalidated tokens from storage, keeping the system efficient and secure.

--- 

## How to Run This Project

To get started with this project, please follow the steps below:

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) must be installed on your system.
- [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (Entity Framework Core CLI) must be installed.  
  You can install it globally by running:
  ```
  dotnet tool install --global dotnet-ef
  ```

### Steps

1. Open a terminal window.
2. Navigate to the project directory:
   ```
   cd jwtauth/jwtauth
   ```
3. Apply the database migrations:
   ```
   dotnet ef database update
   ```
   This command will create or update the database schema as needed.

4. Run the project:
   ```
   dotnet run
   ```

The project should now be running and ready for use.
