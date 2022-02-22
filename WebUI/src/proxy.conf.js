const PROXY_CONFIG = [
    {
    context: [
        "/api/users/all",
        "/api/users/info/id",
        "/api/users/info/username",
        "/api/users/update",
        "/api/account/login",
        "/api/account/register",
        "/api/likes",
        "/api/matches",
        "/api/buggy/server-error",
        "/api/buggy/not-found",
        "/api/buggy/auth",
        "/api/buggy/bad-request"
      ],
      target: "https://localhost:5001",
      secure: false
    }
  ]
  
  module.exports = PROXY_CONFIG;
