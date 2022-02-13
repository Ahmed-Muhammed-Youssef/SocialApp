const PROXY_CONFIG = [
    {
      context: [
        "/api/users",
        "/api/user",
        "/api/account/login",
        "/api/account/register"
      ],
      target: "https://localhost:5001",
      secure: false
    }
  ]
  
  module.exports = PROXY_CONFIG;
