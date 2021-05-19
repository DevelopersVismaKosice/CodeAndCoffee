data "aws_caller_identity" "current" {}

provider "aws" {
  region = var.region
}

provider "aws" {
  alias  = "virginia"
  region = "us-east-1"
}

terraform {
  required_version = ">= 0.13.4"
//  backend "s3" {
//    bucket = "xxx-terraform"
//    key    = "terraform-dev.tfstate"
//    region = "eu-central-1"
//  }
}

locals {
  common_tags = {
    "App"       = "xxx"
    "ManagedBy" = "Terraform"
  }
}




