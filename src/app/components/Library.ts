import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { MusicViewModel } from "../models/MusicViewModel";
import { LibraryService, MusicMetaDataModel } from "../services/LibraryService";
import { IndexingService } from "../services/IndexingService";

@Component({
  selector: "library",
  templateUrl: "./Library.web.html",
  styleUrls: ["./Library.web.less"]
})
export class LibraryComponent implements OnInit {
  constructor(
    private _libraryService: LibraryService,
    private _indexingService: IndexingService
  ) {
  }

  public paths: string[] = [];

  public music: MusicMetaDataModel[];

  public progressCurrent: number;
  public progressMax: number;

  public addPath(path: string) {
    if (!path || this.paths.indexOf(path) >= 0) {
      return;
    }

    this.paths.push(path);
    this._libraryService.addPath(path).then(() => { });
  }

  public deletePath(path: string) {
    this.paths.splice(this.paths.indexOf(path), 1);
    this._libraryService.deletePath(path).then(() => { });
  }

  public findMusicFiles() {
    this.progressCurrent = 0;
    this.progressMax = 1;
    this._indexingService.reCreateIndex((current, max) => {
      this.progressCurrent = current;
      this.progressMax = max;
      if (current == max) {
        this.progressCurrent = null;
        this.progressMax = null;
      }
    }).then(() => {
      this._libraryService.getMusic().then((data) => {
        this.music = data;
      });
    });
  }

  public async ngOnInit() {
    this.paths = await this._libraryService.getPaths();
  }
}
