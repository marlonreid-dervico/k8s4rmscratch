* az login
* az account set --subscription 54cb7796-5170-4a43-8f14-30a4fa9cb077
* az group create --location 'NorthEurope' --name AKS
* az deployment group create  --resource-group aks --template-file .\template.json --parameters .\parameters.json
* az aks get-credentials --resource-group AKS --name my-aks-cluster