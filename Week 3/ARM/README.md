* az login
* az account set --subscription <<guid>>
* az group create --location 'NorthEurope' --name AKS
* az deployment group create  --resource-group aks --template-file .\template.json --parameters .\parameters.json
* az aks get-credentials --resource-group AKS --name my-aks-cluster