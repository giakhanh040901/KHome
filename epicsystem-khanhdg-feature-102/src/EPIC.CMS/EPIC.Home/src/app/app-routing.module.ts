import { OverviewInvestComponent } from './overview-invest/overview-invest.component';
import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { AppMainComponent } from "./layout/main/app.main.component";
import { HomeComponent } from "./home/home.component";
import { UserComponent } from "./user/user.component";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";
import { ChatComponent } from "./support/chat/chat.component";
import { RocketchatComponent } from "./rocketchat/rocketchat.component";
import { InvestVerifyReceiveContractComponent } from "./invest-verify-receive-contract/invest-verify-receive-contract.component";
import { BondVerifyReceiveContractComponent } from "./bond-verify-receive-contract/bond-verify-receive-contract.component";
import { HomeMobileComponent } from "./home-mobile/home-mobile.component";
import { OverviewBondComponent } from './overview-bond/overview-bond.component';
import { GarnerVerifyReceiveContractComponent } from './garner-verify-receive-contract/garner-verify-receive-contract.component';


@NgModule({
	imports: [
		RouterModule.forChild([
			{
				path: "",
				component: AppMainComponent,
				children: [
					{ path: "home", component: HomeComponent, canActivate: [AppRouteGuard] },
					{ path: "mobile/home", component: HomeMobileComponent, canActivate: [AppRouteGuard] },
					{ path: "user", component: UserComponent, canActivate: [AppRouteGuard] },
					{ path: "mobile/overview-invest", component: OverviewInvestComponent, canActivate: [AppRouteGuard] },
					{ path: "mobile/overview-bond", component: OverviewBondComponent, canActivate: [AppRouteGuard] },
				],
			},
			{
				path: "support",
				component: AppMainComponent,
				children: [
					{ path: "chat", component: ChatComponent, canActivate: [AppRouteGuard] } 
				]
			},
			{
				path: "rocketchat",
				component: RocketchatComponent,
			},
			{
				path: "invest-verify-receive-contract/:id",
				component: InvestVerifyReceiveContractComponent,
			},
			{
				path: "bond-verify-receive-contract/:id",
				component: BondVerifyReceiveContractComponent,
			},
			{
				path: "garner-verify-receive-contract/:id",
				component: GarnerVerifyReceiveContractComponent,
			},
		]),
	],
	exports: [RouterModule],
})
export class AppRoutingModule {}
