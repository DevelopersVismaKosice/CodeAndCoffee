environment = "dev"
secret = "ahoj"

cdn_ttl = 60
cdn_max_ttl = 60

cognito_callback 	 = [ "http://localhost:3000/cognito" ]

api_gateway_salt 	 = "xxx"
aggregator_gateway 	 = "yyy"

cognito_users = [
    {
      email    = "test@test.sk"
      customId = "aaa"
      group    = "user"
      password = "xxx"
    },
        {
      email    = "test2@test.sk"
      customId = "aaa"
      group    = "user"
      password = "xxx"
    }  
]

allowed_countries = [
  "SK", "RO", "US", "GB"
]
