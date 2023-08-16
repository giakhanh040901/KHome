import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MediaComponent } from './media/media.component';
import { SharedModule } from '@shared/shared.module';
import { AddMediaComponent } from './media/add-media/add-media.component';
import { ReactiveFormsModule } from '@angular/forms';
import { KnowledgeBaseComponent } from './knowledge-base/knowledge-base.component';
import { BroadcastNewsComponent } from './broadcast-news/broadcast-news.component';
import { AddNewsComponent } from './broadcast-news/add-news/add-news.component';

@NgModule({
  declarations: [
    MediaComponent,
    AddMediaComponent,
    KnowledgeBaseComponent,
    BroadcastNewsComponent,
    AddNewsComponent
  ],
  imports: [CommonModule, SharedModule,ReactiveFormsModule],
  
})
export class MediaManagementModule { }
