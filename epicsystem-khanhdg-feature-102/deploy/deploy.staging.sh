#!/bin/bash#
# variable BE
FOLDER_PUBLISH="/home/jenkins/publish"

FOLDER_PUBLISH_IDENTITY_SERVER="$FOLDER_PUBLISH/EPIC.IdentityServer"
FOLDER_PUBLISH_CORE_SERVICE="$FOLDER_PUBLISH/EPIC.CoreService"
FOLDER_PUBLISH_BOND_SERVICE="$FOLDER_PUBLISH/EPIC.BondService"
FOLDER_PUBLISH_IMAGE_SERVICE="$FOLDER_PUBLISH/EPIC.ImageService"
FOLDER_PUBLISH_INVEST_SERVICE="$FOLDER_PUBLISH/EPIC.InvestService"
FOLDER_PUBLISH_ROCKETCHAT_SERVICE="$FOLDER_PUBLISH/EPIC.RocketchatService"
FOLDER_PUBLISH_API_GATEWAY="$FOLDER_PUBLISH/EPIC.APIGateway"
FOLDER_PUBLISH_PAYMENT_SERVICE="$FOLDER_PUBLISH/EPIC.PaymentService"
FOLDER_PUBLISH_SHARED_SERVICE="$FOLDER_PUBLISH/EPIC.SharedService"

FOLDER_BE_IDENTITY_SERVER="EPIC.IdentityServer/EPIC.IdentityServer"
FOLDER_BE_CORE_SERVICE="EPIC.CoreService/EPIC.CoreAPI"
FOLDER_BE_BOND_SERVICE="EPIC.BondService/EPIC.Trading"
FOLDER_BE_IMAGE_SERVICE="EPIC.ImageService/EPIC.ImageAPI"
FOLDER_BE_INVEST_SERVICE="EPIC.InvestService/EPIC.InvestAPI"
FOLDER_BE_ROCKETCHAT_SERVICE="EPIC.RocketchatService/EPIC.Rocketchat"
FOLDER_BE_API_GATEWAY="EPIC.APIGateway/EPIC.APIGateway"
FOLDER_BE_MEDIA="EPIC.Media"
FOLDER_BE_PAYMENT_SERVICE="EPIC.PaymentService/EPIC.PaymentAPI"
FOLDER_BE_SHARED_SERVICE="EPIC.SharedService/EPIC.SharedAPI"

# variable FE
FOLDER_SRC_CODE="/var/lib/jenkins/workspace/epic-staging/src"

FOLDER_FE_HOME="EPIC.Home"
FOLDER_FE_CORE="EPIC.SuperAdminCMS"
FOLDER_FE_BOND="EPIC.Ebond"
FOLDER_FE_INVEST="EPIC.Invest"
FOLDER_FE_USER="EPIC.Euser"

# set permission
cd /var/lib/jenkins/workspace/epic-staging/.git
sudo chown -R jenkins:jenkins *

# DEPLOY BACK END
cd /var/lib/jenkins/workspace/epic-staging/src

# kill all dotnet app
echo === Kill all dotnet app ===
killall -9 dotnet

# create publish folder
mkdir -p "$FOLDER_PUBLISH_IDENTITY_SERVER"
mkdir -p "$FOLDER_PUBLISH_CORE_SERVICE"
mkdir -p "$FOLDER_PUBLISH_BOND_SERVICE"
mkdir -p "$FOLDER_PUBLISH_IMAGE_SERVICE"
mkdir -p "$FOLDER_PUBLISH_INVEST_SERVICE"
mkdir -p "$FOLDER_PUBLISH_ROCKETCHAT_SERVICE"
mkdir -p "$FOLDER_PUBLISH_API_GATEWAY"
mkdir -p "$FOLDER_PUBLISH_PAYMENT_SERVICE"
mkdir -p "$FOLDER_PUBLISH_SHARED_SERVICE"

# set enviroment variable
export ASPNETCORE_ENVIRONMENT=Staging
export OCELOT_ENVIRONMENT=Staging
export BUILD_ID=dontKillMe 

# gateway
echo === Publish EPIC.APIGateway ===

cd "$FOLDER_BE_API_GATEWAY"
dotnet publish -c Release -o "$FOLDER_PUBLISH_API_GATEWAY"
nohup dotnet "$FOLDER_PUBLISH_API_GATEWAY/EPIC.APIGateway.dll" --environment=Production > "$FOLDER_PUBLISH_API_GATEWAY/dotnetcore.log" &

# identity server
echo  === Publish EPIC.IdentityServer ===

cd ../..
cd "$FOLDER_BE_IDENTITY_SERVER"
dotnet publish -c Release -o "$FOLDER_PUBLISH_IDENTITY_SERVER"
nohup dotnet "$FOLDER_PUBLISH_IDENTITY_SERVER/EPIC.IdentityServer.dll" > "$FOLDER_PUBLISH_IDENTITY_SERVER/dotnetcore.log" &

# core
echo === Publish EPIC.CoreService ===

cd ../..
cd "$FOLDER_BE_CORE_SERVICE"
dotnet publish -c Release -o "$FOLDER_PUBLISH_CORE_SERVICE"
nohup dotnet "$FOLDER_PUBLISH_CORE_SERVICE/EPIC.CoreAPI.dll" > "$FOLDER_PUBLISH_CORE_SERVICE/dotnetcore.log" &

# bond
echo === Publish EPIC.BondService ===

cd ../..
cd "$FOLDER_BE_BOND_SERVICE"
dotnet publish -c Release -o "$FOLDER_PUBLISH_BOND_SERVICE"
nohup dotnet "$FOLDER_PUBLISH_BOND_SERVICE/EPIC.BondAPI.dll" > "$FOLDER_PUBLISH_BOND_SERVICE/dotnetcore.log" &

# image
echo === Publish EPIC.ImageService ===

cd ../..
cd "$FOLDER_BE_IMAGE_SERVICE"
dotnet publish -c Release -o "$FOLDER_PUBLISH_IMAGE_SERVICE"
nohup dotnet "$FOLDER_PUBLISH_IMAGE_SERVICE/EPIC.ImageAPI.dll" > "$FOLDER_PUBLISH_IMAGE_SERVICE/dotnetcore.log" &

# invest
echo === Publish EPIC.InvestService ===

cd ../..
cd "$FOLDER_BE_INVEST_SERVICE"
dotnet publish -c Release -o "$FOLDER_PUBLISH_INVEST_SERVICE"
nohup dotnet "$FOLDER_PUBLISH_INVEST_SERVICE/EPIC.InvestAPI.dll" > "$FOLDER_PUBLISH_INVEST_SERVICE/dotnetcore.log" &

# rocketchat
echo === Publish EPIC.RocketchatService ===

cd ../..
cd "$FOLDER_BE_ROCKETCHAT_SERVICE"
dotnet publish -c Release -o "$FOLDER_PUBLISH_ROCKETCHAT_SERVICE"
nohup dotnet "$FOLDER_PUBLISH_ROCKETCHAT_SERVICE/EPIC.Rocketchat.dll" > "$FOLDER_PUBLISH_ROCKETCHAT_SERVICE/dotnetcore.log" &

# payment
echo === Publish EPIC.PaymentService ===

cd ../..
cd "$FOLDER_BE_PAYMENT_SERVICE"
dotnet publish -c Release -o "$FOLDER_PUBLISH_PAYMENT_SERVICE"
nohup dotnet "$FOLDER_PUBLISH_PAYMENT_SERVICE/EPIC.PaymentAPI.dll" > "$FOLDER_PUBLISH_PAYMENT_SERVICE/dotnetcore.log" &

# shared
echo === Publish EPIC.SharedService ===

cd ../..
cd "$FOLDER_BE_SHARED_SERVICE"
dotnet publish -c Release -o "$FOLDER_PUBLISH_SHARED_SERVICE"
nohup dotnet "$FOLDER_PUBLISH_SHARED_SERVICE/EPIC.SharedAPI.dll" > "$FOLDER_PUBLISH_SHARED_SERVICE/dotnetcore.log" &

# media
# echo === Publish EPIC.Media ===

# cd ../..
# cd "$FOLDER_BE_MEDIA"
# npm i
# yarn docker:prod
# ===================================================================================================== #

# DEPLOY FRONT END
cd "$FOLDER_SRC_CODE/EPIC.CMS"

# core
cd "$FOLDER_FE_CORE"
npm i
rm -rf dist
npm run build:staging

# home
cd ..
cd "$FOLDER_FE_HOME"
npm i
rm -rf dist
npm run build:staging

# bond
cd ..
cd "$FOLDER_FE_BOND"
npm i
rm -rf dist
npm run build:staging

# bond
cd ..
cd "$FOLDER_FE_INVEST"
npm i
rm -rf dist
npm run build:staging
