import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { SourceViewComponent } from './source-view/source-view.component';
import { DemoViewComponent } from './demo-view/demo-view.component';

@NgModule({
  declarations: [
    AppComponent,
    SourceViewComponent,
    DemoViewComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
