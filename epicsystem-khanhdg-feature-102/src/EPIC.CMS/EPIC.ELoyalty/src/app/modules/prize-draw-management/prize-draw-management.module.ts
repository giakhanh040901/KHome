import { NgModule } from '@angular/core';
import { LuckySpinDisplaySettingComponent } from './create-prize-draw/program-configuration/create-or-edit-lucky-scenario/lucky-spin-display-setting/lucky-spin-display-setting.component';
import { SharedModule } from '@shared/shared.module';
import { DetailPrizeDrawComponent } from './detail-prize-draw/detail-prize-draw.component';
import { PrizeDrawManagementComponent } from './prize-draw-management.component';
import { CreatePrizeDrawComponent } from './create-prize-draw/create-prize-draw.component';
import { ProgramInfomationComponent } from './create-prize-draw/program-infomation/program-infomation.component';
import { ProgramConfigurationComponent } from './create-prize-draw/program-configuration/program-configuration.component';
import { CreateOrEditLuckyScenarioComponent } from './create-prize-draw/program-configuration/create-or-edit-lucky-scenario/create-or-edit-lucky-scenario.component';
import { HistoryUpdatePrizeDrawComponent } from './detail-prize-draw/history-update-prize-draw/history-update-prize-draw.component';
import { JoinSettingPrizeDrawComponent } from './join-setting-prize-draw/join-setting-prize-draw.component';
import { AddPersonListComponent } from './join-setting-prize-draw/add-person-list/add-person-list.component';

@NgModule({
  declarations: [
    LuckySpinDisplaySettingComponent,
    DetailPrizeDrawComponent,
    PrizeDrawManagementComponent,
    CreatePrizeDrawComponent,
    ProgramInfomationComponent,
    ProgramConfigurationComponent,
    CreateOrEditLuckyScenarioComponent,
    HistoryUpdatePrizeDrawComponent,
    JoinSettingPrizeDrawComponent,
    AddPersonListComponent,
  ],
  imports: [
    SharedModule,
  ],
  exports: []
})
export class PrizeDrawManagementModule { }
