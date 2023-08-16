import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { ContractCodeItemComponent } from './crud-contract-code/contract-code-item/contract-code-item.component';
import { CrudContractCodeComponent } from './crud-contract-code/crud-contract-code.component';
import { SettingContractCodeComponent } from './setting-contract-code.component';

@NgModule({
  declarations: [SettingContractCodeComponent, CrudContractCodeComponent, ContractCodeItemComponent],
  imports: [SharedModule],
  exports: [SettingContractCodeComponent],
})
export class SettingContractCodeModule {}
