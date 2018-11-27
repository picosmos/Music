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
    console.log("findMusic");
    this._indexingService.reCreateIndex().then(() => {
      console.log("reCreateIndex End");
      this._libraryService.getMusic().then((data) => {
        console.log("got Data");
        this.music = data;
      });
    });
  }

  public async ngOnInit() {
    this.paths = await this._libraryService.getPaths();
  }
}
