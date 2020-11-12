import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
// revised from here
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ToDoItemService } from './services/todoitem.service';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DEFAULT_OPTIONS} from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { DialogComponent } from './components/dialog/dialog.component';
import { SubToDoItemFormComponent } from './components/sub-todoitemform/sub-todoitemform.component';
import { ToDoItemFormComponent } from './components/todoitemsform/todoitemsform.component';

@NgModule({
  declarations: [
    AppComponent,
    ToDoItemFormComponent,
    SubToDoItemFormComponent,
    DialogComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    CommonModule,
    MatDialogModule,
    FlexLayoutModule,
    RouterModule.forRoot([
      { path: '', pathMatch: 'full', redirectTo: 'items' },
      { path: 'items', component: ToDoItemFormComponent },
      { path: 'item/:id', component: SubToDoItemFormComponent },
      { path: 'item', component: SubToDoItemFormComponent }
    ]),
    BrowserAnimationsModule
  ],
  providers: [ ToDoItemService, {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: false}}],
  bootstrap: [AppComponent]
})
export class AppModule { }
