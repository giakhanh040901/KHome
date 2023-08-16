myDate=$(date +'%Y.%m.%d')

ssh root@10.0.0.22 \
    "cat /var/lib/jenkins/workspace/publish/EPIC.BondService/logs/bond_$myDate.*" \
    "cat /var/lib/jenkins/workspace/publish/EPIC.InvestService/logs/invest_$myDate.*" \
    "cat /var/lib/jenkins/workspace/publish/EPIC.CoreService/logs/core_$myDate.*" \
    "cat /var/lib/jenkins/workspace/publish/EPIC.GarnerService/logs/garner_$myDate.*" \
    "cat /var/lib/jenkins/workspace/publish/EPIC.IdentityServer/logs/identity_$myDate.*" \
    "cat /var/lib/jenkins/workspace/publish/EPIC.PaymentService/logs/payment_$myDate.*" \
    "cat /var/lib/jenkins/workspace/publish/EPIC.SharedService/logs/shared_$myDate.*" \
    | grep "MsbPayMoneyServices - CreateRequest" -A 10
