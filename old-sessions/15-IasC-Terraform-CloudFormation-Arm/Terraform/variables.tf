variable "region" {
  default = "eu-central-1"
}

variable "environment" {
  type = string
}

variable "aggregator_gateway" {
  type = string
}

variable "secret" {
  description = "A secret string between CloudFront and S3 to control access"
  type        = string
}

variable "api_gateway_salt" {
  type = string
}

variable "cdn_ttl" {
  type = number
}

variable "cdn_max_ttl" {
  type = number
}

variable "cognito_callback" {
  type = list
}

variable "cognito_users" {
  type        = list
  description = "list of complex objects - cognito users with 2 attributes (email, customId)"
  default     = []
}

variable "allowed_countries" {
  type        = list
  description = "list of allowed countries for WAF GeoMatch"
  default     = []
}
