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
