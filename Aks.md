# AKS Test Deployment

## Install a Aks cluster
**Note:** Should run commands in a Azure cloud PowerShell
```powershell
$subscriptionid = 'ee6c4145-f2fc-4366-bca4-1a38775414f3'
$aksClusterName = 'xfakseuwe01'
$aksClusterResourceGroup = 'xfakseuwe01'
$nodeCount = '1'
$resourceGroupLocation = 'westeurope'
$acrName = 'xfacr01'
$sku = 'basic'

az login

# Change subscription
az account set --subscription $subscriptionid

# Confirm subscription
az account show --output table

# Create resource group
az group create `
   --location $resourceGroupLocation `
   --name $aksClusterResourceGroup

# Create the service principle
$svcPrincipleName = $aksClusterName + 'SvcPrinciple'
$svcPrinciple = (az ad sp create-for-rbac --skip-assignment  --name $svcPrincipleName | ConvertFrom-Json)
Write-Host $svcPrinciple

# Create AKS
az aks create `
   --name $aksClusterName `
   --resource-group $aksClusterResourceGroup `
   --node-count $nodeCount `
   --service-principal $svcPrinciple.appId `
   --client-secret  $svcPrinciple.password
   # --generate-ssh-keys `
```

## Creating a azure container registry
```powershell
# Create ACR
az acr create `
   --name $acrName `
   --resource-group $aksClusterResourceGroup `
   --sku $sku `
   --location $resourceGroupLocation

# Confirm ACR created
az acr list --output table

$acrId = az acr show `
   --name $acrName `
   --resource-group $aksClusterResourceGroup `
   --query "id" `
   --output tsv

az role assignment create `
   --assignee $svcPrinciple.appId `
   --role Reader `
   --scope $acrId

az aks update-credentials `
   --name $aksClusterName `
   --resource-group $aksClusterResourceGroup `
   --reset-service-principal `
   --service-principal $svcPrinciple.appId `
   --client-secret $svcPrinciple.password

az aks list --resource-group $aksClusterResourceGroup
```

## Create docker image and run it locally
```powershell
docker images

cd <Path to solution of AksTest>

# Run in the directory where the code is
dotnet build

# Create the image
cd AksTestFrontend
docker build . -t xinfli/akstestfrontend:dev

cd ..\AksTestBackend
docker build . -t xinfli/akstestbackend:dev

# Start docker
cd ..
docker run -p 8081:80 `
           --name AksTest_Backend `
           -e Options__FrontendApiEndpoint=http://localhost:8082/api/Message/SendMessage `
           xinfli/akstestbackend:dev

# Parameter below can be added for debugging
# -e ASPNETCORE_ENVIRONMENT=Development `

docker run -p 8082:80 `
           --name AksTest_Frontend `
           xinfli/akstestfrontend:dev
```

## Push image to acr
**Note:** Should run commands in a local PowerShell
```powershell
$acrName = 'xfacr01'

# Its a cli on the docker login command
az acr login --name $acrName

# Check the login server
az acr list --output table

# List the repositories in the acr
az acr repository list --name $acrName

# Show all local images
docker image list
# Tag image
docker tag xinfli/akstestbackend:dev xfacr01.azurecr.io/xfdemo/akstestbackend:v1
# Show updated local images
docker image list
# Push image to new created acr
docker push xfacr01.azurecr.io/xfdemo/akstestbackend:v1

docker image list
docker tag xinfli/akstestfrontend:dev xfacr01.azurecr.io/xfdemo/akstestfrontend:v1
docker image list
docker push xfacr01.azurecr.io/xfdemo/akstestfrontend:v1

az acr repository list -n $acrName -o table
```

## Add node pool

```powershell
$subscriptionid = "ee6c4145-f2fc-4366-bca4-1a38775414f3"
$aksClusterResourceGroup = "xfakseuwe01"
$aksClusterName = "xfakseuwe01"
$acrName = "xfacr01"

# Login
az account set --subscription $subscriptionid
az account show
az acr login --name $acrName

# Getting credentials from AKS into the kubectl environment.
az aks get-credentials --resource-group $aksClusterResourceGroup --name $aksClusterName

kubectl config get-contexts

# Create namespace
kubectl create -f .\\namespace.yaml
kubectl get namespace
kubectl get services

# Deploy frontend and backend application
kubectl apply -f .\\deploy-frontend.yaml --namespace=aksdemo
# Question: How to get frontend node IP address and apply it to backend?
kubectl apply -f .\\deploy-backend.yaml --namespace=aksdemo

kubectl get pods -o wide
kubectl proxy
kubectl get pods -n aksdemo

kubectl get deployments
kubectl get deployments -n aksdemo

kubectl get nodes
az aks nodepool add `
   --resource-group $aksClusterResourceGroup `
   --cluster-name $aksClusterName `
   --name nodepool1 `
   --labels service=backend

az aks nodepool list `
   --resource-group $aksClusterResourceGroup `
   --cluster-name $aksClusterName

kubectl get nodes
kubectl label node aks-nodepool1-28427571-vmss000000 service=backend
kubectl show label

kubectl describe node aks-nodepool1-28427571-vmss000000
kubectl apply -f .\\deploy-backend.yaml
az aks enable-addons --resource-group $aksClusterResourceGroup --name $aksClusterName -a kube-dashboard

```
