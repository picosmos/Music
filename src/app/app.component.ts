import { Component, OnInit } from '@angular/core';
import { MusicViewModel } from './models/MusicViewModel';
import { PlayerService } from './services/PlayerService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  constructor(
    private _playerService: PlayerService
  ) {
  }

  public itemClicked(item: MusicViewModel) {
    this._playerService.setFiles([
      item
    ]);
  }
}
