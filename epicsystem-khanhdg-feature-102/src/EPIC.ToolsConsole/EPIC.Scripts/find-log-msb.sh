myDate=$(date +'%Y.%m.%d')
ip=22 #23

identityLog="/var/lib/jenkins/workspace/publish/EPIC.IdentityServer/logs/identity_$myDate.*"
coreLog="/var/lib/jenkins/workspace/publish/EPIC.CoreService/logs/core_$myDate.*"
investLog="/var/lib/jenkins/workspace/publish/EPIC.InvestService/logs/invest_$myDate.*"
garnerLog="/var/lib/jenkins/workspace/publish/EPIC.GarnerService/logs/garner_$myDate.*"
paymentLog="/var/lib/jenkins/workspace/publish/EPIC.PaymentService/logs/payment_$myDate.*"
sharedLog="/var/lib/jenkins/workspace/publish/EPIC.SharedService/logs/shared_$myDate.*"

logs=(identityLog coreLog investLog garnerLog paymentLog sharedLog)


ssh root@10.0.0.$ip "
    echo $logs
"

#ssh root@10.0.0.$ip "for file in $logs[@]; do if [ -f "$file" ]; then cat "$file"; fi; done; end"
    # "cat /var/lib/jenkins/workspace/publish/EPIC.IdentityServer/logs/identity_$myDate.*" \
    # #"cat /var/lib/jenkins/workspace/publish/EPIC.CoreService/logs/core_$myDate.*" \
    # #"cat /var/lib/jenkins/workspace/publish/EPIC.BondService/logs/bond_$myDate.*" \
    # "/var/lib/jenkins/workspace/publish/EPIC.InvestService/logs/invest_$myDate.*" \
    # "/var/lib/jenkins/workspace/publish/EPIC.GarnerService/logs/garner_$myDate.*" \
    # "/var/lib/jenkins/workspace/publish/EPIC.PaymentService/logs/payment_$myDate.*" \
    # "/var/lib/jenkins/workspace/publish/EPIC.SharedService/logs/shared_$myDate.*" \
    # > /dev/null \
    # | grep "MsbPayMoneyServices - CreateRequest" -A 10