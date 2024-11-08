provider "azurerm" {
  version         = "2.58.0"
  subscription_id = "8e2ac2a9-5c90-48e8-8cc8-08c706f255a1"

  features {
  }
}

variable location {
    type    = string
    default = "West Europe"
}

resource "azurerm_virtual_network" "this" {
  name                = "vnet-test2"
  location            = azurerm_resource_group.this.location
  resource_group_name = azurerm_resource_group.this.name
  address_space       = ["172.0.0.0/21"]
}

resource "azurerm_resource_group" "this" {
  name     = "Sample"
  location = var.location
}