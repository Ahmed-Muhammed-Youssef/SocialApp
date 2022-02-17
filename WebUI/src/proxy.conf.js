const PROXY_CONFIG = [
    {
      context: [
        "/api/users",
        "/api/user",
        "/api/account/login",
        "/api/account/register",
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
