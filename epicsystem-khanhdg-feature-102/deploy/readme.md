buid EPIC.APIGateway:
        - docker build -f deploy/EPIC.APIGateway/Dockerfile -t apigateway
run EPIC.APIGateway
        -  docker run -d -p 8089:8089 --name apigateway apigateway

buid EPIC.IdentityServer:
	- docker build -f deploy/EPIC.IdentityServer/Dockerfile -t identityserver .
        -  docker run -d -p 8022:8022 --name identityserver identityserver
