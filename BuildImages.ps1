$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
cd $scriptPath

# Run in the directory where the code is
dotnet build

# Create the image
docker build ".\AksTestBackend" -t xinfli/akstestbackend:dev
docker build ".\AksTestFrontend" -t xinfli/akstestfrontend:dev

docker run -d `
           -p 8082:80 `
           --name AksTest_Backend `
           --restart=always `
           xinfli/akstestbackend:dev

docker run -d `
           -p 8081:80 `
           --name AksTest_Frontend `
           --restart=always `
           -e Options__BackendApiEndpoint=http://172.17.0.4:80/api/Message/SendMessage `
           xinfli/akstestfrontend:dev
