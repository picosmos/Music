import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { PlayerComponent } from './components/Player';
import { MusicListComponent } from './components/MusicList';
import { PlayerService } from './services/PlayerService';


@NgModule({
  declarations: [
    AppComponent,
    PlayerComponent,
    MusicListComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [
    PlayerService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
