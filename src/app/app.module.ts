import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { PlayerComponent } from './components/Player';
import { MusicListComponent } from './components/MusicList';
import { PlayerService } from './services/PlayerService';
import { LibraryService } from './services/LibraryService';
import { LibraryComponent } from './components/Library';


@NgModule({
  declarations: [
    AppComponent,
    PlayerComponent,
    MusicListComponent,
    LibraryComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [
    PlayerService,
    LibraryService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
