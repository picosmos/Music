import { Component, OnInit, Input } from "@angular/core";
import { MusicViewModel } from "../models/MusicViewModel";
import { PlayerService } from "../services/PlayerService";
import { ThrowStmt } from "@angular/compiler";

@Component({
  selector: "player",
  templateUrl: "./Player.web.html",
  styleUrls: ["./Player.web.less"]
})
export class PlayerComponent {
  constructor(
    private _playerService: PlayerService
  ) {
    this._playerService.currentItemChanged.subscribe({
      next: (item: MusicViewModel) => {
        if (item) {
          this.hasFile = true;
          this.marquee = item.path;
        }
        else {
          this.hasFile = false;
          this.marquee = null;
        }
      }
    });
  }

  public marquee: string;
  public hasFile: boolean;

  public onplay() {
    this._playerService.play();
  }
  public onpause() {
    this._playerService.pause();
  }
}